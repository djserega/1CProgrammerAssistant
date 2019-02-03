using System;
using System.IO;

namespace _1CProgrammerAssistant.ModifiedFiles.Events
{
    public delegate void NewVersionCreatedEvents(FileInfo modifiedFile);
    internal delegate void CreateNewVersionEvents(FileInfo modifiedFile);

    public class CreateNewVersionEvent : EventArgs
    {
        internal static event CreateNewVersionEvents CreateNewVersionEvents;
        internal static void FileModified(FileInfo modifiedFile) => CreateNewVersionEvents?.Invoke(modifiedFile);

        public static event NewVersionCreatedEvents NewVersionCreatedEvents;
        internal static void NewVersionCreated(FileInfo modifiedFile) => NewVersionCreatedEvents?.Invoke(modifiedFile);
    }
}
