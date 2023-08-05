using ClashN;
using ClashN.Mode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClashCore.Handler
{
    public class CoreHandler
    {
        private CoreInfo coreInfo;
        private Process _process;
        public void CoreStart()
        {
            try { 
            coreInfo = new CoreInfo
            {
                coreType = CoreKind.ClashPremium,
                coreExes = new List<string> { "clash-windows-amd64-v3", "clash-windows-amd64", "clash-windows-386", "clash" },
                arguments = "-f ../config/sub.yaml",
                coreUrl = Global.clashCoreUrl,
                coreLatestUrl = Global.clashCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashCoreUrl + "/download/{0}/clash-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashCoreUrl + "/download/{0}/clash-windows-amd64-{0}.zip",
                match = "Clash"
            };
            string fileName = GetCorePosition.GetExecution();
            if (fileName == "") return;
            var arguments = coreInfo.arguments;
            var data = Utils.GetPath("data");
            if (Directory.Exists(data))
            {
                arguments += $" -d \"{data}\"";
            }
            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = Utils.GetConfigPath(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                }
            };
            p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    string msg = e.Data + Environment.NewLine;
                    Console.WriteLine(msg);
                }
            });
            p.Start();
            //p.PriorityClass = ProcessPriorityClass.High;
            p.BeginOutputReadLine();
            //processId = p.Id;
            _process = p;

            if (p.WaitForExit(1000))
            {
                throw new Exception(p.StandardError.ReadToEnd());
            }
                Global.processJob = new Job();
                Global.processJob.AddProcess(p.Handle);
        }
        catch (Exception ex)
        {
                Console.WriteLine(ex.Message);
        }
    }
        /// <summary>
        /// Core停止
        /// </summary>
        public void CoreStop()
        {
            try
            {
                if (_process != null)
                {
                    KillProcess(_process);
                    _process.Dispose();
                    _process = null;
                }
                else
                {
                    if (coreInfo == null || coreInfo.coreExes == null)
                    {
                        return;
                    }

                    foreach (string vName in coreInfo.coreExes)
                    {
                        Process[] existing = Process.GetProcessesByName(vName);
                        foreach (Process p in existing)
                        {
                            string path = p.MainModule.FileName;
                            if (path == $"{Utils.GetBinPath(vName, coreInfo.coreType)}.exe")
                            {
                                KillProcess(p);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Core停止
        /// </summary>
        public void CoreStopPid(int pid)
        {
            try
            {
                Process _p = Process.GetProcessById(pid);
                KillProcess(_p);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void KillProcess(Process p)
        {
            try
            {
                p.CloseMainWindow();
                p.WaitForExit(100);
                if (!p.HasExited)
                {
                    p.Kill();
                    p.WaitForExit(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
