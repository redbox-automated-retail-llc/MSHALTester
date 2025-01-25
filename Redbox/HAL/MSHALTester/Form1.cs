using log4net;
using Redbox.HAL.Client;
using Redbox.HAL.Client.Executors;
using Redbox.HAL.Client.Services;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Extensions;
using Redbox.HAL.Component.Model.Services;
using Redbox.HAL.Core;
using Redbox.HAL.MSHALTester.Properties;
using Redbox.IPC.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class Form1 : Form, IDisposable, IClientOutputSink, ILogger
    {
        private ImageViewer ImageViewer;
        private bool RinglightOn;
        private bool FormDisposed;
        private ButtonAspectsManager Manager;
        private bool VMZConfigured;
        private bool DoorSensorsConfigured;
        private RadioButton m_offsetGetRadioButton;
        private readonly List<HardwareJob> m_stoppedJobs = new List<HardwareJob>();
        private readonly CameraAdapter CameraHelper;
        private readonly OutputBox OutputBox;
        private readonly string Version;
        private readonly AirExchangerState AirExchangerState;
        private readonly ILog m_log;
        private readonly bool m_secure;
        private readonly ClientHelper Helper;
        private readonly HardwareService HardwareService;
        private readonly MoveHelper MoveHelper;
        private readonly Button[] FormButtons;
        private const int MaxMoveXUnits = 10;
        private const int MaxMoveYUnits = 200;
        private IContainer components;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private GroupBox m_qlmGroupBox;
        private GroupBox groupBox10;
        private GroupBox groupBox11;
        private GroupBox groupBox12;
        private TextBox m_encoderUnitsTextBox;
        private Button button1;
        private TextBox m_sourceSlotTextBox;
        private TextBox m_sourceDeckTextBox;
        private Button m_readPosButton;
        private Button button4;
        private Button button3;
        private Button button2;
        private Button button18;
        private Button button17;
        private Button button16;
        private Button button14;
        private Button button13;
        private Button button12;
        private Button button11;
        private Button button10;
        private Button button8;
        private Button button7;
        private Button m_qlmDoorStatus;
        private Button m_qlmCaseStatusButton;
        private Button m_qlmStopButton;
        private Button m_qlmDisengageButton;
        private Button m_qlmEngageButton;
        private Button button39;
        private Button button38;
        private Button button37;
        private Button button36;
        private Button button35;
        private Button button34;
        private Button m_gripperSellButton;
        private Button button32;
        private Button button31;
        private Button button30;
        private Button button29;
        private Button button28;
        private Button m_cameraPreview;
        private Button button26;
        private Button m_startButtonCamera;
        private Button m_snapImageButton;
        private Button m_homeYButton;
        private Button m_homeXButton;
        private Button button44;
        private Button m_runInitButton;
        private Button button50;
        private Button button47;
        private Button button54;
        private Button button53;
        private Button button51;
        private TextBox m_rollToSensorText;
        private ListBox m_outputBox;
        private Button m_clearOutputButton;
        private Label label11;
        private Label label10;
        private TextBox m_destinationSlotText;
        private TextBox m_destinationDeckText;
        private Label label14;
        private Button m_qlmDownButton;
        private Button m_qlmUpButton;
        private Button m_updateConfigurationButton;
        private Label label16;
        private Label label15;
        private TextBox m_rollerTimeoutTextBox;
        private Label label17;
        private TextBox m_versionTextBox;
        private Label label19;
        private Label label20;
        private TextBox m_kioskIDTextBox;
        private GroupBox groupBox1;
        private Button button57;
        private Button button5;
        private ErrorProvider m_errorProvider;
        private Button m_deckConfigurationButton;
        private Button button63;
        private Button button62;
        private Button button61;
        private Button button60;
        private Button button64;
        private Button m_takeDiskButton;
        private CheckBox m_sensorCheckBox;
        private Button m_openLogsButton;
        private Button m_configureDevicesButton;
        private Button button66;
        private Button m_resetProteusButton;
        private Button button19;
        private Button button20;
        private Button m_cameraWorkingButton;
        private Button m_readLimitsButton;
        private Button m_showTimeoutLogButton;
        private Button button22;
        private Button m_qlmTestButton;
        private Button m_closeSerialButton;
        private Button m_fanButton;
        private Button m_testArcusCommButton;
        private Button m_getWithOffset;
        private RadioButton m_getOffXRadio;
        private RadioButton m_getOffYRadio;
        private TextBox m_getOffsetTB;
        private BackgroundWorker m_startupWorker;
        private BackgroundWorker m_initWorker;

        public void WriteMessage(string msg) => this.OutputBox.Write(msg);

        public void WriteMessage(string fmt, params object[] stuff) => this.OutputBox.Write(fmt, stuff);

        public void Log(string message, Exception e) => this.m_log.Error((object)message, e);

        public void Log(string message, LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.Info:
                    this.m_log.Info((object)message);
                    break;
                case LogEntryType.Debug:
                    this.m_log.Debug((object)message);
                    break;
                case LogEntryType.Error:
                    this.m_log.Error((object)message);
                    break;
                case LogEntryType.Fatal:
                    this.m_log.Fatal((object)message);
                    break;
            }
        }

        public void Log(string message, Exception e, LogEntryType type)
        {
            switch (type)
            {
                case LogEntryType.Info:
                    this.m_log.Info((object)message, e);
                    break;
                case LogEntryType.Debug:
                    this.m_log.Debug((object)message, e);
                    break;
                case LogEntryType.Error:
                    this.m_log.Error((object)message, e);
                    break;
                case LogEntryType.Fatal:
                    this.m_log.Fatal((object)message, e);
                    break;
            }
        }

        public bool IsLevelEnabled(LogEntryType entryLogLevel)
        {
            switch (entryLogLevel)
            {
                case LogEntryType.Info:
                    return this.m_log.IsInfoEnabled;
                case LogEntryType.Debug:
                    return this.m_log.IsDebugEnabled;
                case LogEntryType.Error:
                    return this.m_log.IsErrorEnabled;
                case LogEntryType.Fatal:
                    return this.m_log.IsFatalEnabled;
                default:
                    return false;
            }
        }

        public Form1(bool secure, string user)
        {
            this.InitializeComponent();
            this.Version = typeof(Form1).Assembly.GetName().Version.ToString();
            TesterSession user1 = new TesterSession(user);
            ServiceLocator.Instance.AddService<ISessionUserService>((object)new TesterSessionImplemtation((ISessionUser)user1));
            ServiceLocator.Instance.AddService<IDeviceSetupClassFactory>((object)new DeviceSetupClassFactory());
            ServiceLocator.Instance.AddService<IUsbDeviceService>((object)new UsbDeviceService(Settings.Default.UsbServiceDebug));
            this.OutputBox = (OutputBox)new TesterOutputBox(this.m_outputBox, this);
            this.m_log = LogManager.GetLogger("TesterLog");
            this.m_secure = secure;
            this.FormButtons = new Button[63]
            {
        this.m_clearOutputButton,
        this.m_readPosButton,
        this.m_runInitButton,
        this.m_homeXButton,
        this.m_homeYButton,
        this.m_resetProteusButton,
        this.m_testArcusCommButton,
        this.button57,
        this.button5,
        this.button47,
        this.button50,
        this.button51,
        this.button53,
        this.button54,
        this.m_qlmTestButton,
        this.button22,
        this.button66,
        this.button20,
        this.m_showTimeoutLogButton,
        this.m_readLimitsButton,
        this.button36,
        this.button37,
        this.button38,
        this.button39,
        this.button64,
        this.button31,
        this.button32,
        this.m_gripperSellButton,
        this.button34,
        this.button35,
        this.button28,
        this.button29,
        this.button30,
        this.m_qlmEngageButton,
        this.m_qlmDisengageButton,
        this.m_qlmStopButton,
        this.m_qlmCaseStatusButton,
        this.m_qlmUpButton,
        this.m_qlmDownButton,
        this.m_qlmDoorStatus,
        this.m_snapImageButton,
        this.m_startButtonCamera,
        this.m_cameraPreview,
        this.m_cameraWorkingButton,
        this.button19,
        this.button26,
        this.button7,
        this.button8,
        this.button10,
        this.button11,
        this.button12,
        this.button13,
        this.button16,
        this.button14,
        this.button17,
        this.button18,
        this.button1,
        this.button2,
        this.button3,
        this.button4,
        this.m_closeSerialButton,
        this.m_getWithOffset,
        this.m_takeDiskButton
            };
            this.HardwareService = new HardwareService(IPCProtocol.Parse(Settings.Default.CommunicationURL));
            this.Helper = new ClientHelper((IClientOutputSink)this, this.HardwareService);
            this.AirExchangerState = new AirExchangerState(this.HardwareService);
            this.CameraHelper = new CameraAdapter(this.HardwareService);
            this.MoveHelper = new MoveHelper(this.HardwareService);
            ServiceLocator.Instance.AddService<IControlSystem>((object)new ClientControlSystem(this.HardwareService));
            if (Settings.Default.TestCommOnStartup && !this.Helper.TestCommunication())
            {
                this.OutputBox.Write("Failed to communicate with HAL.");
                LogHelper.Instance.Log("Unable to contact service on {0}", (object)Settings.Default.CommunicationURL);
            }
            else
            {
                this.m_offsetGetRadioButton = this.m_getOffXRadio;
                this.m_offsetGetRadioButton.Checked = true;
                ServiceLocator.Instance.AddService<IRuntimeService>((object)new RuntimeService());
                ServiceLocator.Instance.AddService<ILogger>((object)this);
                LogHelper.Instance.Log("Tester startup; form {0} in secure mode, user = {1}.", this.m_secure ? (object)"is" : (object)"isn't", (object)user1.User);
                this.FormClosing += new FormClosingEventHandler(this.OnFormClosing);
                PickerSensorsBar pickerSensorsBar = new PickerSensorsBar(this.HardwareService);
                pickerSensorsBar.Dock = DockStyle.Bottom;
                this.groupBox5.Controls.Add((Control)pickerSensorsBar);
                pickerSensorsBar.ReadEvents += new PickerSensorsBar.SensorOperationHandler(this.OnSensorReadDone);
                pickerSensorsBar.BarEvents += new PickerSensorsBar.BarToggle(this.OnSensorOperation);
                this.Manager = new ButtonAspectsManager();
                this.m_versionTextBox.Text = this.Version;
                string id = "UNKNOWN";
                if (!this.HardwareService.GetKioskID(out id).Success)
                    id = "UNKNOWN";
                this.m_kioskIDTextBox.Text = id;
                this.m_startupWorker.RunWorkerAsync();
            }
        }

        private void OnSensorReadDone(bool readError)
        {
            this.OutputBox.Write(readError ? "Sensor Read Failure: PCB Not Responsive" : "Sensors read.");
        }

        private void OnSensorOperation(string msg, IControlResponse response)
        {
            string str = !response.CommError ? (response.TimedOut ? ErrorCodes.Timeout.ToString().ToUpper() : ErrorCodes.Success.ToString().ToUpper()) : response.Diagnostic;
            this.OutputBox.Write(string.Format("{0}: {1}", (object)msg, (object)str));
        }

        private void ChangeUIAfterconfig()
        {
            using (MachineConfiguration machineConfiguration = new MachineConfiguration(this.HardwareService))
            {
                machineConfiguration.Run();
                this.VMZConfigured = machineConfiguration.VMZConfigured;
                this.DoorSensorsConfigured = machineConfiguration.DoorSensorsConfigured;
                if (this.VMZConfigured)
                    this.OutputBox.Write("The kiosk is configured for a VMZ.");
                if (this.DoorSensorsConfigured)
                {
                    this.OutputBox.Write("The kiosk is configured for door sensors.");
                    if (machineConfiguration.DoorSensorStatus != "OK")
                        this.OutputBox.Write(string.Format("WARNING: DoorSensor status returned {0} (door may be open!)", (object)machineConfiguration.DoorSensorStatus));
                }
                else
                    this.OutputBox.Write(string.Format("Door sensor configuration shows: NOT CONFIGURED."));
                if ((machineConfiguration.QuickReturnStatus & DeviceStatus.Found) != DeviceStatus.None)
                    this.OutputBox.Write("A Quick Return device is attached; it {0} configured.", (machineConfiguration.QuickReturnStatus & DeviceStatus.Configured) != DeviceStatus.None ? (object)"is" : (object)"is not");
                if (machineConfiguration.HasFraudDevice)
                {
                    this.m_gripperSellButton.Enabled = false;
                    this.OutputBox.Write("The kiosk is configured with a fraud sensor.");
                }
                this.CameraHelper.Reset(machineConfiguration.LegacyCamera);
                if (!this.CameraHelper.LegacyCamera)
                {
                    this.m_cameraPreview.Enabled = false;
                    this.m_startButtonCamera.Enabled = true;
                    this.ToggleNonLegacyStart();
                }
                this.ChangeQlmPanelState();
                this.SetFanButtonState(this.AirExchangerState.Configure(machineConfiguration.AirExchangerStatus, machineConfiguration.AirExchangerFanStatus));
                this.m_cameraWorkingButton.Enabled = this.CameraHelper.CameraInError();
            }
        }

        private void ToggleNonLegacyStart()
        {
            CameraState cameraStatus = this.CameraHelper.GetCameraStatus();
            if (cameraStatus == CameraState.Unknown)
                this.OutputBox.Write("Could not find current camera status.");
            else
                this.m_startButtonCamera.Text = CameraState.Stopped == cameraStatus ? "Start Camera" : "Stop Camera";
        }

        private void DisplayInventoryStats()
        {
            using (InventoryStatsJob inventoryStatsJob = new InventoryStatsJob(this.HardwareService))
            {
                inventoryStatsJob.Run();
                inventoryStatsJob.Results.ForEach((Action<ProgramResult>)(result =>
                {
                    if (result.Code == "TotalEmptyCount")
                        this.OutputBox.Write("  EMPTY slots: {0}", (object)result.Message);
                    else if (result.Code == "UnknownCount")
                    {
                        this.OutputBox.Write("  UNKNOWN slots: {0}", (object)result.Message);
                    }
                    else
                    {
                        if (!(result.Code == "InventoryStoreError"))
                            return;
                        this.OutputBox.Write(result.Message);
                    }
                }));
                this.OutputBox.Write("Inventory Stats: ");
            }
        }

        private void CompileScripts()
        {
            string path = ServiceLocator.Instance.GetService<IRuntimeService>().RuntimePath("Scripts");
            if (!Directory.Exists(path))
                return;
            foreach (string file in Directory.GetFiles(path, "*.hs"))
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(file);
                if (!this.HardwareService.CompileProgram(file, withoutExtension, false).Success)
                    LogHelper.Instance.Log("Failed to compile script {0} ", (object)withoutExtension);
            }
        }

        private void OnDropQLM(object sender, EventArgs args)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
            {
                using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                {
                    string str = instructionHelper.ExecuteGeneric("HOMEY CLEARGRIPPER=TRUE");
                    if (string.IsNullOrEmpty(str))
                        return;
                    if (str != "SUCCESS")
                        this.OutputBox.Write(string.Format("The HOMEY function failed with error {0}", (object)str));
                    else
                        this.OnExecuteInstructionBlind(aspect);
                }
            }
        }

        private void OnDoSnap(object sender, EventArgs args)
        {
            using (this.Manager.MakeAspect(sender))
            {
                ISnapResult snapResult = this.CameraHelper.Snap();
                if (snapResult == null)
                    this.OutputBox.Write("Command error.");
                else if (snapResult.SnapOk)
                {
                    if (this.ImageViewer == null)
                    {
                        this.ImageViewer = new ImageViewer();
                        this.ImageViewer.FormClosing += new FormClosingEventHandler(this.OnImageViewClosing);
                    }
                    else
                        this.ImageViewer.Hide();
                    this.ImageViewer.DisplayFile(snapResult.Path);
                    this.ImageViewer.Show();
                }
                else
                    this.OutputBox.Write("CAMERA CAPTURE error. I'm sorry!");
            }
        }

        private void OnImageViewClosing(object sender, FormClosingEventArgs args)
        {
            this.ImageViewer = (ImageViewer)null;
        }

        private void OnStartCamera(object sender, EventArgs args)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                if (this.CameraHelper.LegacyCamera)
                {
                    bool on = !this.RinglightOn;
                    if (!ServiceLocator.Instance.GetService<IControlSystem>().ToggleRingLight(on, new int?()).Success)
                        return;
                    this.RinglightOn = on;
                    buttonAspects.Button.Text = this.RinglightOn ? "Turn Ringlight Off" : "Turn Ringlight On";
                }
                else
                {
                    CameraState cameraStatus = this.CameraHelper.GetCameraStatus();
                    if (cameraStatus == CameraState.Unknown)
                        return;
                    CameraState cameraState = this.CameraHelper.ToggleState();
                    if (cameraState != cameraStatus)
                    {
                        this.OutputBox.Write(string.Format("Changed the camera state to {0}.", (object)cameraState.ToString()));
                        this.ToggleNonLegacyStart();
                    }
                    else
                        this.OutputBox.Write(string.Format("Couldn't change camera state to {0}.I'm sorry!", (object)cameraState.ToString()));
                }
            }
        }

        private void OnExecuteInstructionBlind(object sender, EventArgs e)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
                this.OnExecuteInstructionBlind(aspect);
        }

        private void OnExecuteInstructionBlind(ButtonAspects aspect)
        {
            string tagInstruction = aspect.GetTagInstruction();
            if (!string.IsNullOrEmpty(tagInstruction))
            {
                HardwareCommandResult result = this.HardwareService.ExecuteImmediate(tagInstruction, out HardwareJob _);
                result.Dump();
                if (result.Success)
                    this.OutputBox.Write("Sent instruction " + tagInstruction);
                else
                    this.OutputBox.Write("Sending instruction " + tagInstruction + " failed.");
            }
            else
                this.OutputBox.Write("{0} failed.", (object)tagInstruction);
        }

        private void OnExecuteErrorCodeInstruction(object sender, EventArgs e)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
                this.OnExecuteErrorCodeInstruction(aspect);
        }

        private void OnExecuteErrorCodeInstruction(ButtonAspects aspect)
        {
            string tagInstruction = aspect.GetTagInstruction();
            using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                this.OutputBox.Write("{0} - {1}", (object)tagInstruction, (object)instructionHelper.ExecuteErrorCode(tagInstruction));
        }

        private void OnExecuteResponseInstruction(object sender, EventArgs e)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
                this.OnExecuteResponseInstruction(aspect);
        }

        private void OnExecuteResponseInstruction(ButtonAspects aspect)
        {
            string tagInstruction = aspect.GetTagInstruction();
            using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                this.LogResponse(instructionHelper.ExecuteWithResponse(tagInstruction), tagInstruction);
        }

        private void OnExecuteInstructionRaw(object s, EventArgs e)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(s))
            {
                string tagInstruction = buttonAspects.GetTagInstruction();
                using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                {
                    string str = instructionHelper.ExecuteGeneric(tagInstruction);
                    if (string.IsNullOrEmpty(str))
                        str = ErrorCodes.ServiceChannelError.ToString().ToUpper();
                    this.OutputBox.Write("{0} - {1}", (object)tagInstruction, (object)str);
                }
            }
        }

        private void LogResponse(IControlResponse response, string tag)
        {
            string fmt = "{0} - {1}";
            if (response.CommError)
                this.OutputBox.Write(fmt, (object)tag, (object)response.Diagnostic);
            else if (response.TimedOut)
                this.OutputBox.Write(fmt, (object)tag, (object)ErrorCodes.Timeout.ToString().ToUpper());
            else
                this.OutputBox.Write(fmt, (object)tag, (object)ErrorCodes.Success.ToString().ToUpper());
        }

        private void OnRunQlmDisengage(object sender, EventArgs e)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
            {
                if (!this.VMZConfigured)
                {
                    this.OnExecuteResponseInstruction(aspect);
                }
                else
                {
                    if (new ConfirmationDialog().ShowDialog() != DialogResult.OK)
                        return;
                    using (TestRetrofitDeck testRetrofitDeck = new TestRetrofitDeck(this.HardwareService))
                    {
                        testRetrofitDeck.Run();
                        if (HardwareJobStatus.Completed == testRetrofitDeck.EndStatus)
                            this.OutputBox.Write("Test succeeded.");
                        else
                            this.OutputBox.Write("The test failed.");
                    }
                }
            }
        }

        private void OnTestBoards(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                this.OutputBox.Write("Test boards:");
                if (this.TestBoards())
                {
                    this.OutputBox.Write("Board test ok.");
                }
                else
                {
                    this.OutputBox.Write(" One or more boards not responsive - issue RESET.");
                    using (ResetControlSystemJob controlSystemJob = new ResetControlSystemJob(this.HardwareService))
                    {
                        controlSystemJob.Run();
                        foreach (ProgramResult result in controlSystemJob.Results)
                            this.OutputBox.Write(result.Message);
                        if (controlSystemJob.EndStatus != HardwareJobStatus.Completed)
                            return;
                        this.TestBoards();
                    }
                }
            }
        }

        private bool TestBoards()
        {
            using (BoardTestJob boardTestJob = new BoardTestJob(this.HardwareService))
            {
                boardTestJob.Run();
                foreach (ProgramResult result in boardTestJob.Results)
                {
                    if (result.Code == "VersionResult")
                        this.OutputBox.Write(result.Message);
                }
                return boardTestJob.EndStatus == HardwareJobStatus.Completed;
            }
        }

        private Form1.Direction DirectionFromButton(object sender)
        {
            Form1.Direction direction = Form1.Direction.Unknown;
            if (!(sender is Button button))
                return direction;
            if (!(button.Tag is string tag))
                return direction;
            try
            {
                return (Form1.Direction)Enum.Parse(typeof(Form1.Direction), tag, true);
            }
            catch (Exception ex)
            {
                return Form1.Direction.Unknown;
            }
        }

        private void OnDirectionMove(object sender, EventArgs args)
        {
            using (this.Manager.MakeAspect(sender))
            {
                Form1.Direction direction = this.DirectionFromButton(sender);
                if (direction == Form1.Direction.Unknown)
                    return;
                bool flag = false;
                if (this.m_secure && this.m_sensorCheckBox.Checked)
                {
                    flag = true;
                    this.m_sensorCheckBox.Checked = false;
                }
                this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, string.Empty);
                Application.DoEvents();
                string text = this.m_encoderUnitsTextBox.Text;
                int num1 = 0;
                ref int local = ref num1;
                if (int.TryParse(text, out local))
                {
                    if (direction == Form1.Direction.Left || direction == Form1.Direction.Right)
                    {
                        if (Math.Abs(num1) > 10)
                        {
                            this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, string.Format("Cannot exceed {0} units.", (object)10));
                            this.OutputBox.Write("ERROR: move cannot exceed {0} units on X axis.", (object)10);
                            Application.DoEvents();
                            return;
                        }
                    }
                    else if ((direction == Form1.Direction.Up || direction == Form1.Direction.Down) && Math.Abs(num1) > 200)
                    {
                        this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, string.Format("Cannot exceed {0} units.", (object)200));
                        this.OutputBox.Write("ERROR: move cannot exceed {0} units on Y axis.", (object)200);
                        Application.DoEvents();
                        return;
                    }
                    if (direction == Form1.Direction.Left || direction == Form1.Direction.Down)
                    {
                        num1 = -num1;
                        this.OutputBox.Write(string.Format("{0} moving {1} eu", (object)direction.ToString(), (object)num1));
                    }
                    IControllerPosition position = this.MoveHelper.GetPosition();
                    if (!position.ReadOk)
                    {
                        this.OutputBox.Write("Unable to query motor positions.");
                    }
                    else
                    {
                        int num2 = position.XCoordinate.Value;
                        int num3 = position.YCoordinate.Value;
                        Axis axis = direction == Form1.Direction.Up || direction == Form1.Direction.Down ? Axis.Y : Axis.X;
                        int units = Axis.Y == axis ? num3 + num1 : num2 + num1;
                        this.OutputBox.Write(this.MoveHelper.MoveAbs(axis, units, !(this.m_secure & flag)).ToString().ToUpper());
                    }
                }
                else
                    this.m_errorProvider.SetError((Control)this.m_encoderUnitsTextBox, "Must specify an integer!");
            }
        }

        private void OnReadPositions(object sender, EventArgs ea)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (MotionControlDataExecutor controlDataExecutor = new MotionControlDataExecutor(this.HardwareService))
                {
                    controlDataExecutor.Run();
                    if (HardwareJobStatus.Errored == controlDataExecutor.EndStatus)
                    {
                        this.OutputBox.Write("Unable to run position job.");
                    }
                    else
                    {
                        this.OutputBox.Write(controlDataExecutor.CurrentLocation);
                        if (controlDataExecutor.Position.ReadOk)
                            this.OutputBox.Write(string.Format("X = {0}, Y = {1}", (object)controlDataExecutor.Position.XCoordinate.Value, (object)controlDataExecutor.Position.YCoordinate.Value));
                        else
                            this.OutputBox.Write("Unable to obtain position information.");
                    }
                }
            }
        }

        private void StopJobs()
        {
            int num = 0;
            HardwareJob[] jobs;
            if (!this.HardwareService.GetJobs(out jobs).Success)
            {
                this.OutputBox.Write("Unable to read job list - JOBS MAY BE ACTIVE!");
            }
            else
            {
                foreach (HardwareJob hardwareJob in jobs)
                {
                    if ((hardwareJob.Status == HardwareJobStatus.Pending || hardwareJob.Status == HardwareJobStatus.Running) && !hardwareJob.ProgramName.Equals("job-watchdog"))
                        ++num;
                }
                if (num > 0)
                    this.OutputBox.Write("WARNING: jobs are currently running or pending.");
                else
                    this.OutputBox.Write("There are no jobs running or pending.");
                this.HardwareService.ExecuteServiceCommand("SERVICE diagnostic-mode status: true");
            }
        }

        private void OnRollerToPosition(object sender, EventArgs e)
        {
            this.m_errorProvider.Clear();
            using (this.Manager.MakeAspect(sender))
            {
                if (string.IsNullOrEmpty(this.m_rollToSensorText.Text))
                    return;
                int integer1 = this.m_rollToSensorText.GetInteger("Roller Position", this.OutputBox);
                if (-1 == integer1)
                {
                    this.OutputBox.Write("Invalid sensor specified for position.");
                    this.m_errorProvider.SetError((Control)this.m_rollToSensorText, "Invalid position.");
                }
                else
                {
                    int integer2 = this.m_rollerTimeoutTextBox.GetInteger("Roller Timeout", this.OutputBox);
                    int num = integer2 != -1 ? integer2 : 5000;
                    string str = string.Format("ROLLER POS={0} TIMEOUT={1} WAIT=TRUE", (object)integer1, (object)num);
                    using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                        this.LogResponse(instructionHelper.ExecuteWithResponse(str), str);
                }
            }
        }

        private void OnReadDiskInPicker(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                ScanResult scanResult = ScanResult.ReadBarcodeOfDiskInPicker(this.HardwareService);
                this.OutputBox.Write(string.Format("Found {0} secure codes.", (object)scanResult.SecureCount));
                this.OutputBox.Write(scanResult.ToString());
            }
        }

        private void OnLaunchCameraProperties(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                TunerLaunchService.LaunchTunerAndWait(this.HardwareService, this.Manager);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) => this.CloseForm();

        private void OnCloseForm(object sender, EventArgs e) => this.CloseForm();

        private void CloseForm()
        {
            if (this.FormDisposed)
                return;
            this.FormDisposed = true;
            if (this.ImageViewer != null)
                this.ImageViewer.Dispose();
            this.OutputBox.Write("Cleanup Hardware.");
            this.HardwareService.ExecuteServiceCommand("SERVICE diagnostic-mode status: false");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" AIRXCHGR FANON");
            stringBuilder.AppendLine(" VENDDOOR CLOSE");
            stringBuilder.AppendLine(" GRIPPER RENT");
            stringBuilder.AppendLine(" GRIPPER RETRACT");
            stringBuilder.AppendLine(" SENSOR PICKER-OFF");
            stringBuilder.AppendLine(" ROLLER STOP");
            stringBuilder.AppendLine(" RINGLIGHT OFF");
            stringBuilder.AppendLine(" CLEAR");
            this.HardwareService.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
            this.OutputBox.Write("Restart suspended jobs.");
            this.m_stoppedJobs.ForEach((Action<HardwareJob>)(each => each.Pend()));
            Application.Exit();
        }

        private void OnRunInit(object sender, EventArgs e)
        {
            this.m_runInitButton.BackColor = Color.Red;
            Array.ForEach<Button>(this.FormButtons, (Action<Button>)(button => button.Enabled = false));
            this.m_initWorker.RunWorkerAsync();
        }

        private void OnClearOutput(object sender, EventArgs e) => this.OutputBox.Clear();

        private void OnGotoSlot(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
                }
                else
                {
                    using (TesterMoveToSlotExecutor moveToSlotExecutor = new TesterMoveToSlotExecutor(this.HardwareService, sourceDeck, sourceSlot))
                    {
                        moveToSlotExecutor.Run();
                        moveToSlotExecutor.Results.ForEach((Action<ProgramResult>)(pr => this.OutputBox.Write(pr.Message)));
                    }
                }
            }
        }

        private void OnSyncSlot(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
                }
                else
                {
                    List<Redbox.HAL.Client.Location> locationList = new List<Redbox.HAL.Client.Location>()
          {
            new Redbox.HAL.Client.Location() { Deck = sourceDeck, Slot = sourceSlot }
          };
                    HardwareService hardwareService = this.HardwareService;
                    List<Redbox.HAL.Client.Location> locations = locationList;
                    HardwareJobSchedule schedule = new HardwareJobSchedule();
                    schedule.Priority = HardwareJobPriority.Highest;
                    HardwareJob hardwareJob;
                    if (!hardwareService.HardSync(locations, "Tester Location Sync", schedule, out hardwareJob).Success)
                    {
                        this.OutputBox.Write("Unable to communicate with HAL.");
                    }
                    else
                    {
                        hardwareJob.Pend();
                        hardwareJob.WaitForCompletion(300000);
                        IDictionary<string, string> symbols;
                        if (!hardwareJob.GetSymbols(out symbols).Success)
                            return;
                        foreach (string key in (IEnumerable<string>)symbols.Keys)
                        {
                            if (key.StartsWith("MSTESTER-SYMBOL"))
                                this.OutputBox.Write(symbols[key]);
                        }
                    }
                }
            }
        }

        private void OnPut(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                    this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
                else
                    CompositeFunctions.PutItem(this.HardwareService, sourceDeck, sourceSlot, this.OutputBox);
            }
        }

        private void OnGetAndRead(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.GetDVD(sender);
        }

        private void OnTransfer(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
                }
                else
                {
                    int integer1 = this.m_destinationDeckText.GetInteger("Destination Deck", this.OutputBox);
                    int integer2 = this.m_destinationSlotText.GetInteger("Destination Slot", this.OutputBox);
                    if (-1 == integer1 || -1 == integer2)
                    {
                        this.OutputBox.Write(Resources.DestDeckSlotInvalid);
                    }
                    else
                    {
                        HardwareJob job;
                        HardwareCommandResult result = this.HardwareService.ExecuteImmediate("GRIPPER  STATUS", out job);
                        if (!result.Success)
                        {
                            result.Dump();
                            LogHelper.Instance.Log("Service call failure.");
                        }
                        else if (job.GetTopOfStack().Equals("FULL"))
                        {
                            this.OutputBox.Write("The gripper is full; no transfer. Sorry! Have a great day!");
                        }
                        else
                        {
                            this.OutputBox.Write("Transfer " + new TransferDisk(this.HardwareService).Transfer(new TransferLocation(sourceDeck, sourceSlot), new TransferLocation(integer1, integer2)));
                            this.m_destinationDeckText.Text = string.Empty;
                            this.m_destinationSlotText.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void OnVendDVD(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                CompositeFunctions.VendDisk(this.HardwareService, this.OutputBox);
        }

        private void OnPushDVDInSlot(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (PushInDvdJob pushInDvdJob = new PushInDvdJob(this.HardwareService))
                {
                    pushInDvdJob.Run();
                    int num = 0;
                    foreach (ProgramResult result in pushInDvdJob.Results)
                    {
                        if (result.Code == "MachineError")
                        {
                            ++num;
                            this.OutputBox.Write(result.Message);
                        }
                    }
                    if (num == 0)
                        this.OutputBox.Write("push-in-dvd SUCCESS");
                    else
                        this.OutputBox.Write(string.Format("push-in-dvd {0}", pushInDvdJob.Symbols.ContainsKey("ERROR-STATE") ? (object)pushInDvdJob.Symbols["ERROR-STATE"] : (object)"ERROR"));
                }
            }
        }

        private void OnPutInEmptySlot(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (PutInEmptySlotResult inEmptySlotResult = new PutInEmptySlotResult(this.HardwareService))
                {
                    inEmptySlotResult.Run();
                    foreach (ProgramResult result in inEmptySlotResult.Results)
                        this.OutputBox.Write(result.Message);
                }
            }
        }

        private void OnReadPickerInputs(object sender, EventArgs args)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                if (buttonAspects.Button == null)
                    return;
                this.WriteInputs<PickerInputs>(ServiceLocator.Instance.GetService<IControlSystem>().ReadPickerInputs(), "PICKER CONTROLLER");
            }
        }

        private void OnReadAuxInputs(object sender, EventArgs args)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                if (buttonAspects.Button == null)
                    return;
                this.WriteInputs<AuxInputs>(ServiceLocator.Instance.GetService<IControlSystem>().ReadAuxInputs(), "AUX-QLM");
            }
        }

        private void WriteInputs<T>(IReadInputsResult<T> inputs, string header)
        {
            if (!inputs.Success)
            {
                this.OutputBox.Write("Read inputs error: {0}", (object)inputs.Error);
            }
            else
            {
                inputs.Log();
                char[] _inputs = new char[inputs.InputCount];
                int count = 0;
                inputs.Foreach((Action<T>)(each => _inputs[count++] = inputs.IsInputActive(each) ? '1' : '0'));
                for (int index = _inputs.Length - 1; index >= 0; --index)
                    this.OutputBox.Write("{0} I/O {1} = {2}", (object)header, (object)(index + 1), (object)_inputs[index]);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int num = (int)new AdvancedMode(this.HardwareService).ShowDialog();
        }

        private int SourceDeck => this.m_sourceDeckTextBox.GetInteger("Source Deck", this.OutputBox);

        private int SourceSlot => this.m_sourceSlotTextBox.GetInteger("Source Slot", this.OutputBox);

        private void button58_Click(object sender, EventArgs e)
        {
            try
            {
                int num = (int)new DeckConfigurationForm(this.HardwareService).ShowDialog();
            }
            catch (Exception ex)
            {
                this.OutputBox.Write("Unable to run Decks form.");
                LogHelper.Instance.Log("Unable to run Decks form.");
                LogHelper.Instance.Log(ex.Message);
            }
        }

        private void button59_Click(object sender, EventArgs e)
        {
            try
            {
                new Process()
                {
                    StartInfo = {
            Arguments = "c:\\Program Files\\Redbox\\KioskLogs\\ErrorLogs",
            FileName = "explorer.exe"
          }
                }.Start();
            }
            catch (Exception ex)
            {
                this.OutputBox.Write("Unable to launch explorer.");
            }
        }

        private void OnSourceDeck_Click(object s, EventArgs e)
        {
            this.m_sourceDeckTextBox.InputNumber();
        }

        private void OnSourceSlot_Click(object s, EventArgs e)
        {
            this.m_sourceSlotTextBox.InputNumber();
        }

        private void OnDestDeck_Click(object s, EventArgs e)
        {
            this.m_destinationDeckText.InputNumber();
        }

        private void OnDestSlot_Click(object s, EventArgs e)
        {
            this.m_destinationSlotText.InputNumber();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (MSPullInDvdJob msPullInDvdJob = new MSPullInDvdJob(this.HardwareService))
                {
                    msPullInDvdJob.Run();
                    if (msPullInDvdJob.EndStatus == HardwareJobStatus.Completed)
                    {
                        this.OutputBox.Write("pull-dvd-in SUCCESS");
                    }
                    else
                    {
                        foreach (ProgramResult result in msPullInDvdJob.Results)
                        {
                            if (result.Code == "InvalidJobUse")
                            {
                                this.OutputBox.Write("pull-dvd-in is intended for a disk stuck in the picker.");
                                this.OutputBox.Write("Please use the 'GET' function instead.");
                                break;
                            }
                            if (result.Code == "MachineError")
                                this.OutputBox.Write("pull-dvd-in " + result.Message);
                        }
                    }
                }
            }
        }

        private void button65_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.DisplayInventoryStats();
        }

        private void button67_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int num = (int)new ConfiguredDevicesForm(this.HardwareService)
                {
                    AllowSessionDisplay = Settings.Default.AllowSessionDisplay,
                    RouterPowerCyclePause = Settings.Default.RouterRelayPause,
                    DisplayEngineeringTab = Settings.Default.DisplayEngineeringTab
                }.ShowDialog();
                this.ChangeUIAfterconfig();
            }
        }

        private void ChangeQlmPanelState()
        {
            if (!this.VMZConfigured)
            {
                this.ToggleQLMButtons(true);
            }
            else
            {
                this.ToggleQLMButtons(false);
                this.m_qlmTestButton.Text = "Put In bin";
                this.m_qlmTestButton.Enabled = true;
                this.m_qlmGroupBox.Text = "Door sensors and others.";
                this.m_qlmEngageButton.Enabled = this.DoorSensorsConfigured;
                this.m_qlmEngageButton.Text = "Query Door Sensors";
                this.m_qlmDisengageButton.Enabled = true;
                this.m_qlmDisengageButton.Text = "Test deck 8.";
                this.m_qlmCaseStatusButton.Visible = this.m_qlmDownButton.Visible = this.m_qlmStopButton.Visible = this.m_qlmUpButton.Visible = this.m_qlmDoorStatus.Visible = false;
            }
        }

        private void ToggleQLMButtons(bool show)
        {
            this.m_qlmCaseStatusButton.Enabled = show;
            this.m_qlmDisengageButton.Enabled = show;
            this.m_qlmDownButton.Enabled = show;
            this.m_qlmEngageButton.Enabled = show;
            this.m_qlmStopButton.Enabled = show;
            this.m_qlmUpButton.Enabled = show;
            this.m_qlmDoorStatus.Enabled = show;
            this.m_qlmTestButton.Enabled = show;
        }

        private bool JobCompletedWithoutError(HardwareJob job)
        {
            return job.Status == HardwareJobStatus.Completed || job.Status == HardwareJobStatus.Stopped;
        }

        private bool JobCompleted(HardwareJob job)
        {
            return job.Status == HardwareJobStatus.Completed || job.Status == HardwareJobStatus.Stopped || job.Status == HardwareJobStatus.Errored || job.Status == HardwareJobStatus.Garbage;
        }

        private void button66_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.RunHardwareStatus();
        }

        private void RunHardwareStatus()
        {
            using (HardwareStatusExecutor hardwareStatusExecutor = new HardwareStatusExecutor(this.HardwareService))
            {
                hardwareStatusExecutor.Run();
                if (!hardwareStatusExecutor.HardwareOk)
                    hardwareStatusExecutor.Results.ForEach((Action<ProgramResult>)(res =>
                    {
                        if (!(res.Code != "HardwareStatusInError"))
                            return;
                        this.OutputBox.Write(res.Message);
                    }));
                this.OutputBox.Write(!hardwareStatusExecutor.HardwareOk ? "Hardware error status follows:" : "Hardware status ok.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (ResetMotionControlExecutor motionControlExecutor = new ResetMotionControlExecutor(this.HardwareService))
                {
                    motionControlExecutor.Run();
                    motionControlExecutor.Results.ForEach((Action<ProgramResult>)(each => this.OutputBox.Write(each.Message)));
                    if (motionControlExecutor.EndStatus == HardwareJobStatus.Completed)
                        return;
                    this.OutputBox.Write("ERROR: reset-arcus failed.");
                }
            }
        }

        private void ExecuteEngageOrOther(object sender, EventArgs e)
        {
            using (ButtonAspects aspect = this.Manager.MakeAspect(sender))
            {
                if (this.VMZConfigured)
                {
                    using (InstructionHelper instructionHelper = new InstructionHelper(this.HardwareService))
                    {
                        string str = instructionHelper.ExecuteGeneric("DOORSENSORS TESTERQUERY");
                        this.OutputBox.Write(string.IsNullOrEmpty(str) ? ErrorCodes.ServiceChannelError.ToString().ToUpper() : str);
                    }
                }
                else
                    this.OnExecuteResponseInstruction(aspect);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
                }
                else
                {
                    using (GetAndReadExecutor executor = new GetAndReadExecutor(this.HardwareService, sourceDeck, sourceSlot, true))
                    {
                        executor.Run();
                        executor.Results.ForEach((Action<ProgramResult>)(result =>
                        {
                            if (!(result.Code == "ErrorMessage"))
                                return;
                            this.OutputBox.Write(result.Message);
                        }));
                        if (HardwareJobStatus.Completed == executor.EndStatus)
                        {
                            ScanResult scanResult = ScanResult.From((JobExecutor)executor);
                            this.OutputBox.Write(string.Format("found {0} secure codes.", (object)scanResult.SecureCount));
                            this.OutputBox.Write(scanResult.ToString());
                        }
                        this.OutputBox.Write("Get and read ended with status {0}", (object)executor.EndStatus.ToString());
                    }
                }
            }
        }

        private void GetDVD(object sender)
        {
            int sourceDeck = this.SourceDeck;
            int sourceSlot = this.SourceSlot;
            if (-1 == sourceDeck || -1 == sourceSlot)
                this.OutputBox.Write(Resources.SourceDeckSlotInvalid);
            else
                CompositeFunctions.GetItem(sourceDeck, sourceSlot, this.OutputBox, this.HardwareService);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.m_errorProvider.Clear();
            using (this.Manager.MakeAspect(sender))
            {
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceSlot)
                {
                    this.m_errorProvider.SetError((Control)this.m_sourceSlotTextBox, "Need a valid slot!");
                }
                else
                {
                    using (VerticalSync verticalSync = new VerticalSync(this.HardwareService, sourceSlot))
                    {
                        verticalSync.Run();
                        if (HardwareJobStatus.Completed == verticalSync.EndStatus)
                        {
                            this.OutputBox.Write("The job completed successfully.");
                        }
                        else
                        {
                            foreach (ProgramResult result in verticalSync.Results)
                                this.OutputBox.Write(string.Format("Failure at Deck {0} Slot {1} MSG: {2}", (object)result.Deck, (object)result.Slot, (object)result.Message));
                        }
                    }
                }
            }
        }

        private void m_cameraWorkingButton_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                if (this.CameraHelper.ResetReturnCounter())
                {
                    this.OutputBox.Write("Return counter reset.");
                    this.m_cameraWorkingButton.Enabled = false;
                }
                else
                    this.OutputBox.Write("Command failed.");
            }
        }

        private void m_showTimeoutLogButton_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                string countersFile = Settings.Default.CountersFile;
                using (GetControllerTimeoutsExecutor timeoutsExecutor = new GetControllerTimeoutsExecutor(this.HardwareService))
                {
                    timeoutsExecutor.Run();
                    if (HardwareJobStatus.Completed == timeoutsExecutor.EndStatus)
                    {
                        using (StreamWriter log = new StreamWriter((Stream)File.Open(countersFile, FileMode.Append, FileAccess.Write, FileShare.Read)))
                            timeoutsExecutor.Log(log);
                        Process.Start(countersFile);
                    }
                    else
                        this.OutputBox.Write("Command failed.");
                }
            }
        }

        private void m_readLimitsButton_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                IMotionControlLimitResponse controlLimitResponse = this.MoveHelper.ReadLimits();
                if (!controlLimitResponse.ReadOk)
                {
                    this.OutputBox.Write("Unable to read limits.");
                }
                else
                {
                    foreach (IMotionControlLimit limit in controlLimitResponse.Limits)
                        this.OutputBox.Write(string.Format("{0} LIMIT: {1}", (object)limit.Limit.ToString().ToUpper(), limit.Blocked ? (object)"BLOCKED" : (object)"CLEAR"));
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (this.HardwareService.ServiceUnknownSync().Success)
                this.OutputBox.Write("Unknown sync scheduled.");
            else
                this.OutputBox.Write("Unknown sync schedule failed.");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.m_errorProvider.Clear();
            using (this.Manager.MakeAspect(sender))
            {
                if (!this.VMZConfigured)
                {
                    using (QlmTestSyncJob qlmTestSyncJob = new QlmTestSyncJob(this.HardwareService))
                    {
                        qlmTestSyncJob.Run();
                        foreach (ProgramResult result in qlmTestSyncJob.Results)
                            this.OutputBox.Write(string.Format("Failure at Deck {0} Slot {1} MSG: {2}", (object)result.Deck, (object)result.Slot, (object)result.Message));
                    }
                }
                else
                {
                    using (PutDiskInPickerInBinJob inPickerInBinJob = new PutDiskInPickerInBinJob(this.HardwareService))
                    {
                        inPickerInBinJob.Run();
                        this.OutputBox.Write(string.Format("Job ended with status = {0}", (object)inPickerInBinJob.EndStatus.ToString()));
                        inPickerInBinJob.Results.ForEach((Action<ProgramResult>)(res =>
                        {
                            if (res.Code == "PutToBinOK")
                                this.OutputBox.Write(string.Format("The disk ID = {0} was put into the bin.", res.ItemID.IsUnknown() ? (object)res.ItemID.Metadata : (object)res.ItemID.Barcode));
                            else
                                this.OutputBox.Write(string.Format("Code = {0} Message = {1}", (object)res.Code, (object)res.Message));
                        }));
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                ExchangerFanStatus fanStatus = this.AirExchangerState.ToggleFan();
                this.OutputBox.Write(ExchangerFanStatus.On == fanStatus ? "Xchanger fan should be on." : "Xchanger fan should be off.");
                this.SetFanButtonState(fanStatus);
            }
        }

        private void SetFanButtonState(ExchangerFanStatus fanStatus)
        {
            this.m_fanButton.Enabled = this.AirExchangerState.Configured;
            if (!this.AirExchangerState.Configured)
                return;
            this.m_fanButton.BackColor = ExchangerFanStatus.On == fanStatus ? Color.LightGray : Color.Red;
            this.m_fanButton.Text = ExchangerFanStatus.On == fanStatus ? "Turn Off Xchgr Fan" : "Turn On Xchgr Fan";
        }

        private void m_getWithOffset_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                    this.m_errorProvider.SetError((Control)this.m_sourceDeckTextBox, "Must provide a deck/slot value.");
                else if (string.IsNullOrEmpty(this.m_getOffsetTB.Text))
                {
                    this.GetDVD(sender);
                }
                else
                {
                    if (this.m_offsetGetRadioButton == null)
                    {
                        this.OutputBox.Write("Please specify an axis.");
                        this.m_errorProvider.SetError((Control)this.m_getOffsetTB, "Please specify an axis.");
                    }
                    HardwareJob job;
                    this.HardwareService.ExecuteImmediate("CLEAR", out job);
                    HardwareCommandResult result1 = this.HardwareService.ExecuteImmediate("GRIPPER STATUS", out job);
                    if (!result1.Success)
                    {
                        result1.Dump();
                        this.OutputBox.Write("Unable to communicate with service.");
                    }
                    else if ("FULL".Equals(job.GetTopOfStack(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        this.OutputBox.Write("The gripper is full - clear picker & try again.");
                    }
                    else
                    {
                        Axis ignoringCase = Enum<Axis>.ParseIgnoringCase(this.m_offsetGetRadioButton.Tag as string, Axis.X);
                        int result2;
                        if (!int.TryParse(this.m_getOffsetTB.Text, out result2))
                            this.OutputBox.Write(string.Format("Could not decode {0} as offset.", (object)this.m_getOffsetTB.Text));
                        else if (ignoringCase == Axis.X && Math.Abs(result2) > 50)
                            this.OutputBox.Write(string.Format("Offset {0} exceeds 50 units.", (object)this.m_getOffsetTB.Text));
                        else if (Axis.Y == ignoringCase && Math.Abs(result2) > 500)
                        {
                            this.OutputBox.Write(string.Format("Offset {0} exceeds 500 units.", (object)this.m_getOffsetTB.Text));
                        }
                        else
                        {
                            using (PickAtOffsetExecutor atOffsetExecutor = new PickAtOffsetExecutor(this.HardwareService, sourceDeck, sourceSlot, ignoringCase, result2))
                            {
                                atOffsetExecutor.Run();
                                if (HardwareJobStatus.Completed == atOffsetExecutor.EndStatus)
                                {
                                    this.OutputBox.Write("GET was successful.");
                                }
                                else
                                {
                                    foreach (ProgramResult result3 in atOffsetExecutor.Results)
                                        this.OutputBox.Write(result3.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is RadioButton radioButton) || !radioButton.Checked)
                return;
            this.m_offsetGetRadioButton = radioButton;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Helper.WaitforInit();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.StopJobs();
            this.CompileScripts();
            this.ToggleFormButtons(true);
            this.ChangeUIAfterconfig();
            this.RunHardwareStatus();
            this.DisplayInventoryStats();
        }

        private void ToggleFormButtons(bool enable)
        {
            this.m_configureDevicesButton.Enabled = this.m_secure & enable;
            this.m_deckConfigurationButton.Enabled = this.m_secure & enable;
            this.m_updateConfigurationButton.Enabled = this.m_secure & enable;
            this.m_sensorCheckBox.Enabled = this.m_secure & enable;
            this.m_openLogsButton.Enabled = this.m_secure & enable;
            Array.ForEach<Button>(this.FormButtons, (Action<Button>)(each => each.Enabled = enable));
        }

        private void m_initWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitJob initJob = new InitJob(this.HardwareService);
            initJob.Run();
            e.Result = (object)initJob;
        }

        private void m_initWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            using (InitJob result = e.Result as InitJob)
            {
                if (HardwareJobStatus.Completed == result.EndStatus)
                    this.OutputBox.Write("Init succeeded.");
                else if (result.Errors.Count > 0)
                {
                    result.Errors.ForEach((Action<Error>)(each => this.OutputBox.Write(each.Details)));
                    this.OutputBox.Write("Init didn't succeed; errors follow:");
                }
                Array.ForEach<Button>(this.FormButtons, (Action<Button>)(button => button.Enabled = true));
                this.m_runInitButton.BackColor = Color.LightGray;
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
            this.groupBox2 = new GroupBox();
            this.m_testArcusCommButton = new Button();
            this.m_resetProteusButton = new Button();
            this.m_homeYButton = new Button();
            this.m_homeXButton = new Button();
            this.button44 = new Button();
            this.m_runInitButton = new Button();
            this.m_fanButton = new Button();
            this.groupBox3 = new GroupBox();
            this.m_closeSerialButton = new Button();
            this.button50 = new Button();
            this.button47 = new Button();
            this.button5 = new Button();
            this.button57 = new Button();
            this.groupBox4 = new GroupBox();
            this.button54 = new Button();
            this.button53 = new Button();
            this.button51 = new Button();
            this.groupBox5 = new GroupBox();
            this.groupBox6 = new GroupBox();
            this.m_rollerTimeoutTextBox = new TextBox();
            this.label17 = new Label();
            this.m_rollToSensorText = new TextBox();
            this.button39 = new Button();
            this.button38 = new Button();
            this.button37 = new Button();
            this.button36 = new Button();
            this.groupBox7 = new GroupBox();
            this.button64 = new Button();
            this.button35 = new Button();
            this.button34 = new Button();
            this.m_gripperSellButton = new Button();
            this.button32 = new Button();
            this.button31 = new Button();
            this.groupBox8 = new GroupBox();
            this.button30 = new Button();
            this.button29 = new Button();
            this.button28 = new Button();
            this.m_qlmGroupBox = new GroupBox();
            this.m_qlmDownButton = new Button();
            this.m_qlmUpButton = new Button();
            this.m_qlmDoorStatus = new Button();
            this.m_qlmCaseStatusButton = new Button();
            this.m_qlmStopButton = new Button();
            this.m_qlmDisengageButton = new Button();
            this.m_qlmEngageButton = new Button();
            this.groupBox10 = new GroupBox();
            this.button63 = new Button();
            this.button62 = new Button();
            this.button61 = new Button();
            this.button60 = new Button();
            this.m_destinationSlotText = new TextBox();
            this.m_destinationDeckText = new TextBox();
            this.label11 = new Label();
            this.label10 = new Label();
            this.button18 = new Button();
            this.button17 = new Button();
            this.button16 = new Button();
            this.button14 = new Button();
            this.button13 = new Button();
            this.button12 = new Button();
            this.button11 = new Button();
            this.button10 = new Button();
            this.button8 = new Button();
            this.button7 = new Button();
            this.m_sourceSlotTextBox = new TextBox();
            this.m_sourceDeckTextBox = new TextBox();
            this.groupBox11 = new GroupBox();
            this.m_getOffXRadio = new RadioButton();
            this.m_getOffYRadio = new RadioButton();
            this.m_getOffsetTB = new TextBox();
            this.m_getWithOffset = new Button();
            this.m_sensorCheckBox = new CheckBox();
            this.label16 = new Label();
            this.label15 = new Label();
            this.button4 = new Button();
            this.button3 = new Button();
            this.button2 = new Button();
            this.m_encoderUnitsTextBox = new TextBox();
            this.button1 = new Button();
            this.m_readPosButton = new Button();
            this.groupBox12 = new GroupBox();
            this.m_cameraWorkingButton = new Button();
            this.button19 = new Button();
            this.m_cameraPreview = new Button();
            this.button26 = new Button();
            this.m_startButtonCamera = new Button();
            this.m_snapImageButton = new Button();
            this.m_outputBox = new ListBox();
            this.m_clearOutputButton = new Button();
            this.label14 = new Label();
            this.m_updateConfigurationButton = new Button();
            this.m_versionTextBox = new TextBox();
            this.label19 = new Label();
            this.label20 = new Label();
            this.m_kioskIDTextBox = new TextBox();
            this.groupBox1 = new GroupBox();
            this.button22 = new Button();
            this.m_qlmTestButton = new Button();
            this.m_readLimitsButton = new Button();
            this.m_showTimeoutLogButton = new Button();
            this.button20 = new Button();
            this.button66 = new Button();
            this.m_errorProvider = new ErrorProvider(this.components);
            this.m_deckConfigurationButton = new Button();
            this.m_takeDiskButton = new Button();
            this.m_openLogsButton = new Button();
            this.m_configureDevicesButton = new Button();
            this.m_startupWorker = new BackgroundWorker();
            this.m_initWorker = new BackgroundWorker();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.m_qlmGroupBox.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize)this.m_errorProvider).BeginInit();
            this.SuspendLayout();
            this.groupBox2.Controls.Add((Control)this.m_testArcusCommButton);
            this.groupBox2.Controls.Add((Control)this.m_resetProteusButton);
            this.groupBox2.Controls.Add((Control)this.m_homeYButton);
            this.groupBox2.Controls.Add((Control)this.m_homeXButton);
            this.groupBox2.Controls.Add((Control)this.button44);
            this.groupBox2.Controls.Add((Control)this.m_runInitButton);
            this.groupBox2.Location = new Point(13, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(309, 130);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Init";
            this.m_testArcusCommButton.BackColor = Color.LightGray;
            this.m_testArcusCommButton.Enabled = false;
            this.m_testArcusCommButton.Location = new Point(204, 21);
            this.m_testArcusCommButton.Name = "m_testArcusCommButton";
            this.m_testArcusCommButton.Size = new Size(93, 43);
            this.m_testArcusCommButton.TabIndex = 6;
            this.m_testArcusCommButton.Tag = (object)"MOTIONCONTROL COMSTATUS";
            this.m_testArcusCommButton.Text = "Test Proteus Comm";
            this.m_testArcusCommButton.UseVisualStyleBackColor = false;
            this.m_testArcusCommButton.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.m_resetProteusButton.BackColor = Color.LightGray;
            this.m_resetProteusButton.Enabled = false;
            this.m_resetProteusButton.Location = new Point(204, 79);
            this.m_resetProteusButton.Name = "m_resetProteusButton";
            this.m_resetProteusButton.Size = new Size(93, 45);
            this.m_resetProteusButton.TabIndex = 5;
            this.m_resetProteusButton.Text = "Reset Proteus";
            this.m_resetProteusButton.UseVisualStyleBackColor = false;
            this.m_resetProteusButton.Click += new EventHandler(this.button9_Click);
            this.m_homeYButton.BackColor = Color.LightGray;
            this.m_homeYButton.Enabled = false;
            this.m_homeYButton.Location = new Point(105, 79);
            this.m_homeYButton.Name = "m_homeYButton";
            this.m_homeYButton.Size = new Size(75, 45);
            this.m_homeYButton.TabIndex = 3;
            this.m_homeYButton.Tag = (object)"HOMEY";
            this.m_homeYButton.Text = "Home Y";
            this.m_homeYButton.UseVisualStyleBackColor = false;
            this.m_homeYButton.Click += new EventHandler(this.OnExecuteErrorCodeInstruction);
            this.m_homeXButton.BackColor = Color.LightGray;
            this.m_homeXButton.Enabled = false;
            this.m_homeXButton.Location = new Point(6, 79);
            this.m_homeXButton.Name = "m_homeXButton";
            this.m_homeXButton.Size = new Size(75, 45);
            this.m_homeXButton.TabIndex = 2;
            this.m_homeXButton.Tag = (object)"HOMEX";
            this.m_homeXButton.Text = "Home X";
            this.m_homeXButton.UseVisualStyleBackColor = false;
            this.m_homeXButton.Click += new EventHandler(this.OnExecuteErrorCodeInstruction);
            this.button44.BackColor = Color.GreenYellow;
            this.button44.Location = new Point(105, 19);
            this.button44.Name = "button44";
            this.button44.Size = new Size(75, 45);
            this.button44.TabIndex = 1;
            this.button44.Text = "Exit";
            this.button44.UseVisualStyleBackColor = false;
            this.button44.Click += new EventHandler(this.OnCloseForm);
            this.m_runInitButton.BackColor = Color.LightGray;
            this.m_runInitButton.Enabled = false;
            this.m_runInitButton.Location = new Point(6, 19);
            this.m_runInitButton.Name = "m_runInitButton";
            this.m_runInitButton.Size = new Size(75, 45);
            this.m_runInitButton.TabIndex = 0;
            this.m_runInitButton.Text = "Init";
            this.m_runInitButton.UseVisualStyleBackColor = false;
            this.m_runInitButton.Click += new EventHandler(this.OnRunInit);
            this.m_fanButton.BackColor = Color.LightGray;
            this.m_fanButton.Enabled = false;
            this.m_fanButton.Location = new Point(6, 76);
            this.m_fanButton.Name = "m_fanButton";
            this.m_fanButton.Size = new Size(93, 42);
            this.m_fanButton.TabIndex = 6;
            this.m_fanButton.Text = "Turn Off Xchgr Fan";
            this.m_fanButton.UseVisualStyleBackColor = false;
            this.m_fanButton.Click += new EventHandler(this.button6_Click);
            this.groupBox3.Controls.Add((Control)this.m_fanButton);
            this.groupBox3.Controls.Add((Control)this.m_closeSerialButton);
            this.groupBox3.Controls.Add((Control)this.button50);
            this.groupBox3.Controls.Add((Control)this.button47);
            this.groupBox3.Controls.Add((Control)this.button5);
            this.groupBox3.Controls.Add((Control)this.button57);
            this.groupBox3.Location = new Point(13, 355);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(309, 135);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Boards";
            this.m_closeSerialButton.BackColor = Color.LightGray;
            this.m_closeSerialButton.Enabled = false;
            this.m_closeSerialButton.Location = new Point(210, 19);
            this.m_closeSerialButton.Name = "m_closeSerialButton";
            this.m_closeSerialButton.Size = new Size(87, 45);
            this.m_closeSerialButton.TabIndex = 4;
            this.m_closeSerialButton.Tag = (object)"SERIALBOARD CLOSEPORT";
            this.m_closeSerialButton.Text = "Close Serial Port";
            this.m_closeSerialButton.UseVisualStyleBackColor = false;
            this.m_closeSerialButton.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.button50.BackColor = Color.LightGray;
            this.button50.Enabled = false;
            this.button50.Location = new Point(210, 75);
            this.button50.Name = "button50";
            this.button50.Size = new Size(93, 45);
            this.button50.TabIndex = 3;
            this.button50.Tag = (object)"SERIALBOARD RESET";
            this.button50.Text = "Reset";
            this.button50.UseVisualStyleBackColor = false;
            this.button50.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button47.BackColor = Color.LightGray;
            this.button47.Enabled = false;
            this.button47.Location = new Point(111, 76);
            this.button47.Name = "button47";
            this.button47.Size = new Size(93, 45);
            this.button47.TabIndex = 0;
            this.button47.Tag = (object)"";
            this.button47.Text = "Board Test";
            this.button47.UseVisualStyleBackColor = false;
            this.button47.Click += new EventHandler(this.OnTestBoards);
            this.button5.BackColor = Color.LightGray;
            this.button5.Enabled = false;
            this.button5.Location = new Point(111, 19);
            this.button5.Name = "button5";
            this.button5.Size = new Size(93, 45);
            this.button5.TabIndex = 0;
            this.button5.Tag = (object)"AUX";
            this.button5.Text = "Read Aux Inputs";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new EventHandler(this.OnReadAuxInputs);
            this.button57.BackColor = Color.LightGray;
            this.button57.Enabled = false;
            this.button57.Location = new Point(6, 19);
            this.button57.Name = "button57";
            this.button57.Size = new Size(93, 45);
            this.button57.TabIndex = 1;
            this.button57.Tag = (object)"PICKER";
            this.button57.Text = "Read Picker Controller Inputs";
            this.button57.UseVisualStyleBackColor = false;
            this.button57.Click += new EventHandler(this.OnReadPickerInputs);
            this.groupBox4.Controls.Add((Control)this.button54);
            this.groupBox4.Controls.Add((Control)this.button53);
            this.groupBox4.Controls.Add((Control)this.button51);
            this.groupBox4.Location = new Point(13, 496);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(309, 93);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Vend Door";
            this.button54.BackColor = Color.LightGray;
            this.button54.Enabled = false;
            this.button54.Location = new Point(204, 25);
            this.button54.Name = "button54";
            this.button54.Size = new Size(75, 45);
            this.button54.TabIndex = 3;
            this.button54.Tag = (object)"VENDDOOR STATUS";
            this.button54.Text = "Check Status";
            this.button54.UseVisualStyleBackColor = false;
            this.button54.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.button53.BackColor = Color.LightGray;
            this.button53.Enabled = false;
            this.button53.Location = new Point(105, 25);
            this.button53.Name = "button53";
            this.button53.Size = new Size(75, 45);
            this.button53.TabIndex = 2;
            this.button53.Tag = (object)"VENDDOOR CLOSE";
            this.button53.Text = "Close";
            this.button53.UseVisualStyleBackColor = false;
            this.button53.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button51.BackColor = Color.LightGray;
            this.button51.Enabled = false;
            this.button51.Location = new Point(6, 25);
            this.button51.Name = "button51";
            this.button51.Size = new Size(75, 45);
            this.button51.TabIndex = 0;
            this.button51.Tag = (object)"VENDDOOR RENT";
            this.button51.Text = "Rent";
            this.button51.UseVisualStyleBackColor = false;
            this.button51.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.groupBox5.Location = new Point(328, 9);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(373, 146);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sensors";
            this.groupBox6.Controls.Add((Control)this.m_rollerTimeoutTextBox);
            this.groupBox6.Controls.Add((Control)this.label17);
            this.groupBox6.Controls.Add((Control)this.m_rollToSensorText);
            this.groupBox6.Controls.Add((Control)this.button39);
            this.groupBox6.Controls.Add((Control)this.button38);
            this.groupBox6.Controls.Add((Control)this.button37);
            this.groupBox6.Controls.Add((Control)this.button36);
            this.groupBox6.Location = new Point(328, 161);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(373, 119);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Rollers";
            this.m_rollerTimeoutTextBox.Location = new Point(243, 35);
            this.m_rollerTimeoutTextBox.MaxLength = 5;
            this.m_rollerTimeoutTextBox.Name = "m_rollerTimeoutTextBox";
            this.m_rollerTimeoutTextBox.Size = new Size(87, 20);
            this.m_rollerTimeoutTextBox.TabIndex = 6;
            this.label17.AutoSize = true;
            this.label17.Location = new Point(240, 20);
            this.label17.Name = "label17";
            this.label17.Size = new Size(101, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Roll To Timeout(ms)";
            this.m_rollToSensorText.Location = new Point(240, 93);
            this.m_rollToSensorText.Name = "m_rollToSensorText";
            this.m_rollToSensorText.Size = new Size(50, 20);
            this.m_rollToSensorText.TabIndex = 4;
            this.button39.BackColor = Color.LightGray;
            this.button39.Enabled = false;
            this.button39.Location = new Point(146, 68);
            this.button39.Name = "button39";
            this.button39.Size = new Size(88, 45);
            this.button39.TabIndex = 3;
            this.button39.Text = "Roll To Sensor";
            this.button39.UseVisualStyleBackColor = false;
            this.button39.Click += new EventHandler(this.OnRollerToPosition);
            this.button38.BackColor = Color.LightGray;
            this.button38.Enabled = false;
            this.button38.Location = new Point(23, 69);
            this.button38.Name = "button38";
            this.button38.Size = new Size(105, 45);
            this.button38.TabIndex = 2;
            this.button38.Tag = (object)"ROLLER STOP";
            this.button38.Text = "Stop";
            this.button38.UseVisualStyleBackColor = false;
            this.button38.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button37.BackColor = Color.LightGray;
            this.button37.Enabled = false;
            this.button37.Location = new Point(146, 18);
            this.button37.Name = "button37";
            this.button37.Size = new Size(88, 45);
            this.button37.TabIndex = 1;
            this.button37.Tag = (object)"ROLLER OUT";
            this.button37.Text = "Out";
            this.button37.UseVisualStyleBackColor = false;
            this.button37.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button36.BackColor = Color.LightGray;
            this.button36.Enabled = false;
            this.button36.Location = new Point(23, 18);
            this.button36.Name = "button36";
            this.button36.Size = new Size(105, 45);
            this.button36.TabIndex = 0;
            this.button36.Tag = (object)"ROLLER IN";
            this.button36.Text = "In";
            this.button36.UseVisualStyleBackColor = false;
            this.button36.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.groupBox7.Controls.Add((Control)this.button64);
            this.groupBox7.Controls.Add((Control)this.button35);
            this.groupBox7.Controls.Add((Control)this.button34);
            this.groupBox7.Controls.Add((Control)this.m_gripperSellButton);
            this.groupBox7.Controls.Add((Control)this.button32);
            this.groupBox7.Controls.Add((Control)this.button31);
            this.groupBox7.Location = new Point(328, 286);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(373, 125);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Gripper";
            this.button64.BackColor = Color.LightGray;
            this.button64.Enabled = false;
            this.button64.Location = new Point(10, 19);
            this.button64.Name = "button64";
            this.button64.Size = new Size(97, 45);
            this.button64.TabIndex = 5;
            this.button64.Tag = (object)"GRIPPER PEEK";
            this.button64.Text = "Check for DVD In Slot";
            this.button64.UseVisualStyleBackColor = false;
            this.button64.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.button35.BackColor = Color.LightGray;
            this.button35.Enabled = false;
            this.button35.Location = new Point(241, 70);
            this.button35.Name = "button35";
            this.button35.Size = new Size(105, 45);
            this.button35.TabIndex = 4;
            this.button35.Tag = (object)"GRIPPER CLOSE";
            this.button35.Text = "Close";
            this.button35.UseVisualStyleBackColor = false;
            this.button35.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button34.BackColor = Color.LightGray;
            this.button34.Enabled = false;
            this.button34.Location = new Point(10, 70);
            this.button34.Name = "button34";
            this.button34.Size = new Size(97, 45);
            this.button34.TabIndex = 3;
            this.button34.Tag = (object)"GRIPPER RENT";
            this.button34.Text = "Rent";
            this.button34.UseVisualStyleBackColor = false;
            this.button34.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.m_gripperSellButton.BackColor = Color.LightGray;
            this.m_gripperSellButton.Enabled = false;
            this.m_gripperSellButton.Location = new Point(125, 70);
            this.m_gripperSellButton.Name = "m_gripperSellButton";
            this.m_gripperSellButton.Size = new Size(96, 45);
            this.m_gripperSellButton.TabIndex = 2;
            this.m_gripperSellButton.Tag = (object)"GRIPPER OPEN";
            this.m_gripperSellButton.Text = "Sell";
            this.m_gripperSellButton.UseVisualStyleBackColor = false;
            this.m_gripperSellButton.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button32.BackColor = Color.LightGray;
            this.button32.Enabled = false;
            this.button32.Location = new Point(241, 19);
            this.button32.Name = "button32";
            this.button32.Size = new Size(105, 45);
            this.button32.TabIndex = 1;
            this.button32.Tag = (object)"GRIPPER RETRACT";
            this.button32.Text = "Retract";
            this.button32.UseVisualStyleBackColor = false;
            this.button32.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button31.BackColor = Color.LightGray;
            this.button31.Enabled = false;
            this.button31.Location = new Point(125, 19);
            this.button31.Name = "button31";
            this.button31.Size = new Size(97, 45);
            this.button31.TabIndex = 0;
            this.button31.Tag = (object)"GRIPPER EXTEND";
            this.button31.Text = "Extend";
            this.button31.UseVisualStyleBackColor = false;
            this.button31.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.groupBox8.Controls.Add((Control)this.button30);
            this.groupBox8.Controls.Add((Control)this.button29);
            this.groupBox8.Controls.Add((Control)this.button28);
            this.groupBox8.Location = new Point(328, 417);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new Size(373, 73);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Track";
            this.button30.BackColor = Color.LightGray;
            this.button30.Enabled = false;
            this.button30.Location = new Point(249, 19);
            this.button30.Name = "button30";
            this.button30.Size = new Size(97, 45);
            this.button30.TabIndex = 2;
            this.button30.Tag = (object)"TRACK STATUS";
            this.button30.Text = "Status";
            this.button30.UseVisualStyleBackColor = false;
            this.button30.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.button29.BackColor = Color.LightGray;
            this.button29.Enabled = false;
            this.button29.Location = new Point(125, 19);
            this.button29.Name = "button29";
            this.button29.Size = new Size(108, 45);
            this.button29.TabIndex = 1;
            this.button29.Tag = (object)"TRACK CLOSE";
            this.button29.Text = "Close";
            this.button29.UseVisualStyleBackColor = false;
            this.button29.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button28.BackColor = Color.LightGray;
            this.button28.Enabled = false;
            this.button28.Location = new Point(10, 19);
            this.button28.Name = "button28";
            this.button28.Size = new Size(97, 45);
            this.button28.TabIndex = 0;
            this.button28.Tag = (object)"TRACK OPEN";
            this.button28.Text = "Open";
            this.button28.UseVisualStyleBackColor = false;
            this.button28.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmDownButton);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmUpButton);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmDoorStatus);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmCaseStatusButton);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmStopButton);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmDisengageButton);
            this.m_qlmGroupBox.Controls.Add((Control)this.m_qlmEngageButton);
            this.m_qlmGroupBox.Location = new Point(328, 497);
            this.m_qlmGroupBox.Name = "m_qlmGroupBox";
            this.m_qlmGroupBox.Size = new Size(373, 118);
            this.m_qlmGroupBox.TabIndex = 8;
            this.m_qlmGroupBox.TabStop = false;
            this.m_qlmGroupBox.Text = "QLM";
            this.m_qlmDownButton.BackColor = Color.LightGray;
            this.m_qlmDownButton.Enabled = false;
            this.m_qlmDownButton.Location = new Point(90, 70);
            this.m_qlmDownButton.Name = "m_qlmDownButton";
            this.m_qlmDownButton.Size = new Size(75, 40);
            this.m_qlmDownButton.TabIndex = 6;
            this.m_qlmDownButton.Tag = (object)"QLM DROP";
            this.m_qlmDownButton.Text = "Down";
            this.m_qlmDownButton.UseVisualStyleBackColor = false;
            this.m_qlmDownButton.Click += new EventHandler(this.OnDropQLM);
            this.m_qlmUpButton.BackColor = Color.LightGray;
            this.m_qlmUpButton.Enabled = false;
            this.m_qlmUpButton.Location = new Point(10, 70);
            this.m_qlmUpButton.Name = "m_qlmUpButton";
            this.m_qlmUpButton.Size = new Size(75, 40);
            this.m_qlmUpButton.TabIndex = 5;
            this.m_qlmUpButton.Tag = (object)"QLM LIFT";
            this.m_qlmUpButton.Text = "Up";
            this.m_qlmUpButton.UseVisualStyleBackColor = false;
            this.m_qlmUpButton.Click += new EventHandler(this.OnExecuteInstructionBlind);
            this.m_qlmDoorStatus.BackColor = Color.LightGray;
            this.m_qlmDoorStatus.Enabled = false;
            this.m_qlmDoorStatus.Location = new Point(262, 65);
            this.m_qlmDoorStatus.Name = "m_qlmDoorStatus";
            this.m_qlmDoorStatus.Size = new Size(75, 45);
            this.m_qlmDoorStatus.TabIndex = 4;
            this.m_qlmDoorStatus.Tag = (object)"QLMDOOR STATUS";
            this.m_qlmDoorStatus.Text = "Door Status";
            this.m_qlmDoorStatus.UseVisualStyleBackColor = false;
            this.m_qlmDoorStatus.Visible = false;
            this.m_qlmDoorStatus.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.m_qlmCaseStatusButton.BackColor = Color.LightGray;
            this.m_qlmCaseStatusButton.Enabled = false;
            this.m_qlmCaseStatusButton.Location = new Point((int)byte.MaxValue, 18);
            this.m_qlmCaseStatusButton.Name = "m_qlmCaseStatusButton";
            this.m_qlmCaseStatusButton.Size = new Size(75, 45);
            this.m_qlmCaseStatusButton.TabIndex = 3;
            this.m_qlmCaseStatusButton.Tag = (object)"QLM STATUS";
            this.m_qlmCaseStatusButton.Text = "Case Status";
            this.m_qlmCaseStatusButton.UseVisualStyleBackColor = false;
            this.m_qlmCaseStatusButton.Click += new EventHandler(this.OnExecuteInstructionRaw);
            this.m_qlmStopButton.BackColor = Color.LightGray;
            this.m_qlmStopButton.Enabled = false;
            this.m_qlmStopButton.Location = new Point(172, 19);
            this.m_qlmStopButton.Name = "m_qlmStopButton";
            this.m_qlmStopButton.Size = new Size(75, 45);
            this.m_qlmStopButton.TabIndex = 2;
            this.m_qlmStopButton.Tag = (object)"QLM HALT";
            this.m_qlmStopButton.Text = "Stop";
            this.m_qlmStopButton.UseVisualStyleBackColor = false;
            this.m_qlmStopButton.Click += new EventHandler(this.OnExecuteInstructionBlind);
            this.m_qlmDisengageButton.BackColor = Color.LightGray;
            this.m_qlmDisengageButton.Enabled = false;
            this.m_qlmDisengageButton.Location = new Point(91, 19);
            this.m_qlmDisengageButton.Name = "m_qlmDisengageButton";
            this.m_qlmDisengageButton.Size = new Size(75, 45);
            this.m_qlmDisengageButton.TabIndex = 1;
            this.m_qlmDisengageButton.Tag = (object)"QLM DISENGAGE";
            this.m_qlmDisengageButton.Text = "Disengage";
            this.m_qlmDisengageButton.UseVisualStyleBackColor = false;
            this.m_qlmDisengageButton.Click += new EventHandler(this.OnRunQlmDisengage);
            this.m_qlmEngageButton.BackColor = Color.LightGray;
            this.m_qlmEngageButton.Enabled = false;
            this.m_qlmEngageButton.Location = new Point(10, 19);
            this.m_qlmEngageButton.Name = "m_qlmEngageButton";
            this.m_qlmEngageButton.Size = new Size(75, 45);
            this.m_qlmEngageButton.TabIndex = 0;
            this.m_qlmEngageButton.Tag = (object)"QLM ENGAGE";
            this.m_qlmEngageButton.Text = "Engage";
            this.m_qlmEngageButton.UseVisualStyleBackColor = false;
            this.m_qlmEngageButton.Click += new EventHandler(this.ExecuteEngageOrOther);
            this.groupBox10.Controls.Add((Control)this.button63);
            this.groupBox10.Controls.Add((Control)this.button62);
            this.groupBox10.Controls.Add((Control)this.button61);
            this.groupBox10.Controls.Add((Control)this.button60);
            this.groupBox10.Controls.Add((Control)this.m_destinationSlotText);
            this.groupBox10.Controls.Add((Control)this.m_destinationDeckText);
            this.groupBox10.Controls.Add((Control)this.label11);
            this.groupBox10.Controls.Add((Control)this.label10);
            this.groupBox10.Controls.Add((Control)this.button18);
            this.groupBox10.Controls.Add((Control)this.button17);
            this.groupBox10.Controls.Add((Control)this.button16);
            this.groupBox10.Controls.Add((Control)this.button14);
            this.groupBox10.Controls.Add((Control)this.button13);
            this.groupBox10.Controls.Add((Control)this.button12);
            this.groupBox10.Controls.Add((Control)this.button11);
            this.groupBox10.Controls.Add((Control)this.button10);
            this.groupBox10.Controls.Add((Control)this.button8);
            this.groupBox10.Controls.Add((Control)this.button7);
            this.groupBox10.Controls.Add((Control)this.m_sourceSlotTextBox);
            this.groupBox10.Controls.Add((Control)this.m_sourceDeckTextBox);
            this.groupBox10.Location = new Point(720, 12);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new Size(271, 358);
            this.groupBox10.TabIndex = 9;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Movements";
            this.button63.BackColor = Color.LightGray;
            this.button63.Location = new Point(133, 89);
            this.button63.Name = "button63";
            this.button63.Size = new Size(56, 29);
            this.button63.TabIndex = 25;
            this.button63.Tag = (object)"DestSlot";
            this.button63.Text = "Slot";
            this.button63.UseVisualStyleBackColor = false;
            this.button63.Click += new EventHandler(this.OnDestSlot_Click);
            this.button62.BackColor = Color.LightGray;
            this.button62.Location = new Point(12, 89);
            this.button62.Name = "button62";
            this.button62.Size = new Size(60, 29);
            this.button62.TabIndex = 24;
            this.button62.Tag = (object)"DestDeck";
            this.button62.Text = "Deck";
            this.button62.UseVisualStyleBackColor = false;
            this.button62.Click += new EventHandler(this.OnDestDeck_Click);
            this.button61.BackColor = Color.LightGray;
            this.button61.Location = new Point(133, 32);
            this.button61.Name = "button61";
            this.button61.Size = new Size(56, 32);
            this.button61.TabIndex = 23;
            this.button61.Tag = (object)"SourceSlot";
            this.button61.Text = "Slot";
            this.button61.UseVisualStyleBackColor = false;
            this.button61.Click += new EventHandler(this.OnSourceSlot_Click);
            this.button60.BackColor = Color.LightGray;
            this.button60.Location = new Point(13, 32);
            this.button60.Name = "button60";
            this.button60.Size = new Size(59, 32);
            this.button60.TabIndex = 22;
            this.button60.Tag = (object)"SourceDeck";
            this.button60.Text = "Deck";
            this.button60.UseVisualStyleBackColor = false;
            this.button60.Click += new EventHandler(this.OnSourceDeck_Click);
            this.m_destinationSlotText.Location = new Point(203, 94);
            this.m_destinationSlotText.Name = "m_destinationSlotText";
            this.m_destinationSlotText.Size = new Size(49, 20);
            this.m_destinationSlotText.TabIndex = 21;
            this.m_destinationDeckText.Location = new Point(81, 95);
            this.m_destinationDeckText.Name = "m_destinationDeckText";
            this.m_destinationDeckText.Size = new Size(40, 20);
            this.m_destinationDeckText.TabIndex = 20;
            this.label11.AutoSize = true;
            this.label11.Location = new Point(11, 73);
            this.label11.Name = "label11";
            this.label11.Size = new Size(102, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Transfer Destination";
            this.label10.AutoSize = true;
            this.label10.Location = new Point(18, 19);
            this.label10.Name = "label10";
            this.label10.Size = new Size(41, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Source";
            this.button18.BackColor = Color.LightGray;
            this.button18.Enabled = false;
            this.button18.Location = new Point(133, 308);
            this.button18.Name = "button18";
            this.button18.Size = new Size(119, 40);
            this.button18.TabIndex = 15;
            this.button18.Text = "Put in Empty Slot";
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new EventHandler(this.OnPutInEmptySlot);
            this.button17.BackColor = Color.LightGray;
            this.button17.Enabled = false;
            this.button17.Location = new Point(14, 308);
            this.button17.Name = "button17";
            this.button17.Size = new Size(102, 40);
            this.button17.TabIndex = 14;
            this.button17.Tag = (object)"";
            this.button17.Text = "Pull DVD Into Picker";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new EventHandler(this.button17_Click);
            this.button16.BackColor = Color.LightGray;
            this.button16.Enabled = false;
            this.button16.Location = new Point(14, 262);
            this.button16.Name = "button16";
            this.button16.Size = new Size(102, 40);
            this.button16.TabIndex = 13;
            this.button16.Text = "Push DVD In Slot";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new EventHandler(this.OnPushDVDInSlot);
            this.button14.BackColor = Color.LightGray;
            this.button14.Enabled = false;
            this.button14.Location = new Point(133, 262);
            this.button14.Name = "button14";
            this.button14.Size = new Size(119, 40);
            this.button14.TabIndex = 11;
            this.button14.Text = "Vend DVD in Picker";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new EventHandler(this.OnVendDVD);
            this.button13.BackColor = Color.LightGray;
            this.button13.Enabled = false;
            this.button13.Location = new Point(14, 216);
            this.button13.Name = "button13";
            this.button13.Size = new Size(102, 40);
            this.button13.TabIndex = 10;
            this.button13.Tag = (object)"MOVEVEND";
            this.button13.Text = "Move to Vend";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new EventHandler(this.OnExecuteErrorCodeInstruction);
            this.button12.BackColor = Color.LightGray;
            this.button12.Enabled = false;
            this.button12.Location = new Point(133, 216);
            this.button12.Name = "button12";
            this.button12.Size = new Size(119, 40);
            this.button12.TabIndex = 9;
            this.button12.Text = "Transfer";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new EventHandler(this.OnTransfer);
            this.button11.BackColor = Color.LightGray;
            this.button11.Enabled = false;
            this.button11.Location = new Point(14, 170);
            this.button11.Name = "button11";
            this.button11.Size = new Size(102, 40);
            this.button11.TabIndex = 8;
            this.button11.Text = "Get";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new EventHandler(this.OnGetAndRead);
            this.button10.BackColor = Color.LightGray;
            this.button10.Enabled = false;
            this.button10.Location = new Point(133, 124);
            this.button10.Name = "button10";
            this.button10.Size = new Size(119, 40);
            this.button10.TabIndex = 7;
            this.button10.Text = "Put";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new EventHandler(this.OnPut);
            this.button8.BackColor = Color.LightGray;
            this.button8.Enabled = false;
            this.button8.Location = new Point(133, 170);
            this.button8.Name = "button8";
            this.button8.Size = new Size(119, 40);
            this.button8.TabIndex = 5;
            this.button8.Text = "Sync Slot";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new EventHandler(this.OnSyncSlot);
            this.button7.BackColor = Color.LightGray;
            this.button7.Enabled = false;
            this.button7.Location = new Point(14, 124);
            this.button7.Name = "button7";
            this.button7.Size = new Size(102, 40);
            this.button7.TabIndex = 4;
            this.button7.Text = "Go To";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new EventHandler(this.OnGotoSlot);
            this.m_sourceSlotTextBox.Location = new Point(203, 39);
            this.m_sourceSlotTextBox.Name = "m_sourceSlotTextBox";
            this.m_sourceSlotTextBox.Size = new Size(49, 20);
            this.m_sourceSlotTextBox.TabIndex = 3;
            this.m_sourceDeckTextBox.Location = new Point(81, 39);
            this.m_sourceDeckTextBox.Name = "m_sourceDeckTextBox";
            this.m_sourceDeckTextBox.Size = new Size(38, 20);
            this.m_sourceDeckTextBox.TabIndex = 0;
            this.groupBox11.Controls.Add((Control)this.m_getOffXRadio);
            this.groupBox11.Controls.Add((Control)this.m_getOffYRadio);
            this.groupBox11.Controls.Add((Control)this.m_getOffsetTB);
            this.groupBox11.Controls.Add((Control)this.m_getWithOffset);
            this.groupBox11.Controls.Add((Control)this.m_sensorCheckBox);
            this.groupBox11.Controls.Add((Control)this.label16);
            this.groupBox11.Controls.Add((Control)this.label15);
            this.groupBox11.Controls.Add((Control)this.button4);
            this.groupBox11.Controls.Add((Control)this.button3);
            this.groupBox11.Controls.Add((Control)this.button2);
            this.groupBox11.Controls.Add((Control)this.m_encoderUnitsTextBox);
            this.groupBox11.Controls.Add((Control)this.button1);
            this.groupBox11.Location = new Point(720, 381);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new Size(271, 193);
            this.groupBox11.TabIndex = 10;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "X/Y Moves";
            this.m_getOffXRadio.AutoSize = true;
            this.m_getOffXRadio.Location = new Point(93, 149);
            this.m_getOffXRadio.Name = "m_getOffXRadio";
            this.m_getOffXRadio.Size = new Size(32, 17);
            this.m_getOffXRadio.TabIndex = 18;
            this.m_getOffXRadio.TabStop = true;
            this.m_getOffXRadio.Tag = (object)"X";
            this.m_getOffXRadio.Text = "X";
            this.m_getOffXRadio.UseVisualStyleBackColor = true;
            this.m_getOffXRadio.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            this.m_getOffYRadio.AutoSize = true;
            this.m_getOffYRadio.Location = new Point(93, 170);
            this.m_getOffYRadio.Name = "m_getOffYRadio";
            this.m_getOffYRadio.Size = new Size(32, 17);
            this.m_getOffYRadio.TabIndex = 17;
            this.m_getOffYRadio.TabStop = true;
            this.m_getOffYRadio.Tag = (object)"Y";
            this.m_getOffYRadio.Text = "Y";
            this.m_getOffYRadio.UseVisualStyleBackColor = true;
            this.m_getOffYRadio.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
            this.m_getOffsetTB.Location = new Point(6, 165);
            this.m_getOffsetTB.Name = "m_getOffsetTB";
            this.m_getOffsetTB.Size = new Size(81, 20);
            this.m_getOffsetTB.TabIndex = 16;
            this.m_getWithOffset.BackColor = Color.LightGray;
            this.m_getWithOffset.Enabled = false;
            this.m_getWithOffset.Location = new Point(177, 146);
            this.m_getWithOffset.Name = "m_getWithOffset";
            this.m_getWithOffset.Size = new Size(84, 41);
            this.m_getWithOffset.TabIndex = 15;
            this.m_getWithOffset.Text = "Get With Offset";
            this.m_getWithOffset.UseVisualStyleBackColor = false;
            this.m_getWithOffset.Click += new EventHandler(this.m_getWithOffset_Click);
            this.m_sensorCheckBox.AutoSize = true;
            this.m_sensorCheckBox.Location = new Point(6, 134);
            this.m_sensorCheckBox.Name = "m_sensorCheckBox";
            this.m_sensorCheckBox.Size = new Size(133, 17);
            this.m_sensorCheckBox.TabIndex = 14;
            this.m_sensorCheckBox.Text = "Without Sensor Check";
            this.m_sensorCheckBox.UseVisualStyleBackColor = true;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(174, 37);
            this.label16.Name = "label16";
            this.label16.Size = new Size(94, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "Counter-clockwise";
            this.label15.AutoSize = true;
            this.label15.Location = new Point(11, 37);
            this.label15.Name = "label15";
            this.label15.Size = new Size(55, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Clockwise";
            this.button4.BackColor = Color.LightGray;
            this.button4.Enabled = false;
            this.button4.Location = new Point(87, 91);
            this.button4.Name = "button4";
            this.button4.Size = new Size(75, 40);
            this.button4.TabIndex = 4;
            this.button4.Tag = (object)"Down";
            this.button4.Text = "Down";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new EventHandler(this.OnDirectionMove);
            this.button3.BackColor = Color.LightGray;
            this.button3.Enabled = false;
            this.button3.Location = new Point(87, 18);
            this.button3.Name = "button3";
            this.button3.Size = new Size(75, 40);
            this.button3.TabIndex = 3;
            this.button3.Tag = (object)"Up";
            this.button3.Text = "Up";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new EventHandler(this.OnDirectionMove);
            this.button2.BackColor = Color.LightGray;
            this.button2.Enabled = false;
            this.button2.Location = new Point(177, 54);
            this.button2.Name = "button2";
            this.button2.Size = new Size(75, 40);
            this.button2.TabIndex = 2;
            this.button2.Tag = (object)"Right";
            this.button2.Text = "Right";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new EventHandler(this.OnDirectionMove);
            this.m_encoderUnitsTextBox.Location = new Point(87, 65);
            this.m_encoderUnitsTextBox.Name = "m_encoderUnitsTextBox";
            this.m_encoderUnitsTextBox.Size = new Size(75, 20);
            this.m_encoderUnitsTextBox.TabIndex = 1;
            this.button1.BackColor = Color.LightGray;
            this.button1.Enabled = false;
            this.button1.Location = new Point(0, 55);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 40);
            this.button1.TabIndex = 0;
            this.button1.Tag = (object)"Left";
            this.button1.Text = "Left";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new EventHandler(this.OnDirectionMove);
            this.m_readPosButton.BackColor = Color.LightGray;
            this.m_readPosButton.Enabled = false;
            this.m_readPosButton.Location = new Point(118, 161);
            this.m_readPosButton.Name = "m_readPosButton";
            this.m_readPosButton.Size = new Size(99, 45);
            this.m_readPosButton.TabIndex = 7;
            this.m_readPosButton.Tag = (object)"";
            this.m_readPosButton.Text = "Read Positions";
            this.m_readPosButton.UseVisualStyleBackColor = false;
            this.m_readPosButton.Click += new EventHandler(this.OnReadPositions);
            this.groupBox12.Controls.Add((Control)this.m_cameraWorkingButton);
            this.groupBox12.Controls.Add((Control)this.button19);
            this.groupBox12.Controls.Add((Control)this.m_cameraPreview);
            this.groupBox12.Controls.Add((Control)this.button26);
            this.groupBox12.Controls.Add((Control)this.m_startButtonCamera);
            this.groupBox12.Controls.Add((Control)this.m_snapImageButton);
            this.groupBox12.Location = new Point(328, 621);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new Size(370, 107);
            this.groupBox12.TabIndex = 11;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Camera";
            this.m_cameraWorkingButton.BackColor = Color.LightGray;
            this.m_cameraWorkingButton.Enabled = false;
            this.m_cameraWorkingButton.Location = new Point(128, 62);
            this.m_cameraWorkingButton.Name = "m_cameraWorkingButton";
            this.m_cameraWorkingButton.Size = new Size(93, 40);
            this.m_cameraWorkingButton.TabIndex = 5;
            this.m_cameraWorkingButton.Text = "Reset CCF Counter";
            this.m_cameraWorkingButton.UseVisualStyleBackColor = false;
            this.m_cameraWorkingButton.Click += new EventHandler(this.m_cameraWorkingButton_Click);
            this.button19.BackColor = Color.LightGray;
            this.button19.Enabled = false;
            this.button19.Location = new Point(262, 13);
            this.button19.Name = "button19";
            this.button19.Size = new Size(93, 40);
            this.button19.TabIndex = 4;
            this.button19.Text = "Get and Read";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new EventHandler(this.button19_Click);
            this.m_cameraPreview.BackColor = Color.LightGray;
            this.m_cameraPreview.Enabled = false;
            this.m_cameraPreview.Location = new Point(128, 13);
            this.m_cameraPreview.Name = "m_cameraPreview";
            this.m_cameraPreview.Size = new Size(93, 40);
            this.m_cameraPreview.TabIndex = 3;
            this.m_cameraPreview.Text = "Camera Settings and Preview";
            this.m_cameraPreview.UseVisualStyleBackColor = false;
            this.m_cameraPreview.Click += new EventHandler(this.OnLaunchCameraProperties);
            this.button26.BackColor = Color.LightGray;
            this.button26.Enabled = false;
            this.button26.Location = new Point(262, 62);
            this.button26.Name = "button26";
            this.button26.Size = new Size(93, 40);
            this.button26.TabIndex = 2;
            this.button26.Text = "Read Disk In Picker";
            this.button26.UseVisualStyleBackColor = false;
            this.button26.Click += new EventHandler(this.OnReadDiskInPicker);
            this.m_startButtonCamera.BackColor = Color.LightGray;
            this.m_startButtonCamera.Enabled = false;
            this.m_startButtonCamera.Location = new Point(10, 62);
            this.m_startButtonCamera.Name = "m_startButtonCamera";
            this.m_startButtonCamera.Size = new Size(93, 40);
            this.m_startButtonCamera.TabIndex = 1;
            this.m_startButtonCamera.Tag = (object)"";
            this.m_startButtonCamera.Text = "Turn Ringlight On";
            this.m_startButtonCamera.UseVisualStyleBackColor = false;
            this.m_startButtonCamera.Click += new EventHandler(this.OnStartCamera);
            this.m_snapImageButton.BackColor = Color.LightGray;
            this.m_snapImageButton.Enabled = false;
            this.m_snapImageButton.Location = new Point(10, 16);
            this.m_snapImageButton.Name = "m_snapImageButton";
            this.m_snapImageButton.Size = new Size(93, 40);
            this.m_snapImageButton.TabIndex = 0;
            this.m_snapImageButton.Tag = (object)"";
            this.m_snapImageButton.Text = "Snap";
            this.m_snapImageButton.UseVisualStyleBackColor = false;
            this.m_snapImageButton.Click += new EventHandler(this.OnDoSnap);
            this.m_outputBox.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.m_outputBox.FormattingEnabled = true;
            this.m_outputBox.HorizontalScrollbar = true;
            this.m_outputBox.ItemHeight = 14;
            this.m_outputBox.Location = new Point(13, 25);
            this.m_outputBox.Name = "m_outputBox";
            this.m_outputBox.Size = new Size(301, 130);
            this.m_outputBox.TabIndex = 12;
            this.m_clearOutputButton.BackColor = Color.LightGray;
            this.m_clearOutputButton.Enabled = false;
            this.m_clearOutputButton.Location = new Point(13, 161);
            this.m_clearOutputButton.Name = "m_clearOutputButton";
            this.m_clearOutputButton.Size = new Size(99, 45);
            this.m_clearOutputButton.TabIndex = 14;
            this.m_clearOutputButton.Text = "Clear Output";
            this.m_clearOutputButton.UseVisualStyleBackColor = false;
            this.m_clearOutputButton.Click += new EventHandler(this.OnClearOutput);
            this.label14.AutoSize = true;
            this.label14.Location = new Point(10, 9);
            this.label14.Name = "label14";
            this.label14.Size = new Size(39, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Output";
            this.m_updateConfigurationButton.BackColor = Color.LightGray;
            this.m_updateConfigurationButton.Enabled = false;
            this.m_updateConfigurationButton.Location = new Point(720, 580);
            this.m_updateConfigurationButton.Name = "m_updateConfigurationButton";
            this.m_updateConfigurationButton.Size = new Size(132, 40);
            this.m_updateConfigurationButton.TabIndex = 16;
            this.m_updateConfigurationButton.Text = "Update Configuration";
            this.m_updateConfigurationButton.UseVisualStyleBackColor = false;
            this.m_updateConfigurationButton.Click += new EventHandler(this.button15_Click);
            this.m_versionTextBox.Location = new Point(859, 676);
            this.m_versionTextBox.Name = "m_versionTextBox";
            this.m_versionTextBox.ReadOnly = true;
            this.m_versionTextBox.Size = new Size(100, 20);
            this.m_versionTextBox.TabIndex = 17;
            this.label19.AutoSize = true;
            this.label19.Location = new Point(717, 683);
            this.label19.Name = "label19";
            this.label19.Size = new Size(78, 13);
            this.label19.TabIndex = 18;
            this.label19.Text = "Tester Version:";
            this.label20.AutoSize = true;
            this.label20.Location = new Point(717, 709);
            this.label20.Name = "label20";
            this.label20.Size = new Size(50, 13);
            this.label20.TabIndex = 19;
            this.label20.Text = "Kiosk ID:";
            this.m_kioskIDTextBox.Location = new Point(859, 702);
            this.m_kioskIDTextBox.Name = "m_kioskIDTextBox";
            this.m_kioskIDTextBox.ReadOnly = true;
            this.m_kioskIDTextBox.Size = new Size(100, 20);
            this.m_kioskIDTextBox.TabIndex = 20;
            this.m_kioskIDTextBox.Text = "UNKNOWN";
            this.groupBox1.Controls.Add((Control)this.button22);
            this.groupBox1.Controls.Add((Control)this.m_qlmTestButton);
            this.groupBox1.Controls.Add((Control)this.m_readLimitsButton);
            this.groupBox1.Controls.Add((Control)this.m_showTimeoutLogButton);
            this.groupBox1.Controls.Add((Control)this.button20);
            this.groupBox1.Controls.Add((Control)this.button66);
            this.groupBox1.Location = new Point(13, 596);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(309, 132);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Other";
            this.button22.BackColor = Color.LightGray;
            this.button22.Enabled = false;
            this.button22.Location = new Point(107, 17);
            this.button22.Name = "button22";
            this.button22.Size = new Size(75, 46);
            this.button22.TabIndex = 7;
            this.button22.Text = "Sync Unknowns";
            this.button22.UseVisualStyleBackColor = false;
            this.button22.Click += new EventHandler(this.button22_Click);
            this.m_qlmTestButton.BackColor = Color.LightGray;
            this.m_qlmTestButton.Enabled = false;
            this.m_qlmTestButton.Location = new Point(8, 17);
            this.m_qlmTestButton.Name = "m_qlmTestButton";
            this.m_qlmTestButton.Size = new Size(75, 46);
            this.m_qlmTestButton.TabIndex = 6;
            this.m_qlmTestButton.Text = "Qlm Slot Test";
            this.m_qlmTestButton.UseVisualStyleBackColor = false;
            this.m_qlmTestButton.Click += new EventHandler(this.button21_Click);
            this.m_readLimitsButton.BackColor = Color.LightGray;
            this.m_readLimitsButton.Enabled = false;
            this.m_readLimitsButton.Location = new Point(204, 69);
            this.m_readLimitsButton.Name = "m_readLimitsButton";
            this.m_readLimitsButton.Size = new Size(75, 57);
            this.m_readLimitsButton.TabIndex = 5;
            this.m_readLimitsButton.Text = "Read Controller Limits";
            this.m_readLimitsButton.UseVisualStyleBackColor = false;
            this.m_readLimitsButton.Click += new EventHandler(this.m_readLimitsButton_Click);
            this.m_showTimeoutLogButton.BackColor = Color.LightGray;
            this.m_showTimeoutLogButton.Enabled = false;
            this.m_showTimeoutLogButton.Location = new Point(105, 69);
            this.m_showTimeoutLogButton.Name = "m_showTimeoutLogButton";
            this.m_showTimeoutLogButton.Size = new Size(75, 57);
            this.m_showTimeoutLogButton.TabIndex = 4;
            this.m_showTimeoutLogButton.Text = "Show Timeouts";
            this.m_showTimeoutLogButton.UseVisualStyleBackColor = false;
            this.m_showTimeoutLogButton.Click += new EventHandler(this.m_showTimeoutLogButton_Click);
            this.button20.BackColor = Color.LightGray;
            this.button20.Enabled = false;
            this.button20.Location = new Point(6, 69);
            this.button20.Name = "button20";
            this.button20.Size = new Size(75, 57);
            this.button20.TabIndex = 3;
            this.button20.Text = "Vertical Slot Test";
            this.button20.UseVisualStyleBackColor = false;
            this.button20.Click += new EventHandler(this.button20_Click);
            this.button66.BackColor = Color.LightGray;
            this.button66.Enabled = false;
            this.button66.Location = new Point(204, 18);
            this.button66.Name = "button66";
            this.button66.Size = new Size(75, 45);
            this.button66.TabIndex = 2;
            this.button66.Text = "Hardware Check";
            this.button66.UseVisualStyleBackColor = false;
            this.button66.Click += new EventHandler(this.button66_Click);
            this.m_errorProvider.ContainerControl = (ContainerControl)this;
            this.m_deckConfigurationButton.BackColor = Color.LightGray;
            this.m_deckConfigurationButton.Enabled = false;
            this.m_deckConfigurationButton.Location = new Point(859, 580);
            this.m_deckConfigurationButton.Name = "m_deckConfigurationButton";
            this.m_deckConfigurationButton.Size = new Size(132, 40);
            this.m_deckConfigurationButton.TabIndex = 23;
            this.m_deckConfigurationButton.Text = "Deck Configuration";
            this.m_deckConfigurationButton.UseVisualStyleBackColor = false;
            this.m_deckConfigurationButton.Click += new EventHandler(this.button58_Click);
            this.m_takeDiskButton.BackColor = Color.LightGray;
            this.m_takeDiskButton.Enabled = false;
            this.m_takeDiskButton.Location = new Point(721, 626);
            this.m_takeDiskButton.Name = "m_takeDiskButton";
            this.m_takeDiskButton.Size = new Size(132, 44);
            this.m_takeDiskButton.TabIndex = 27;
            this.m_takeDiskButton.Text = "Unknown Count";
            this.m_takeDiskButton.UseVisualStyleBackColor = false;
            this.m_takeDiskButton.Click += new EventHandler(this.button65_Click);
            this.m_openLogsButton.BackColor = Color.LightGray;
            this.m_openLogsButton.Enabled = false;
            this.m_openLogsButton.Location = new Point(223, 161);
            this.m_openLogsButton.Name = "m_openLogsButton";
            this.m_openLogsButton.Size = new Size(99, 45);
            this.m_openLogsButton.TabIndex = 28;
            this.m_openLogsButton.Text = "Open Error Logs";
            this.m_openLogsButton.UseVisualStyleBackColor = false;
            this.m_openLogsButton.Click += new EventHandler(this.button59_Click);
            this.m_configureDevicesButton.BackColor = Color.LightGray;
            this.m_configureDevicesButton.Enabled = false;
            this.m_configureDevicesButton.Location = new Point(859, 626);
            this.m_configureDevicesButton.Name = "m_configureDevicesButton";
            this.m_configureDevicesButton.Size = new Size(132, 44);
            this.m_configureDevicesButton.TabIndex = 29;
            this.m_configureDevicesButton.Text = "Configure Devices";
            this.m_configureDevicesButton.UseVisualStyleBackColor = false;
            this.m_configureDevicesButton.Click += new EventHandler(this.button67_Click);
            this.m_startupWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.m_startupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.m_initWorker.DoWork += new DoWorkEventHandler(this.m_initWorker_DoWork);
            this.m_initWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.m_initWorker_RunWorkerCompleted);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.ClientSize = new Size(1013, 742);
            this.Controls.Add((Control)this.m_configureDevicesButton);
            this.Controls.Add((Control)this.m_openLogsButton);
            this.Controls.Add((Control)this.m_takeDiskButton);
            this.Controls.Add((Control)this.m_deckConfigurationButton);
            this.Controls.Add((Control)this.m_readPosButton);
            this.Controls.Add((Control)this.groupBox1);
            this.Controls.Add((Control)this.m_kioskIDTextBox);
            this.Controls.Add((Control)this.label20);
            this.Controls.Add((Control)this.label19);
            this.Controls.Add((Control)this.m_versionTextBox);
            this.Controls.Add((Control)this.m_updateConfigurationButton);
            this.Controls.Add((Control)this.label14);
            this.Controls.Add((Control)this.m_clearOutputButton);
            this.Controls.Add((Control)this.m_outputBox);
            this.Controls.Add((Control)this.groupBox12);
            this.Controls.Add((Control)this.groupBox11);
            this.Controls.Add((Control)this.groupBox10);
            this.Controls.Add((Control)this.m_qlmGroupBox);
            this.Controls.Add((Control)this.groupBox8);
            this.Controls.Add((Control)this.groupBox7);
            this.Controls.Add((Control)this.groupBox6);
            this.Controls.Add((Control)this.groupBox5);
            this.Controls.Add((Control)this.groupBox4);
            this.Controls.Add((Control)this.groupBox3);
            this.Controls.Add((Control)this.groupBox2);
            this.Name = nameof(Form1);
            this.Text = "HAL Tester";
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.m_qlmGroupBox.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((ISupportInitialize)this.m_errorProvider).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private enum Direction
        {
            Unknown,
            Left,
            Right,
            Up,
            Down,
        }
    }
}
