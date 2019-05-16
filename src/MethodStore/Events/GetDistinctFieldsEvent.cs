using System.Linq;

namespace _1CProgrammerAssistant.MethodStore.Events
{
    public delegate IQueryable<string> GetElements(NamesDistinctField name);
    public class GetDistinctFieldsEvent
    {
        public static event GetElements GetElementsEvents;

        internal static IQueryable<string> Get(NamesDistinctField name)
        {
            return GetElementsEvents?.Invoke(name);
        }

    }
}
