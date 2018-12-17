using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant.DescriptionsTheMethods.Models
{
    internal class ObjectParameter : TypeObjectParameter
    {
        public ObjectParameter()
        {
        }

        public ObjectParameter(string name)
        {
            Name = name;
        }

        public ObjectParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public ObjectParameter(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }

        internal override string Name { get; set; }
        internal override string Type { get; set; }
        internal string Description { get; set; }
    }
}
