using Redbox.HAL.Client;


namespace Redbox.HAL.MSHALTester
{
    internal class DecodeExecutor : JobExecutor
    {
        private readonly string File;

        protected override void SetupJob() => this.Job.Push((object)this.File);

        protected override void OnJobCompleted() => this.ScanResult = ScanResult.From(this.Results);

        protected override string JobName => "decode-image";

        internal ScanResult ScanResult { get; private set; }

        internal DecodeExecutor(HardwareService service, string file)
          : base(service)
        {
            this.File = file;
        }
    }
}
