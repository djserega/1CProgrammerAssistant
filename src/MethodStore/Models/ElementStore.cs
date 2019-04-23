using System;
using System.Collections.Generic;
using System.Text;

namespace _1CProgrammerAssistant.MethodStore.Models
{
    public class ElementStore
    {
        public int ID { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string Module { get; set; }
        public string Method { get; set; }
        public string Comment { get; set; }

        public void Save()
        {

        }
    }
}
