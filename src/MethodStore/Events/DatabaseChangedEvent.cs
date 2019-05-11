using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate void DatabaseChanged();
    public class DatabaseChangedEvent : EventArgs
    {
        private static FileSystemWatcher _watcher;
        public static bool WatcherInitialized { get; private set; }

        public static void InitializeWatcher(string directory, string databaseName)
        { 
            if (WatcherInitialized)
                return;

            _watcher = new FileSystemWatcher()
            {
                Path = directory,
                Filter = $"{databaseName}.db",
                NotifyFilter = NotifyFilters.LastWrite
            };

            _watcher.Changed += (object sender, FileSystemEventArgs e) => { Changed(); };

            _watcher.EnableRaisingEvents = true;

            WatcherInitialized = true;
        }


        public static event DatabaseChanged DatabaseChangedEvents;
        private static void Changed()
        {
            DatabaseChangedEvents?.Invoke();
        }
    }
}
