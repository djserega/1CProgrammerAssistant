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

using Models = _1CProgrammerAssistant.MethodStore.Models;

namespace _1CProgrammerAssistant.Views.MethodStore
{
    /// <summary>
    /// Логика взаимодействия для ElementStore.xaml
    /// </summary>
    public partial class ElementStore : Window
    {
        private Models.ElementStore _refObj;
        public ElementStore(int? id = null)
        {
            InitializeComponent();

            if (id == null)
                _refObj = new Models.ElementStore();

            DataContext = _refObj;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _refObj.Save();
        }
    }
}
