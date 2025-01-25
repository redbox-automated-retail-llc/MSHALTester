namespace Redbox.HAL.Component.Model
{
    public interface IBarcodeReader
    {
        IScanResult Scan(string file);

        IScanResult Scan(ISnapResult sr);

        bool IsLicensed { get; }

        BarcodeServices Service { get; }
    }
}
