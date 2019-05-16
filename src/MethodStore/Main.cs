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

        public void LoadMethod()
        {
            ListMethods.Clear();

            List<Models.ElementStore> listElementStores = Events.LoadElementsStoreEvent.Load() ?? new List<Models.ElementStore>();

            foreach (Models.ElementStore record in listElementStores)
            {
                ListMethods.Add(record);
            }
        }

        public IQueryable<string> GetUniqueGroups() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Group);
        public IQueryable<string> GetUniqueType() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Type);
        public IQueryable<string> GetUniqueModule() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Module);
    }
}
