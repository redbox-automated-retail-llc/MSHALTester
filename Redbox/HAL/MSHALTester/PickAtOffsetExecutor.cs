using Redbox.HAL.Client;
using Redbox.HAL.Component.Model;


namespace Redbox.HAL.MSHALTester
{
    internal sealed class PickAtOffsetExecutor : JobExecutor
    {
        private readonly int Deck;
        private readonly int Slot;
        private readonly int Offset;
        private readonly Axis Axis;

        protected override void SetupJob()
        {
            this.Job.Push((object)this.Offset);
            this.Job.Push((object)this.Axis.ToString());
            this.Job.Push((object)this.Slot);
            this.Job.Push((object)this.Deck);
        }

        protected override string JobName => "pick-at-offset";

        internal PickAtOffsetExecutor(
          HardwareService service,
          int deck,
          int slot,
          Axis axis,
          int offset)
          : base(service)
        {
            this.Deck = deck;
            this.Slot = slot;
            this.Axis = axis;
            this.Offset = offset;
        }
    }
}
