using Microsoft.Win32;
using Redbox.HAL.Component.Model.Extensions;
using Redbox.HAL.Component.Model.Timers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;


namespace Redbox.HAL.Component.Model.Services
{
    public sealed class RuntimeService : IRuntimeService
    {
        public const string RunningPath = "RunningPath";
        private Platform Platform;
        private string m_kioskId;
        private readonly WaitHandle m_waitObject = (WaitHandle)new ManualResetEvent(false);
        private readonly StringDictionary Values = new StringDictionary();

        public string ExpandRuntimeMacros(string value) => this.ExpandMacros(value, "${", "}");

        public string ExpandConstantMacros(string value) => this.ExpandMacros(value, "$(", ")");

        public string RuntimePath(string fileName) => Path.Combine(this.AssemblyDirectory, fileName);

        public string InstallPath(string subfolder) => Path.Combine(this.InstallRoot, subfolder);

        public string CreateBackup(string path) => this.CreateBackup(path, BackupAction.Delete);

        public string CreateBackup(string path, BackupAction action)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            string backupName = this.CreateBackupName(path);
            if (action == BackupAction.Move)
                File.Move(path, backupName);
            else if (BackupAction.Copy == action)
                File.Copy(path, backupName);
            else if (BackupAction.Delete == action)
                this.SafeDelete(path);
            return backupName;
        }

        public string GenerateUniqueFile(string suffix)
        {
            if (!suffix.StartsWith("."))
                suffix = string.Format(".{0}", (object)suffix);
            DateTime now = DateTime.Now;
            return string.Format("m{0}d{1}y{2}_h{3}m{4}s{5}t{6}{7}", (object)now.Month, (object)now.Day, (object)now.Year, (object)now.Hour, (object)now.Minute, (object)now.Second, (object)now.Millisecond, (object)suffix);
        }

