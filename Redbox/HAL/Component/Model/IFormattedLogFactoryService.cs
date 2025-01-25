namespace Redbox.HAL.Component.Model
{
    public interface IFormattedLogFactoryService
    {
        string CreateSubpath(string subfolder);

        string LogsBasePath { get; }

        IFormattedLog NilLog { get; }
    }
}
