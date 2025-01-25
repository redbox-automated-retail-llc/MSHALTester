using Redbox.HAL.Client;
using Redbox.HAL.Client.Executors;
using Redbox.HAL.Client.Services;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.Component.Model.Extensions;
using Redbox.HAL.Component.Model.Timers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;


namespace Redbox.HAL.MSHALTester
{
    public class ConfiguredDevicesForm : Form
    {
        private bool m_allowSessionDisplay;
        private KioskFunctionTest FunctionTest;
        private readonly HardwareService Service;
        private readonly OutputBox KioskTestOutput;
        private readonly OutputBox AirExchangerOutput;
        private readonly OutputBox FraudSensorOutput;
        private readonly OutputBox OutputBox;
        private readonly OutputBox EngineeringOutput;
        private readonly OutputBox HardwareStatsOutput;
        private readonly ButtonAspectsManager Manager;
        private readonly OutputBox IrOutput;
        private bool m_irConfigured;
        private readonly byte[] CenterItemProgram;
        private readonly AirExchangerState ExchangerState;
        private ScannerServices ScannerService = ScannerServices.Emulated;
        private bool HasFraudSensor;
        private bool HasRouterPowerRelay;
        private bool ArcusResetConfigured;
        private bool m_displayEngineeringTab;
        private IContainer components;
        private TabControl m_devicesTab;
        private TabPage m_airExchangerTab;
        private Label m_iceQubeExchangerLabel;
        private Button m_configureQubeButton;
        private Button m_resetQubeBoard;
        private Button m_qubeStatusButton;
        private Button m_resetPersistentCounter;
        private Button m_exitButton;
        private TabPage m_cortexTab;
        private Button m_installCortexButton;
        private Button m_uninstallCortexButton;
        private Label label3;
        private ListBox m_cortexStatusBox;
        private Button m_startCortexButton;
        private TabPage m_fraudSensorTab;
        private ListBox m_fraudSensorOutput;
        private Button m_fraudPOSTButton;
        private Button m_testFraudButton;
        private Button m_configureFraudSensor;
        private TextBox m_fraudDeckTB;
        private GroupBox groupBox1;
        private Button button1;
        private TextBox m_fraudSlotTB;
        private Button button4;
        private ListBox m_iceQubeOutput;
        private Button m_resetFraudButton;
        private Button button6;
        private Button button5;
        private Button button7;
        private Button m_stopFraudScan;
        private Button m_startFraudScan;
        private Button m_testDiskForMarkers;
        private Button button3;
        private Button button10;
        private Button m_powerOnFraudSensor;
        private Button m_powerOffFraudSensor;
        private Button button13;
        private TextBox m_scanPauseTB;
        private Label label4;
        private Label label2;
        private TextBox m_stressIterationsTB;
        private Label label5;
        private TextBox m_readPauseTB;
        private Button button14;
        private Button button15;
        private TabPage m_kioskFunctionCheck;
        private Button button2;
        private ListBox m_kioskTestOutput;
        private TextBox m_userName;
        private Label label1;
        private GroupBox groupBox2;
        private Button btn_KFCCheckDrivers;
        private Button btn_KFCInit;
        private Button btn_KFCTestVendDoor;
        private Button btn_KFCVerticalSlotTest;
        private Button btn_KFCUnknownCount;
        private Button button19;
        private TextBox m_verticalTestTB;
        private TextBox m_initTB;
        private TextBox m_testVendDoorTB;
        private TextBox m_checkDriversStatusTB;
        private TextBox m_unknownCountTB;
        private TextBox m_cameraSnapTB;
        private Button btn_KFCDecodeTest;
        private ErrorProvider errorProvider1;
        private TextBox m_sourceDeckTB;
        private Button button21;
        private TextBox m_sourceSlotTB;
        private Button button22;
        private Button m_displaySessionsButton;
        private TabPage tabPage1;
        private Button button16;
        private Button button11;
        private ListView listView1;
        private TabPage m_hwCorrections;
        private Button button23;
        private ListBox m_hwStatsOutput;
        private Button button24;
        private GroupBox m_hwCorrectiontypeGB;
        private RadioButton radioButton3;
        private RadioButton radioButton1;
        private TabPage m_engineeringTab;
        private Button button25;
        private ListBox m_engineeringOutput;
        private Button button26;
        private Button button27;
        private Button button28;
        private Button button29;
        private TextBox m_engSlotTB;
        private TextBox m_engDeckTB;
        private Button button30;
        private GroupBox groupBox3;
        private Button button31;
        private TextBox m_engEndSlotTB;
        private TextBox m_engEndDeckTB;
        private Button button32;
        private Button button34;
        private Button button33;
        private Button button36;
        private Button button35;
        private GroupBox groupBox5;
        private Button m_restartTouchscreen;
        private GroupBox groupBox6;
        private Button m_restartArcus;
        private Button m_configureArcus;
        private RadioButton radioButton2;
        private RadioButton radioButton5;
        private RadioButton radioButton4;
        private GroupBox groupBox4;
        private Button m_configureRouterRelay;
        private Button m_testRouterRelay;
        private BackgroundWorker m_verticalSyncWorker;
        private BackgroundWorker m_runInitWorker;
        private BackgroundWorker m_runDecodeWorker;
        private TabPage m_irConfigurationTab;
        private ListBox m_irCameraOutput;
        private Button button37;
        private GroupBox groupBox8;
        private TextBox textBox2;
        private TextBox textBox1;
        private Button button47;
        private Button button46;
        private Button button45;
        private Button button44;
        private Button button43;
        private GroupBox groupBox7;
        private Button button42;
        private Button button41;
        private Button button40;
        private Button button39;
        private Button button38;

