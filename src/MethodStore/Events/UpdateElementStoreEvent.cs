﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate bool UpdateElementStore(Models.ElementStore elementStore);

    public class UpdateElementStoreEvent : EventArgs
    {
        public static event UpdateElementStore UpdateElementStoreEvents;
        internal static bool Update(Models.ElementStore elementStore) 
            => UpdateElementStoreEvents?.Invoke(elementStore) ?? false;
    }
}
