using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Events = _1CProgrammerAssistant.MethodStore.Events;
using Models = _1CProgrammerAssistant.MethodStore.Models;

namespace _1CProgrammerAssistant.Views.MethodStore
{
    /// <summary>
    /// Логика взаимодействия для ElementStore.xaml
    /// </summary>
    public partial class ElementStore : Window
    {
        public Models.ElementStore RefObject
        {
            get { return (Models.ElementStore)GetValue(RefObjectProperty); }
            set { SetValue(RefObjectProperty, value); }
        }

        public static readonly DependencyProperty RefObjectProperty =
            DependencyProperty.Register("RefObject", typeof(Models.ElementStore), typeof(ElementStore), null);


        public ElementStore(int? id = null)
        {
            InitializeComponent();

            LoadObjectById(id);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (RefObject.Save())
            {
                LoadObjectById(RefObject.ID);
            }
        }

        private void LoadObjectById(int? id)
        {
            if (id == null)
                RefObject = new Models.ElementStore();
            else
                RefObject = Events.LoadElementStoreEvent.Load((int)id);

            DataContext = null;
            DataContext = RefObject;
        }
    }
}
