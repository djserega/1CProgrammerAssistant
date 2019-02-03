using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1CProgrammerAssistant.DescriptionsTheMethods.Models
{
    internal class ObjectParameter : TypeObjectParameter
    {

        #region Constructors

        public ObjectParameter()
        {
        }

        public ObjectParameter(string name) : this()
        {
            Name = name;
        }

        public ObjectParameter(string name, string type) : this(name)
        {
            Type = type;
        }

        public ObjectParameter(string name, string type, string description) : this(name, type)
        {
            Description = description;
        }

        #endregion
        
        internal override string Name { get; set; }
        internal override string Type { get; set; }
        internal string Description { get; set; }
    }
}
