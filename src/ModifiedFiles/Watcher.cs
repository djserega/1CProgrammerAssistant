﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModifiedFiles
{
    internal class Watcher
    {
        internal Dictionary<Models.File, FileSystemWatcher> FileWatcher { get; } = new Dictionary<Models.File, FileSystemWatcher>();

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
                    Events.CreateNewVersionEvent.NewVersion(new FileInfo(e.FullPath));
                };
                fileWatcher.EnableRaisingEvents = true;

                FileWatcher.Add(file, fileWatcher);
            }
        }

        internal void UnSubscribe(Models.File file)
        {

        }
    }
}