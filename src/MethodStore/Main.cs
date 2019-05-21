using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace _1CProgrammerAssistant.MethodStore
{
    public class Main
    {
        public Main()
        {
        }

        public ObservableCollection<Models.ElementStore> ListMethods { get; set; } = new ObservableCollection<Models.ElementStore>();

        public void LoadMethod(string filter)
        {
            ListMethods.Clear();

            List<Models.ElementStore> listElementStores = Events.LoadElementsStoreEvent.Load() ?? new List<Models.ElementStore>();

            string filterToLower = string.Empty;
            if (!string.IsNullOrEmpty(filter))
                filterToLower = filter.ToLower();

            foreach (Models.ElementStore record in listElementStores)
            {
                if (
                    !string.IsNullOrEmpty(filterToLower)
                    && record.Group.ToLower().Contains(filterToLower)
                    || record.Type.ToLower().Contains(filterToLower)
                    || record.Module.ToLower().Contains(filterToLower)
                    || record.Method.ToLower().Contains(filterToLower)
                    )
                {
                    ListMethods.Add(record);
                }
            }
        }

        public IQueryable<string> GetUniqueGroups() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Group);
        public IQueryable<string> GetUniqueType() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Type);
        public IQueryable<string> GetUniqueModule() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Module);
    }
}
