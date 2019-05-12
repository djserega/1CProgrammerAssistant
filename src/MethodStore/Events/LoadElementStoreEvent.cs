using System;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate Models.ElementStore LoadElementStore(int id);

    public class LoadElementStoreEvent : EventArgs
    {
        public static event LoadElementStore LoadElementStoreEvents;

        public static Models.ElementStore Load(int id)
        {
            return LoadElementStoreEvents?.Invoke(id);
        }
    }
}
