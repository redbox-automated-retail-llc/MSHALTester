using Redbox.HAL.Common.GUI.Functions;
using System;


namespace Redbox.HAL.MSHALTester
{
    internal sealed class TesterSession : ISessionUser
    {
        public string User { get; private set; }

        public DateTime SessionStart { get; private set; }

        internal TesterSession(string uname)
        {
            this.SessionStart = DateTime.Now;
            this.User = uname;
        }
    }
}
