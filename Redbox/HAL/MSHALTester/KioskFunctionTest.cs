using Redbox.HAL.Client;
using Redbox.HAL.Client.Executors;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Redbox.HAL.MSHALTester
{
    internal sealed class KioskFunctionTest : JobExecutor, IKioskFunctionCheckData
    {
        private TestState TSDriverStatus;
        private TestState CameraDriverStatus;
        private TestState SnapDecodeStatus;
        private TestState VendDoorStatus;
        private TestState InitStatus;
        private TestState VerticalSlotTestStatus;
        private readonly bool HasCortex;
        private readonly OutputBox Box;

        public string VerticalSlotTestResult => this.VerticalSlotTestStatus.ToString();

        public string InitTestResult => this.InitStatus.ToString();

        public string VendDoorTestResult => this.VendDoorStatus.ToString();

        public string TrackTestResult => this.InitStatus.ToString();

        public string SnapDecodeTestResult => this.SnapDecodeStatus.ToString();

        public string TouchscreenDriverTestResult => this.TSDriverStatus.ToString();

        public string CameraDriverTestResult => this.CameraDriverStatus.ToString();

        public DateTime Timestamp { get; private set; }

        public string UserIdentifier { get; private set; }

        protected override void SetupJob()
        {
            this.Job.Push((object)this.UserIdentifier);
            this.Job.Push((object)this.Timestamp.ToString());
            this.Job.Push((object)this.TouchscreenDriverTestResult.ToString());
            this.Job.Push((object)this.CameraDriverTestResult.ToString());
            this.Job.Push((object)this.SnapDecodeTestResult.ToString());
            this.Job.Push((object)this.TrackTestResult.ToString());
            this.Job.Push((object)this.VendDoorTestResult.ToString());
            this.Job.Push((object)this.InitTestResult.ToString());
            this.Job.Push((object)this.VerticalSlotTestResult.ToString());
        }

        protected override string JobName => "kiosk-function-test-data";

        internal void SendData(string username)
        {
            this.UserIdentifier = username;
            this.Run();
            this.Box.Write("Report queued.");
        }

        internal TestState TestDrivers()
        {
            TestState testState1 = this.MatchTS();
            TestState testState2 = this.MatchCamera();
            return TestState.Success != testState1 || TestState.Success != testState2 ? TestState.Failure : TestState.Success;
        }

        internal TestState TestCameraSnap(Location loc)
        {
            this.SnapDecodeStatus = TestState.Failure;
            List<Location> locationList = new List<Location>();
            locationList.Add(loc);
            HardwareService service = this.Service;
            List<Location> locations = locationList;
            HardwareJobSchedule schedule = new HardwareJobSchedule();
            schedule.Priority = HardwareJobPriority.Highest;
            HardwareJob job;
            if (!service.HardSync(locations, "KFC Test Sync", schedule, out job).Success)
            {
                this.Box.Write("Unable to communicate with HAL.");
            }
            else
            {
                using (ClientHelper clientHelper = new ClientHelper(this.Service))
                {
                    HardwareJobStatus endStatus;
                    clientHelper.WaitForJob(job, out endStatus);
                    if (HardwareJobStatus.Completed == endStatus)
                    {
                        IDictionary<string, string> symbols;
                        if (job.GetSymbols(out symbols).Success)
                        {
                            foreach (string key in (IEnumerable<string>)symbols.Keys)
                            {
                                if (key.Equals("MSTESTER-SYMBOL-FORMATTED-DETAIL-MSG"))
                                {
                                    string str1 = symbols[key];
                                    LogHelper.Instance.Log("[TestCameraSnap] val = {0}", (object)str1);
                                    int num1 = this.HasCortex ? 1 : 3;
                                    int num2 = str1.IndexOf(':');
                                    string[] strArray = str1.Substring(num2 + 1).Split(new char[1]
                                    {
                    ' '
                                    }, StringSplitOptions.RemoveEmptyEntries);
                                    string str2 = strArray[0];
                                    int int32 = Convert.ToInt32(strArray[2].Substring(1, 1));
                                    TimeSpan timeSpan = TimeSpan.Parse(strArray[5]);
                                    this.SnapDecodeStatus = int32 < num1 || timeSpan.Seconds > 0 ? TestState.Failure : TestState.Success;
                                    break;
                                }
                            }
                        }
                    }
                    else
                        this.Box.Write("Decode test failed.");
                }
            }
            return this.SnapDecodeStatus;
        }

        internal TestState TestVendDoor()
        {
            this.VendDoorStatus = TestState.Failure;
            IControlSystem service = ServiceLocator.Instance.GetService<IControlSystem>();
            if (service.VendDoorRent().Success)
            {
                Thread.Sleep(500);
                this.VendDoorStatus = service.VendDoorClose().Success ? TestState.Success : TestState.Failure;
            }
            return this.VendDoorStatus;
        }

        internal TestState RunVertialSlotTest(int slot)
        {
            using (VerticalSync verticalSync = new VerticalSync(this.Service, slot))
            {
                verticalSync.Run();
                if (HardwareJobStatus.Completed == verticalSync.EndStatus)
                {
                    this.Box.Write("The job completed successfully.");
                    this.VerticalSlotTestStatus = TestState.Success;
                }
                else
                {
                    foreach (ProgramResult result in verticalSync.Results)
                        this.Box.Write(string.Format("Failure at Deck {0} Slot {1} MSG: {2}", (object)result.Deck, (object)result.Slot, (object)result.Message));
                    this.VerticalSlotTestStatus = TestState.Failure;
                }
            }
            return this.VerticalSlotTestStatus;
        }

        internal TestState RunInit()
        {
            using (InitJob initJob = new InitJob(this.Service))
            {
                initJob.Run();
                if (HardwareJobStatus.Completed == initJob.EndStatus)
                {
                    this.Box.Write("Init succeeded.");
                    this.InitStatus = TestState.Success;
                }
                else
                {
                    this.InitStatus = TestState.Failure;
                    if (initJob.Errors.Count > 0)
                    {
                        foreach (Error error in (List<Error>)initJob.Errors)
                            this.Box.Write(error.Details);
                        this.Box.Write("Init didn't succeed; errors follow:");
                    }
                }
                return this.InitStatus;
            }
        }

        internal bool GetUnknownStats()
        {
            InventoryStatsJob inventoryStatsJob = new InventoryStatsJob(this.Service);
            inventoryStatsJob.Run();
            if (inventoryStatsJob.EndStatus != HardwareJobStatus.Completed)
                return false;
            inventoryStatsJob.Results.ForEach((Action<ProgramResult>)(result =>
            {
                if (result.Code == "TotalEmptyCount")
                {
                    this.Box.Write(string.Format("  EMPTY slots: {0}", (object)result.Message));
                }
                else
                {
                    if (!(result.Code == "UnknownCount"))
                        return;
                    this.Box.Write(string.Format("  UNKNOWN slots: {0}", (object)result.Message));
                }
            }));
            this.Box.Write("Inventory Stats: ");
            return true;
        }

        internal KioskFunctionTest(
          HardwareService service,
          OutputBox box,
          bool hasCortex,
          string uname)
          : base(service)
        {
            this.Box = box;
            this.HasCortex = hasCortex;
            this.Timestamp = DateTime.Now;
            this.UserIdentifier = uname;
            this.TSDriverStatus = this.CameraDriverStatus = this.SnapDecodeStatus = TestState.NotStarted;
            this.VendDoorStatus = this.InitStatus = this.VerticalSlotTestStatus = TestState.NotStarted;
        }

        private TestState MatchCamera()
        {
            IDeviceDescriptor activeCamera = ServiceLocator.Instance.GetService<IUsbDeviceService>().FindActiveCamera(true);
            if (activeCamera != null)
            {
                this.CameraDriverStatus = TestState.Success;
                this.Box.Write("Matched camera {0}", (object)activeCamera.Friendlyname);
            }
            else
            {
                this.Box.Write("Failed to locate camera or match camera driver.");
                this.CameraDriverStatus = TestState.Failure;
            }
            return this.CameraDriverStatus;
        }

        private TestState MatchTS()
        {
            IDeviceDescriptor touchScreen = (IDeviceDescriptor)ServiceLocator.Instance.GetService<IUsbDeviceService>().FindTouchScreen(true);
            if (touchScreen != null)
            {
                this.TSDriverStatus = TestState.Success;
                this.Box.Write("Found Touchscreen {0}", (object)touchScreen.Friendlyname);
            }
            else
            {
                this.TSDriverStatus = TestState.Failure;
                this.Box.Write("Failed to locate touchscreen or match driver.");
            }
            return this.TSDriverStatus;
        }
    }
}
