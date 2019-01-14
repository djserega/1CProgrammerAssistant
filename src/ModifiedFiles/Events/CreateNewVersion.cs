using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModifiedFiles.Events
{
    internal delegate void CreateNewVersionEvents(FileInfo fileInfoNewVersion);
    internal class CreateNewVersionEvent : EventArgs
    {
        internal static event CreateNewVersionEvents CreateNewVersionEvents;
        internal static void FileModified(FileInfo fileInfoNewVersion) => CreateNewVersionEvents?.Invoke(fileInfoNewVersion);
    }
}
