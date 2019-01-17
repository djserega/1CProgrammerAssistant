using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ModifiedFiles
{
    
    public class Main
    {
        private List<Models.File> _files = new List<Models.File>();

        private readonly DirectoryInfo _directoryVersion = new DirectoryInfo($"Modified files (versions)");
        private readonly Dictionary<string, int> _idFileByPath = new Dictionary<string, int>();
        private readonly Watcher _watcher = new Watcher();
        private readonly Version _version = new Version();

        public Main()
        {
            if (!_directoryVersion.Exists)
                _directoryVersion.Create();
               
            Events.CreateNewVersionEvent.CreateNewVersionEvents += (FileInfo modifiedFile) =>
            {
                _version.CreateNewVersion(modifiedFile, _files[_idFileByPath[modifiedFile.FullName]].DirectoryVersion);

                Events.CreateNewVersionEvent.NewVersionCreated(modifiedFile);
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

                _watcher.Subscribe(file);

                _version.InitializeControlHashByDirectory(file.DirectoryVersion);

                i++;
            }

            _watcher.RemoveUnusedFiles(_files);
        }

        public void OpenDirectoryVersion()
        {
            _directoryVersion.Refresh();
            if (_directoryVersion.Exists)
                Process.Start("explorer.exe", _directoryVersion.FullName);
        }

        public List<Models.Version> GetListVersion(Models.File file) => _version[file?.DirectoryVersion];
    }
}
