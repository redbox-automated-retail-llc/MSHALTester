using System;


namespace Redbox.HAL.Component.Model
{
    public sealed class LogHelper
    {
        private static LogHelper m_instance = new LogHelper();

        public static LogHelper Instance => LogHelper.m_instance;

        public void Log(string msg) => this.Log(msg, LogEntryType.Info);

        public void Log(string fmt, params object[] stuff)
        {
            if (ServiceLocator.Instance.GetService<ILogger>() == null)
                return;
            this.Log(string.Format(fmt, stuff), LogEntryType.Info);
        }

        public void Log(string message, Exception e)
        {
            ServiceLocator.Instance.GetService<ILogger>()?.Log(message, e);
        }

        public void Log(string message, LogEntryType type)
        {
            ServiceLocator.Instance.GetService<ILogger>()?.Log(message, type);
        }

        public void Log(string message, Exception e, LogEntryType type)
        {
            ServiceLocator.Instance.GetService<ILogger>()?.Log(message, e, type);
        }

        public void Log(LogEntryType type, string message, params object[] args)
        {
            ILogger service = ServiceLocator.Instance.GetService<ILogger>();
            if (service == null || !this.IsLevelEnabled(type))
                return;
            string message1 = string.Format(message, args);
            service.Log(message1, type);
        }

        public void WithContext(string fmt, params object[] stuff)
        {
            this.WithContext(LogEntryType.Info, fmt, stuff);
        }

        public void WithContext(LogEntryType type, string msg) => this.WithContext(true, type, msg);

        public void WithContext(LogEntryType type, string fmt, params object[] stuff)
        {
            this.WithContext(type, string.Format(fmt, stuff));
        }

        public void WithContext(
          bool toFormattedLog,
          LogEntryType type,
          string fmt,
          params object[] stuff)
        {
            this.WithContext(toFormattedLog, type, string.Format(fmt, stuff));
        }

        public void WithContext(bool toFormattedLog, LogEntryType type, string msg)
        {
            IExecutionService service = ServiceLocator.Instance.GetService<IExecutionService>();
            if (service == null)
            {
                this.Log(type, msg);
            }
            else
            {
                IExecutionContext activeContext = service.GetActiveContext();
                if (activeContext == null)
                {
                    this.Log(type, msg);
                }
                else
                {
                    this.Log(type, "[{0}, {1}]: {2}", (object)activeContext.ProgramName, (object)activeContext.ID, (object)msg);
                    if (!toFormattedLog)
                        return;
                    activeContext.ContextLog.WriteFormatted(msg);
                }
            }
        }

        public bool IsLevelEnabled(LogEntryType logEntryLevel)
        {
            ILogger service = ServiceLocator.Instance.GetService<ILogger>();
            return service != null && service.IsLevelEnabled(logEntryLevel);
        }

        private LogHelper()
        {
        }
    }
}
