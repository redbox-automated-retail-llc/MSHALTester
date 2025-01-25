using Redbox.HAL.Client;


namespace Redbox.HAL.MSHALTester
{
    internal sealed class TransferDisk
    {
        private readonly HardwareService Service;

        internal string Transfer(TransferLocation source, TransferLocation target)
        {
            HardwareJob job;
            if (!this.Service.ExecuteImmediate(string.Format("XFER SRC-DECK={0} SRC-SLOT={1} DEST-DECK={2} DEST-SLOT={3}", (object)source.Deck, (object)source.Slot, (object)target.Deck, (object)target.Slot), new int?(120000), out job).Success)
                return "SERVICE COMM ERROR";
            string topOfStack = job.GetTopOfStack();
            return !string.IsNullOrEmpty(topOfStack) ? topOfStack : "SERVICE COMM ERROR";
        }

        internal TransferDisk(HardwareService service) => this.Service = service;
    }
}
