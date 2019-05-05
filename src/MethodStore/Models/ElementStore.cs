using System;
using System.Collections.Generic;
using System.Text;

namespace _1CProgrammerAssistant.MethodStore.Models
{
    public class ElementStore
    {
        public int ID { get; set; }
        public string Group { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public void Save()
        {
            Events.UpdateElementStoreEvent.Update(this);
        }
    }
}
