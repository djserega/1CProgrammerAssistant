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

namespace ViewerFiles
{
    public class Main
    {
        private readonly string _locationPath = AppDomain.CurrentDomain.BaseDirectory;

        public void OpenFileVersion(string path)
        {
            Process processV8 = new Process()
            {
                StartInfo = new ProcessStartInfo(
                    Path.Combine(_locationPath, "v8viewer.exe"),
                    $"\"{path}\"")
            };
            processV8.Start();
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
