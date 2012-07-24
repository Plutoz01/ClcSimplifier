namespace clcSimplifier
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChooseInputButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.OFD1 = new System.Windows.Forms.OpenFileDialog();
            this.SFD1 = new System.Windows.Forms.SaveFileDialog();
            this.inputFileBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ChooseOutputButton = new System.Windows.Forms.Button();
            this.outputFileBox = new System.Windows.Forms.TextBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resultlabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChooseInputButton
            // 
            this.ChooseInputButton.Location = new System.Drawing.Point(415, 3);
            this.ChooseInputButton.Name = "ChooseInputButton";
            this.ChooseInputButton.Size = new System.Drawing.Size(73, 21);
            this.ChooseInputButton.TabIndex = 0;
            this.ChooseInputButton.Text = "Choose";
            this.ChooseInputButton.UseVisualStyleBackColor = true;
            this.ChooseInputButton.Click += new System.EventHandler(this.ChooseInputButton_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label1.Size = new System.Drawing.Size(62, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input file:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OFD1
            // 
            this.OFD1.DefaultExt = "osm";
            this.OFD1.Filter = "OSM files|*.osm|All files|*.*";
            // 
            // SFD1
            // 
            this.SFD1.DefaultExt = "osm";
            this.SFD1.Filter = "OSM files|*.osm|All files|*.*";
            // 
            // inputFileBox
            // 
            this.inputFileBox.BackColor = System.Drawing.Color.White;
            this.inputFileBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputFileBox.Location = new System.Drawing.Point(71, 3);
            this.inputFileBox.Name = "inputFileBox";
            this.inputFileBox.ReadOnly = true;
            this.inputFileBox.Size = new System.Drawing.Size(338, 20);
            this.inputFileBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 27);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label2.Size = new System.Drawing.Size(62, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output file:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.Controls.Add(this.ChooseOutputButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChooseInputButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.inputFileBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.outputFileBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(491, 55);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // ChooseOutputButton
            // 
            this.ChooseOutputButton.Location = new System.Drawing.Point(415, 30);
            this.ChooseOutputButton.Name = "ChooseOutputButton";
            this.ChooseOutputButton.Size = new System.Drawing.Size(73, 22);
            this.ChooseOutputButton.TabIndex = 5;
            this.ChooseOutputButton.Text = "Choose";
            this.ChooseOutputButton.UseVisualStyleBackColor = true;
            this.ChooseOutputButton.Click += new System.EventHandler(this.ChooseOutputButton_Click);
            // 
            // outputFileBox
            // 
            this.outputFileBox.BackColor = System.Drawing.Color.White;
            this.outputFileBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputFileBox.Location = new System.Drawing.Point(71, 30);
            this.outputFileBox.Name = "outputFileBox";
            this.outputFileBox.ReadOnly = true;
            this.outputFileBox.Size = new System.Drawing.Size(338, 20);
            this.outputFileBox.TabIndex = 4;
            // 
            // logBox
            // 
            this.logBox.BackColor = System.Drawing.Color.White;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.Location = new System.Drawing.Point(0, 55);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(491, 173);
            this.logBox.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.resultlabel);
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 27);
            this.panel1.TabIndex = 6;
            // 
            // resultlabel
            // 
            this.resultlabel.AutoSize = true;
            this.resultlabel.Location = new System.Drawing.Point(88, 7);
            this.resultlabel.Name = "resultlabel";
            this.resultlabel.Size = new System.Drawing.Size(0, 13);
            this.resultlabel.TabIndex = 1;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(6, 1);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 255);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "CLC Simplifier v0.1.7 by Plutoz";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChooseInputButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog OFD1;
        private System.Windows.Forms.SaveFileDialog SFD1;
        private System.Windows.Forms.TextBox inputFileBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button ChooseOutputButton;
        private System.Windows.Forms.TextBox outputFileBox;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label resultlabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

