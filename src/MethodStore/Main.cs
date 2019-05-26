using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace _1CProgrammerAssistant.MethodStore
{
    public class Main
    {
        private readonly Filter _filter = new Filter();
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
            List<Models.ElementStore> listElementStores = Events.LoadElementsStoreEvent.Load() ?? new List<Models.ElementStore>();

            bool needFiltered =
                !string.IsNullOrEmpty(filter)
                && (_filter.IsCheckedFilterGroup
                    || _filter.IsCheckedFilterType
                    || _filter.IsCheckedFilterMethod
                    || _filter.IsCheckedFilterModule);

            if (needFiltered)
            {
                string filterToLower = string.Empty;
                if (!string.IsNullOrEmpty(filter))
                    filterToLower = filter.ToLower();

                IEnumerable<Models.ElementStore> listElementStoresFilter = ((IEnumerable<Models.ElementStore>)(listElementStores))
                    .Where(
                        item =>
                            _filter.IsCheckedFilterGroup && item.Group.ToLower().Contains(filterToLower)
                            ||
                            _filter.IsCheckedFilterType && item.Type.ToLower().Contains(filterToLower)
                            ||
                            _filter.IsCheckedFilterModule && item.Module.ToLower().Contains(filterToLower)
                            ||
                            _filter.IsCheckedFilterMethod && item.Method.ToLower().Contains(filterToLower)
                            );
                ListMethods = new ObservableCollection<Models.ElementStore>(listElementStoresFilter);
            }
            else
                ListMethods = new ObservableCollection<Models.ElementStore>(listElementStores);
        }

        public IQueryable<string> GetUniqueGroups() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Group);
        public IQueryable<string> GetUniqueType() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Type);
        public IQueryable<string> GetUniqueModule() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Module);
    }
}
