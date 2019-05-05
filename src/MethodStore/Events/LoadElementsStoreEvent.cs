using System;
using System.Collections.Generic;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate List<Models.ElementStore> LoadElementsStore();
    public class LoadElementsStoreEvent : EventArgs
    {
        public static event LoadElementsStore LoadElementsStoreEvents;
        internal static List<Models.ElementStore> Load()
        {
            return LoadElementsStoreEvents?.Invoke();
        }
    }
}
