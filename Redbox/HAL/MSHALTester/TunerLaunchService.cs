using Redbox.HAL.Client;
using Redbox.HAL.Common.GUI.Functions;
using Redbox.HAL.Component.Model;
using Redbox.HAL.MSHALTester.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Redbox.HAL.MSHALTester
{
    internal static class TunerLaunchService
    {
        private static readonly byte[] ShutdownCommand;
        private static readonly byte[] StartupCommand;

        internal static void LaunchTunerAndWait(HardwareService service, ButtonAspectsManager manager)
        {
            HardwareJob job;
            service.ExecuteImmediateProgram(TunerLaunchService.ShutdownCommand, out job);
            ServiceLocator.Instance.GetService<IRuntimeService>().SpinWait(Settings.Default.TunerStartPause);
            string tunerApplication = Settings.Default.TunerApplication;
            if (File.Exists(tunerApplication))
            {
                Process.Start(tunerApplication, string.Empty).WaitForExit();
            }
            else
            {
                using (CameraPreviewForm cameraPreviewForm = new CameraPreviewForm(service, manager, Settings.Default.ImageDirectory))
                {
                    int num = (int)cameraPreviewForm.ShowDialog();
                }
            }
            service.ExecuteImmediateProgram(TunerLaunchService.StartupCommand, out job);
        }

        static TunerLaunchService()
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("CAMERA STOP FORCE=TRUE" + Environment.NewLine);
            stringBuilder1.Append("RINGLIGHT ON" + Environment.NewLine);
            TunerLaunchService.ShutdownCommand = Encoding.ASCII.GetBytes(stringBuilder1.ToString());
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("RINGLIGHT OFF" + Environment.NewLine);
            stringBuilder2.Append("CAMERA START" + Environment.NewLine);
            TunerLaunchService.StartupCommand = Encoding.ASCII.GetBytes(stringBuilder2.ToString());
        }
    }
}
