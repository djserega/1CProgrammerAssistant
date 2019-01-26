using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _1CProgrammerAssistant.ViewerFiles
{
    public class Main : IDisposable
    {
        private readonly string _locationPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly List<Process> _processesV8 = new List<Process>();

        public void Dispose()
        {
            foreach (Process itemProcess in _processesV8)
                if (!itemProcess.HasExited)
                    itemProcess.CloseMainWindow();
        }

        public void OpenFileVersion(string path)
        {
            Process processV8 = new Process()
            {
                StartInfo = new ProcessStartInfo(
                    Path.Combine(_locationPath, "v8viewer.exe"),
                    $"\"{path}\"")
            };
            processV8.Start();

            _processesV8.Add(processV8);
        }

        public void CompareFilesVersion(string firstPath, string secondPath)
        {
            Process processV8 = new Process()
            {
                StartInfo = new ProcessStartInfo(
                    Path.Combine(_locationPath, "v8viewer.exe"),
                    $"-diff -name1 \"{firstPath}\" -name2 \"{secondPath}\"")
            };
            processV8.Start();

            _processesV8.Add(processV8);
        }

        private void Temp()
        {
            V8Reader.Program.Main(new string[] { "" });

            //AppDomain domainV8 = AppDomain.CreateDomain("v8readerDomain");
            //try
            //{
            //    //domainV8.ExecuteAssembly("v8viewer.exe", new string[] { path });
            //    //domainV8.ExecuteAssembly("v8viewer.exe");
            //    domainV8.ExecuteAssemblyByName("v8viewer");
            //}
            //catch (Exception ex)
            //{
            //}

            //V8Reader.Program.Main(new string[] { path });

            //Task.Run(() => { V8Reader.Program.Main(new string[] { path }); });
        }
    }
}
