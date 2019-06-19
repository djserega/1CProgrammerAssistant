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

        public void Fill(ElementStore elementStore)
        {
            if (!Group.Equals(elementStore.Group))
                Group = elementStore.Group;

            if (!Type.Equals(elementStore.Type))
                Type = elementStore.Type;

            if (!Module.Equals(elementStore.Module))
                Module = elementStore.Module;

            if (!Method.Equals(elementStore.Method))
                Method = elementStore.Method;

            if (!Comment.Equals(elementStore.Comment))
                Comment = elementStore.Comment;
        }

        public bool Save()
        {
            bool result = Events.UpdateElementStoreEvent.Update(this);

            return result;
        }

        public string GetTextToClipboard()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Module);
            stringBuilder.Append('.');
            stringBuilder.Append(Method);
            stringBuilder.Append("();");

            return stringBuilder.ToString();
        }
    }
}
