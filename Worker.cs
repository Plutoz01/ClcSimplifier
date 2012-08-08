using System;
using System.Collections.Generic;
using System.Linq;
using GraphTools;
using GraphTools.OSM;
using Logging;
using System.Threading.Tasks;

namespace clcSimplifier
{
    public class Worker
    {
        public static void DoWork(IEnumerable<OSMWay> ways, IEnumerable<OSMRelation> relations,
            out  ICollection<OSMWay> newWays, out ICollection<OSMRelation> newRelations)
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

             //collect relations
            var waysById = ways.ToDictionary(x => x.ID);

            //collect data for relations already have from ways
            foreach (var rel in relations)
            {
                if (rel.Tags.ContainsKey("CLC:id")) continue;
                foreach (var member in rel.Members)
                {
                    OSMWay w;
                    if(waysById.TryGetValue(member.Reference,out w)){
                        if(w.Tags.ContainsKey("CLC:id")){
                            rel.Tags = new Dictionary<string, string>(w.Tags);
                            break;
                        }
                    }
                }
            }
            waysById = null;

            var allRelationsById = relations.ToDictionary(x => x.Tags["CLC:id"]);
            foreach (var rel in GetRelations(ways))
            {
                string clcId = rel.Tags["CLC:id"];
                if (!allRelationsById.ContainsKey(clcId)) allRelationsById.Add(clcId, rel);
            }
            Log.getInstance().writeLine("Collecting relations: complete");
            //adding fixme and type=multipolygon tags for new relations
            foreach (var w in allRelationsById.Values)
            {
                w.Tags.Add(OSMKeys.Fixme, "unchecked");
                if (!w.Tags.ContainsKey(OSMKeys.Type)) w.Tags.Add(OSMKeys.Type, "multipolygon");
            }

            //adding ways to relations
            foreach (var way in ways)
            {
                string clcID;
                //remove tags
                if (way.Tags.TryGetValue("CLC:id", out clcID))
                {
                    way.Tags.Clear();
                }
                else continue;
                //add to relation if it doesnt' contains yet
                var parentRelation = allRelationsById[clcID];
                bool isContains = false;
                foreach (var m in parentRelation.Members)
                {
                    if (m.Reference == way.ID)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (!isContains)
                {
                    parentRelation.Members.Add(new OSMRelation.Member(OSMKeys.WayType,way.ID,OSMKeys.RoleOuter));
                }
            }
            Log.getInstance().writeLine("Adding ways to relations: complete");

            //create index of relations by used ways
            Dictionary<int, List<OSMRelation>> relationsByUsedWays = GetRelationsByUsedWays(allRelationsById.Values,ways);  

            var intersections = OSMWay.GetIntersections(ways);
            var splittedWays = new List<OSMWay>();
            //foreach (var currentWay in ways)
            Parallel.ForEach(ways, parallelOptions, currentWay =>
            {
                var partialWays = currentWay.SplitN(intersections);
                lock (splittedWays)
                {
                    splittedWays.AddRange(partialWays);
                }
                //add partial ways to original way's relations
                var affectedRelations = relationsByUsedWays[currentWay.ID];
                foreach (var rel in affectedRelations)
                {
                    lock (rel.Members)
                    {
                        OSMRelation.Member oldMember = rel.Members.Where(x => x.Reference == currentWay.ID).First();
                        //add new partial ways
                        foreach (var newWay in partialWays)
                        {
                            rel.Members.Add(new OSMRelation.Member(oldMember.Type, newWay.ID, oldMember.Role));
                        }
                        //remove old way
                        rel.Members.Remove(oldMember);
                    }
                }
                });            
            //}
            intersections = null;
            Log.getInstance().writeLine("Way splitting: complete");

            //reindex because of earlier change (ways=>partial ways)
            relationsByUsedWays = GetRelationsByUsedWays(allRelationsById.Values,splittedWays);

            OverlapsEqualityComparer comparer = new OverlapsEqualityComparer();

            var filtered = new Dictionary<OSMWay, int>(comparer);
            var unneccesaryWays = new HashSet<OSMWay>();
            foreach (var way in splittedWays)
            {
                if (filtered.ContainsKey(way))
                {
                    foreach (var rel in relationsByUsedWays[way.ID])
                    {
                        foreach (var member in rel.Members)
                        {
                            if (member.Reference == way.ID) member.Reference = filtered[way];
                        }
                    }
                    unneccesaryWays.Add(way);
                }
                else
                {
                    filtered.Add(way, way.ID);
                }
            }
            filtered = null;
            splittedWays.RemoveAll(x => unneccesaryWays.Contains(x));
            unneccesaryWays = null;

            Log.getInstance().writeLine("Merge Overlaping ways: complete");


            //remove empty relations
            newRelations = allRelationsById.Values.Where(x => x.Members.Count > 0).ToList();
            Log.getInstance().writeLine("Remove empty relations: complete");

            //convert back 1 member relations to ways
            var oneMemberRelations = newRelations.Where(r => r.Members.Count == 1 && r.Members.First().Role.Equals(OSMKeys.RoleOuter));
            waysById = splittedWays.ToDictionary(x => x.ID);
            foreach (var rel in oneMemberRelations)
            {
                waysById[rel.Members.First().Reference].Tags = rel.Tags;
            }
            newRelations = newRelations.Except(oneMemberRelations).ToList();
            Log.getInstance().writeLine("Convert 1 member relations to simple way (all: "+oneMemberRelations.Count()+"): complete");

            newWays = waysById.Values;
        }

        protected static ICollection<OSMRelation> GetRelations(IEnumerable<OSMWay> ways)
        {
            Dictionary<string,OSMRelation> result = new Dictionary<string,OSMRelation>();
            foreach (OSMWay way in ways)
            {
                string CLC_ID;
                if (way.Tags.TryGetValue("CLC:id", out CLC_ID))
                {
                    if (!result.ContainsKey(CLC_ID))
                    {
                        OSMRelation tmprelation = new OSMRelation();
                        tmprelation.Tags = new Dictionary<string, string>(way.Tags);
                        tmprelation.Tags.Add("type", "multipolygon");
                        result.Add(CLC_ID, tmprelation);
                    }
                }
            }
            return result.Values;
        }

        protected static Dictionary<int, List<OSMRelation>> GetRelationsByUsedWays(IEnumerable<OSMRelation> relations, IEnumerable<OSMWay> ways)
        {
            Dictionary<int, List<OSMRelation>> result = new Dictionary<int, List<OSMRelation>>();
            //init
            foreach (var w in ways)
            {
                result.Add(w.ID, new List<OSMRelation>());
            }
            //select
            foreach (var rel in relations)
            {
                foreach (var m in rel.Members)
                {
                    result[m.Reference].Add(rel);
                }
            }
            return result;
        }
    }
}
