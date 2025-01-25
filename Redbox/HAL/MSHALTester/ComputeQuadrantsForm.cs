using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class ComputeQuadrantsForm : Form
    {
        private IContainer components;
        private Label label1;
        private TextBox m_startOffsetTextBox;
        private Label label2;
        private TextBox m_numberOfQuadrantsTextBox;
        private ErrorProvider m_errorProvider;
        private Button m_okButton;
        private Button m_cancelButton;
        private TextBox m_slotsPerQuadrantTextBox;
        private Label label3;

        public ComputeQuadrantsForm() => this.InitializeComponent();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Decimal? StartOffset
        {
            get
            {
                Decimal result;
                return Decimal.TryParse(this.m_startOffsetTextBox.Text, out result) ? new Decimal?(result) : new Decimal?();
            }
            set
            {
                this.m_startOffsetTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_startOffsetTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? NumberOfQuadrants
        {
            get
            {
                int result;
                return int.TryParse(this.m_numberOfQuadrantsTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_numberOfQuadrantsTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_numberOfQuadrantsTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? SlotsPerQuadrant
        {
            get
            {
                int result;
                return int.TryParse(this.m_slotsPerQuadrantTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_slotsPerQuadrantTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_slotsPerQuadrantTextBox.Text = value.ToString();
            }
        }

        private void OnOK(object sender, EventArgs e)
        {
            this.m_errorProvider.SetError((Control)this.m_startOffsetTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_numberOfQuadrantsTextBox, string.Empty);
            if (!this.StartOffset.HasValue)
                this.m_errorProvider.SetError((Control)this.m_startOffsetTextBox, "Start Offset is a required value.");
            else if (!this.NumberOfQuadrants.HasValue)
                this.m_errorProvider.SetError((Control)this.m_numberOfQuadrantsTextBox, "Number of Quadrants is a required value.");
            else if (!this.SlotsPerQuadrant.HasValue)
            {
                this.m_errorProvider.SetError((Control)this.m_slotsPerQuadrantTextBox, "Slots per Quadrant is a required value.");
            }
            else
            {
                Decimal? startOffset = this.StartOffset;
                Decimal num1 = 0M;
                if (startOffset.GetValueOrDefault() < num1 & startOffset.HasValue)
                {
                    this.m_errorProvider.SetError((Control)this.m_startOffsetTextBox, "Start Offset must be a positive value.");
                }
                else
                {
                    int? nullable = this.NumberOfQuadrants;
                    int num2 = 0;
                    if (nullable.GetValueOrDefault() < num2 & nullable.HasValue)
                    {
                        this.m_errorProvider.SetError((Control)this.m_numberOfQuadrantsTextBox, "Number of Quadrants must be a positive value.");
                    }
                    else
                    {
                        nullable = this.SlotsPerQuadrant;
                        int num3 = 0;
                        if (nullable.GetValueOrDefault() < num3 & nullable.HasValue)
                        {
                            this.m_errorProvider.SetError((Control)this.m_slotsPerQuadrantTextBox, "Slots per Quadrant must be a positive value.");
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
            this.label1 = new Label();
            this.m_startOffsetTextBox = new TextBox();
            this.label2 = new Label();
            this.m_numberOfQuadrantsTextBox = new TextBox();
            this.m_errorProvider = new ErrorProvider(this.components);
            this.m_okButton = new Button();
            this.m_cancelButton = new Button();
            this.label3 = new Label();
            this.m_slotsPerQuadrantTextBox = new TextBox();
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Offset:";
            this.m_startOffsetTextBox.Location = new Point(129, 9);
            this.m_startOffsetTextBox.Name = "m_startOffsetTextBox";
            this.m_startOffsetTextBox.Size = new Size(100, 20);
            this.m_startOffsetTextBox.TabIndex = 1;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new Size(111, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of Quadrants:";
            this.m_numberOfQuadrantsTextBox.Enabled = false;
            this.m_numberOfQuadrantsTextBox.Location = new Point(129, 34);
            this.m_numberOfQuadrantsTextBox.Name = "m_numberOfQuadrantsTextBox";
            this.m_numberOfQuadrantsTextBox.Size = new Size(100, 20);
            this.m_numberOfQuadrantsTextBox.TabIndex = 3;
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.m_okButton.Location = new Point(85, 100);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(75, 23);
            this.m_okButton.TabIndex = 6;
            this.m_okButton.Text = "OK";
            this.m_okButton.UseVisualStyleBackColor = true;
            this.m_okButton.Click += new EventHandler(this.OnOK);
            this.m_cancelButton.DialogResult = DialogResult.Cancel;
            this.m_cancelButton.Location = new Point(166, 100);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new Size(75, 23);
            this.m_cancelButton.TabIndex = 7;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new EventHandler(this.OnCancel);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new Size(98, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Slots per Quadrant:";
            this.m_slotsPerQuadrantTextBox.Enabled = false;
            this.m_slotsPerQuadrantTextBox.Location = new Point(129, 60);
            this.m_slotsPerQuadrantTextBox.Name = "m_slotsPerQuadrantTextBox";
            this.m_slotsPerQuadrantTextBox.Size = new Size(100, 20);
            this.m_slotsPerQuadrantTextBox.TabIndex = 5;
            this.AcceptButton = (IButtonControl)this.m_okButton;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = (IButtonControl)this.m_cancelButton;
            this.ClientSize = new Size(253, 135);
            this.ControlBox = false;
            this.Controls.Add((Control)this.m_slotsPerQuadrantTextBox);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.m_cancelButton);
            this.Controls.Add((Control)this.m_okButton);
            this.Controls.Add((Control)this.m_numberOfQuadrantsTextBox);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.m_startOffsetTextBox);
            this.Controls.Add((Control)this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = nameof(ComputeQuadrantsForm);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Compute Quadrants";
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
