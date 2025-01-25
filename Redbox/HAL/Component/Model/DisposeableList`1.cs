using System;
using System.Collections.Generic;


namespace Redbox.HAL.Component.Model
{
    public struct DisposeableList<T> : IDisposable
    {
        private bool m_disposed;
        private readonly IList<T> TheList;

        public void Dispose()
        {
            if (this.m_disposed)
                return;
            this.m_disposed = true;
            if (this.TheList == null || this.TheList.Count <= 0)
                return;
            this.TheList.Clear();
        }

        public DisposeableList(IList<T> list)
          : this()
        {
            this.m_disposed = false;
            this.TheList = list;
        }
    }
}
