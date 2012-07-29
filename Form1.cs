using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GraphTools;
using GraphTools.OSM;
using System.Collections;
using Logging;

namespace clcSimplifier
{
    public partial class Form1 : Form
    {
        protected string inputFile;
        protected string outputFile;    
    
        protected IEnumerable<OSMNode> nodes;
        protected IEnumerable<OSMWay> ways;
        protected IEnumerable<OSMRelation> relations;

        public string InputFile
        {
            get{ return inputFile;}
            set{
                inputFile = value;
                inputFileBox.Text = value;
            }
        }

        public string OutputFile
        {
            get { return outputFile; }
            set {
                outputFile = value;
                outputFileBox.Text = value;
            }
        }        

        public Form1()
        {
            InitializeComponent();
            Log.getInstance().addLogger(new WinformsTextBoxLogger(logBox,true));
        }

        private void ChooseInputButton_Click(object sender, EventArgs e)
        {
            if (OFD1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InputFile = OFD1.FileName;
            }
        }

        private void ChooseOutputButton_Click(object sender, EventArgs e)
        {
            if (SFD1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputFile = SFD1.FileName;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            logBox.Clear();
            if (String.IsNullOrEmpty(InputFile))
            {
                logBox.AppendText("Error: Bad input file.\n");
                return;
            }
            if (String.IsNullOrEmpty(OutputFile))
            {
                logBox.AppendText("Error: bad output file.\n");
                return;
            }

            startButton.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Version vrs = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            this.Text = "CLC Simplifier v" + vrs.Major + "." + vrs.Minor + "." + vrs.Build + " by Plutoz";
            //MessageBox.Show("v" + vrs.Major + "." + vrs.Minor+"."+vrs.Build);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            IDictionary<string, string> rootAttributes;

            DateTime startTime, endTime;
            TimeSpan workTime;
            Log.getInstance().writeLine("Start job...");
            Log.getInstance().writeLine("Parsing input...");

            startTime = DateTime.Now;
            OSMFileHandler.ReadFile(inputFile, out nodes, out ways, out relations, out rootAttributes);
            endTime = DateTime.Now;
            workTime = endTime.Subtract(startTime);
            Log.getInstance().writeLine("File parsing time: " + workTime.TotalMilliseconds + " ms\n");

            GC.Collect();
            OSMWay.SetNextID((ways.Max(x => x.ID) + 1));

            ICollection<OSMWay> newWays;
            ICollection<OSMRelation> newRelations;
            Log.getInstance().writeLine("Processing data...\n");

            startTime = DateTime.Now;
            Worker.DoWork(ways, relations, out newWays, out newRelations);
            endTime = DateTime.Now;
            workTime = endTime.Subtract(startTime);
            Log.getInstance().writeLine("DoWork job time: " + workTime.TotalMilliseconds + " ms");
            Log.getInstance().writeLine("Filtered ways: " + newWays.Count);

            //resultlabel.Text = "Total: nodes=" + nodes.Count + " ways=" + ways.Count + " relations=" + relations.Count;

            Log.getInstance().writeLine("Writing output...");
            startTime = DateTime.Now;

            OSMFileHandler.WriteFile(outputFile, nodes, newWays, newRelations, rootAttributes: rootAttributes);
            endTime = DateTime.Now;
            workTime = endTime.Subtract(startTime);
            Log.getInstance().writeLine("Filewriting time: " + workTime.TotalMilliseconds + " ms");
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startButton.Enabled = true;
            logBox.AppendText("Job done.");
        }
    }
}
