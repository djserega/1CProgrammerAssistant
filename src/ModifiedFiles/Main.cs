
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace ModifiedFiles
{
    public class Main
    {
        private readonly DirectoryInfo _directoryVersion = new DirectoryInfo($"Modified files (versions)");
        private List<Models.File> _files = new List<Models.File>();
        private readonly Dictionary<string, int> _idFileByPath = new Dictionary<string, int>();
        private readonly Watcher watcher = new Watcher();

        public Main()
        {
            if (!_directoryVersion.Exists)
                _directoryVersion.Create();

            Events.CreateNewVersionEvent.CreateNewVersionEvents += (FileInfo fileInfoNewVersion) =>
            {
                string pathVersion = Path.Combine(
                    _files[_idFileByPath[fileInfoNewVersion.FullName]].DirectoryVersion,
                    $"version_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

                pathVersion = $"{pathVersion}{fileInfoNewVersion.Extension}";

                FileInfo fileInfoVersion = new FileInfo(pathVersion);
                if (!fileInfoVersion.Exists)
                {
                    bool copied = false;
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            fileInfoNewVersion.CopyTo(pathVersion);
                            copied = true;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                        }

                        if (copied)
                            break;
                    }
                }
            };
        }

        public List<Models.File> Files { get => _files; set { _files = value; InitializeListFiles(); } }

        private void InitializeListFiles()
        {
            _idFileByPath.Clear();

            int i = 0;
            foreach (Models.File file in _files)
            {
                file.DirectoryVersion = _directoryVersion.CreateSubdirectory(file.FileNameWithoutExtension).FullName;
                _idFileByPath.Add(file.Path, i);

                watcher.Subscribe(file);

                i++;
            }
        }

        public void OpenDirectoryVersion()
        {
            _directoryVersion.Refresh();
            if (_directoryVersion.Exists)
                Process.Start("explorer.exe", _directoryVersion.FullName);
        }
    }
}
