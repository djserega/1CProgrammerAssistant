using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate void UpdateElementStore(Models.ElementStore elementStore);

    public class UpdateElementStoreEvent : EventArgs
    {
        public static event UpdateElementStore UpdateElementStoreEvents;
        internal static void Update(Models.ElementStore elementStore) => UpdateElementStoreEvents?.Invoke(elementStore);
    }
}
