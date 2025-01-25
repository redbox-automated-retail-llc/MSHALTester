using System.Collections.Generic;


namespace Redbox.HAL.Client.Executors
{
    public sealed class ExcludedLocsJob(HardwareService service) : JobExecutor(service)
    {
        private readonly List<Location> m_excluded = new List<Location>();

        public IList<Location> ExcludedLocations
        {
            get
            {
                foreach (ProgramResult result in this.Results)
                {
                    if (result.Code == "ExcludedSlot")
                        this.m_excluded.Add(new Location()
                        {
                            Deck = result.Deck,
                            Slot = result.Slot
                        });
                }
                return (IList<Location>)this.m_excluded;
            }
        }

        protected override void DisposeInner() => this.m_excluded.Clear();

        protected override string JobName => "get-excluded-locations";
    }
}
