using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
             //collect relations
            newRelations = GetRelations(ways);
            //adding fixme tags for new relations
            foreach (var w in newRelations)
            {
                w.Tags.Add(OSMKeys.Fixme, "unchecked");
            }

            
            
            //Bad solution, unknown relation properties will be added to new relations

            /*Dictionary<int,OSMWay> tmpDict= ways.ToDictionary(x=>x.ID);
            foreach (var r in relations)
            {
                foreach (var m in r.Members)
                {
                    tmpDict[m.Reference].Tags.Add(OSMKeys.Role, m.Role);
                }
            } */           

            //remove duplicates
            DateTime startTime, endTime;
            TimeSpan workTime;

            startTime = DateTime.Now;

            var intersections = OSMWay.GetIntersections(ways);
            //var filtered = OSMWay.DistinctSplit(OSMWay.GetIntersections(ways), ways);
            var splittedWays = new List<OSMWay>();
            Parallel.ForEach(ways, currentWay =>
            {
                var partialWays = currentWay.SplitN(intersections);
                lock (splittedWays)
                {
                    splittedWays.AddRange(partialWays);
                }
            });


            intersections = null;

            OverlapsEqualityComparer comparer = new OverlapsEqualityComparer();
            var filtered = new Dictionary<OSMWay, List<Dictionary<string,string>>>(comparer);

            string CLCid;
            string role;

            List<OSMWay> withoutTags = new List<OSMWay>();
            foreach (var sp in splittedWays)
            {
                if (!sp.Tags.ContainsKey("CLC:id")) withoutTags.Add(sp);
            }
            Log.getInstance().writeLine("Warning! There are "+withoutTags.Count+" ways without CLC:id");
            withoutTags.Clear();

            foreach (var sp in splittedWays)
            {
                if (sp.Tags.TryGetValue("CLC:id", out CLCid))
                {
                    //create container contains CLC:id & role in the relation
                    Dictionary<string, string> relationData = new Dictionary<string, string>();
                    relationData.Add("CLC:id", CLCid);
                    if (sp.Tags.TryGetValue(OSMKeys.Role, out role))
                    {
                        relationData.Add(OSMKeys.Role, role);
                    }

                    if (filtered.ContainsKey(sp))
                    {
                        filtered[sp].Add(relationData);
                    }
                    else
                    {
                        var list = new List<Dictionary<string, string>>();
                        list.Add(relationData);
                        filtered.Add(sp, list);
                        sp.Tags.Clear();
                    }
                }
                //at the end will add to result
                else
                {
                    if (!filtered.ContainsKey(sp))
                        filtered.Add(sp, new List<Dictionary<string, string>>());
                }
            }

            endTime = DateTime.Now;
            workTime = endTime.Subtract(startTime);
            Log.getInstance().writeLine("Filter time: " + workTime.TotalMilliseconds +"ms");
            //end remove duplicates
            

            //match newWays to filtered ways, adding to relation
            startTime = DateTime.Now;

            Dictionary<string, OSMRelation> tmpNewRelations = newRelations.ToDictionary(x => x.Tags["CLC:id"]);


            foreach (var way in filtered)
            {
                string relationKey;
                foreach(var r in way.Value){
                    OSMRelation.Member tmpMember = new OSMRelation.Member(OSMKeys.WayType,
                        way.Key.ID, r.TryGetValue(OSMKeys.Role, out role) ? role : string.Empty);
                
                    relationKey = r["CLC:id"];
                    tmpNewRelations[relationKey].Members.Add(tmpMember);
                }
            }

            /*string role;
            foreach (var way in filtered)
            {
                OSMRelation.Member tmpMember = new OSMRelation.Member(OSMKeys.WayType,
                            way.ID, way.Tags.TryGetValue(OSMKeys.Role, out role) ? role : string.Empty);
                //string relationKey;
                if (way.Tags.TryGetValue("CLC:id", out relationKey))
                {
                    tmpNewRelations[relationKey].Members.Add(tmpMember);
                }
            }*/
            endTime = DateTime.Now;
            workTime = endTime.Subtract(startTime);
            Log.getInstance().writeLine("Match time: " + workTime.TotalMilliseconds);

            //flush ways, keep filtered ways
            newWays = new List<OSMWay>(filtered.Keys);

            //clear tags of ways
            foreach (OSMWay way in newWays)
            {
                way.Tags.Clear();
            }
        }

        protected static ICollection<OSMRelation> GetRelations(IEnumerable<OSMWay> ways)
        {
            Dictionary<string,OSMRelation> result = new Dictionary<string,OSMRelation>();
            foreach (OSMWay way in ways)
            {
                string CLC_ID;
                if (way.Tags.TryGetValue("CLC:id", out CLC_ID))
                {
                    if (!result.ContainsKey(CLC_ID)){
                        OSMRelation tmprelation = new OSMRelation();
                        tmprelation.Tags = new Dictionary<string, string>(way.Tags);
                        tmprelation.Tags.Add("type", "multipolygon");                        
                        result.Add(CLC_ID, tmprelation);
                    }
                }
            }
            return result.Values;
        }
    }
}
