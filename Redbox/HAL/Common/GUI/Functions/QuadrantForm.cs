using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Redbox.HAL.Common.GUI.Functions
{
    public class QuadrantForm : Form
    {
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox m_offsetTextBox;
        private TextBox m_startSlotTextBox;
        private TextBox m_endSlotTextBox;
        private ErrorProvider m_errorProvider;
        private Button m_okButton;
        private Button m_cancelButton;

        public QuadrantForm() => this.InitializeComponent();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? Offset
        {
            get
            {
                int result;
                return int.TryParse(this.m_offsetTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_offsetTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_offsetTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? StartSlot
        {
            get
            {
                int result;
                return int.TryParse(this.m_startSlotTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_startSlotTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_startSlotTextBox.Text = value.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int? EndSlot
        {
            get
            {
                int result;
                return int.TryParse(this.m_endSlotTextBox.Text, out result) ? new int?(result) : new int?();
            }
            set
            {
                this.m_endSlotTextBox.Clear();
                if (!value.HasValue)
                    return;
                this.m_endSlotTextBox.Text = value.ToString();
            }
        }

        private void OnOK(object sender, EventArgs e)
        {
            this.m_errorProvider.SetError((Control)this.m_offsetTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_startSlotTextBox, string.Empty);
            this.m_errorProvider.SetError((Control)this.m_endSlotTextBox, string.Empty);
            if (this.Offset.HasValue)
            {
                int? offset = this.Offset;
                int num = 0;
                if (!(offset.GetValueOrDefault() < num & offset.HasValue))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            this.m_errorProvider.SetError((Control)this.m_offsetTextBox, "Offset is a required value and must be a postive integer value.");
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
            this.label2 = new Label();
            this.label3 = new Label();
            this.m_offsetTextBox = new TextBox();
            this.m_startSlotTextBox = new TextBox();
            this.m_endSlotTextBox = new TextBox();
            this.m_errorProvider = new ErrorProvider(this.components);
            this.m_okButton = new Button();
            this.m_cancelButton = new Button();
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Offset:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Start Slot:";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 75);
            this.label3.Name = "label3";
            this.label3.Size = new Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "End Slot:";
            this.m_offsetTextBox.Location = new Point(75, 12);
            this.m_offsetTextBox.Name = "m_offsetTextBox";
            this.m_offsetTextBox.Size = new Size(100, 20);
            this.m_offsetTextBox.TabIndex = 1;
            this.m_startSlotTextBox.Location = new Point(75, 40);
            this.m_startSlotTextBox.Name = "m_startSlotTextBox";
            this.m_startSlotTextBox.Size = new Size(100, 20);
            this.m_startSlotTextBox.TabIndex = 3;
            this.m_endSlotTextBox.Location = new Point(75, 72);
            this.m_endSlotTextBox.Name = "m_endSlotTextBox";
            this.m_endSlotTextBox.Size = new Size(100, 20);
            this.m_endSlotTextBox.TabIndex = 5;
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.m_okButton.Location = new Point(15, 107);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(75, 23);
            this.m_okButton.TabIndex = 6;
            this.m_okButton.Text = "OK";
            this.m_okButton.UseVisualStyleBackColor = true;
            this.m_okButton.Click += new EventHandler(this.OnOK);
            this.m_cancelButton.DialogResult = DialogResult.Cancel;
            this.m_cancelButton.Location = new Point(133, 108);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new Size(75, 23);
            this.m_cancelButton.TabIndex = 7;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new EventHandler(this.OnCancel);
            this.AcceptButton = (IButtonControl)this.m_okButton;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = (IButtonControl)this.m_cancelButton;
            this.ClientSize = new Size(220, 142);
            this.ControlBox = false;
            this.Controls.Add((Control)this.m_cancelButton);
            this.Controls.Add((Control)this.m_okButton);
            this.Controls.Add((Control)this.m_endSlotTextBox);
            this.Controls.Add((Control)this.m_startSlotTextBox);
            this.Controls.Add((Control)this.m_offsetTextBox);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Name = nameof(QuadrantForm);
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Deck Quadrant";
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
