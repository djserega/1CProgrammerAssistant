using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace _1CProgrammerAssistant.MethodStore
{
    public class Main
    {
        private readonly Filter _filter = new Filter();
        private string _filterText = string.Empty;
        private string[] _filterTextArray;

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
                _filterText = filter.ToLower();
                _filterTextArray = _filterText.Split(' ');

                IEnumerable<Models.ElementStore> listElementStoresFilter = ((IEnumerable<Models.ElementStore>)(listElementStores))
                    .Where(
                        item =>
                            _filter.IsCheckedFilterGroup && TextContainsArrayFilter(item.Group.ToLower())
                            ||
                            _filter.IsCheckedFilterType && TextContainsArrayFilter(item.Type.ToLower())
                            ||
                            _filter.IsCheckedFilterModule && TextContainsArrayFilter(item.Module.ToLower())
                            ||
                            _filter.IsCheckedFilterMethod && TextContainsArrayFilter(item.Method.ToLower())
                            );
                ListMethods = new ObservableCollection<Models.ElementStore>(listElementStoresFilter);
            }
            else
                ListMethods = new ObservableCollection<Models.ElementStore>(listElementStores);
        }

        private bool TextContainsArrayFilter(string text)
        {
            bool finded = false;

            foreach (string textFilter in _filterTextArray)
            {
                if (string.IsNullOrEmpty(textFilter))
                    continue;

                if (text.Contains(textFilter))
                {
                    finded = true;
                    break;
                }
            }

            return finded;
        }

        public IQueryable<string> GetUniqueGroups() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Group);
        public IQueryable<string> GetUniqueType() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Type);
        public IQueryable<string> GetUniqueModule() => Events.GetDistinctFieldsEvent.Get(NamesDistinctField.Module);
    }
}
