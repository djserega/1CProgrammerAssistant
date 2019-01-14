using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModifiedFiles
{
    internal class Watcher
    {
        internal Dictionary<Models.File, FileSystemWatcher> FileWatcher { get; } = new Dictionary<Models.File, FileSystemWatcher>();

        internal void RemoveUnusedFiles(List<Models.File> files)
        {
            List<Models.File> removeKey = new List<Models.File>();

            foreach (KeyValuePair<Models.File, FileSystemWatcher> keyWatcher in FileWatcher)
                if (!files.Contains(keyWatcher.Key))
                    removeKey.Add(keyWatcher.Key);

            foreach (Models.File key in removeKey)
                UnSubscribe(key);
        }

        internal void Subscribe(Models.File file)
        {
            if (!FileWatcher.ContainsKey(file))
            {
                FileInfo fileInfo = new FileInfo(file.Path);

                FileSystemWatcher fileWatcher = new FileSystemWatcher(fileInfo.Directory.FullName, fileInfo.Name)
                {
                    NotifyFilter = NotifyFilters.LastWrite
                };
                fileWatcher.Changed += (object sender, FileSystemEventArgs e) =>
                {
                    Events.CreateNewVersionEvent.FileModified(new FileInfo(e.FullPath));
                };
                fileWatcher.EnableRaisingEvents = true;

                FileWatcher.Add(file, fileWatcher);
            }
        }

        internal void UnSubscribe(Models.File file)
        {
            if (FileWatcher.ContainsKey(file))
            {
                FileWatcher[file].Dispose();

                FileWatcher.Remove(file);
            }
        }
    }
}
