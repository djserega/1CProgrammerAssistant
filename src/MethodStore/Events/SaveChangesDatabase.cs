using System;
using System.Collections.Generic;
using System.Text;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate void SaveChangesDatabase();
    public class SaveChangesDatabaseEvent : EventArgs
    {
        public static event SaveChangesDatabase SaveChangesDatabaseEvents;
        public static void SaveChanges()
            => SaveChangesDatabaseEvents?.Invoke();
    }
}
