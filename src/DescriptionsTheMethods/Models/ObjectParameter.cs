namespace _1CProgrammerAssistant.DescriptionsTheMethods.Models
{
    internal class ObjectParameter : TypeObjectParameter
    {

        #region Constructors

        internal ObjectParameter()
        {
        }

        internal ObjectParameter(string name) : this()
        {
            Name = name;
        }

        internal ObjectParameter(string name, string type) : this(name)
        {
            Type = type;
        }

        internal ObjectParameter(string name, string type, string description) : this(name, type)
        {
            Description = description;
        }

        #endregion
        
        internal override string Name { get; set; }
        internal override string Type { get; set; }
        internal string Description { get; set; }
    }
}