        public ConfiguredDevicesForm(HardwareService service)
        {
            this.InitializeComponent();
            this.OutputBox = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_cortexStatusBox);
            this.Service = service;
            this.m_devicesTab.Selected += new TabControlEventHandler(this.OnTabSelectedChange);
            this.Manager = new ButtonAspectsManager();
            this.AirExchangerOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_iceQubeOutput);
            this.FraudSensorOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_fraudSensorOutput);
            this.KioskTestOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_kioskTestOutput);
            this.HardwareStatsOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_hwStatsOutput);
            this.EngineeringOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_engineeringOutput);
            this.ExchangerState = new AirExchangerState(service);
            this.IrOutput = (OutputBox)new ConfiguredDevicesForm.ConfiguredDeviceOuptutBox(this.m_irCameraOutput);
            using (MachineConfiguration machineConfiguration = new MachineConfiguration(this.Service))
            {
                machineConfiguration.Run();
                int num = (int)this.ExchangerState.Configure(machineConfiguration.AirExchangerStatus, machineConfiguration.AirExchangerFanStatus);
                this.HasFraudSensor = machineConfiguration.HasFraudDevice;
                this.ScannerService = machineConfiguration.ConfiguredCamera;
                this.HasRouterPowerRelay = machineConfiguration.HasRouterPowerRelay;
                this.ArcusResetConfigured = machineConfiguration.ArcusResetConfigured;
                this.m_irConfigured = machineConfiguration.HasIRHardware;
                this.btn_KFCCheckDrivers.Enabled = !machineConfiguration.KFCDisableCheckDrivers;
                this.btn_KFCDecodeTest.Enabled = !machineConfiguration.KFCDisableDecodeTest;
                this.btn_KFCTestVendDoor.Enabled = !machineConfiguration.KFCDisableVendDoorTest;
                this.btn_KFCInit.Enabled = !machineConfiguration.KFCDisableInit;
                this.btn_KFCVerticalSlotTest.Enabled = !machineConfiguration.KFCDisableVerticalSlotTest;
                this.btn_KFCUnknownCount.Enabled = !machineConfiguration.KFCDisableUnknownCount;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("ROLLER POS=6 TIMEOUT=4000 WAIT=TRUE ");
            stringBuilder.AppendLine(" ROLLER POS=3 TIMEOUT=4000 WAIT=TRUE ");
            stringBuilder.AppendLine(" CLEAR");
            this.CenterItemProgram = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            this.listView1.Columns.Add("Component", 200);
            this.listView1.Columns.Add("Value", 200);
            this.RouterPowerCyclePause = 1500;
        }

        public bool DisplayEngineeringTab
        {
            get => this.m_displayEngineeringTab;
            set
            {
                this.m_displayEngineeringTab = value;
                if (this.m_displayEngineeringTab)
                    return;
                this.m_devicesTab.TabPages.Remove(this.m_engineeringTab);
            }
        }

        public int RouterPowerCyclePause { get; set; }

        public bool AllowSessionDisplay
        {
            get => this.m_allowSessionDisplay;
            set
            {
                this.m_allowSessionDisplay = value;
                this.m_displaySessionsButton.Enabled = this.m_displaySessionsButton.Visible = this.m_allowSessionDisplay;
            }
        }

        private void ConfigureCortexPanel()
        {
            bool flag = ScannerServices.Cortex == this.ScannerService;
            this.m_installCortexButton.Enabled = !flag;
            this.m_uninstallCortexButton.Enabled = flag;
            this.WriteToBox(string.Format("The camera {0} enabled.", flag ? (object)"is" : (object)"is not"));
        }

        private void OnConfigureIceQubeBoard()
        {
            bool configured = this.ExchangerState.Configured;
            this.m_iceQubeExchangerLabel.Text = string.Format("The board {0} configured.", configured ? (object)"is" : (object)"is not");
            this.m_configureQubeButton.Text = configured ? "Remove" : "Configure";
            this.m_resetQubeBoard.Enabled = configured;
            this.m_qubeStatusButton.Enabled = configured;
            this.m_resetPersistentCounter.Enabled = configured;
        }

        private void OnTabSelectedChange(object sender, EventArgs e)
        {
            switch (this.m_devicesTab.SelectedIndex)
            {
                case 0:
                    this.OnConfigureIceQubeBoard();
                    break;
                case 1:
                    this.ConfigureCortexPanel();
                    break;
                case 2:
                    this.OnConfigureFraudSensor();
                    break;
                case 3:
                    this.OnConfigureKioskTest();
                    break;
                case 5:
                    this.ConfigureRouterGroup();
                    this.ConfigureArcusGroup();
                    break;
                case 6:
                    this.ConfigureIrCameraGroup();
                    break;
            }
        }

        private void ConfigureIrCameraGroup()
        {
            bool flag = ScannerServices.Cortex == this.ScannerService;
            if (!flag)
                this.button37.Text = this.m_irConfigured ? "Un-configure" : "Configure";
            else
                this.button37.Text = "Unavailable";
            this.button38.Enabled = this.button39.Enabled = this.button40.Enabled = this.button41.Enabled = !flag;
            this.button42.Enabled = this.button43.Enabled = this.button46.Enabled = this.button47.Enabled = !flag;
            this.button44.Enabled = this.button45.Enabled = this.button37.Enabled = !flag;
        }

        private void OnConfigureKioskTest()
        {
            ISessionUser currentSession = ServiceLocator.Instance.GetService<ISessionUserService>().GetCurrentSession();
            this.FunctionTest = new KioskFunctionTest(this.Service, this.KioskTestOutput, ScannerServices.Cortex == this.ScannerService, currentSession.User);
            this.m_testVendDoorTB.Text = this.m_checkDriversStatusTB.Text = this.m_cameraSnapTB.Text = this.m_initTB.Text = this.m_verticalTestTB.Text = this.m_unknownCountTB.Text = "Not Started";
            this.m_userName.Text = currentSession.User;
        }

        private void OnConfigureFraudSensor()
        {
            this.FraudSensorOutput.Write("Fraud sensor is {0}.", this.HasFraudSensor ? (object)"enabled" : (object)"disabled");
            this.m_configureFraudSensor.Text = this.HasFraudSensor ? "Disable" : "Enable";
            this.m_fraudPOSTButton.Enabled = this.m_resetFraudButton.Enabled = this.HasFraudSensor;
            this.m_powerOnFraudSensor.Enabled = this.m_powerOffFraudSensor.Enabled = this.HasFraudSensor;
            this.m_startFraudScan.Enabled = this.m_stopFraudScan.Enabled = this.HasFraudSensor;
            this.m_testDiskForMarkers.Enabled = this.m_testFraudButton.Enabled = this.HasFraudSensor;
        }

        private void WriteToOutput(string msg)
        {
            ServiceLocator.Instance.GetService<ILogger>().Log(msg, LogEntryType.Info);
        }

        private void m_configureQubeButton_Click(object sender, EventArgs e)
        {
            if (!this.ExchangerState.ToggleConfiguration())
            {
                this.AirExchangerOutput.Write("Failed to update configuration.");
            }
            else
            {
                this.AirExchangerOutput.Write("The air exchanger is {0}.", this.ExchangerState.Configured ? (object)"configured" : (object)"not configured");
                this.OnConfigureIceQubeBoard();
            }
        }

        private void m_configureAuxBoardButton_Click(object sender, EventArgs e)
        {
            this.m_iceQubeExchangerLabel.Text = "Power Relay board no longer supported.";
        }

        private void OnExecuteFraudSensorInstruction(object sender, EventArgs e)
        {
            this.ExecuteInstruction(sender, this.FraudSensorOutput, "FRAUDSENSOR");
        }

        private void OnExecuteIceQubeBoardCommand(object sender, EventArgs e)
        {
            this.ExecuteInstruction(sender, this.AirExchangerOutput, "AIRXCHGR");
        }

        private void ExecuteInstruction(object sender, OutputBox box, string mnemonic)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                string tagInstruction = buttonAspects.GetTagInstruction();
                if (string.IsNullOrEmpty(tagInstruction))
                {
                    box.Write("There is no operand associated with that button.");
                }
                else
                {
                    using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                    {
                        string instruction = string.Format("{0} {1}", (object)mnemonic, (object)tagInstruction);
                        string str = instructionHelper.ExecuteGeneric(instruction);
                        box.Write(string.IsNullOrEmpty(str) ? "Instruction execution failed." : str);
                    }
                }
            }
        }

        private void m_exitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void m_installCortexButton_Click(object sender, EventArgs e)
        {
            this.ToggleCortex(sender);
        }

        private void m_uninstallCortexButton_Click(object sender, EventArgs e)
        {
            this.ToggleCortex(sender);
        }

        private void ToggleCortex(object sender)
        {
            ScannerServices newService = ScannerServices.Cortex == this.ScannerService ? ScannerServices.Legacy : ScannerServices.Cortex;
            ServiceLocator.Instance.GetService<IRuntimeService>();
            using (this.Manager.MakeAspect(sender))
            {
                using (ChangeCameraConfigurationExecutor configurationExecutor = new ChangeCameraConfigurationExecutor(this.Service, newService))
                {
                    configurationExecutor.Run();
                    if (HardwareJobStatus.Completed == configurationExecutor.EndStatus)
                    {
                        this.ScannerService = newService;
                        bool flag = ScannerServices.Cortex == this.ScannerService;
                        this.WriteToBox("The camera was successfully installed");
                        this.m_installCortexButton.Enabled = !flag;
                        this.m_uninstallCortexButton.Enabled = this.m_startCortexButton.Enabled = flag;
                    }
                    else
                    {
                        configurationExecutor.Errors.ForEach((Action<Error>)(each => this.WriteToBox(each.Details)));
                        this.WriteToBox("Unable to change camera configuration.");
                    }
                }
            }
        }

        private void WriteToBox(string msg) => this.OutputBox.Write(msg);

        private void button1_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                {
                    string str = instructionHelper.ExecuteGeneric("CAMERA START");
                    if (string.IsNullOrEmpty(str))
                        this.OutputBox.Write(ErrorCodes.ServiceChannelError.ToString().ToUpper());
                    else
                        this.WriteToBox(bool.Parse(str) ? "Camera started." : "Camera DIDN'T START.");
                }
            }
        }

        private void RunPostTest()
        {
            using (FraudSensorPost fraudSensorPost = new FraudSensorPost(this.Service))
            {
                fraudSensorPost.AddSink((HardwareEvent)((j, time, msg) => this.FraudSensorOutput.Write("[{0} {1}] {2}", (object)time.ToShortDateString(), (object)time.ToShortTimeString(), (object)msg)));
                fraudSensorPost.Run();
                fraudSensorPost.Results.ForEach((Action<ProgramResult>)(each => this.FraudSensorOutput.Write("{0} : {1}", (object)each.Code, (object)each.Message)));
            }
            this.BeginInvoke((Delegate)(() => this.m_fraudPOSTButton.Enabled = true));
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                bool flag = !this.HasFraudSensor;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("SETCFG \"EnableSecureDiskValidator\" \"{0}\" TYPE=CONTROLLER", (object)flag.ToString()));
                stringBuilder.AppendLine(string.Format(" SETCFG \"EnableFraudSensorCheck\" \"{0}\" TYPE=CONTROLLER", (object)flag.ToString()));
                stringBuilder.AppendLine(string.Format(" SETCFG \"FraudScanJobBitmask2\" \"{0}\" TYPE=CONTROLLER", (object)(flag ? 32 : 0)));
                stringBuilder.AppendLine(" CLEAR");
                HardwareCommandResult result = this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                if (!result.Success)
                {
                    this.FraudSensorOutput.Write("Failed to update configuration.");
                    this.DumpResult(result, this.FraudSensorOutput);
                }
                else
                {
                    this.HasFraudSensor = flag;
                    this.OnConfigureFraudSensor();
                }
            }
        }

        private void DumpResult(HardwareCommandResult result, OutputBox box)
        {
            result.Errors.ForEach((Action<Error>)(err => box.Write(err.ToString())));
        }

        private void m_testFraudButton_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (ConfiguredDevicesForm.FraudTestExecutor fraudTestExecutor = new ConfiguredDevicesForm.FraudTestExecutor(this.Service))
                {
                    fraudTestExecutor.Run();
                    foreach (ProgramResult result in fraudTestExecutor.Results)
                        this.FraudSensorOutput.Write("Code: {0}, Message = {1}", (object)result.Code, (object)result.Message);
                }
            }
        }

        private void m_testUserDiskForFraud(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (ConfiguredDevicesForm.FraudTakeReadReturnExecutor readReturnExecutor = new ConfiguredDevicesForm.FraudTakeReadReturnExecutor(this.Service))
                {
                    readReturnExecutor.Run();
                    foreach (ProgramResult result in readReturnExecutor.Results)
                        this.FraudSensorOutput.Write("Code: {0}, Message = {1}", (object)result.Code, (object)result.Message);
                }
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int integer1 = this.m_fraudDeckTB.GetInteger("Deck", this.FraudSensorOutput);
                int integer2 = this.m_fraudSlotTB.GetInteger("Slot", this.FraudSensorOutput);
                if (-1 == integer1 || -1 == integer2)
                {
                    this.FraudSensorOutput.Write("Check deck and slot!");
                }
                else
                {
                    CompositeFunctions.GetItem(integer1, integer2, this.FraudSensorOutput, this.Service);
                    this.Service.ExecuteImmediateProgram(this.CenterItemProgram, out HardwareJob _);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                CompositeFunctions.VendDisk(this.Service, this.FraudSensorOutput);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int integer1 = this.m_fraudDeckTB.GetInteger("Deck", this.FraudSensorOutput);
                int integer2 = this.m_fraudSlotTB.GetInteger("Slot", this.FraudSensorOutput);
                if (-1 == integer1 || -1 == integer2)
                    this.FraudSensorOutput.Write("Check deck and slot!");
                else
                    CompositeFunctions.PutItem(this.Service, integer1, integer2, this.OutputBox);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (TakeDiskAtDoorJob takeDiskAtDoorJob = new TakeDiskAtDoorJob(this.Service))
                {
                    takeDiskAtDoorJob.Run();
                    foreach (ProgramResult result in takeDiskAtDoorJob.Results)
                        this.OutputBox.Write("Code: {0} Message: {1}", (object)result.Code, (object)result.Message);
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            LocationNumberPad locationNumberPad = new LocationNumberPad();
            if (locationNumberPad.ShowDialog() != DialogResult.OK)
                return;
            this.m_fraudDeckTB.Text = locationNumberPad.Number.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LocationNumberPad locationNumberPad = new LocationNumberPad();
            if (locationNumberPad.ShowDialog() != DialogResult.OK)
                return;
            this.m_fraudSlotTB.Text = locationNumberPad.Number.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                {
                    string tagInstruction = buttonAspects.GetTagInstruction();
                    if (string.IsNullOrEmpty(tagInstruction))
                        this.FraudSensorOutput.Write("There is no instruction for that button.");
                    else
                        instructionHelper.ExecuteGeneric(tagInstruction);
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e) => this.FraudSensorOutput.Clear();

        private void button10_Click(object sender, EventArgs e) => this.AirExchangerOutput.Clear();

        private void RunStressTest()
        {
            int integer1 = this.m_scanPauseTB.GetInteger("Scan Pause", this.FraudSensorOutput);
            int integer2 = this.m_stressIterationsTB.GetInteger("iterations", this.FraudSensorOutput);
            int integer3 = this.m_readPauseTB.GetInteger("Iteration Pause", this.FraudSensorOutput);
            using (ConfiguredDevicesForm.FraudStressTestExecutor stressTestExecutor = new ConfiguredDevicesForm.FraudStressTestExecutor(this.Service, integer2, integer1, integer3))
            {
                stressTestExecutor.AddSink((HardwareEvent)((j, time, msg) => this.FraudSensorOutput.Write("[{0} {1}] {2}", (object)time.ToShortDateString(), (object)time.ToShortTimeString(), (object)msg)));
                stressTestExecutor.Run();
                foreach (ProgramResult result in stressTestExecutor.Results)
                    this.FraudSensorOutput.Write("{0} : {1}", (object)result.Code, (object)result.Message);
            }
            this.BeginInvoke((Delegate)(() => this.button13.Enabled = true));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int num = (int)ServiceLocator.Instance.GetService<IControlSystem>().Center(CenterDiskMethod.VendDoorAndBack);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int integer = this.m_scanPauseTB.GetInteger("Scan Pause", this.FraudSensorOutput);
            using (this.Manager.MakeAspect(sender))
            {
                using (ConfiguredDevicesForm.FraudDetectionTestExecutor detectionTestExecutor = new ConfiguredDevicesForm.FraudDetectionTestExecutor(this.Service, integer))
                    detectionTestExecutor.Run();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.m_checkDriversStatusTB.Text = this.FunctionTest.TestDrivers().ToString().ToUpper();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.KioskTestOutput.Clear();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.m_testVendDoorTB.Text = this.FunctionTest.TestVendDoor().ToString().ToUpper();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.btn_KFCInit.BackColor = Color.Red;
            this.ToggleKfcButtons(false);
            this.m_runInitWorker.RunWorkerAsync();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.errorProvider1.Clear();
            int integer = this.m_sourceSlotTB.GetInteger("Test Slot", this.KioskTestOutput);
            if (-1 == integer)
            {
                this.errorProvider1.SetError((Control)this.m_sourceSlotTB, "Select slot");
            }
            else
            {
                this.ToggleKfcButtons(false);
                this.btn_KFCVerticalSlotTest.BackColor = Color.Red;
                this.m_verticalSyncWorker.RunWorkerAsync((object)integer);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.m_unknownCountTB.Text = this.FunctionTest.GetUnknownStats() ? "SUCCESS" : "FAILURE";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.errorProvider1.Clear();
            if (string.IsNullOrEmpty(this.m_userName.Text))
            {
                this.KioskTestOutput.Write("Please specify a user name.");
                this.errorProvider1.SetError((Control)this.m_userName, "User name required.");
            }
            else
            {
                using (this.Manager.MakeAspect(sender))
                    this.FunctionTest.SendData(this.m_userName.Text);
                this.FunctionTest = (KioskFunctionTest)null;
                this.OnConfigureKioskTest();
            }
        }

        private void button16_Click(object sender, EventArgs e) => this.m_sourceSlotTB.InputNumber();

        private void button20_Click(object sender, EventArgs e)
        {
            int integer1 = this.m_sourceDeckTB.GetInteger("Sync deck", this.KioskTestOutput);
            int integer2 = this.m_sourceSlotTB.GetInteger("Sync slot", this.KioskTestOutput);
            if (-1 == integer1)
                this.errorProvider1.SetError((Control)this.m_sourceDeckTB, "Please enter valid deck.");
            else if (-1 == integer2)
            {
                this.errorProvider1.SetError((Control)this.m_sourceSlotTB, "Please enter valid slot.");
            }
            else
            {
                this.ToggleKfcButtons(false);
                this.btn_KFCDecodeTest.BackColor = Color.Red;
                this.m_runDecodeWorker.RunWorkerAsync((object)new Redbox.HAL.Client.Location()
                {
                    Deck = integer1,
                    Slot = integer2
                });
            }
        }

        private void button21_Click(object sender, EventArgs e) => this.m_sourceDeckTB.InputNumber();

        private void button22_Click(object sender, EventArgs e) => this.m_sourceSlotTB.InputNumber();

        private void button16_Click_1(object sender, EventArgs e)
        {
            IList<IKioskFunctionCheckData> sessions;
            if (!this.Service.GetKioskFunctionCheckData(out sessions).Success)
                return;
            int num = 1;
            foreach (IKioskFunctionCheckData session in (IEnumerable<IKioskFunctionCheckData>)sessions)
                this.WriteSessionData(session, num++);
        }

        private void WriteSessionData(IKioskFunctionCheckData session, int sessionCount)
        {
            this.KioskTestOutput.Write("   Vertical result = {0}", (object)session.VerticalSlotTestResult);
            this.KioskTestOutput.Write("   Init result = {0}", (object)session.InitTestResult);
            this.KioskTestOutput.Write("   Venddoor result = {0}", (object)session.VendDoorTestResult);
            this.KioskTestOutput.Write("   Track result = {0}", (object)session.TrackTestResult);
            this.KioskTestOutput.Write("   Snap and decode result = {0}", (object)session.SnapDecodeTestResult);
            this.KioskTestOutput.Write("   Touchscreen driver result = {0}", (object)session.TouchscreenDriverTestResult);
            this.KioskTestOutput.Write("   Camera driver result = {0}", (object)session.CameraDriverTestResult);
            this.KioskTestOutput.Write("   Timestamp {0}", (object)session.Timestamp.ToString());
            this.KioskTestOutput.Write("   User identifier {0}", (object)session.UserIdentifier);
            this.KioskTestOutput.Write("Session {0}", (object)sessionCount);
        }

        private void button11_Click(object sender, EventArgs e1)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (HardwareSurveyExecutor hardwareSurveyExecutor = new HardwareSurveyExecutor(this.Service))
                {
                    using (ExecutionTimer executionTimer = new ExecutionTimer())
                    {
                        hardwareSurveyExecutor.Run();
                        this.AddItem("Timestamp", hardwareSurveyExecutor.Timestamp.ToString());
                        this.AddItem("Camera", hardwareSurveyExecutor.CameraVersion);
                        string col2 = hardwareSurveyExecutor.TouchscreenFirmware;
                        try
                        {
                            Version version = new Version(hardwareSurveyExecutor.TouchscreenFirmware);
                            bool flag = version.Major == 4 && version.Minor == 30;
                            col2 = string.Format("{0} ( {1} CURRENT )", (object)hardwareSurveyExecutor.TouchscreenFirmware, flag ? (object)"MOST" : (object)"NOT MOST");
                        }
                        catch (Exception ex)
                        {
                        }
                        this.AddItem("Touchscreen firmware", col2);
                        this.AddItem("Touchscreen model", hardwareSurveyExecutor.Touchscreen);
                        this.AddItem("ABE device", hardwareSurveyExecutor.ABEDevice.ToString());
                        this.AddItem("Fraud sensor", hardwareSurveyExecutor.FraudDevice.ToString());
                        this.AddItem("AuxRelay board", hardwareSurveyExecutor.HasAuxRelayBoard ? "Yes" : "No");
                        this.AddItem("Air exchanger", hardwareSurveyExecutor.AirExchanger.ToString());
                        this.AddItem("Quick return", hardwareSurveyExecutor.QuickReturn.ToString());
                        this.AddItem("Disk free space", string.Format("{0} GB", (object)(hardwareSurveyExecutor.FreeDiskSpace >> 30)));
                        this.AddItem("Memory", string.Format("{0} GB", (object)(hardwareSurveyExecutor.Memory >> 20)));
                        this.AddItem("PC Model", hardwareSurveyExecutor.PcModel);
                        this.AddItem("PC Manufacturer", hardwareSurveyExecutor.PcManufacturer);
                        this.AddItem("UPS", hardwareSurveyExecutor.UpsModel);
                        this.AddItem("Monitor Model", hardwareSurveyExecutor.Monitor);
                        this.AddItem("Serial controller version", hardwareSurveyExecutor.SerialControllerVersion);
                        executionTimer.Stop();
                        LogHelper.Instance.Log("[{0}] Hardware survey execution time = {1}", (object)this.GetType().Name, (object)executionTimer.Elapsed);
                    }
                }
            }
        }

        private void AddItem(string col1, string col2)
        {
            this.listView1.Items.Add(new ListViewItem(new string[2]
            {
        col1,
        col2
            }));
        }

        private void button16_Click_2(object sender, EventArgs e) => this.listView1.Items.Clear();

        private void m_configureRouterRelay_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                bool flag = !this.HasRouterPowerRelay;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("SETCFG \"RouterPowerCyclePause\" \"{0}\" TYPE=CONTROLLER", (object)(flag ? this.RouterPowerCyclePause : 0)));
                if (flag)
                    stringBuilder.AppendLine("SETCFG \"TrackHardwareCorrections\" \"TRUE\" TYPE=CONTROLLER");
                stringBuilder.AppendLine(" CLEAR");
                HardwareCommandResult result = this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                if (!result.Success)
                {
                    this.DumpResult(result, this.HardwareStatsOutput);
                    this.HardwareStatsOutput.Write("Failed to update configuration.");
                }
                else
                {
                    this.HasRouterPowerRelay = flag;
                    this.ConfigureRouterGroup();
                }
            }
        }

        private void ConfigureRouterGroup()
        {
            this.m_configureRouterRelay.Text = this.HasRouterPowerRelay ? "Disable" : "Enable";
            this.HardwareStatsOutput.Write("Router relay is {0}.", this.HasRouterPowerRelay ? (object)"configured" : (object)"not configured");
            this.m_testRouterRelay.Enabled = this.HasRouterPowerRelay;
        }

        private void ConfigureArcusGroup()
        {
            this.m_restartArcus.Enabled = this.ArcusResetConfigured;
            this.m_configureArcus.Text = this.ArcusResetConfigured ? "Disable" : "Enable";
            this.HardwareStatsOutput.Write("Kiosk is {0} to reset the Arcus.", this.ArcusResetConfigured ? (object)"configured" : (object)"not configured");
        }

        private void m_testRouterRelay_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                HardwareService service = this.Service;
                HardwareJobSchedule s = new HardwareJobSchedule();
                s.Priority = HardwareJobPriority.High;
                HardwareJob job;
                HardwareCommandResult result = service.PowerCycleRouter(s, out job);
                if (result.Success)
                {
                    using (ClientHelper clientHelper = new ClientHelper(this.Service))
                    {
                        HardwareJobStatus endStatus;
                        clientHelper.WaitForJob(job, out endStatus);
                        this.HardwareStatsOutput.Write("Power cycle router job {0} ended with status {1}", (object)job.ID, (object)endStatus);
                    }
                }
                else
                {
                    this.DumpResult(result, this.HardwareStatsOutput);
                    this.HardwareStatsOutput.Write("Failed to schedule router power cycle job.");
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            HardwareCorrectionStatistic stat = HardwareCorrectionStatistic.None;
            foreach (Control control in (ArrangedElementCollection)this.m_hwCorrectiontypeGB.Controls)
            {
                if (control.GetType() == typeof(RadioButton))
                {
                    RadioButton radioButton = (RadioButton)control;
                    if (radioButton.Checked)
                    {
                        stat = Enum<HardwareCorrectionStatistic>.ParseIgnoringCase((string)radioButton.Tag, HardwareCorrectionStatistic.None);
                        break;
                    }
                }
            }
            if (stat != HardwareCorrectionStatistic.None)
            {
                using (GetHardwareCorrectionStatistics correctionStatistics = new GetHardwareCorrectionStatistics(this.Service, stat))
                {
                    correctionStatistics.Run();
                    if (HardwareJobStatus.Completed != correctionStatistics.EndStatus)
                        this.HardwareStatsOutput.Write("Failed to get statistics from service.");
                    else
                        this.DumpHardwareStats(correctionStatistics.Stats);
                }
            }
            else
            {
                using (GetAllHardwareCorrectionStatistics correctionStatistics = new GetAllHardwareCorrectionStatistics(this.Service))
                {
                    correctionStatistics.Run();
                    if (HardwareJobStatus.Completed != correctionStatistics.EndStatus)
                        this.HardwareStatsOutput.Write("Failed to get statistics from service.");
                    else
                        this.DumpHardwareStats(correctionStatistics.Stats);
                }
            }
        }

        private void DumpHardwareStats(List<IHardwareCorrectionStatistic> stats)
        {
            stats.ForEach((Action<IHardwareCorrectionStatistic>)(s => this.HardwareStatsOutput.Write("{0} {1}: {2} {3}", (object)s.CorrectionTime, (object)s.Statistic, (object)s.ProgramName, s.CorrectionOk ? (object)"SUCCESS" : (object)"FAILURE ")));
            this.HardwareStatsOutput.Write("Total of {0} statistics", (object)stats.Count);
        }

        private void button23_Click(object sender, EventArgs e) => this.HardwareStatsOutput.Clear();

        private void button27_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                CompositeFunctions.VendDisk(this.Service, this.EngineeringOutput);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                this.EngineeringOutput.Write(ScanResult.ReadBarcodeOfDiskInPicker(this.Service).ToString());
        }

        private void button26_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                HardwareService service = this.Service;
                string empty = string.Empty;
                HardwareJobSchedule schedule = new HardwareJobSchedule();
                schedule.Priority = HardwareJobPriority.High;
                HardwareJob job;
                if (!service.ScheduleJob("ms-take-disk-at-door", empty, false, schedule, out job).Success)
                {
                    this.EngineeringOutput.Write("Failed to schedule job");
                }
                else
                {
                    job.Pend();
                    using (ClientHelper clientHelper = new ClientHelper(this.Service))
                    {
                        HardwareJobStatus endStatus;
                        clientHelper.WaitForJob(job, out endStatus);
                        if (endStatus != HardwareJobStatus.Completed)
                            return;
                        ProgramResult[] results;
                        if (!job.GetResults(out results).Success)
                        {
                            this.EngineeringOutput.Write("Failed to get program results. I'm sorry!");
                        }
                        else
                        {
                            foreach (ProgramResult programResult in results)
                                this.EngineeringOutput.Write(programResult.Message);
                        }
                    }
                }
            }
        }

        private void button25_Click(object sender, EventArgs e) => this.EngineeringOutput.Clear();

        private void button29_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.EngineeringOutput.Write("Please specify source deck/slot properly");
                }
                else
                {
                    int destinationDeck = this.DestinationDeck;
                    int destinationSlot = this.DestinationSlot;
                    if (-1 == destinationDeck || -1 == destinationSlot)
                    {
                        this.EngineeringOutput.Write("Please specify destination deck/slot properly");
                    }
                    else
                    {
                        SyncRange syncRange = new SyncRange(sourceDeck, destinationDeck, new SlotRange(sourceSlot, destinationSlot));
                        HardwareService service = this.Service;
                        SyncRange range = syncRange;
                        HardwareJobSchedule schedule = new HardwareJobSchedule();
                        schedule.Priority = HardwareJobPriority.Highest;
                        HardwareJob hardwareJob;
                        if (!service.HardSync(range, schedule, out hardwareJob).Success)
                        {
                            this.EngineeringOutput.Write("Unable to communicate with HAL.");
                        }
                        else
                        {
                            hardwareJob.Pend();
                            hardwareJob.WaitForCompletion(300000);
                            IDictionary<string, string> symbols;
                            if (hardwareJob.GetSymbols(out symbols).Success)
                            {
                                foreach (string key in (IEnumerable<string>)symbols.Keys)
                                {
                                    if (key.StartsWith("MSTESTER-SYMBOL"))
                                        this.EngineeringOutput.Write(symbols[key]);
                                }
                            }
                            ProgramResult[] results;
                            if (!hardwareJob.GetResults(out results).Success)
                            {
                                this.EngineeringOutput.Write("Failed to get program results. I'm sorry!");
                            }
                            else
                            {
                                foreach (ProgramResult programResult in results)
                                    this.EngineeringOutput.Write(programResult.Message);
                            }
                        }
                    }
                }
            }
        }

        private void button34_Click(object sender, EventArgs e) => this.m_engSlotTB.InputNumber();

        private void button33_Click(object sender, EventArgs e) => this.m_engDeckTB.InputNumber();

        private void button31_Click(object sender, EventArgs e) => this.m_engEndDeckTB.InputNumber();

        private void button32_Click(object sender, EventArgs e) => this.m_engEndSlotTB.InputNumber();

        private int SourceDeck => this.m_engDeckTB.GetInteger("Source Deck", this.EngineeringOutput);

        private int SourceSlot => this.m_engSlotTB.GetInteger("Source Slot", this.EngineeringOutput);

        private int DestinationDeck
        {
            get => this.m_engEndDeckTB.GetInteger("Source Deck", this.EngineeringOutput);
        }

        private int DestinationSlot
        {
            get => this.m_engEndSlotTB.GetInteger("Source Slot", this.EngineeringOutput);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                {
                    this.EngineeringOutput.Write("Please specify deck/slot properly");
                }
                else
                {
                    List<Redbox.HAL.Client.Location> locationList = new List<Redbox.HAL.Client.Location>()
          {
            new Redbox.HAL.Client.Location() { Deck = sourceDeck, Slot = sourceSlot }
          };
                    HardwareService service = this.Service;
                    List<Redbox.HAL.Client.Location> locations = locationList;
                    HardwareJobSchedule schedule = new HardwareJobSchedule();
                    schedule.Priority = HardwareJobPriority.Highest;
                    HardwareJob hardwareJob;
                    if (!service.HardSync(locations, "Engineering Location Sync", schedule, out hardwareJob).Success)
                    {
                        this.EngineeringOutput.Write("Unable to communicate with HAL.");
                    }
                    else
                    {
                        hardwareJob.Pend();
                        hardwareJob.WaitForCompletion(300000);
                        IDictionary<string, string> symbols;
                        if (hardwareJob.GetSymbols(out symbols).Success)
                        {
                            foreach (string key in (IEnumerable<string>)symbols.Keys)
                            {
                                if (key.StartsWith("MSTESTER-SYMBOL"))
                                    this.EngineeringOutput.Write(symbols[key]);
                            }
                        }
                        ProgramResult[] results;
                        if (!hardwareJob.GetResults(out results).Success)
                        {
                            this.EngineeringOutput.Write("Failed to get program results. I'm sorry!");
                        }
                        else
                        {
                            foreach (ProgramResult programResult in results)
                                this.EngineeringOutput.Write(programResult.Message);
                        }
                    }
                }
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                    this.EngineeringOutput.Write("Please specify deck/slot properly");
                else
                    CompositeFunctions.GetItem(sourceDeck, sourceSlot, this.EngineeringOutput, this.Service);
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int sourceDeck = this.SourceDeck;
                int sourceSlot = this.SourceSlot;
                if (-1 == sourceDeck || -1 == sourceSlot)
                    this.EngineeringOutput.Write("Please specify deck/slot properly");
                else
                    CompositeFunctions.PutItem(this.Service, sourceDeck, sourceSlot, this.OutputBox);
            }
        }

        private void m_restartTouchscreen_Click(object sender, EventArgs e)
        {
            HardwareService service = this.Service;
            HardwareJobSchedule s = new HardwareJobSchedule();
            s.Priority = HardwareJobPriority.High;
            HardwareJob job;
            if (service.ResetTouchscreenController(s, out job).Success)
            {
                using (ClientHelper clientHelper = new ClientHelper(this.Service))
                {
                    HardwareJobStatus endStatus;
                    clientHelper.WaitForJob(job, out endStatus);
                    this.HardwareStatsOutput.Write("Reset touchscreen controller ended with status {0}", (object)endStatus.ToString());
                }
            }
            else
                this.HardwareStatsOutput.Write("Unable to schedule reset touchscreen job.");
        }

        private void m_restartArcus_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                using (ResetMotionControlExecutor motionControlExecutor = new ResetMotionControlExecutor(this.Service))
                {
                    motionControlExecutor.Run();
                    if (motionControlExecutor.EndStatus != HardwareJobStatus.Completed)
                    {
                        this.HardwareStatsOutput.Write("ERROR: reset-arcus failed.");
                    }
                    else
                    {
                        foreach (ProgramResult result in motionControlExecutor.Results)
                            this.HardwareStatsOutput.Write(result.Message);
                    }
                }
            }
        }

        private void m_configureArcus_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                bool flag = !this.ArcusResetConfigured;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("SETCFG \"RestartControllerDuringUserJobs\" \"{0}\" TYPE=CONTROLLER", (object)flag));
                if (flag)
                    stringBuilder.AppendLine("SETCFG \"TrackHardwareCorrections\" \"TRUE\" TYPE=CONTROLLER");
                stringBuilder.AppendLine("CLEAR");
                HardwareCommandResult result = this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                if (!result.Success)
                {
                    this.DumpResult(result, this.HardwareStatsOutput);
                    this.HardwareStatsOutput.Write("Failed to update configuration.");
                }
                else
                {
                    this.ArcusResetConfigured = flag;
                    this.ConfigureArcusGroup();
                }
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                bool flag = !this.HasRouterPowerRelay;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("SETCFG \"RouterPowerCyclePause\" \"{0}\" TYPE=CONTROLLER", (object)(flag ? this.RouterPowerCyclePause : 0)));
                if (flag)
                    stringBuilder.AppendLine("SETCFG \"TrackHardwareCorrections\" \"TRUE\" TYPE=CONTROLLER");
                stringBuilder.AppendLine(" CLEAR");
                HardwareCommandResult result = this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                if (!result.Success)
                {
                    this.DumpResult(result, this.HardwareStatsOutput);
                    this.HardwareStatsOutput.Write("Failed to update configuration.");
                }
                else
                {
                    this.HasRouterPowerRelay = flag;
                    this.ConfigureRouterGroup();
                }
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                HardwareService service = this.Service;
                HardwareJobSchedule s = new HardwareJobSchedule();
                s.Priority = HardwareJobPriority.High;
                HardwareJob job;
                HardwareCommandResult result = service.PowerCycleRouter(s, out job);
                if (result.Success)
                {
                    using (ClientHelper clientHelper = new ClientHelper(this.Service))
                    {
                        HardwareJobStatus endStatus;
                        clientHelper.WaitForJob(job, out endStatus);
                        this.HardwareStatsOutput.Write("Power cycle router job {0} ended with status {1}", (object)job.ID, (object)endStatus);
                    }
                }
                else
                {
                    this.DumpResult(result, this.HardwareStatsOutput);
                    this.HardwareStatsOutput.Write("Failed to schedule router power cycle job.");
                }
            }
        }

        private void ToggleKfcButtons(bool enable)
        {
            this.btn_KFCCheckDrivers.Enabled = this.btn_KFCDecodeTest.Enabled = this.btn_KFCTestVendDoor.Enabled = this.btn_KFCInit.Enabled = this.btn_KFCVerticalSlotTest.Enabled = this.btn_KFCUnknownCount.Enabled = enable;
        }

        private void m_verticalSyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int slot = (int)e.Argument;
            e.Result = (object)this.FunctionTest.RunVertialSlotTest(slot).ToString().ToUpper();
        }

        private void m_verticalSyncWorker_RunWorkerCompleted(
          object sender,
          RunWorkerCompletedEventArgs e)
        {
            this.ToggleKfcButtons(true);
            this.m_verticalTestTB.Text = e.Result as string;
            this.btn_KFCVerticalSlotTest.BackColor = Color.LightGray;
        }

        private void m_runInitWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = (object)this.FunctionTest.RunInit().ToString().ToUpper();
        }

        private void m_runInitWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ToggleKfcButtons(true);
            this.btn_KFCInit.BackColor = Color.LightGray;
            this.m_initTB.Text = e.Result as string;
        }

        private void m_runDecodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Redbox.HAL.Client.Location loc = e.Argument as Redbox.HAL.Client.Location;
            e.Result = (object)this.FunctionTest.TestCameraSnap(loc).ToString().ToUpper();
        }

        private void m_runDecodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ToggleKfcButtons(true);
            this.btn_KFCDecodeTest.BackColor = Color.LightGray;
            this.m_cameraSnapTB.Text = e.Result as string;
        }

        private void button37_Click_1(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                bool flag = !this.m_irConfigured;
                DateTime universalTime = DateTime.Now.ToUniversalTime();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("SETCFG \"IRHardwareInstallDate\" \"{0}\" TYPE=KIOSK", flag ? (object)universalTime.ToString() : (object)"NONE"));
                stringBuilder.AppendLine("CLEAR");
                HardwareCommandResult hardwareCommandResult = this.Service.ExecuteImmediateProgram(Encoding.ASCII.GetBytes(stringBuilder.ToString()), out HardwareJob _);
                if (!hardwareCommandResult.Success)
                {
                    hardwareCommandResult.Errors.ForEach((Action<Error>)(err => this.IrOutput.Write(err.ToString())));
                    this.IrOutput.Write("Failed to update configuration.");
                }
                else
                {
                    this.m_irConfigured = flag;
                    this.ConfigureIrCameraGroup();
                }
            }
        }

        private void OnExecuteResponseInstruction(object sender, EventArgs e)
        {
            using (ButtonAspects buttonAspects = this.Manager.MakeAspect(sender))
            {
                string tagInstruction = buttonAspects.GetTagInstruction();
                using (InstructionHelper instructionHelper = new InstructionHelper(this.Service))
                {
                    string str = instructionHelper.ExecuteGeneric(tagInstruction);
                    this.IrOutput.Write("{0} - {1}", (object)tagInstruction, (object)str);
                }
            }
        }

        private void button46_Click(object sender, EventArgs e) => this.textBox1.InputNumber();

        private void button47_Click(object sender, EventArgs e) => this.textBox2.InputNumber();

        private void button43_Click(object sender, EventArgs e)
        {
            int? getDeck = this.GetDeck;
            int? getSlot = this.GetSlot;
            if (!getDeck.HasValue || !getSlot.HasValue)
                return;
            CompositeFunctions.GetItem(getDeck.Value, getSlot.Value, this.IrOutput, this.Service);
            using (this.Manager.MakeAspect(sender))
            {
                ScanResult scanResult = ScanResult.ReadBarcodeOfDiskInPicker(this.Service);
                this.IrOutput.Write(string.Format("found {0} secure codes.", (object)scanResult.SecureCount));
                this.IrOutput.Write(scanResult.ToString());
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
            {
                int? getDeck = this.GetDeck;
                int? getSlot = this.GetSlot;
                if (!getDeck.HasValue || !getSlot.HasValue)
                    return;
                CompositeFunctions.PutItem(this.Service, getDeck.Value, getSlot.Value, this.IrOutput);
            }
        }

        private int? GetDeck
        {
            get
            {
                int integer = this.textBox1.GetInteger("Deck", this.IrOutput);
                if (-1 != integer)
                    return new int?(integer);
                this.errorProvider1.SetError((Control)this.textBox1, "Please specify a valid deck");
                return new int?();
            }
        }

        private int? GetSlot
        {
            get
            {
                int integer = this.textBox2.GetInteger("Slot", this.IrOutput);
                if (-1 != integer)
                    return new int?(integer);
                this.errorProvider1.SetError((Control)this.textBox2, "Please specify a valid deck");
                return new int?();
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            using (this.Manager.MakeAspect(sender))
                TunerLaunchService.LaunchTunerAndWait(this.Service, this.Manager);
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
            this.m_devicesTab = new TabControl();
            this.m_airExchangerTab = new TabPage();
            this.button10 = new Button();
            this.m_iceQubeOutput = new ListBox();
            this.m_resetPersistentCounter = new Button();
            this.m_qubeStatusButton = new Button();
            this.m_resetQubeBoard = new Button();
            this.m_configureQubeButton = new Button();
            this.m_iceQubeExchangerLabel = new Label();
            this.m_cortexTab = new TabPage();
            this.m_startCortexButton = new Button();
            this.m_cortexStatusBox = new ListBox();
            this.label3 = new Label();
            this.m_uninstallCortexButton = new Button();
            this.m_installCortexButton = new Button();
            this.m_fraudSensorTab = new TabPage();
            this.button15 = new Button();
            this.button14 = new Button();
            this.label5 = new Label();
            this.m_readPauseTB = new TextBox();
            this.m_scanPauseTB = new TextBox();
            this.label4 = new Label();
            this.label2 = new Label();
            this.m_stressIterationsTB = new TextBox();
            this.button13 = new Button();
            this.button3 = new Button();
            this.button7 = new Button();
            this.button6 = new Button();
            this.button5 = new Button();
            this.button4 = new Button();
            this.m_fraudSlotTB = new TextBox();
            this.m_fraudDeckTB = new TextBox();
            this.groupBox1 = new GroupBox();
            this.m_powerOffFraudSensor = new Button();
            this.m_powerOnFraudSensor = new Button();
            this.m_testDiskForMarkers = new Button();
            this.m_stopFraudScan = new Button();
            this.m_startFraudScan = new Button();
            this.m_resetFraudButton = new Button();
            this.m_testFraudButton = new Button();
            this.m_fraudPOSTButton = new Button();
            this.button1 = new Button();
            this.m_configureFraudSensor = new Button();
            this.m_fraudSensorOutput = new ListBox();
            this.m_kioskFunctionCheck = new TabPage();
            this.m_displaySessionsButton = new Button();
            this.m_sourceSlotTB = new TextBox();
            this.button19 = new Button();
            this.m_sourceDeckTB = new TextBox();
            this.button22 = new Button();
            this.groupBox2 = new GroupBox();
            this.m_unknownCountTB = new TextBox();
            this.m_cameraSnapTB = new TextBox();
            this.btn_KFCDecodeTest = new Button();
            this.m_verticalTestTB = new TextBox();
            this.m_initTB = new TextBox();
            this.m_testVendDoorTB = new TextBox();
            this.m_checkDriversStatusTB = new TextBox();
            this.btn_KFCUnknownCount = new Button();
            this.btn_KFCVerticalSlotTest = new Button();
            this.btn_KFCTestVendDoor = new Button();
            this.btn_KFCInit = new Button();
            this.btn_KFCCheckDrivers = new Button();
            this.m_userName = new TextBox();
            this.button21 = new Button();
            this.label1 = new Label();
            this.button2 = new Button();
            this.m_kioskTestOutput = new ListBox();
            this.tabPage1 = new TabPage();
            this.listView1 = new ListView();
            this.button16 = new Button();
            this.button11 = new Button();
            this.m_hwCorrections = new TabPage();
            this.groupBox4 = new GroupBox();
            this.m_testRouterRelay = new Button();
            this.m_configureRouterRelay = new Button();
            this.groupBox6 = new GroupBox();
            this.m_restartArcus = new Button();
            this.m_configureArcus = new Button();
            this.groupBox5 = new GroupBox();
            this.m_restartTouchscreen = new Button();
            this.m_hwCorrectiontypeGB = new GroupBox();
            this.radioButton5 = new RadioButton();
            this.radioButton4 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.button24 = new Button();
            this.radioButton1 = new RadioButton();
            this.button23 = new Button();
            this.m_hwStatsOutput = new ListBox();
            this.m_irConfigurationTab = new TabPage();
            this.button37 = new Button();
            this.groupBox8 = new GroupBox();
            this.textBox2 = new TextBox();
            this.textBox1 = new TextBox();
            this.button47 = new Button();
            this.button46 = new Button();
            this.button45 = new Button();
            this.button44 = new Button();
            this.button43 = new Button();
            this.groupBox7 = new GroupBox();
            this.button42 = new Button();
            this.button41 = new Button();
            this.button40 = new Button();
            this.button39 = new Button();
            this.button38 = new Button();
            this.m_irCameraOutput = new ListBox();
            this.m_engineeringTab = new TabPage();
            this.groupBox3 = new GroupBox();
            this.button36 = new Button();
            this.button35 = new Button();
            this.button34 = new Button();
            this.button29 = new Button();
            this.button30 = new Button();
            this.button33 = new Button();
            this.button32 = new Button();
            this.m_engEndSlotTB = new TextBox();
            this.button31 = new Button();
            this.m_engEndDeckTB = new TextBox();
            this.m_engDeckTB = new TextBox();
            this.m_engSlotTB = new TextBox();
            this.button28 = new Button();
            this.button27 = new Button();
            this.button26 = new Button();
            this.button25 = new Button();
            this.m_engineeringOutput = new ListBox();
            this.m_exitButton = new Button();
            this.errorProvider1 = new ErrorProvider(this.components);
            this.m_verticalSyncWorker = new BackgroundWorker();
            this.m_runInitWorker = new BackgroundWorker();
            this.m_runDecodeWorker = new BackgroundWorker();
            this.m_devicesTab.SuspendLayout();
            this.m_airExchangerTab.SuspendLayout();
            this.m_cortexTab.SuspendLayout();
            this.m_fraudSensorTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.m_kioskFunctionCheck.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.m_hwCorrections.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.m_hwCorrectiontypeGB.SuspendLayout();
            this.m_irConfigurationTab.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.m_engineeringTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((ISupportInitialize)this.errorProvider1).BeginInit();
            this.SuspendLayout();
            this.m_devicesTab.Controls.Add((Control)this.m_airExchangerTab);
            this.m_devicesTab.Controls.Add((Control)this.m_cortexTab);
            this.m_devicesTab.Controls.Add((Control)this.m_fraudSensorTab);
            this.m_devicesTab.Controls.Add((Control)this.m_kioskFunctionCheck);
            this.m_devicesTab.Controls.Add((Control)this.tabPage1);
            this.m_devicesTab.Controls.Add((Control)this.m_hwCorrections);
            this.m_devicesTab.Controls.Add((Control)this.m_irConfigurationTab);
            this.m_devicesTab.Controls.Add((Control)this.m_engineeringTab);
            this.m_devicesTab.Location = new Point(2, 12);
            this.m_devicesTab.Name = "m_devicesTab";
            this.m_devicesTab.Padding = new Point(10, 20);
            this.m_devicesTab.SelectedIndex = 0;
            this.m_devicesTab.Size = new Size(1057, 626);
            this.m_devicesTab.TabIndex = 0;
            this.m_airExchangerTab.BackColor = Color.LightGray;
            this.m_airExchangerTab.Controls.Add((Control)this.button10);
            this.m_airExchangerTab.Controls.Add((Control)this.m_iceQubeOutput);
            this.m_airExchangerTab.Controls.Add((Control)this.m_resetPersistentCounter);
            this.m_airExchangerTab.Controls.Add((Control)this.m_qubeStatusButton);
            this.m_airExchangerTab.Controls.Add((Control)this.m_resetQubeBoard);
            this.m_airExchangerTab.Controls.Add((Control)this.m_configureQubeButton);
            this.m_airExchangerTab.Controls.Add((Control)this.m_iceQubeExchangerLabel);
            this.m_airExchangerTab.Location = new Point(4, 56);
            this.m_airExchangerTab.Name = "m_airExchangerTab";
            this.m_airExchangerTab.Padding = new Padding(3);
            this.m_airExchangerTab.Size = new Size(1049, 566);
            this.m_airExchangerTab.TabIndex = 3;
            this.m_airExchangerTab.Text = "Ice Qube Air Exchanger";
            this.button10.Location = new Point(26, 473);
            this.button10.Name = "button10";
            this.button10.Size = new Size(120, 65);
            this.button10.TabIndex = 8;
            this.button10.Text = "Clear output";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new EventHandler(this.button10_Click);
            this.m_iceQubeOutput.FormattingEnabled = true;
            this.m_iceQubeOutput.Location = new Point(26, 164);
            this.m_iceQubeOutput.Name = "m_iceQubeOutput";
            this.m_iceQubeOutput.Size = new Size(436, 290);
            this.m_iceQubeOutput.TabIndex = 7;
            this.m_resetPersistentCounter.Location = new Point(528, 73);
            this.m_resetPersistentCounter.Name = "m_resetPersistentCounter";
            this.m_resetPersistentCounter.Size = new Size(120, 65);
            this.m_resetPersistentCounter.TabIndex = 6;
            this.m_resetPersistentCounter.Tag = (object)"RESETFAILURECOUNTER";
            this.m_resetPersistentCounter.Text = "Clear Error Counter";
            this.m_resetPersistentCounter.UseVisualStyleBackColor = true;
            this.m_resetPersistentCounter.Click += new EventHandler(this.OnExecuteIceQubeBoardCommand);
            this.m_qubeStatusButton.Location = new Point(188, 73);
            this.m_qubeStatusButton.Name = "m_qubeStatusButton";
            this.m_qubeStatusButton.Size = new Size(120, 65);
            this.m_qubeStatusButton.TabIndex = 3;
            this.m_qubeStatusButton.Tag = (object)"BOARDSTATUS";
            this.m_qubeStatusButton.Text = "Exchanger Board Status";
            this.m_qubeStatusButton.UseVisualStyleBackColor = true;
            this.m_qubeStatusButton.Click += new EventHandler(this.OnExecuteIceQubeBoardCommand);
            this.m_resetQubeBoard.Location = new Point(363, 73);
            this.m_resetQubeBoard.Name = "m_resetQubeBoard";
            this.m_resetQubeBoard.Size = new Size(120, 65);
            this.m_resetQubeBoard.TabIndex = 2;
            this.m_resetQubeBoard.Tag = (object)"RESET";
            this.m_resetQubeBoard.Text = "Reset Board";
            this.m_resetQubeBoard.UseVisualStyleBackColor = true;
            this.m_resetQubeBoard.Click += new EventHandler(this.OnExecuteIceQubeBoardCommand);
            this.m_configureQubeButton.Location = new Point(26, 73);
            this.m_configureQubeButton.Name = "m_configureQubeButton";
            this.m_configureQubeButton.Size = new Size(120, 65);
            this.m_configureQubeButton.TabIndex = 1;
            this.m_configureQubeButton.Text = "button1";
            this.m_configureQubeButton.UseVisualStyleBackColor = true;
            this.m_configureQubeButton.Click += new EventHandler(this.m_configureQubeButton_Click);
            this.m_iceQubeExchangerLabel.AutoSize = true;
            this.m_iceQubeExchangerLabel.Location = new Point(23, 27);
            this.m_iceQubeExchangerLabel.Name = "m_iceQubeExchangerLabel";
            this.m_iceQubeExchangerLabel.Size = new Size(35, 13);
            this.m_iceQubeExchangerLabel.TabIndex = 0;
            this.m_iceQubeExchangerLabel.Text = "label1";
            this.m_cortexTab.BackColor = Color.LightGray;
            this.m_cortexTab.Controls.Add((Control)this.m_startCortexButton);
            this.m_cortexTab.Controls.Add((Control)this.m_cortexStatusBox);
            this.m_cortexTab.Controls.Add((Control)this.label3);
            this.m_cortexTab.Controls.Add((Control)this.m_uninstallCortexButton);
            this.m_cortexTab.Controls.Add((Control)this.m_installCortexButton);
            this.m_cortexTab.Location = new Point(4, 56);
            this.m_cortexTab.Name = "m_cortexTab";
            this.m_cortexTab.Padding = new Padding(3);
            this.m_cortexTab.Size = new Size(1049, 566);
            this.m_cortexTab.TabIndex = 4;
            this.m_cortexTab.Text = "Cortex Camera";
            this.m_startCortexButton.Enabled = false;
            this.m_startCortexButton.Location = new Point(375, 51);
            this.m_startCortexButton.Name = "m_startCortexButton";
            this.m_startCortexButton.Size = new Size(150, 75);
            this.m_startCortexButton.TabIndex = 4;
            this.m_startCortexButton.Text = "Start Camera";
            this.m_startCortexButton.UseVisualStyleBackColor = true;
            this.m_startCortexButton.Click += new EventHandler(this.button1_Click);
            this.m_cortexStatusBox.FormattingEnabled = true;
            this.m_cortexStatusBox.Location = new Point(72, 161);
            this.m_cortexStatusBox.Name = "m_cortexStatusBox";
            this.m_cortexStatusBox.Size = new Size(453, 290);
            this.m_cortexStatusBox.TabIndex = 3;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(16, 161);
            this.label3.Name = "label3";
            this.label3.Size = new Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Status:";
            this.m_uninstallCortexButton.Location = new Point(189, 51);
            this.m_uninstallCortexButton.Name = "m_uninstallCortexButton";
            this.m_uninstallCortexButton.Size = new Size(150, 75);
            this.m_uninstallCortexButton.TabIndex = 1;
            this.m_uninstallCortexButton.Text = "Uninstall";
            this.m_uninstallCortexButton.UseVisualStyleBackColor = true;
            this.m_uninstallCortexButton.Click += new EventHandler(this.m_uninstallCortexButton_Click);
            this.m_installCortexButton.Location = new Point(19, 51);
            this.m_installCortexButton.Name = "m_installCortexButton";
            this.m_installCortexButton.Size = new Size(150, 75);
            this.m_installCortexButton.TabIndex = 0;
            this.m_installCortexButton.Text = "Install";
            this.m_installCortexButton.UseVisualStyleBackColor = true;
            this.m_installCortexButton.Click += new EventHandler(this.m_installCortexButton_Click);
            this.m_fraudSensorTab.BackColor = Color.LightGray;
            this.m_fraudSensorTab.Controls.Add((Control)this.button15);
            this.m_fraudSensorTab.Controls.Add((Control)this.button14);
            this.m_fraudSensorTab.Controls.Add((Control)this.label5);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_readPauseTB);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_scanPauseTB);
            this.m_fraudSensorTab.Controls.Add((Control)this.label4);
            this.m_fraudSensorTab.Controls.Add((Control)this.label2);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_stressIterationsTB);
            this.m_fraudSensorTab.Controls.Add((Control)this.button13);
            this.m_fraudSensorTab.Controls.Add((Control)this.button3);
            this.m_fraudSensorTab.Controls.Add((Control)this.button7);
            this.m_fraudSensorTab.Controls.Add((Control)this.button6);
            this.m_fraudSensorTab.Controls.Add((Control)this.button5);
            this.m_fraudSensorTab.Controls.Add((Control)this.button4);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_fraudSlotTB);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_fraudDeckTB);
            this.m_fraudSensorTab.Controls.Add((Control)this.groupBox1);
            this.m_fraudSensorTab.Controls.Add((Control)this.button1);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_configureFraudSensor);
            this.m_fraudSensorTab.Controls.Add((Control)this.m_fraudSensorOutput);
            this.m_fraudSensorTab.Location = new Point(4, 56);
            this.m_fraudSensorTab.Name = "m_fraudSensorTab";
            this.m_fraudSensorTab.Padding = new Padding(3);
            this.m_fraudSensorTab.Size = new Size(1049, 566);
            this.m_fraudSensorTab.TabIndex = 5;
            this.m_fraudSensorTab.Text = "Fraud Sensor";
            this.button15.Enabled = false;
            this.button15.Location = new Point(670, 435);
            this.button15.Name = "button15";
            this.button15.Size = new Size(99, 45);
            this.button15.TabIndex = 26;
            this.button15.Text = "Run Detection Test";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Visible = false;
            this.button15.Click += new EventHandler(this.button15_Click);
            this.button14.Location = new Point(510, 428);
            this.button14.Name = "button14";
            this.button14.Size = new Size(120, 52);
            this.button14.TabIndex = 25;
            this.button14.Text = "Center Disk";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new EventHandler(this.button14_Click);
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new Point(801, 516);
            this.label5.Name = "label5";
            this.label5.Size = new Size(110, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Pause between reads";
            this.label5.Visible = false;
            this.m_readPauseTB.Enabled = false;
            this.m_readPauseTB.Location = new Point(798, 532);
            this.m_readPauseTB.Name = "m_readPauseTB";
            this.m_readPauseTB.Size = new Size(100, 20);
            this.m_readPauseTB.TabIndex = 23;
            this.m_readPauseTB.Visible = false;
            this.m_scanPauseTB.Enabled = false;
            this.m_scanPauseTB.Location = new Point(798, 493);
            this.m_scanPauseTB.Name = "m_scanPauseTB";
            this.m_scanPauseTB.Size = new Size(100, 20);
            this.m_scanPauseTB.TabIndex = 22;
            this.m_scanPauseTB.Visible = false;
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new Point(801, 477);
            this.label4.Name = "label4";
            this.label4.Size = new Size(65, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Scan Pause";
            this.label4.Visible = false;
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new Point(801, 435);
            this.label2.Name = "label2";
            this.label2.Size = new Size(76, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Test iterations:";
            this.label2.Visible = false;
            this.m_stressIterationsTB.Enabled = false;
            this.m_stressIterationsTB.Location = new Point(798, 454);
            this.m_stressIterationsTB.Name = "m_stressIterationsTB";
            this.m_stressIterationsTB.Size = new Size(100, 20);
            this.m_stressIterationsTB.TabIndex = 19;
            this.m_stressIterationsTB.Visible = false;
            this.button13.Location = new Point(670, 495);
            this.button13.Name = "button13";
            this.button13.Size = new Size(99, 65);
            this.button13.TabIndex = 18;
            this.button13.Tag = (object)"SERIALBOARD RESET";
            this.button13.Text = "Initialize Control System";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new EventHandler(this.button7_Click);
            this.button3.Location = new Point(6, 454);
            this.button3.Name = "button3";
            this.button3.Size = new Size(120, 65);
            this.button3.TabIndex = 17;
            this.button3.Text = "Clear Output";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click_1);
            this.button7.Location = new Point(510, 493);
            this.button7.Name = "button7";
            this.button7.Size = new Size(120, 65);
            this.button7.TabIndex = 16;
            this.button7.Tag = (object)"SERIALBOARD CLOSEPORT";
            this.button7.Text = "Shutdown Control System";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new EventHandler(this.button7_Click);
            this.button6.Location = new Point(670, 372);
            this.button6.Name = "button6";
            this.button6.Size = new Size(75, 50);
            this.button6.TabIndex = 15;
            this.button6.Text = "Slot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new EventHandler(this.button6_Click);
            this.button5.Location = new Point(670, 316);
            this.button5.Name = "button5";
            this.button5.Size = new Size(75, 50);
            this.button5.TabIndex = 14;
            this.button5.Text = "Deck";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new EventHandler(this.button5_Click_1);
            this.button4.Location = new Point(510, 372);
            this.button4.Name = "button4";
            this.button4.Size = new Size(120, 50);
            this.button4.TabIndex = 13;
            this.button4.Text = "Put";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click_1);
            this.m_fraudSlotTB.Location = new Point(751, 402);
            this.m_fraudSlotTB.Name = "m_fraudSlotTB";
            this.m_fraudSlotTB.Size = new Size(100, 20);
            this.m_fraudSlotTB.TabIndex = 12;
            this.m_fraudDeckTB.Location = new Point(751, 346);
            this.m_fraudDeckTB.Name = "m_fraudDeckTB";
            this.m_fraudDeckTB.Size = new Size(100, 20);
            this.m_fraudDeckTB.TabIndex = 8;
            this.groupBox1.Controls.Add((Control)this.m_powerOffFraudSensor);
            this.groupBox1.Controls.Add((Control)this.m_powerOnFraudSensor);
            this.groupBox1.Controls.Add((Control)this.m_testDiskForMarkers);
            this.groupBox1.Controls.Add((Control)this.m_stopFraudScan);
            this.groupBox1.Controls.Add((Control)this.m_startFraudScan);
            this.groupBox1.Controls.Add((Control)this.m_resetFraudButton);
            this.groupBox1.Controls.Add((Control)this.m_testFraudButton);
            this.groupBox1.Controls.Add((Control)this.m_fraudPOSTButton);
            this.groupBox1.Location = new Point(487, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(390, 297);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sensor Tests";
            this.m_powerOffFraudSensor.Location = new Point(222, 85);
            this.m_powerOffFraudSensor.Name = "m_powerOffFraudSensor";
            this.m_powerOffFraudSensor.Size = new Size(150, 60);
            this.m_powerOffFraudSensor.TabIndex = 10;
            this.m_powerOffFraudSensor.Tag = (object)"SHUTDOWN";
            this.m_powerOffFraudSensor.Text = "Turn Off Power";
            this.m_powerOffFraudSensor.UseVisualStyleBackColor = true;
            this.m_powerOffFraudSensor.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.m_powerOnFraudSensor.Location = new Point(23, 85);
            this.m_powerOnFraudSensor.Name = "m_powerOnFraudSensor";
            this.m_powerOnFraudSensor.Size = new Size(150, 60);
            this.m_powerOnFraudSensor.TabIndex = 9;
            this.m_powerOnFraudSensor.Tag = (object)"INITIALIZE";
            this.m_powerOnFraudSensor.Text = "Power Sensor";
            this.m_powerOnFraudSensor.UseVisualStyleBackColor = true;
            this.m_powerOnFraudSensor.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.m_testDiskForMarkers.Location = new Point(23, 218);
            this.m_testDiskForMarkers.Name = "m_testDiskForMarkers";
            this.m_testDiskForMarkers.Size = new Size(150, 60);
            this.m_testDiskForMarkers.TabIndex = 8;
            this.m_testDiskForMarkers.Text = "Test Disk In Picker";
            this.m_testDiskForMarkers.UseVisualStyleBackColor = true;
            this.m_testDiskForMarkers.Click += new EventHandler(this.m_testFraudButton_Click);
            this.m_stopFraudScan.Location = new Point(222, 152);
            this.m_stopFraudScan.Name = "m_stopFraudScan";
            this.m_stopFraudScan.Size = new Size(150, 60);
            this.m_stopFraudScan.TabIndex = 7;
            this.m_stopFraudScan.Tag = (object)"STOPSCAN";
            this.m_stopFraudScan.Text = "Stop Scan";
            this.m_stopFraudScan.UseVisualStyleBackColor = true;
            this.m_stopFraudScan.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.m_startFraudScan.Location = new Point(23, 152);
            this.m_startFraudScan.Name = "m_startFraudScan";
            this.m_startFraudScan.Size = new Size(150, 60);
            this.m_startFraudScan.TabIndex = 6;
            this.m_startFraudScan.Tag = (object)"STARTSCAN";
            this.m_startFraudScan.Text = "Start Scan";
            this.m_startFraudScan.UseVisualStyleBackColor = true;
            this.m_startFraudScan.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.m_resetFraudButton.Location = new Point(222, 19);
            this.m_resetFraudButton.Name = "m_resetFraudButton";
            this.m_resetFraudButton.Size = new Size(150, 60);
            this.m_resetFraudButton.TabIndex = 5;
            this.m_resetFraudButton.Tag = (object)"RESET";
            this.m_resetFraudButton.Text = "Reset";
            this.m_resetFraudButton.UseVisualStyleBackColor = true;
            this.m_resetFraudButton.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.m_testFraudButton.Location = new Point(222, 218);
            this.m_testFraudButton.Name = "m_testFraudButton";
            this.m_testFraudButton.Size = new Size(150, 60);
            this.m_testFraudButton.TabIndex = 3;
            this.m_testFraudButton.Text = "Take - Test - Return";
            this.m_testFraudButton.UseVisualStyleBackColor = true;
            this.m_testFraudButton.Click += new EventHandler(this.m_testUserDiskForFraud);
            this.m_fraudPOSTButton.Location = new Point(23, 19);
            this.m_fraudPOSTButton.Name = "m_fraudPOSTButton";
            this.m_fraudPOSTButton.Size = new Size(150, 60);
            this.m_fraudPOSTButton.TabIndex = 4;
            this.m_fraudPOSTButton.Tag = (object)"LASTPOSTRESULT";
            this.m_fraudPOSTButton.Text = "Last POST Result";
            this.m_fraudPOSTButton.UseVisualStyleBackColor = true;
            this.m_fraudPOSTButton.Click += new EventHandler(this.OnExecuteFraudSensorInstruction);
            this.button1.Location = new Point(510, 309);
            this.button1.Name = "button1";
            this.button1.Size = new Size(120, 57);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click_2);
            this.m_configureFraudSensor.Location = new Point(6, 62);
            this.m_configureFraudSensor.Name = "m_configureFraudSensor";
            this.m_configureFraudSensor.Size = new Size(150, 75);
            this.m_configureFraudSensor.TabIndex = 1;
            this.m_configureFraudSensor.Text = "Configure";
            this.m_configureFraudSensor.UseVisualStyleBackColor = true;
            this.m_configureFraudSensor.Click += new EventHandler(this.button1_Click_1);
            this.m_fraudSensorOutput.FormattingEnabled = true;
            this.m_fraudSensorOutput.HorizontalScrollbar = true;
            this.m_fraudSensorOutput.Location = new Point(6, 158);
            this.m_fraudSensorOutput.Name = "m_fraudSensorOutput";
            this.m_fraudSensorOutput.ScrollAlwaysVisible = true;
            this.m_fraudSensorOutput.Size = new Size(450, 290);
            this.m_fraudSensorOutput.TabIndex = 0;
            this.m_kioskFunctionCheck.BackColor = Color.LightGray;
            this.m_kioskFunctionCheck.Controls.Add((Control)this.m_displaySessionsButton);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.m_sourceSlotTB);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.button19);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.m_sourceDeckTB);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.button22);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.groupBox2);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.m_userName);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.button21);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.label1);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.button2);
            this.m_kioskFunctionCheck.Controls.Add((Control)this.m_kioskTestOutput);
            this.m_kioskFunctionCheck.Location = new Point(4, 56);
            this.m_kioskFunctionCheck.Name = "m_kioskFunctionCheck";
            this.m_kioskFunctionCheck.Padding = new Padding(3);
            this.m_kioskFunctionCheck.Size = new Size(1049, 566);
            this.m_kioskFunctionCheck.TabIndex = 6;
            this.m_kioskFunctionCheck.Text = "Kiosk Function Check";
            this.m_displaySessionsButton.Location = new Point(680, 501);
            this.m_displaySessionsButton.Name = "m_displaySessionsButton";
            this.m_displaySessionsButton.Size = new Size(104, 54);
            this.m_displaySessionsButton.TabIndex = 22;
            this.m_displaySessionsButton.Text = "Show Sessions";
            this.m_displaySessionsButton.UseVisualStyleBackColor = true;
            this.m_displaySessionsButton.Click += new EventHandler(this.button16_Click_1);
            this.m_sourceSlotTB.Location = new Point(277, 83);
            this.m_sourceSlotTB.Name = "m_sourceSlotTB";
            this.m_sourceSlotTB.Size = new Size(75, 20);
            this.m_sourceSlotTB.TabIndex = 11;
            this.button19.Location = new Point(801, 501);
            this.button19.Name = "button19";
            this.button19.Size = new Size(100, 55);
            this.button19.TabIndex = 13;
            this.button19.Text = "Send Report";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new EventHandler(this.button19_Click);
            this.m_sourceDeckTB.Location = new Point(90, 83);
            this.m_sourceDeckTB.Name = "m_sourceDeckTB";
            this.m_sourceDeckTB.Size = new Size(68, 20);
            this.m_sourceDeckTB.TabIndex = 9;
            this.button22.Location = new Point(196, 65);
            this.button22.Name = "button22";
            this.button22.Size = new Size(75, 38);
            this.button22.TabIndex = 10;
            this.button22.Text = "Slot";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new EventHandler(this.button22_Click);
            this.groupBox2.Controls.Add((Control)this.m_unknownCountTB);
            this.groupBox2.Controls.Add((Control)this.m_cameraSnapTB);
            this.groupBox2.Controls.Add((Control)this.btn_KFCDecodeTest);
            this.groupBox2.Controls.Add((Control)this.m_verticalTestTB);
            this.groupBox2.Controls.Add((Control)this.m_initTB);
            this.groupBox2.Controls.Add((Control)this.m_testVendDoorTB);
            this.groupBox2.Controls.Add((Control)this.m_checkDriversStatusTB);
            this.groupBox2.Controls.Add((Control)this.btn_KFCUnknownCount);
            this.groupBox2.Controls.Add((Control)this.btn_KFCVerticalSlotTest);
            this.groupBox2.Controls.Add((Control)this.btn_KFCTestVendDoor);
            this.groupBox2.Controls.Add((Control)this.btn_KFCInit);
            this.groupBox2.Controls.Add((Control)this.btn_KFCCheckDrivers);
            this.groupBox2.Location = new Point(9, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(325, 437);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Steps:";
            this.m_unknownCountTB.Location = new Point(133, 411);
            this.m_unknownCountTB.Name = "m_unknownCountTB";
            this.m_unknownCountTB.ReadOnly = true;
            this.m_unknownCountTB.Size = new Size(129, 20);
            this.m_unknownCountTB.TabIndex = 17;
            this.m_cameraSnapTB.Location = new Point(133, 129);
            this.m_cameraSnapTB.Name = "m_cameraSnapTB";
            this.m_cameraSnapTB.ReadOnly = true;
            this.m_cameraSnapTB.Size = new Size(129, 20);
            this.m_cameraSnapTB.TabIndex = 16;
            this.btn_KFCDecodeTest.Location = new Point(12, 89);
            this.btn_KFCDecodeTest.Name = "btn_KFCDecodeTest";
            this.btn_KFCDecodeTest.Size = new Size(100, 60);
            this.btn_KFCDecodeTest.TabIndex = 2;
            this.btn_KFCDecodeTest.Text = "Decode Test";
            this.btn_KFCDecodeTest.UseVisualStyleBackColor = true;
            this.btn_KFCDecodeTest.Click += new EventHandler(this.button20_Click);
            this.m_verticalTestTB.Location = new Point(133, 338);
            this.m_verticalTestTB.Name = "m_verticalTestTB";
            this.m_verticalTestTB.ReadOnly = true;
            this.m_verticalTestTB.Size = new Size(129, 20);
            this.m_verticalTestTB.TabIndex = 13;
            this.m_initTB.Location = new Point(133, 272);
            this.m_initTB.Name = "m_initTB";
            this.m_initTB.ReadOnly = true;
            this.m_initTB.Size = new Size(129, 20);
            this.m_initTB.TabIndex = 12;
            this.m_testVendDoorTB.Location = new Point(133, 195);
            this.m_testVendDoorTB.Name = "m_testVendDoorTB";
            this.m_testVendDoorTB.ReadOnly = true;
            this.m_testVendDoorTB.Size = new Size(129, 20);
            this.m_testVendDoorTB.TabIndex = 11;
            this.m_checkDriversStatusTB.Location = new Point(133, 59);
            this.m_checkDriversStatusTB.Name = "m_checkDriversStatusTB";
            this.m_checkDriversStatusTB.ReadOnly = true;
            this.m_checkDriversStatusTB.Size = new Size(129, 20);
            this.m_checkDriversStatusTB.TabIndex = 9;
            this.btn_KFCUnknownCount.Location = new Point(12, 371);
            this.btn_KFCUnknownCount.Name = "btn_KFCUnknownCount";
            this.btn_KFCUnknownCount.Size = new Size(100, 60);
            this.btn_KFCUnknownCount.TabIndex = 7;
            this.btn_KFCUnknownCount.Text = "Unknown Count";
            this.btn_KFCUnknownCount.UseVisualStyleBackColor = true;
            this.btn_KFCUnknownCount.Click += new EventHandler(this.button18_Click);
            this.btn_KFCVerticalSlotTest.Location = new Point(12, 298);
            this.btn_KFCVerticalSlotTest.Name = "btn_KFCVerticalSlotTest";
            this.btn_KFCVerticalSlotTest.Size = new Size(100, 60);
            this.btn_KFCVerticalSlotTest.TabIndex = 6;
            this.btn_KFCVerticalSlotTest.Text = "Vertical Slot Test";
            this.btn_KFCVerticalSlotTest.UseVisualStyleBackColor = true;
            this.btn_KFCVerticalSlotTest.Click += new EventHandler(this.button17_Click);
            this.btn_KFCTestVendDoor.Location = new Point(12, 155);
            this.btn_KFCTestVendDoor.Name = "btn_KFCTestVendDoor";
            this.btn_KFCTestVendDoor.Size = new Size(100, 60);
            this.btn_KFCTestVendDoor.TabIndex = 4;
            this.btn_KFCTestVendDoor.Text = "Test Vend Door";
            this.btn_KFCTestVendDoor.UseVisualStyleBackColor = true;
            this.btn_KFCTestVendDoor.Click += new EventHandler(this.button12_Click);
            this.btn_KFCInit.Location = new Point(12, 232);
            this.btn_KFCInit.Name = "btn_KFCInit";
            this.btn_KFCInit.Size = new Size(100, 60);
            this.btn_KFCInit.TabIndex = 5;
            this.btn_KFCInit.Text = "Init";
            this.btn_KFCInit.UseVisualStyleBackColor = true;
            this.btn_KFCInit.Click += new EventHandler(this.button9_Click);
            this.btn_KFCCheckDrivers.Location = new Point(12, 19);
            this.btn_KFCCheckDrivers.Name = "btn_KFCCheckDrivers";
            this.btn_KFCCheckDrivers.Size = new Size(100, 60);
            this.btn_KFCCheckDrivers.TabIndex = 0;
            this.btn_KFCCheckDrivers.Text = "Check Drivers";
            this.btn_KFCCheckDrivers.UseVisualStyleBackColor = true;
            this.btn_KFCCheckDrivers.Click += new EventHandler(this.button8_Click);
            this.m_userName.Location = new Point(6, 31);
            this.m_userName.Name = "m_userName";
            this.m_userName.ReadOnly = true;
            this.m_userName.Size = new Size(228, 20);
            this.m_userName.TabIndex = 3;
            this.button21.Location = new Point(9, 65);
            this.button21.Name = "button21";
            this.button21.Size = new Size(75, 38);
            this.button21.TabIndex = 8;
            this.button21.Text = "Deck";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new EventHandler(this.button21_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            this.button2.Location = new Point(460, 501);
            this.button2.Name = "button2";
            this.button2.Size = new Size(100, 55);
            this.button2.TabIndex = 12;
            this.button2.Text = "Clear Output";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click_1);
            this.m_kioskTestOutput.FormattingEnabled = true;
            this.m_kioskTestOutput.Location = new Point(460, 6);
            this.m_kioskTestOutput.Name = "m_kioskTestOutput";
            this.m_kioskTestOutput.Size = new Size(441, 485);
            this.m_kioskTestOutput.TabIndex = 0;
            this.tabPage1.BackColor = Color.LightGray;
            this.tabPage1.Controls.Add((Control)this.listView1);
            this.tabPage1.Controls.Add((Control)this.button16);
            this.tabPage1.Controls.Add((Control)this.button11);
            this.tabPage1.Location = new Point(4, 56);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(1049, 566);
            this.tabPage1.TabIndex = 7;
            this.tabPage1.Text = "Hardware Survey";
            this.listView1.HideSelection = false;
            this.listView1.Location = new Point(20, 35);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(502, 378);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.button16.Location = new Point(656, 194);
            this.button16.Name = "button16";
            this.button16.Size = new Size(140, 80);
            this.button16.TabIndex = 2;
            this.button16.Text = "Clear";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new EventHandler(this.button16_Click_2);
            this.button11.Location = new Point(656, 74);
            this.button11.Name = "button11";
            this.button11.Size = new Size(140, 80);
            this.button11.TabIndex = 1;
            this.button11.Text = "Run Survey";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new EventHandler(this.button11_Click);
            this.m_hwCorrections.BackColor = Color.LightGray;
            this.m_hwCorrections.Controls.Add((Control)this.groupBox4);
            this.m_hwCorrections.Controls.Add((Control)this.groupBox6);
            this.m_hwCorrections.Controls.Add((Control)this.groupBox5);
            this.m_hwCorrections.Controls.Add((Control)this.m_hwCorrectiontypeGB);
            this.m_hwCorrections.Controls.Add((Control)this.button23);
            this.m_hwCorrections.Controls.Add((Control)this.m_hwStatsOutput);
            this.m_hwCorrections.Location = new Point(4, 56);
            this.m_hwCorrections.Name = "m_hwCorrections";
            this.m_hwCorrections.Padding = new Padding(3);
            this.m_hwCorrections.Size = new Size(1049, 566);
            this.m_hwCorrections.TabIndex = 9;
            this.m_hwCorrections.Text = "Hardware Corrections";
            this.groupBox4.Controls.Add((Control)this.m_testRouterRelay);
            this.groupBox4.Controls.Add((Control)this.m_configureRouterRelay);
            this.groupBox4.Location = new Point(513, 279);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(200, 175);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Router ";
            this.m_testRouterRelay.Location = new Point(23, 100);
            this.m_testRouterRelay.Name = "m_testRouterRelay";
            this.m_testRouterRelay.Size = new Size(120, 50);
            this.m_testRouterRelay.TabIndex = 1;
            this.m_testRouterRelay.Text = "Power Cycle";
            this.m_testRouterRelay.UseVisualStyleBackColor = true;
            this.m_testRouterRelay.Click += new EventHandler(this.button38_Click);
            this.m_configureRouterRelay.Location = new Point(23, 31);
            this.m_configureRouterRelay.Name = "m_configureRouterRelay";
            this.m_configureRouterRelay.Size = new Size(120, 50);
            this.m_configureRouterRelay.TabIndex = 0;
            this.m_configureRouterRelay.Text = "Configure";
            this.m_configureRouterRelay.UseVisualStyleBackColor = true;
            this.m_configureRouterRelay.Click += new EventHandler(this.button37_Click);
            this.groupBox6.Controls.Add((Control)this.m_restartArcus);
            this.groupBox6.Controls.Add((Control)this.m_configureArcus);
            this.groupBox6.Location = new Point(782, 280);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(200, 175);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Arcus";
            this.m_restartArcus.Location = new Point(26, 99);
            this.m_restartArcus.Name = "m_restartArcus";
            this.m_restartArcus.Size = new Size(120, 50);
            this.m_restartArcus.TabIndex = 1;
            this.m_restartArcus.Text = "Restart";
            this.m_restartArcus.UseVisualStyleBackColor = true;
            this.m_restartArcus.Click += new EventHandler(this.m_restartArcus_Click);
            this.m_configureArcus.Location = new Point(26, 30);
            this.m_configureArcus.Name = "m_configureArcus";
            this.m_configureArcus.Size = new Size(120, 50);
            this.m_configureArcus.TabIndex = 0;
            this.m_configureArcus.Text = "Configure";
            this.m_configureArcus.UseVisualStyleBackColor = true;
            this.m_configureArcus.Click += new EventHandler(this.m_configureArcus_Click);
            this.groupBox5.Controls.Add((Control)this.m_restartTouchscreen);
            this.groupBox5.Location = new Point(782, 51);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(200, 117);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Touchscreen";
            this.m_restartTouchscreen.Location = new Point(33, 33);
            this.m_restartTouchscreen.Name = "m_restartTouchscreen";
            this.m_restartTouchscreen.Size = new Size(120, 50);
            this.m_restartTouchscreen.TabIndex = 0;
            this.m_restartTouchscreen.Text = "Reset";
            this.m_restartTouchscreen.UseVisualStyleBackColor = true;
            this.m_restartTouchscreen.Click += new EventHandler(this.m_restartTouchscreen_Click);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.radioButton5);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.radioButton4);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.radioButton2);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.radioButton3);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.button24);
            this.m_hwCorrectiontypeGB.Controls.Add((Control)this.radioButton1);
            this.m_hwCorrectiontypeGB.Location = new Point(513, 43);
            this.m_hwCorrectiontypeGB.Name = "m_hwCorrectiontypeGB";
            this.m_hwCorrectiontypeGB.Size = new Size(200, 217);
            this.m_hwCorrectiontypeGB.TabIndex = 4;
            this.m_hwCorrectiontypeGB.TabStop = false;
            this.m_hwCorrectiontypeGB.Text = "Correction Type";
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new Point(23, 93);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new Size(141, 17);
            this.radioButton5.TabIndex = 6;
            this.radioButton5.TabStop = true;
            this.radioButton5.Tag = (object)"UnexpectedPowerLoss";
            this.radioButton5.Text = "Unexpected Power Loss";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new Point(23, 70);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(57, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Tag = (object)"RouterRecycle";
            this.radioButton4.Text = "Router";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new Point(23, 116);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(36, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Tag = (object)"None";
            this.radioButton2.Text = "All";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new Point(23, 47);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(88, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Tag = (object)"Touchscreen";
            this.radioButton3.Text = "Touchscreen";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.button24.Location = new Point(23, 150);
            this.button24.Name = "button24";
            this.button24.Size = new Size(120, 50);
            this.button24.TabIndex = 3;
            this.button24.Text = "Show Stats";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new EventHandler(this.button24_Click);
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new Point(23, 24);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(52, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = (object)"Arcus";
            this.radioButton1.Text = "Arcus";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.button23.Location = new Point(21, 473);
            this.button23.Name = "button23";
            this.button23.Size = new Size(120, 57);
            this.button23.TabIndex = 1;
            this.button23.Text = "Clear Output";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new EventHandler(this.button23_Click);
            this.m_hwStatsOutput.FormattingEnabled = true;
            this.m_hwStatsOutput.Location = new Point(21, 35);
            this.m_hwStatsOutput.Name = "m_hwStatsOutput";
            this.m_hwStatsOutput.Size = new Size(422, 420);
            this.m_hwStatsOutput.TabIndex = 0;
            this.m_irConfigurationTab.BackColor = Color.LightGray;
            this.m_irConfigurationTab.Controls.Add((Control)this.button37);
            this.m_irConfigurationTab.Controls.Add((Control)this.groupBox8);
            this.m_irConfigurationTab.Controls.Add((Control)this.groupBox7);
            this.m_irConfigurationTab.Controls.Add((Control)this.m_irCameraOutput);
            this.m_irConfigurationTab.Location = new Point(4, 56);
            this.m_irConfigurationTab.Name = "m_irConfigurationTab";
            this.m_irConfigurationTab.Size = new Size(1049, 566);
            this.m_irConfigurationTab.TabIndex = 11;
            this.m_irConfigurationTab.Text = "IR Camera";
            this.button37.Location = new Point(352, 64);
            this.button37.Name = "button37";
            this.button37.Size = new Size(115, 60);
            this.button37.TabIndex = 3;
            this.button37.Text = "button37";
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Click += new EventHandler(this.button37_Click_1);
            this.groupBox8.Controls.Add((Control)this.textBox2);
            this.groupBox8.Controls.Add((Control)this.textBox1);
            this.groupBox8.Controls.Add((Control)this.button47);
            this.groupBox8.Controls.Add((Control)this.button46);
            this.groupBox8.Controls.Add((Control)this.button45);
            this.groupBox8.Controls.Add((Control)this.button44);
            this.groupBox8.Controls.Add((Control)this.button43);
            this.groupBox8.Location = new Point(36, 299);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new Size(348, 205);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Hardware Control";
            this.textBox2.Location = new Point(225, 81);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(100, 20);
            this.textBox2.TabIndex = 9;
            this.textBox1.Location = new Point(225, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(100, 20);
            this.textBox1.TabIndex = 8;
            this.button47.Location = new Point(144, 73);
            this.button47.Name = "button47";
            this.button47.Size = new Size(75, 40);
            this.button47.TabIndex = 7;
            this.button47.Text = "Slot";
            this.button47.UseVisualStyleBackColor = true;
            this.button47.Click += new EventHandler(this.button47_Click);
            this.button46.Location = new Point(144, 27);
            this.button46.Name = "button46";
            this.button46.Size = new Size(75, 40);
            this.button46.TabIndex = 6;
            this.button46.Text = "Deck";
            this.button46.UseVisualStyleBackColor = true;
            this.button46.Click += new EventHandler(this.button46_Click);
            this.button45.Location = new Point(144, 124);
            this.button45.Name = "button45";
            this.button45.Size = new Size(115, 75);
            this.button45.TabIndex = 5;
            this.button45.Tag = (object)"CONTROLSYSTEM CENTER";
            this.button45.Text = "Center disk";
            this.button45.UseVisualStyleBackColor = true;
            this.button45.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button44.Location = new Point(12, 124);
            this.button44.Name = "button44";
            this.button44.Size = new Size(115, 75);
            this.button44.TabIndex = 4;
            this.button44.Text = "Put";
            this.button44.UseVisualStyleBackColor = true;
            this.button44.Click += new EventHandler(this.button44_Click);
            this.button43.Location = new Point(12, 26);
            this.button43.Name = "button43";
            this.button43.Size = new Size(115, 75);
            this.button43.TabIndex = 4;
            this.button43.Text = "Get and Read";
            this.button43.UseVisualStyleBackColor = true;
            this.button43.Click += new EventHandler(this.button43_Click);
            this.groupBox7.Controls.Add((Control)this.button42);
            this.groupBox7.Controls.Add((Control)this.button41);
            this.groupBox7.Controls.Add((Control)this.button40);
            this.groupBox7.Controls.Add((Control)this.button39);
            this.groupBox7.Controls.Add((Control)this.button38);
            this.groupBox7.Location = new Point(36, 34);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(298, 238);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Camera Control";
            this.button42.Location = new Point(18, 162);
            this.button42.Name = "button42";
            this.button42.Size = new Size(115, 60);
            this.button42.TabIndex = 4;
            this.button42.Text = "Launch Tuner";
            this.button42.UseVisualStyleBackColor = true;
            this.button42.Click += new EventHandler(this.button42_Click);
            this.button41.Location = new Point(163, 96);
            this.button41.Name = "button41";
            this.button41.Size = new Size(115, 60);
            this.button41.TabIndex = 4;
            this.button41.Tag = (object)"CONTROLSYSTEM RINGLIGHTOFF";
            this.button41.Text = "Ringlight Off";
            this.button41.UseVisualStyleBackColor = true;
            this.button41.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button40.Location = new Point(18, 96);
            this.button40.Name = "button40";
            this.button40.Size = new Size(115, 60);
            this.button40.TabIndex = 4;
            this.button40.Tag = (object)"CAMERA STOP";
            this.button40.Text = "Stop Camera";
            this.button40.UseVisualStyleBackColor = true;
            this.button40.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button39.Location = new Point(163, 30);
            this.button39.Name = "button39";
            this.button39.Size = new Size(115, 60);
            this.button39.TabIndex = 4;
            this.button39.Tag = (object)"CONTROLSYSTEM RINGLIGHTON";
            this.button39.Text = "Ringlight On";
            this.button39.UseVisualStyleBackColor = true;
            this.button39.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.button38.Location = new Point(18, 30);
            this.button38.Name = "button38";
            this.button38.Size = new Size(115, 60);
            this.button38.TabIndex = 4;
            this.button38.Tag = (object)"CAMERA START";
            this.button38.Text = "Start Camera";
            this.button38.UseVisualStyleBackColor = true;
            this.button38.Click += new EventHandler(this.OnExecuteResponseInstruction);
            this.m_irCameraOutput.FormattingEnabled = true;
            this.m_irCameraOutput.Location = new Point(558, 48);
            this.m_irCameraOutput.Name = "m_irCameraOutput";
            this.m_irCameraOutput.Size = new Size(440, 420);
            this.m_irCameraOutput.TabIndex = 0;
            this.m_engineeringTab.BackColor = Color.LightGray;
            this.m_engineeringTab.Controls.Add((Control)this.groupBox3);
            this.m_engineeringTab.Controls.Add((Control)this.button28);
            this.m_engineeringTab.Controls.Add((Control)this.button27);
            this.m_engineeringTab.Controls.Add((Control)this.button26);
            this.m_engineeringTab.Controls.Add((Control)this.button25);
            this.m_engineeringTab.Controls.Add((Control)this.m_engineeringOutput);
            this.m_engineeringTab.Location = new Point(4, 56);
            this.m_engineeringTab.Name = "m_engineeringTab";
            this.m_engineeringTab.Padding = new Padding(3);
            this.m_engineeringTab.Size = new Size(1049, 566);
            this.m_engineeringTab.TabIndex = 10;
            this.m_engineeringTab.Text = "Engineering";
            this.groupBox3.Controls.Add((Control)this.button36);
            this.groupBox3.Controls.Add((Control)this.button35);
            this.groupBox3.Controls.Add((Control)this.button34);
            this.groupBox3.Controls.Add((Control)this.button29);
            this.groupBox3.Controls.Add((Control)this.button30);
            this.groupBox3.Controls.Add((Control)this.button33);
            this.groupBox3.Controls.Add((Control)this.button32);
            this.groupBox3.Controls.Add((Control)this.m_engEndSlotTB);
            this.groupBox3.Controls.Add((Control)this.button31);
            this.groupBox3.Controls.Add((Control)this.m_engEndDeckTB);
            this.groupBox3.Controls.Add((Control)this.m_engDeckTB);
            this.groupBox3.Controls.Add((Control)this.m_engSlotTB);
            this.groupBox3.Location = new Point(621, 46);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(292, 392);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Locations";
            this.button36.Location = new Point(161, 293);
            this.button36.Name = "button36";
            this.button36.Size = new Size(110, 60);
            this.button36.TabIndex = 18;
            this.button36.Text = "Put to";
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new EventHandler(this.button36_Click);
            this.button35.Location = new Point(18, 293);
            this.button35.Name = "button35";
            this.button35.Size = new Size(110, 60);
            this.button35.TabIndex = 17;
            this.button35.Text = "Get from";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new EventHandler(this.button35_Click);
            this.button34.Location = new Point(171, 26);
            this.button34.Name = "button34";
            this.button34.Size = new Size(100, 40);
            this.button34.TabIndex = 16;
            this.button34.Text = "Start slot";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new EventHandler(this.button34_Click);
            this.button29.Location = new Point(161, 216);
            this.button29.Name = "button29";
            this.button29.Size = new Size(110, 60);
            this.button29.TabIndex = 9;
            this.button29.Text = "Sync slots";
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new EventHandler(this.button29_Click);
            this.button30.Location = new Point(18, 216);
            this.button30.Name = "button30";
            this.button30.Size = new Size(110, 60);
            this.button30.TabIndex = 10;
            this.button30.Text = "Sync slot";
            this.button30.UseVisualStyleBackColor = true;
            this.button30.Click += new EventHandler(this.button30_Click);
            this.button33.Location = new Point(18, 26);
            this.button33.Name = "button33";
            this.button33.Size = new Size(100, 40);
            this.button33.TabIndex = 15;
            this.button33.Text = "Start deck";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new EventHandler(this.button33_Click);
            this.button32.Location = new Point(171, 121);
            this.button32.Name = "button32";
            this.button32.Size = new Size(100, 40);
            this.button32.TabIndex = 14;
            this.button32.Text = "End slot";
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new EventHandler(this.button32_Click);
            this.m_engEndSlotTB.Location = new Point(171, 167);
            this.m_engEndSlotTB.Name = "m_engEndSlotTB";
            this.m_engEndSlotTB.Size = new Size(100, 20);
            this.m_engEndSlotTB.TabIndex = 12;
            this.button31.Location = new Point(18, 121);
            this.button31.Name = "button31";
            this.button31.Size = new Size(100, 40);
            this.button31.TabIndex = 13;
            this.button31.Text = "End deck";
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new EventHandler(this.button31_Click);
            this.m_engEndDeckTB.Location = new Point(18, 167);
            this.m_engEndDeckTB.Name = "m_engEndDeckTB";
            this.m_engEndDeckTB.Size = new Size(100, 20);
            this.m_engEndDeckTB.TabIndex = 11;
            this.m_engDeckTB.Location = new Point(18, 72);
            this.m_engDeckTB.Name = "m_engDeckTB";
            this.m_engDeckTB.Size = new Size(100, 20);
            this.m_engDeckTB.TabIndex = 5;
            this.m_engSlotTB.Location = new Point(171, 72);
            this.m_engSlotTB.Name = "m_engSlotTB";
            this.m_engSlotTB.Size = new Size(100, 20);
            this.m_engSlotTB.TabIndex = 6;
            this.button28.Location = new Point(441, 220);
            this.button28.Name = "button28";
            this.button28.Size = new Size(120, 61);
            this.button28.TabIndex = 4;
            this.button28.Text = "Read barcode";
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new EventHandler(this.button28_Click);
            this.button27.Location = new Point(441, 133);
            this.button27.Name = "button27";
            this.button27.Size = new Size(120, 65);
            this.button27.TabIndex = 3;
            this.button27.Text = "Vend disk in picker";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new EventHandler(this.button27_Click);
            this.button26.Location = new Point(441, 46);
            this.button26.Name = "button26";
            this.button26.Size = new Size(120, 68);
            this.button26.TabIndex = 2;
            this.button26.Text = "Take Disk at door";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new EventHandler(this.button26_Click);
            this.button25.Location = new Point(21, 455);
            this.button25.Name = "button25";
            this.button25.Size = new Size(120, 58);
            this.button25.TabIndex = 1;
            this.button25.Text = "Clear Output";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new EventHandler(this.button25_Click);
            this.m_engineeringOutput.FormattingEnabled = true;
            this.m_engineeringOutput.Location = new Point(21, 31);
            this.m_engineeringOutput.Name = "m_engineeringOutput";
            this.m_engineeringOutput.Size = new Size(392, 407);
            this.m_engineeringOutput.TabIndex = 0;
            this.m_exitButton.BackColor = Color.GreenYellow;
            this.m_exitButton.Location = new Point(466, 644);
            this.m_exitButton.Name = "m_exitButton";
            this.m_exitButton.Size = new Size(135, 59);
            this.m_exitButton.TabIndex = 14;
            this.m_exitButton.Text = "Exit";
            this.m_exitButton.UseVisualStyleBackColor = false;
            this.m_exitButton.Click += new EventHandler(this.m_exitButton_Click);
            this.errorProvider1.ContainerControl = (ContainerControl)this;
            this.m_verticalSyncWorker.DoWork += new DoWorkEventHandler(this.m_verticalSyncWorker_DoWork);
            this.m_verticalSyncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.m_verticalSyncWorker_RunWorkerCompleted);
            this.m_runInitWorker.DoWork += new DoWorkEventHandler(this.m_runInitWorker_DoWork);
            this.m_runInitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.m_runInitWorker_RunWorkerCompleted);
            this.m_runDecodeWorker.DoWork += new DoWorkEventHandler(this.m_runDecodeWorker_DoWork);
            this.m_runDecodeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.m_runDecodeWorker_RunWorkerCompleted);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1116, 715);
            this.Controls.Add((Control)this.m_exitButton);
            this.Controls.Add((Control)this.m_devicesTab);
            this.MaximizeBox = false;
            this.Name = nameof(ConfiguredDevicesForm);
            this.Text = nameof(ConfiguredDevicesForm);
            this.m_devicesTab.ResumeLayout(false);
            this.m_airExchangerTab.ResumeLayout(false);
            this.m_airExchangerTab.PerformLayout();
            this.m_cortexTab.ResumeLayout(false);
            this.m_cortexTab.PerformLayout();
            this.m_fraudSensorTab.ResumeLayout(false);
            this.m_fraudSensorTab.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.m_kioskFunctionCheck.ResumeLayout(false);
            this.m_kioskFunctionCheck.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.m_hwCorrections.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.m_hwCorrectiontypeGB.ResumeLayout(false);
            this.m_hwCorrectiontypeGB.PerformLayout();
            this.m_irConfigurationTab.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.m_engineeringTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((ISupportInitialize)this.errorProvider1).EndInit();
            this.ResumeLayout(false);
        }

        private sealed class ConfiguredDeviceOuptutBox : OutputBox
        {
            protected override string PrewriteFormat(string msg)
            {
                return string.Format("{0} {1}", (object)DateTime.Now.ToLongTimeString(), (object)msg);
            }

            internal ConfiguredDeviceOuptutBox(ListBox box)
              : base(box)
            {
            }
        }

        private class FraudTestExecutor : JobExecutor
        {
            protected override string JobName => "fraud-test-disk-in-picker";

            internal FraudTestExecutor(HardwareService service)
              : base(service)
            {
            }
        }

        private class FraudTakeReadReturnExecutor : JobExecutor
        {
            protected override string JobName => "fraud-test-take-read-return";

            internal FraudTakeReadReturnExecutor(HardwareService service)
              : base(service)
            {
            }
        }

        private class FraudStressTestExecutor : JobExecutor
        {
            private readonly int Iterations;
            private readonly int ScanPause;
            private readonly int IterationPause;

            protected override void SetupJob()
            {
                this.Job.Push((object)this.IterationPause);
                this.Job.Push((object)this.ScanPause);
                this.Job.Push((object)this.Iterations);
            }

            protected override string JobName => "fraud-sensor-stress-test";

            internal FraudStressTestExecutor(
              HardwareService service,
              int iterations,
              int scanPause,
              int iterationPause)
              : base(service)
            {
                this.Iterations = iterations;
                this.ScanPause = scanPause;
                this.IterationPause = iterationPause;
            }
        }

        private class FraudDetectionTestExecutor : JobExecutor
        {
            private readonly int StartupPause;

            protected override void SetupJob() => this.Job.Push((object)this.StartupPause);

            protected override string JobName => "fraud-sensor-detect-test";

            internal FraudDetectionTestExecutor(HardwareService service, int startupPause)
              : base(service)
            {
                this.StartupPause = startupPause;
            }
        }
    }
}
