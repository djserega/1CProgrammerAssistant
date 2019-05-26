using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace _1CProgrammerAssistant.MethodStore
{
    public class Main
    {
        private Filter _filter = new Filter();
        public Main()
        {
        }

        public ObservableCollection<Models.ElementStore> ListMethods { get; set; } = new ObservableCollection<Models.ElementStore>();

        public void UpdateValueIsCheckedFilter(bool group, bool type, bool module, bool method)
        {
            _filter.IsCheckedFilterGroup = group;
            _filter.IsCheckedFilterType = type;
            _filter.IsCheckedFilterMethod = method;
            _filter.IsCheckedFilterModule = module;
        }

        public void LoadMethod(string filter)
        {
            ListMethods.Clear();

            List<Models.ElementStore> listElementStores = Events.LoadElementsStoreEvent.Load() ?? new List<Models.ElementStore>();

            //IEnumerable<Models.ElementStore> dd = (IEnumerable<Models.ElementStore>)listElementStores;
            //dd.Where();

            string filterToLower = string.Empty;
            if (!string.IsNullOrEmpty(filter))
                filterToLower = filter.ToLower();

            bool needFiltered =
                !string.IsNullOrEmpty(filterToLower)
                && (_filter.IsCheckedFilterGroup
                    || _filter.IsCheckedFilterType
                    || _filter.IsCheckedFilterMethod
                    || _filter.IsCheckedFilterModule);

            if (needFiltered)
            {
                foreach (Models.ElementStore record in listElementStores)
                {
                    if (
                        _filter.IsCheckedFilterGroup && record.Group.ToLower().Contains(filterToLower)
                        ||
                        _filter.IsCheckedFilterType && record.Type.ToLower().Contains(filterToLower)
                        ||
                        _filter.IsCheckedFilterModule && record.Module.ToLower().Contains(filterToLower)
                        ||
                        _filter.IsCheckedFilterMethod && record.Method.ToLower().Contains(filterToLower)
                        )
                    {
                        ListMethods.Add(record);
                    }
                }
            }
            else
                ListMethods = new ObservableCollection<Models.ElementStore>(listElementStores);
        }

        public IQueryable<string> GetUniqueGroups() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Group);
        public IQueryable<string> GetUniqueType() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Type);
        public IQueryable<string> GetUniqueModule() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Module);
    }
}
