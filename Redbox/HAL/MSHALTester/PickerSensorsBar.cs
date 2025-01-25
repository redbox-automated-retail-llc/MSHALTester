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
    public class PickerSensorsBar : UserControl
    {
        private readonly Dictionary<int, Panel> m_sensors = new Dictionary<int, Panel>();
        private readonly ButtonAspectsManager ButtonManager;
        private IContainer components;
        private Panel m_sensor1Panel;
        private Panel m_sensor2Panel;
        private Panel m_sensor3Panel;
        private Panel m_sensor4Panel;
        private Panel m_sensor5Panel;
        private Panel m_sensor6Panel;
        private Button m_readButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button m_onButton;
        private Button m_offButton;

        public PickerSensorsBar(HardwareService service)
        {
            this.InitializeComponent();
            this.m_sensors[0] = this.m_sensor1Panel;
            this.m_sensors[1] = this.m_sensor2Panel;
            this.m_sensors[2] = this.m_sensor3Panel;
            this.m_sensors[3] = this.m_sensor4Panel;
            this.m_sensors[4] = this.m_sensor5Panel;
            this.m_sensors[5] = this.m_sensor6Panel;
            this.Service = service;
            this.ButtonManager = new ButtonAspectsManager();
        }

        public event PickerSensorsBar.SensorOperationHandler ReadEvents;

        public event PickerSensorsBar.BarToggle BarEvents;

        public HardwareService Service { get; private set; }

        private void ExecuteBarInstruction(object sender, EventArgs e)
        {
            using (ButtonAspects buttonAspects = this.ButtonManager.MakeAspect(sender))
            {
                string tagInstruction = buttonAspects.GetTagInstruction();
                using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                {
                    IControlResponse response = instructionHelper.ExecuteWithResponse(tagInstruction);
                    if (this.BarEvents == null)
                        return;
                    this.BarEvents(tagInstruction, response);
                }
            }
        }

        private void Read_Click(object sender, EventArgs e)
        {
            using (this.ButtonManager.MakeAspect(sender))
            {
                bool readError = false;
                try
                {
                    IPickerSensorReadResult result = ServiceLocator.Instance.GetService<IControlSystem>().ReadPickerSensors(false);
                    readError = result.Error != 0;
                    if (readError)
                        return;
                    int i = 0;
                    result.Foreach((Action<PickerInputs>)(each => this.m_sensors[i++].BackColor = result.IsInputActive(each) ? Color.Green : Color.WhiteSmoke));
                }
                finally
                {
                    if (this.ReadEvents != null)
                        this.ReadEvents(readError);
                }
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
            this.m_sensor1Panel = new Panel();
            this.m_sensor2Panel = new Panel();
            this.m_sensor3Panel = new Panel();
            this.m_sensor4Panel = new Panel();
            this.m_sensor5Panel = new Panel();
            this.m_sensor6Panel = new Panel();
            this.m_readButton = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.m_onButton = new Button();
            this.m_offButton = new Button();
            this.SuspendLayout();
            this.m_sensor1Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor1Panel.Enabled = false;
            this.m_sensor1Panel.Location = new Point(14, 16);
            this.m_sensor1Panel.Name = "m_sensor1Panel";
            this.m_sensor1Panel.Size = new Size(40, 35);
            this.m_sensor1Panel.TabIndex = 0;
            this.m_sensor2Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor2Panel.Location = new Point(70, 16);
            this.m_sensor2Panel.Name = "m_sensor2Panel";
            this.m_sensor2Panel.Size = new Size(40, 35);
            this.m_sensor2Panel.TabIndex = 1;
            this.m_sensor3Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor3Panel.Location = new Point((int)sbyte.MaxValue, 16);
            this.m_sensor3Panel.Name = "m_sensor3Panel";
            this.m_sensor3Panel.Size = new Size(40, 35);
            this.m_sensor3Panel.TabIndex = 2;
            this.m_sensor4Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor4Panel.Location = new Point(184, 16);
            this.m_sensor4Panel.Name = "m_sensor4Panel";
            this.m_sensor4Panel.Size = new Size(40, 35);
            this.m_sensor4Panel.TabIndex = 3;
            this.m_sensor5Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor5Panel.Location = new Point(240, 16);
            this.m_sensor5Panel.Name = "m_sensor5Panel";
            this.m_sensor5Panel.Size = new Size(40, 35);
            this.m_sensor5Panel.TabIndex = 4;
            this.m_sensor6Panel.BorderStyle = BorderStyle.FixedSingle;
            this.m_sensor6Panel.Location = new Point(295, 16);
            this.m_sensor6Panel.Name = "m_sensor6Panel";
            this.m_sensor6Panel.Size = new Size(40, 35);
            this.m_sensor6Panel.TabIndex = 5;
            this.m_readButton.BackColor = Color.LightGray;
            this.m_readButton.Location = new Point(14, 57);
            this.m_readButton.Name = "m_readButton";
            this.m_readButton.Size = new Size(75, 45);
            this.m_readButton.TabIndex = 6;
            this.m_readButton.Text = "Read";
            this.m_readButton.UseVisualStyleBackColor = false;
            this.m_readButton.Click += new EventHandler(this.Read_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(30, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(13, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "1";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(86, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(13, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "2";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(140, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(13, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "3";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(198, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(13, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "4";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
            this.label5.AutoSize = true;
            this.label5.Location = new Point((int)byte.MaxValue, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(13, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "5";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(311, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(13, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "6";
            this.label6.TextAlign = ContentAlignment.MiddleCenter;
            this.m_onButton.BackColor = Color.LightGray;
            this.m_onButton.Location = new Point((int)sbyte.MaxValue, 57);
            this.m_onButton.Name = "m_onButton";
            this.m_onButton.Size = new Size(75, 45);
            this.m_onButton.TabIndex = 15;
            this.m_onButton.Tag = (object)"CONTROLSYSTEM PICKERSENSORSON";
            this.m_onButton.Text = "On";
            this.m_onButton.UseVisualStyleBackColor = false;
            this.m_onButton.Click += new EventHandler(this.ExecuteBarInstruction);
            this.m_offButton.BackColor = Color.LightGray;
            this.m_offButton.Location = new Point(240, 57);
            this.m_offButton.Name = "m_offButton";
            this.m_offButton.Size = new Size(75, 45);
            this.m_offButton.TabIndex = 16;
            this.m_offButton.Tag = (object)"CONTROLSYSTEM PICKERSENSORSOFF";
            this.m_offButton.Text = "Off";
            this.m_offButton.UseVisualStyleBackColor = false;
            this.m_offButton.Click += new EventHandler(this.ExecuteBarInstruction);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add((Control)this.m_offButton);
            this.Controls.Add((Control)this.m_onButton);
            this.Controls.Add((Control)this.label6);
            this.Controls.Add((Control)this.label5);
            this.Controls.Add((Control)this.label4);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.m_readButton);
            this.Controls.Add((Control)this.m_sensor6Panel);
            this.Controls.Add((Control)this.m_sensor5Panel);
            this.Controls.Add((Control)this.m_sensor4Panel);
            this.Controls.Add((Control)this.m_sensor3Panel);
            this.Controls.Add((Control)this.m_sensor2Panel);
            this.Controls.Add((Control)this.m_sensor1Panel);
            this.Name = nameof(PickerSensorsBar);
            this.Size = new Size(349, 113);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public delegate void SensorOperationHandler(bool readError);

        public delegate void BarToggle(string instruction, IControlResponse response);
    }
}
