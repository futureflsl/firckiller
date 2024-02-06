using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIRC
{
    public class FIRCKiller
    {
        [DllImport("killer.dll")]
        public static extern int LoadDriver(string serviceName, string driverPath);

        [DllImport("killer.dll")]
        public static extern int ProcessKiller(uint pid);

        public bool StartService()
        {
            try
            {
                LoadDriver("processKiller", Application.StartupPath + "\\killer.sys");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public bool KillPID(uint pid)
        {
            int res = ProcessKiller(pid);
            if (res == 0)
            {
                return true;
            }
            return false;
        }

        public void StopService()
        {
            Execute("cmd.exe", string.Empty, new string[] {
                "sc stop processKiller",
                "sc delete processKiller",
            });
        }
        public string Execute(string fileName, string arg, string[] commands, bool readResult = true)
        {
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = Application.StartupPath;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.Verb = "runas";
            proc.Start();

            if (commands.Length > 0)
            {
                for (int i = 0; i < commands.Length; i++)
                {
                    proc.StandardInput.WriteLine(commands[i]);
                }
            }

            proc.StandardInput.AutoFlush = true;
            proc.StandardInput.WriteLine("exit");
            proc.StandardInput.Close();

            if (readResult)
            {
                string output = proc.StandardOutput.ReadToEnd();
                string error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                proc.Close();
                proc.Dispose();

                return output;
            }
            return string.Empty;
        }
    }
}
