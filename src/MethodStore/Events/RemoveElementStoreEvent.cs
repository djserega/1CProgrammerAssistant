using System;
using System.Collections.Generic;
using System.Text;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate void RemoveElementStore(int id);

    public class RemoveElementStoreEvent : EventArgs
    {
        public static event RemoveElementStore RemoveElementStoreEvents;
        public static void Remove(int id) => RemoveElementStoreEvents?.Invoke(id);
    }
}
