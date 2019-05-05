using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    }
}
