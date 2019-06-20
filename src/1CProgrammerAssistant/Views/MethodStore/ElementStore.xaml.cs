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
using Messages = _1CProgrammerAssistant.MethodStore.Messages;

namespace _1CProgrammerAssistant.Views.MethodStore
{
    /// <summary>
    /// Логика взаимодействия для ElementStore.xaml
    /// </summary>
    public partial class ElementStore : Window
    {
        #region Fields, properties

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

        #endregion

        #region Constructors

        public ElementStore()
        {
            InitializeComponent();

            LoadObjectById(null);
        }

        public ElementStore(int id) : this()
        {
            LoadObjectById(id);
        }

        public ElementStore(Models.ElementStore source) : this()
        {
            CopyObjectElementStore(source);
        }

        #endregion

        #region Window events

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

        #endregion

        #region Buttons

        private void ButtonSaveAndClose_Click(object sender, RoutedEventArgs e)
        {
            ButtonSave_Click(sender, e);
            Close();
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
            if (RefObject.ID == 0)
                return;

            Messages.RemoveElementStore(RefObject.ID);
        }

        private void ButtonProcessedTextFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            string textInClipboard = ActionClipboard.GetTextFromClipboard();

            ProcessedTextInClipboard(textInClipboard);
        }

        private void ButtonGroupSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueGroups(RefObject.Group).ToList());
        }

        private void ButtonTypeSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueType(RefObject.Type).ToList());
        }

        private void ButtonModuleSelect_Click(object sender, RoutedEventArgs e)
        {
            SetContextMenuSelectedButton(sender, MethodStoreMain.GetUniqueModule(RefObject.Module).ToList());
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

        #endregion

        private void SetContextMenuSelectedButton(object container, List<string> listToContextMenu)
        {
            _buttonOpenedContextMenu = container;

            ((Button)container).ContextMenu.ItemsSource = listToContextMenu;
            ((Button)container).ContextMenu.HorizontalContentAlignment = HorizontalAlignment.Center;
            ((Button)container).ContextMenu.IsOpen = true;
        }

        private void LoadObjectById(int? id)
        {
            if (id == null)
                RefObject = new Models.ElementStore();
            else
                RefObject = Events.LoadElementStoreEvent.Load((int)id);

            ReInitializeDataContext();
        }

        private void CopyObjectElementStore(Models.ElementStore source)
        {
            RefObject = new Models.ElementStore();

            RefObject.Fill(source);

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
                RefObject.Method = text.Substring(positionDot + 1);

                if (RefObject.Method.Contains('('))
                    RefObject.Method = RefObject.Method.Left(RefObject.Method.IndexOf('('));

                ReInitializeDataContext();
            }
        }

        private void ButtonSetTextToClipboard_Click(object sender, RoutedEventArgs e)
        {
            if (RefObject != null)
            {
                string textToClipboard = RefObject.GetTextToClipboard();
                if (!string.IsNullOrWhiteSpace(textToClipboard))
                    ActionClipboard.SEtTextToClipboard(textToClipboard, true, "Вызов метода помещен в буфер обмена.");
            };
        }
    }
}
