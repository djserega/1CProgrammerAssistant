using System.Collections.Generic;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate List<string> GetElements(NamesDistinctField name, string filter = null);
    
    public class GetDistinctFieldsEvent
    {
        public static event GetElements GetElementsEvents;

        internal static List<string> Get(NamesDistinctField name)
        {
            return GetElementsEvents?.Invoke(name);
        }

        internal static List<string> Get(NamesDistinctField name, string filter)
        {
            return GetElementsEvents?.Invoke(name, filter);
        }

    }
}