        public byte[] ReadToBuffer(string fileName)
        {
            if (fileName == null)
                return (byte[])null;
            byte[] buffer = (byte[])null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
            }
            return buffer;
        }

        public bool ForceSafeCopy(string source, string dest)
        {
            return this.ForceSafeCopy(TextWriter.Null, source, dest);
        }

        public bool ForceSafeCopy(TextWriter w, string source, string dest)
        {
            return this.OnCopy(w, source, dest, true);
        }

        public bool SafeCopy(string source, string dest)
        {
            return this.SafeCopy(TextWriter.Null, source, dest);
        }

        public bool SafeCopy(TextWriter writer, string source, string dest)
        {
            return this.OnCopy(writer, source, dest, false);
        }

        public bool SafeMove(string source, string dest)
        {
            return this.SafeMove(TextWriter.Null, source, dest);
        }

        public bool SafeMove(TextWriter writer, string source, string dest)
        {
            string str = string.Format("move {0} -> {1}", (object)source, (object)dest);
            try
            {
                File.Move(source, dest);
                writer.WriteLine("[{0}] {1}: SUCCESS", (object)this.GetType().Name, (object)str);
                return true;
            }
            catch (Exception ex)
            {
                writer.WriteLine("[{0}] {1}: FAILURE reason = {2}", (object)this.GetType().Name, (object)str, (object)ex.Message);
                return false;
            }
        }

        public bool SafeDelete(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return false;
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("[{0}] Delete file failed:", (object)this.GetType().Name);
                LogHelper.Instance.Log(ex.Message);
                return false;
            }
        }

        public Platform GetPlatform()
        {
            if (this.Platform != Platform.None)
                return this.Platform;
            this.Platform = Platform.Unknown;
            OperatingSystem osVersion = Environment.OSVersion;
            if (osVersion.Platform != PlatformID.Win32NT)
                return this.Platform;
            switch (osVersion.Version.Major)
            {
                case 5:
                    if (osVersion.Version.Minor != 0)
                    {
                        this.Platform = Platform.WindowsXP;
                        break;
                    }
                    break;
                case 6:
                    if (osVersion.Version.Minor != 0)
                    {
                        this.Platform = Platform.Windows7;
                        break;
                    }
                    break;
            }
            return this.Platform;
        }

        public void SpinWait(int milliseconds) => this.SpinWait(new TimeSpan(0, 0, 0, 0, milliseconds));

        public void SpinWait(TimeSpan timespan)
        {
            using (ExecutionTimer executionTimer = new ExecutionTimer())
            {
                do
                    ;
                while ((double)executionTimer.ElapsedMilliseconds < timespan.TotalMilliseconds);
            }
        }

        public void Wait(int ms)
        {
            if (ms < 1000)
                this.SpinWait(ms);
            else
                this.m_waitObject.WaitOne(ms, false);
        }

        public string KioskId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_kioskId))
                    return this.m_kioskId;
                RegistryKey registryKey = (RegistryKey)null;
                try
                {
                    registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Redbox\\REDS\\Kiosk Engine\\Store");
                    if (registryKey != null)
                    {
                        object obj = registryKey.GetValue("ID");
                        if (obj != null)
                            this.m_kioskId = ConversionHelper.ChangeType<string>(obj);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Instance.Log("[{0}] Unable to read Kiosk ID", (object)this.GetType().Name);
                    LogHelper.Instance.Log(ex.Message);
                    this.m_kioskId = (string)null;
                }
                finally
                {
                    registryKey?.Close();
                }
                return !string.IsNullOrEmpty(this.m_kioskId) ? this.m_kioskId : "UNKNOWN";
            }
        }

        public string DataPath { get; private set; }

        public string AssemblyDirectory { get; private set; }

        public string InstallRoot { get; private set; }

        public int PageSize { get; private set; }

        public string ScriptsPath { get; private set; }

        public RuntimeService()
        {
            this.AssemblyDirectory = Path.GetDirectoryName(typeof(RuntimeService).Assembly.Location);
            this.DataPath = Path.Combine(this.AssemblyDirectory, "data");
            try
            {
                this.InstallRoot = Directory.GetParent(this.AssemblyDirectory).FullName;
                this.ScriptsPath = Path.Combine(this.InstallRoot, "Scripts");
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Log("[RuntimeService] Unable to find parent folder.", (object)ex.Message);
                this.InstallRoot = this.AssemblyDirectory;
            }
            this.Platform = Platform.None;
            this.Values[nameof(RunningPath)] = this.AssemblyDirectory;
            RuntimeService.SYSTEM_INFO lpSystemInfo = new RuntimeService.SYSTEM_INFO();
            RuntimeService.GetSystemInfo(ref lpSystemInfo);
            this.PageSize = (int)lpSystemInfo.dwPageSize;
        }

        private bool OnCopy(TextWriter writer, string source, string dest, bool force)
        {
            string str = string.Format("copy {0} -> {1}", (object)source, (object)dest);
            try
            {
                File.Copy(source, dest, force);
                writer.WriteLine("[{0}] {1}: SUCCESS", (object)this.GetType().Name, (object)str);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.WithContext(false, LogEntryType.Error, "COPY FAILURE {0}: reason = {1}", (object)str, (object)ex.Message);
                return false;
            }
        }

        private string CreateBackupName(string path)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(path);
            string directoryName = Path.GetDirectoryName(path);
            string extension = Path.GetExtension(path);
            string path2 = string.Format("{0}-{1}", (object)withoutExtension, (object)this.GenerateUniqueFile(extension));
            return Path.Combine(directoryName, path2);
        }

        private string ExpandMacros(string value, string preamble, string postamble)
        {
            if (value == null)
                return (string)null;
            while (true)
            {
                string codeFromBrackets = StringExtensions.ExtractCodeFromBrackets(value, preamble, postamble);
                if (!string.IsNullOrEmpty(codeFromBrackets))
                {
                    string newValue = this.Values[codeFromBrackets] ?? string.Empty;
                    value = value.Replace(string.Format("{0}{1}{2}", (object)preamble, (object)codeFromBrackets, (object)postamble), newValue);
                }
                else
                    break;
            }
            return value;
        }

        [DllImport("kernel32.dll")]
        private static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref RuntimeService.SYSTEM_INFO lpSystemInfo);

        private struct SYSTEM_INFO
        {
            internal RuntimeService._PROCESSOR_INFO_UNION uProcessorInfo;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort dwProcessorLevel;
            public ushort dwProcessorRevision;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct _PROCESSOR_INFO_UNION
        {
            [FieldOffset(0)]
            internal uint dwOemId;
            [FieldOffset(0)]
            internal ushort wProcessorArchitecture;
            [FieldOffset(2)]
            internal ushort wReserved;
        }
    }
}
