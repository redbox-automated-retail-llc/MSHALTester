using Redbox.HAL.Client;
using Redbox.HAL.Client.Services;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class AdvancedMode : Form
    {
        private readonly object ListLock = new object();
        private readonly ConfigPropertyList PropertyList;
        private readonly HardwareService Service;
        private readonly List<HardwareJobAdapter> JobList = new List<HardwareJobAdapter>();
        private readonly System.ComponentModel.BindingList<HardwareJobAdapter> BindingList;
        private readonly BindingSource BindingSource;
        private readonly ButtonAspectsManager Manager;
        private IContainer components;
        private Panel panel1;
        private Button m_exitButton;
        private Button button2;
        private Button m_saveConfigButton;
        private DataGridView m_jobListDataView;
        private Button button1;
        private Button button3;
        private Button button4;
        private Timer m_jobTimer;
        private TextBox m_vendMoveOutput;
        private GroupBox groupBox1;

        public AdvancedMode(HardwareService service)
        {
            this.InitializeComponent();
            this.Manager = new ButtonAspectsManager();
            this.Service = service;
            this.PropertyList = new ConfigPropertyList(service);
            this.panel1.Controls.Add((Control)this.PropertyList);
            this.PropertyList.OnConfigItemChange += new ConfigPropertyList.ConfigItemChanged(this.PropertyList_OnConfigItemChange);
            this.PropertyList.OnSave += new ConfigPropertyList.SaveCompleted(this.PropertyList_OnSave);
            this.BindingList = new System.ComponentModel.BindingList<HardwareJobAdapter>((IList<HardwareJobAdapter>)this.JobList);
            this.BindingSource = new BindingSource((object)this.BindingList, (string)null);
            this.m_jobListDataView.DataSource = (object)this.BindingSource;
            this.RefreshJobs();
            this.m_jobListDataView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.m_jobListDataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.m_jobTimer.Enabled = true;
        }

        private void PropertyList_OnSave() => this.m_saveConfigButton.Enabled = false;

        private void PropertyList_OnConfigItemChange() => this.m_saveConfigButton.Enabled = true;

        private void m_exitButton_Click(object sender, EventArgs e)
        {
            this.m_jobTimer.Enabled = false;
            this.PropertyList.OnSave -= new ConfigPropertyList.SaveCompleted(this.PropertyList_OnSave);
            this.PropertyList.Dispose();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.m_jobTimer.Enabled = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int num = (int)new DeckConfigurationForm(this.Service).ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("Unable to run Decks form.", ex);
            }
        }

        private void RefreshJobs()
        {
            this.JobList.ForEach((Action<HardwareJobAdapter>)(s => s.Removable = true));
            HardwareJob[] jobs;
            if (this.Service.GetJobs(out jobs).Success)
            {
                foreach (HardwareJob hardwareJob in jobs)
                {
                    HardwareJob nj = hardwareJob;
                    HardwareJobAdapter hardwareJobAdapter = this.JobList.Find((Predicate<HardwareJobAdapter>)(each => each.ID == nj.ID));
                    if (hardwareJobAdapter == null)
                    {
                        this.BindingSource.Add((object)new HardwareJobAdapter(nj));
                    }
                    else
                    {
                        hardwareJobAdapter.Merge(nj);
                        hardwareJobAdapter.Removable = false;
                    }
                }
            }
            foreach (object obj in this.JobList.FindAll((Predicate<HardwareJobAdapter>)(each => each.Removable)))
                this.BindingSource.Remove(obj);
        }

        private void m_saveConfigButton_Click(object sender, EventArgs e) => this.PropertyList.Save();

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.OnListAction((Action<HardwareJobAdapter>)(j =>
            {
                if (j.Status == HardwareJobStatus.Suspended)
                    return;
                j.Job.Suspend();
            }));
        }

        private void OnListAction(Action<HardwareJobAdapter> action)
        {
            foreach (DataGridViewRow selectedRow in (BaseCollection)this.m_jobListDataView.SelectedRows)
            {
                HardwareJobAdapter dataBoundItem = selectedRow.DataBoundItem as HardwareJobAdapter;
                action(dataBoundItem);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.OnListAction((Action<HardwareJobAdapter>)(j =>
            {
                if (j.Status != HardwareJobStatus.Suspended)
                    return;
                j.Job.Pend();
            }));
        }

        private void timer1_Tick(object sender, EventArgs e) => this.RefreshJobs();

        private void button4_Click(object sender, EventArgs e)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                string tagInstruction = buttonAspects.GetTagInstruction();
                using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                    this.m_vendMoveOutput.Text = instructionHelper.ExecuteErrorCode(tagInstruction).ToString().ToUpper();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new System.ComponentModel.Container();
            this.panel1 = new Panel();
            this.m_exitButton = new Button();
            this.button2 = new Button();
            this.m_saveConfigButton = new Button();
            this.m_jobListDataView = new DataGridView();
            this.button1 = new Button();
            this.button3 = new Button();
            this.button4 = new Button();
            this.m_jobTimer = new Timer(this.components);
            this.m_vendMoveOutput = new TextBox();
            this.groupBox1 = new GroupBox();
            ((ISupportInitialize)this.m_jobListDataView).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            this.panel1.Location = new Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(371, 492);
            this.panel1.TabIndex = 0;
            this.m_exitButton.BackColor = Color.GreenYellow;
            this.m_exitButton.Location = new Point(583, 461);
            this.m_exitButton.Name = "m_exitButton";
            this.m_exitButton.Size = new Size(112, 42);
            this.m_exitButton.TabIndex = 1;
            this.m_exitButton.Text = "Exit";
            this.m_exitButton.UseVisualStyleBackColor = false;
            this.m_exitButton.Click += new EventHandler(this.m_exitButton_Click);
            this.button2.Location = new Point(437, 461);
            this.button2.Name = "button2";
            this.button2.Size = new Size(113, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.m_saveConfigButton.Enabled = false;
            this.m_saveConfigButton.Location = new Point(400, 364);
            this.m_saveConfigButton.Name = "m_saveConfigButton";
            this.m_saveConfigButton.Size = new Size(150, 65);
            this.m_saveConfigButton.TabIndex = 4;
            this.m_saveConfigButton.Text = "Save Configuration Change";
            this.m_saveConfigButton.UseVisualStyleBackColor = true;
            this.m_saveConfigButton.Click += new EventHandler(this.m_saveConfigButton_Click);
            this.m_jobListDataView.AllowUserToAddRows = false;
            this.m_jobListDataView.AllowUserToDeleteRows = false;
            this.m_jobListDataView.AllowUserToOrderColumns = true;
            this.m_jobListDataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_jobListDataView.Location = new Point(18, 19);
            this.m_jobListDataView.Name = "m_jobListDataView";
            this.m_jobListDataView.ReadOnly = true;
            this.m_jobListDataView.Size = new Size(536, 150);
            this.m_jobListDataView.TabIndex = 5;
            this.button1.Location = new Point(18, 175);
            this.button1.Name = "button1";
            this.button1.Size = new Size(145, 50);
            this.button1.TabIndex = 6;
            this.button1.Text = "Suspend Selected";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click_1);
            this.button3.Location = new Point(409, 175);
            this.button3.Name = "button3";
            this.button3.Size = new Size(145, 50);
            this.button3.TabIndex = 7;
            this.button3.Text = "Resume Selected";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button4.Location = new Point(400, 284);
            this.button4.Name = "button4";
            this.button4.Size = new Size(150, 54);
            this.button4.TabIndex = 8;
            this.button4.Tag = (object)"MOVEVEND";
            this.button4.Text = "Move To Vend";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click);
            this.m_jobTimer.Interval = 1000;
            this.m_jobTimer.Tick += new EventHandler(this.timer1_Tick);
            this.m_vendMoveOutput.Location = new Point(556, 318);
            this.m_vendMoveOutput.Name = "m_vendMoveOutput";
            this.m_vendMoveOutput.ReadOnly = true;
            this.m_vendMoveOutput.Size = new Size(139, 20);
            this.m_vendMoveOutput.TabIndex = 9;
            this.groupBox1.Controls.Add((Control)this.m_jobListDataView);
            this.groupBox1.Controls.Add((Control)this.button1);
            this.groupBox1.Controls.Add((Control)this.button3);
            this.groupBox1.Location = new Point(400, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(569, 240);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Job Control";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(990, 520);
            this.Controls.Add((Control)this.groupBox1);
            this.Controls.Add((Control)this.m_vendMoveOutput);
            this.Controls.Add((Control)this.button4);
            this.Controls.Add((Control)this.m_saveConfigButton);
            this.Controls.Add((Control)this.button2);
            this.Controls.Add((Control)this.m_exitButton);
            this.Controls.Add((Control)this.panel1);
            this.Name = nameof(AdvancedMode);
            this.Text = nameof(AdvancedMode);
            ((ISupportInitialize)this.m_jobListDataView).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
