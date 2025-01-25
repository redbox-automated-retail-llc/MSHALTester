using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class ConfirmationDialog : Form
    {
        private string m_confirmText;
        private IContainer components;
        private TextBox m_confirmationBox;
        private Label m_label1;
        private Button m_okButton;
        private Button m_cancelButton;
        private ErrorProvider m_errorProvider;

        public ConfirmationDialog(string confirmText)
        {
            this.InitializeComponent();
            this.Setup(confirmText);
        }

        public ConfirmationDialog()
        {
            this.InitializeComponent();
            this.Setup("test");
        }

        private void Setup(string confirmText)
        {
            this.m_confirmText = string.IsNullOrEmpty(confirmText) ? "test" : confirmText;
            this.m_label1.Text = string.Format("Enter the word '{0}' (without quotes)", (object)this.m_confirmText);
        }

        private void m_okButton_Click(object sender, EventArgs e)
        {
            this.m_errorProvider.Clear();
            if (string.IsNullOrEmpty(this.m_confirmationBox.Text))
                this.m_errorProvider.SetError((Control)this.m_confirmationBox, "Enter a confirmation.");
            else if (!this.m_confirmationBox.Text.Equals(this.m_confirmText, StringComparison.CurrentCultureIgnoreCase))
            {
                this.m_errorProvider.SetError((Control)this.m_confirmationBox, "Enter the correct confirmation.");
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void m_cancelButton_Click(object sender, EventArgs e)
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
            this.m_confirmationBox = new TextBox();
            this.m_label1 = new Label();
            this.m_okButton = new Button();
            this.m_cancelButton = new Button();
            this.m_errorProvider = new ErrorProvider(this.components);
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            this.SuspendLayout();
            this.m_confirmationBox.Location = new Point(26, 52);
            this.m_confirmationBox.Name = "m_confirmationBox";
            this.m_confirmationBox.Size = new Size(197, 20);
            this.m_confirmationBox.TabIndex = 0;
            this.m_label1.AutoSize = true;
            this.m_label1.Location = new Point(23, 26);
            this.m_label1.Name = "m_label1";
            this.m_label1.Size = new Size(202, 13);
            this.m_label1.TabIndex = 1;
            this.m_label1.Text = "Enter the word 'test' (without the quotes) :";
            this.m_okButton.Location = new Point(26, 96);
            this.m_okButton.Name = "m_okButton";
            this.m_okButton.Size = new Size(105, 60);
            this.m_okButton.TabIndex = 2;
            this.m_okButton.Text = "Ok";
            this.m_okButton.UseVisualStyleBackColor = true;
            this.m_okButton.Click += new EventHandler(this.m_okButton_Click);
            this.m_cancelButton.Location = new Point(158, 96);
            this.m_cancelButton.Name = "m_cancelButton";
            this.m_cancelButton.Size = new Size(105, 60);
            this.m_cancelButton.TabIndex = 3;
            this.m_cancelButton.Text = "Cancel";
            this.m_cancelButton.UseVisualStyleBackColor = true;
            this.m_cancelButton.Click += new EventHandler(this.m_cancelButton_Click);
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(298, 186);
            this.Controls.Add((Control)this.m_cancelButton);
            this.Controls.Add((Control)this.m_okButton);
            this.Controls.Add((Control)this.m_label1);
            this.Controls.Add((Control)this.m_confirmationBox);
            this.Name = nameof(ConfirmationDialog);
            this.Text = nameof(ConfirmationDialog);
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
