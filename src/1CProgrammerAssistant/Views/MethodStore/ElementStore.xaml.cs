using _1CProgrammerAssistant.Additions;
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
        private _1CProgrammerAssistant.MethodStore.Main MethodStoreMain;
        private object _buttonOpenedContextMenu;

        public Models.ElementStore RefObject
        {
            get { return (Models.ElementStore)GetValue(RefObjectProperty); }
            set { SetValue(RefObjectProperty, value); }
        }

        public static readonly DependencyProperty RefObjectProperty =
            DependencyProperty.Register("RefObject", typeof(Models.ElementStore), typeof(ElementStore), null);

        internal ActionClipboard ActionClipboard { get; set; }

        public ElementStore(int? id = null)
        {
            InitializeComponent();

            LoadObjectById(id);
        }

        private void WindowElementStore_Loaded(object sender, RoutedEventArgs e)
        {
            MethodStoreMain = ((MainWindow)Owner).assistantObjects.MethodStoreMain;
        }

        private void WindowElementStore_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
            //else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control
            //    && e.Key == Key.Enter)
            //{
            //    if (RefObject.Save())
            //        Close();
            //}
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (RefObject.Save())
            {
                LoadObjectById(RefObject.ID);
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonProcessedTextInClipboard_Click(object sender, RoutedEventArgs e)
        {
            string textInClipboard = ActionClipboard.GetTextInClipboard();

            ProcessedTextInClipboard(textInClipboard);
        }

        private void LoadObjectById(int? id)
        {
            if (id == null)
                RefObject = new Models.ElementStore();
            else
                RefObject = Events.LoadElementStoreEvent.Load((int)id);

            ReInitializeDataContext();
        }

        private void ButtonGroupSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueGroups().ToList());
        }

        private void ButtonTypeSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueType().ToList());
        }

        private void ButtonModuleSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueModule().ToList());
        }

        private void SetContextMenuSelectedButton(object container, List<string> listToContextMenu)
        {
            _buttonOpenedContextMenu = container;

            ((Button)container).ContextMenu.ItemsSource = listToContextMenu;
            ((Button)container).ContextMenu.HorizontalContentAlignment = HorizontalAlignment.Center;
            ((Button)container).ContextMenu.IsOpen = true;
        }

        private void ButtonRightContextMenu_Click(object sender, RoutedEventArgs e)
        {
            string dataContextMenuItem = (string)((MenuItem)e.OriginalSource).DataContext;

            switch (((Button)_buttonOpenedContextMenu).Tag)
            {
                case "Group":
                    RefObject.Group = dataContextMenuItem;
                    break;
                case "Type":
                    RefObject.Type = dataContextMenuItem;
                    break;
                case "Module":
                    RefObject.Module = dataContextMenuItem;
                    break;
            }
            ReInitializeDataContext();
        }

        private void ReInitializeDataContext()
        {
            DataContext = null;
            DataContext = RefObject;
        }

        private void ProcessedTextInClipboard(string text)
        {
            if (text.Contains('.')
                && text.Count(f => f.Equals('.')) == 1)
            {
                int positionDot = text.IndexOf('.');
                RefObject.Module = text.Left(positionDot);
                RefObject.Method = text.Right(positionDot - 3);

                ReInitializeDataContext();
            }
        }
    }
}
