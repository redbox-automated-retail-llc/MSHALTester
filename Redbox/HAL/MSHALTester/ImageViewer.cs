using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class ImageViewer : Form
    {
        private IContainer components;
        private PictureBox m_picturebox;

        public ImageViewer() => this.InitializeComponent();

        public void DisplayFile(string file)
        {
            if (string.IsNullOrEmpty(file) || !File.Exists(file))
                return;
            this.m_picturebox.Image?.Dispose();
            this.m_picturebox.Image = Image.FromFile(file);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.m_picturebox = new PictureBox();
            ((ISupportInitialize)this.m_picturebox).BeginInit();
            this.SuspendLayout();
            this.m_picturebox.Location = new Point(29, 22);
            this.m_picturebox.Name = "m_picturebox";
            this.m_picturebox.Size = new Size(640, 480);
            this.m_picturebox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.m_picturebox.TabIndex = 0;
            this.m_picturebox.TabStop = false;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(733, 552);
            this.Controls.Add((Control)this.m_picturebox);
            this.Name = nameof(ImageViewer);
            this.Text = nameof(ImageViewer);
            ((ISupportInitialize)this.m_picturebox).EndInit();
            this.ResumeLayout(false);
        }
    }
}
