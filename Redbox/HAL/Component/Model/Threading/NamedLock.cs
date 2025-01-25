using System;
using System.Globalization;
using System.Threading;


namespace Redbox.HAL.Component.Model.Threading
{
    public sealed class NamedLock : IDisposable
    {
        private readonly Mutex m_instanceMutex;

        public void Dispose()
        {
            if (this.m_instanceMutex == null)
                return;
            this.m_instanceMutex.Close();
        }

        public NamedLock(string name)
        {
            try
            {
                bool createdNew;
                this.m_instanceMutex = new Mutex(false, string.Format((IFormatProvider)CultureInfo.InvariantCulture, "Local\\{0}", (object)name), out createdNew);
                this.IsOwned = createdNew;
            }
            catch
            {
                this.IsOwned = false;
            }
        }

        public bool IsOwned { get; private set; }
    }
}
