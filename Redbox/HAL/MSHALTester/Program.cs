using Redbox.HAL.Component.Model.Threading;
using Redbox.HAL.Core;
using System;
using System.Windows.Forms;


namespace Redbox.HAL.MSHALTester
{
    public class Program : IDisposable
    {
        private NamedLock m_instanceLock;
        private static Guid m_applicationGuid = new Guid("{8609B87B-126E-489a-98E9-6818639534DF}");

        [STAThread]
        public static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                program.m_instanceLock = new NamedLock(Program.m_applicationGuid.ToString());
                if (!program.m_instanceLock.IsOwned)
                {
                    int num = (int)MessageBox.Show("Only one instance of the MS HAL tester is allowed to run.");
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    bool secure = false;
                    string str = "UNKNOWN";
                    for (int index = 0; index < args.Length; ++index)
                    {
                        if (args[index].Equals("--secure", StringComparison.CurrentCultureIgnoreCase))
                            secure = true;
                        else if (args[index].StartsWith("--username"))
                            str = CommandLineOption.GetOptionVal<string>(args[index], str);
                        else
                            Console.WriteLine("Unrecognized option {0}", (object)args[index]);
                    }
                    Application.Run((Form)new Form1(secure, str));
                }
            }
        }

        public void Dispose()
        {
            if (this.m_instanceLock == null)
                return;
            this.m_instanceLock.Dispose();
        }
    }
}
