using System;


namespace Redbox.HAL.Component.Model
{
    public interface IScannerDeviceService
    {
        IScannerDevice GetConfiguredDevice();

        void Shutdown();

        DateTime? IRHardwareInstall { get; }

        CameraGeneration CurrentCameraGeneration { get; }
    }
}
