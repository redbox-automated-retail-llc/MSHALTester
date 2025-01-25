using Redbox.DirectShow;
using Redbox.HAL.Client;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Threading;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class CameraPreviewForm : Form
    {
        private string CurrentImage;
        private PlayerDevice videoDevice;
        private readonly Size ImageSize;
        private readonly ButtonAspectsManager Manager;
        private readonly AutoResetEvent ImageGrabbedWaiter = new AutoResetEvent(false);
        private readonly string ImagesFolder;
        private readonly AtomicFlag SnapFlag = new AtomicFlag();
        private readonly HardwareService Service;
        private IContainer components;
        private VideoSourcePlayer videoSourcePlayer;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox m_numberOfBarcodesBox;
        private TextBox m_decodeTimeBox;
        private TextBox m_barcodeBox;
        private TextBox m_snapStatusBox;
        private TextBox m_secureReadTB;
        private TextBox m_detectedErrorsTB;
        private BackgroundWorker backgroundWorker1;

        public CameraPreviewForm(
          HardwareService service,
          ButtonAspectsManager manager,
          string imageFolder)
        {
            this.InitializeComponent();
            this.Service = service;
            this.ImagesFolder = imageFolder;
            this.ImageSize = new Size(640, 480);
            this.Manager = manager;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            FilterInfoCollection filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (filterInfoCollection == null || filterInfoCollection.Count == 0)
            {
                this.WriteErrorText("Found no devices.");
            }
            else
            {
                this.videoDevice = new PlayerDevice(filterInfoCollection[0].MonikerString, 500, false);
                if (this.videoDevice == null)
                {
                    this.WriteErrorText("Found no devices.");
                }
                else
                {
                    bool flag = false;
                    foreach (VideoCapabilities videoCapability in this.videoDevice.VideoCapabilities)
                    {
                        if (videoCapability.FrameSize == this.ImageSize)
                        {
                            flag = true;
                            this.videoDevice.VideoResolution = videoCapability;
                            break;
                        }
                    }
                    if (!flag)
                        LogHelper.Instance.Log("Unable to find video capability with size {0}w X {1}h", (object)this.Size.Width, (object)this.Size.Height);
                    this.videoDevice.PlayingFinished += new PlayingFinishedEventHandler(this.videoSource_PlayingFinished);
                    this.videoSourcePlayer.NewFrame += new VideoSourcePlayer.NewFrameHandler(this.SnapFrame);
                    this.videoSourcePlayer.VideoSource = (IVideoSource)this.videoDevice;
                    this.videoSourcePlayer.Start();
                    Thread.Sleep(5000);
                    this.ResetTextBoxes();
                }
            }
        }

        private void videoSource_PlayingFinished(object sender, ReasonToFinishPlaying reason)
        {
            if (ReasonToFinishPlaying.StoppedByUser == reason)
                return;
            string msg = reason.ToString();
            LogHelper.Instance.Log(msg);
            this.WriteErrorText(msg);
        }

        private void SnapFrame(object sender, ref Bitmap image)
        {
            if (!this.SnapFlag.Clear())
                return;
            DateTime now = DateTime.Now;
            this.CurrentImage = Path.Combine(this.ImagesFolder, string.Format("m{0}d{1}y{2}_h{3}m{4}s{5}t{6}.jpg", (object)now.Month, (object)now.Day, (object)now.Year, (object)now.Hour, (object)now.Minute, (object)now.Second, (object)now.Millisecond));
            LogHelper.Instance.Log("Snapframe called captured image {0}", (object)this.CurrentImage);
            image.Save(this.CurrentImage);
            this.ImageGrabbedWaiter.Set();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.BackColor = Color.Red;
            this.button1.Enabled = false;
            this.ResetTextBoxes();
            this.SnapFlag.Set();
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.videoDevice == null)
                return;
            this.videoDevice.DisplayPropertyPage(Process.GetCurrentProcess().MainWindowHandle);
        }

        private void button3_Click(object sender, EventArgs e) => this.WriteErrorText(string.Empty);

        private void button4_Click(object sender, EventArgs e)
        {
            this.ShutdownPlayer();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ResetTextBoxes()
        {
            this.m_barcodeBox.Text = string.Empty;
            this.m_decodeTimeBox.Text = string.Empty;
            this.m_numberOfBarcodesBox.Text = string.Empty;
            this.m_snapStatusBox.Text = string.Empty;
            this.m_secureReadTB.Text = string.Empty;
            Application.DoEvents();
        }

        private void ShutdownPlayer()
        {
            try
            {
                if (this.videoSourcePlayer == null)
                    return;
                this.videoSourcePlayer.SignalToStop();
                this.videoSourcePlayer.WaitForStop();
                if (this.videoDevice != null)
                {
                    this.videoSourcePlayer.NewFrame -= new VideoSourcePlayer.NewFrameHandler(this.SnapFrame);
                    this.videoDevice.PlayingFinished -= new PlayingFinishedEventHandler(this.videoSource_PlayingFinished);
                }
                this.videoSourcePlayer.VideoSource = (IVideoSource)(this.videoDevice = (PlayerDevice)null);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("[CameraPreview] OnShutdown: caught an exception", ex);
            }
        }

        private void WriteErrorText(string msg)
        {
            if (this.m_detectedErrorsTB.InvokeRequired)
                this.Invoke((Delegate)new CameraPreviewForm.SetTextCallback(this.WriteErrorText), (object)msg);
            else
                this.m_detectedErrorsTB.Text = msg;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool flag = false;
            using (new AtomicFlagHelper(this.SnapFlag))
                flag = this.ImageGrabbedWaiter.WaitOne(5000);
            if (!flag || !File.Exists(this.CurrentImage))
            {
                e.Result = (object)ScanResult.New();
            }
            else
            {
                using (DecodeExecutor decodeExecutor = new DecodeExecutor(this.Service, Path.GetFileName(this.CurrentImage)))
                {
                    decodeExecutor.Run();
                    e.Result = (object)decodeExecutor.ScanResult;
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button1.BackColor = Color.LightGray;
            this.button1.Enabled = true;
            ScanResult result = e.Result as ScanResult;
            this.m_snapStatusBox.Text = !result.SnapOk ? "CAPTURE ERROR" : "SUCCESS";
            this.m_numberOfBarcodesBox.Text = result.ReadCount.ToString();
            this.m_decodeTimeBox.Text = result.ExecutionTime;
            this.m_barcodeBox.Text = result.ScannedMatrix;
            this.m_secureReadTB.Text = result.SecureCount.ToString();
            if (File.Exists(this.CurrentImage))
            {
                try
                {
                    File.Delete(this.CurrentImage);
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log(string.Format("Unable to delete image '{0}'", (object)this.CurrentImage), ex);
                }
            }
            this.CurrentImage = (string)null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.button4 = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.m_numberOfBarcodesBox = new TextBox();
            this.m_decodeTimeBox = new TextBox();
            this.m_barcodeBox = new TextBox();
            this.m_snapStatusBox = new TextBox();
            this.m_secureReadTB = new TextBox();
            this.m_detectedErrorsTB = new TextBox();
            this.videoSourcePlayer = new VideoSourcePlayer();
            this.backgroundWorker1 = new BackgroundWorker();
            this.SuspendLayout();
            this.button1.BackColor = Color.LightGray;
            this.button1.Location = new Point(24, 498);
            this.button1.Name = "button1";
            this.button1.Size = new Size(125, 60);
            this.button1.TabIndex = 1;
            this.button1.Text = "Snap and Decode";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.BackColor = Color.LightGray;
            this.button2.Location = new Point(24, 574);
            this.button2.Name = "button2";
            this.button2.Size = new Size(125, 60);
            this.button2.TabIndex = 2;
            this.button2.Text = "Camera Properties";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.BackColor = Color.LightGray;
            this.button3.Location = new Point(542, 553);
            this.button3.Name = "button3";
            this.button3.Size = new Size(125, 45);
            this.button3.TabIndex = 3;
            this.button3.Text = "Clear errors box";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button4.BackColor = Color.GreenYellow;
            this.button4.Location = new Point(542, 604);
            this.button4.Name = "button4";
            this.button4.Size = new Size(125, 60);
            this.button4.TabIndex = 4;
            this.button4.Text = "Exit";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new EventHandler(this.button4_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(181, 508);
            this.label1.Name = "label1";
            this.label1.Size = new Size(89, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number of Codes";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(181, 537);
            this.label2.Name = "label2";
            this.label2.Size = new Size(71, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Decode Time";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(181, 566);
            this.label3.Name = "label3";
            this.label3.Size = new Size(47, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Barcode";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(181, 600);
            this.label4.Name = "label4";
            this.label4.Size = new Size(62, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "SnapStatus";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(181, 635);
            this.label5.Name = "label5";
            this.label5.Size = new Size(108, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Found Secure Code?";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(531, 503);
            this.label6.Name = "label6";
            this.label6.Size = new Size(81, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Detected Errors";
            this.m_numberOfBarcodesBox.Location = new Point(306, 505);
            this.m_numberOfBarcodesBox.Name = "m_numberOfBarcodesBox";
            this.m_numberOfBarcodesBox.Size = new Size(100, 20);
            this.m_numberOfBarcodesBox.TabIndex = 11;
            this.m_decodeTimeBox.Location = new Point(306, 534);
            this.m_decodeTimeBox.Name = "m_decodeTimeBox";
            this.m_decodeTimeBox.Size = new Size(100, 20);
            this.m_decodeTimeBox.TabIndex = 12;
            this.m_barcodeBox.Location = new Point(306, 566);
            this.m_barcodeBox.Name = "m_barcodeBox";
            this.m_barcodeBox.Size = new Size(100, 20);
            this.m_barcodeBox.TabIndex = 13;
            this.m_snapStatusBox.Location = new Point(306, 600);
            this.m_snapStatusBox.Name = "m_snapStatusBox";
            this.m_snapStatusBox.Size = new Size(100, 20);
            this.m_snapStatusBox.TabIndex = 14;
            this.m_secureReadTB.Location = new Point(306, 628);
            this.m_secureReadTB.Name = "m_secureReadTB";
            this.m_secureReadTB.Size = new Size(100, 20);
            this.m_secureReadTB.TabIndex = 15;
            this.m_detectedErrorsTB.Location = new Point(542, 527);
            this.m_detectedErrorsTB.Name = "m_detectedErrorsTB";
            this.m_detectedErrorsTB.Size = new Size(122, 20);
            this.m_detectedErrorsTB.TabIndex = 16;
            this.videoSourcePlayer.Location = new Point(24, 12);
            this.videoSourcePlayer.Name = "videoSourcePlayer";
            this.videoSourcePlayer.Size = new Size(640, 480);
            this.videoSourcePlayer.TabIndex = 0;
            this.videoSourcePlayer.Text = "videoSourcePlayer";
            this.videoSourcePlayer.VideoSource = (IVideoSource)null;
            this.backgroundWorker1.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.DimGray;
            this.ClientSize = new Size(724, 694);
            this.Controls.Add((Control)this.m_detectedErrorsTB);
            this.Controls.Add((Control)this.m_secureReadTB);
            this.Controls.Add((Control)this.m_snapStatusBox);
            this.Controls.Add((Control)this.m_barcodeBox);
            this.Controls.Add((Control)this.m_decodeTimeBox);
            this.Controls.Add((Control)this.m_numberOfBarcodesBox);
            this.Controls.Add((Control)this.label6);
            this.Controls.Add((Control)this.label5);
            this.Controls.Add((Control)this.label4);
            this.Controls.Add((Control)this.label3);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.button4);
            this.Controls.Add((Control)this.button3);
            this.Controls.Add((Control)this.button2);
            this.Controls.Add((Control)this.button1);
            this.Controls.Add((Control)this.videoSourcePlayer);
            this.Name = nameof(CameraPreviewForm);
            this.Text = nameof(CameraPreviewForm);
            this.Load += new EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private delegate void SetTextCallback(string msg);
    }
}
