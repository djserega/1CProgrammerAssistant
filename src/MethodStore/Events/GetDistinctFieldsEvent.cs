using System.Linq;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate IQueryable<string> GetElements(NamesDistinctField name, string filter = null);
    
    public class GetDistinctFieldsEvent
    {
        public static event GetElements GetElementsEvents;

        internal static IQueryable<string> Get(NamesDistinctField name)
        {
            return GetElementsEvents?.Invoke(name);
        }

        internal static IQueryable<string> Get(NamesDistinctField name, string filter)
        {
            return GetElementsEvents?.Invoke(name, filter);
        }

    }
}
