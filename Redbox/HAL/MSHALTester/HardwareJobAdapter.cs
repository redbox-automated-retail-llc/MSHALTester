using Redbox.HAL.Client;
using System.ComponentModel;


namespace Redbox.HAL.MSHALTester
{
    internal sealed class HardwareJobAdapter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ID => this.Job.ID;

        public string ProgramName => this.Job.ProgramName;

        public HardwareJobStatus Status => this.Job.Status;

        public HardwareJobPriority Priority => this.Job.Priority;

        public void Merge(HardwareJob job)
        {
            if (!this.Job.Merge(job))
                return;
            this.NotifyPropertyChanged("Status");
            this.NotifyPropertyChanged("Priority");
        }

        internal HardwareJob Job { get; private set; }

        internal bool Removable { get; set; }

        internal HardwareJobAdapter(HardwareJob job) => this.Job = job;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged((object)this, new PropertyChangedEventArgs(name));
        }
    }
}
