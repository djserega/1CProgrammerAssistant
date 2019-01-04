using _1CProgrammerAssistant.Additions;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace _1CProgrammerAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskbarIcon _taskbarIcon;
        private readonly GlobalHotKeyManager _hotKeyManager = new GlobalHotKeyManager();

        public MainWindow()
        {
            InitializeComponent();

            ProcessTextInClipboardEvents.ProcessTextInClipboardEvent +=
                () =>
                {
                    if (ProcessTextWithClipboard())
                        SetResultTextToClipboard();
                };

            _taskbarIcon = new TaskbarIcon
            {
                IconSource = new BitmapImage(new Uri("pack://application:,,,/Помощник 1Сника;component/" + "1CProgrammerAssistant.ico")),
                ToolTipText = "Помощник 1Сника"
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeTaskbarIcon();
        }

        private void InitializeTaskbarIcon()
        {
            #region Create HeaderTemplate_VisualTree

            FrameworkElementFactory elementFactoryAutostartTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            elementFactoryAutostartTextBlock.SetValue(TextBlock.TextProperty, "Запускать при старте системы");
            elementFactoryAutostartTextBlock.SetValue(MarginProperty, new Thickness(0, 0, 5, 0));

            ImageSource Icon = Imaging.CreateBitmapSourceFromHBitmap(
                  SystemIcons.Shield.ToBitmap().GetHbitmap(),
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  BitmapSizeOptions.FromEmptyOptions());

            FrameworkElementFactory elementFactoryAutostartIcon = new FrameworkElementFactory(typeof(Image));
            elementFactoryAutostartIcon.SetValue(Image.SourceProperty, Icon);
            elementFactoryAutostartIcon.SetValue(WidthProperty, 14.0);
            elementFactoryAutostartIcon.SetValue(HeightProperty, 14.0);

            FrameworkElementFactory elementFactoryAutostart = new FrameworkElementFactory(typeof(StackPanel));
            elementFactoryAutostart.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            elementFactoryAutostart.AppendChild(elementFactoryAutostartTextBlock);
            elementFactoryAutostart.AppendChild(elementFactoryAutostartIcon);

            #endregion

            MenuItem menuItemAutostart = new MenuItem()
            {
                HeaderTemplate = new DataTemplate()
                {
                    DataType = typeof(MenuItem),
                    VisualTree = elementFactoryAutostart
                },
                ToolTip = "Для изменения значения требуются права администратора",
                IsChecked = Permission.GetStatusAutostart(),
                IsCheckable = true
            };
            menuItemAutostart.Click += (object sender, RoutedEventArgs e) => { Permission.SetRemoveAutostart(menuItemAutostart.IsChecked); };

            MenuItem menuItemExit = new MenuItem()
            {
                Header = "Выход"
            };
            menuItemExit.Click += (object sender, RoutedEventArgs e) => { Application.Current.Shutdown(); };

            _taskbarIcon.ContextMenu = new ContextMenu()
            {
                Items =
                {
                    menuItemAutostart,
                    new Separator(),
                    menuItemExit
                }
            };
        }

        #region Public properties - Additions classes

        public DescriptionsTheMethods.Main DescriptionsTheMethodsMain { get; set; } = new DescriptionsTheMethods.Main();
        public QueryParameters.Main QueryParametersMain { get; set; } = new QueryParameters.Main();
        //    new MethodStore.Class1();

        #endregion

        #region DependencyProperty

        public string SourceText
        {
            get { return (string)GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SourceText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceTextProperty =
            DependencyProperty.Register("SourceText", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public string ResultText
        {
            get { return (string)GetValue(ResultTextProperty); }
            set { SetValue(ResultTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResultText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultTextProperty =
            DependencyProperty.Register("ResultText", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        #endregion

        #region Clipboard

        private bool ProcessTextWithClipboard()
        {
            if (Clipboard.ContainsText())
            {
                string textInClipboard = Clipboard.GetText();

                SourceText = textInClipboard;

                DescriptionsTheMethodsMain.StringMethod = textInClipboard;
                if (string.IsNullOrEmpty(DescriptionsTheMethodsMain.TextError)
                    && !string.IsNullOrEmpty(DescriptionsTheMethodsMain.Description))
                {
                    ResultText = DescriptionsTheMethodsMain.Description;
                    ShowNotification($"Получено описание метода: {DescriptionsTheMethodsMain.MethodName}");
                    return true;
                }

                QueryParametersMain.QueryText = textInClipboard;
                if (string.IsNullOrEmpty(QueryParametersMain.TextError)
                    && !string.IsNullOrEmpty(QueryParametersMain.QueryParameters))
                {
                    ResultText = QueryParametersMain.QueryParameters;

                    string message = "Получены параметры запроса";
                    if (!string.IsNullOrEmpty(QueryParametersMain.NameVariableQueryObject))
                        message += $": {QueryParametersMain.NameVariableQueryObject.Trim()}";

                    ShowNotification(message);
                    return true;
                }


                return false;
            }

            return false;
        }

        private void SetResultTextToClipboard()
        {
            try
            {
                Clipboard.SetText(ResultText);
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(
                        ex.Message
                        + "\n" +
                        ex.InnerException?.Message
                        + "\n" +
                        ex.InnerException?.InnerException?.Message,
                        EventLogEntryType.Warning);
                }
            }
        }

        #endregion

        #region Button

        private void ButtonCopyResultToClipboard_Click(object sender, RoutedEventArgs e)
            => SetResultTextToClipboard();

        private void ButtonProcessingTextInClipboard_Click(object sender, RoutedEventArgs e)
            => ProcessTextWithClipboard();

        #endregion

        #region Notifications

        private void ShowNotification(string message, BalloonIcon icon = BalloonIcon.None)
        {
            _taskbarIcon.ShowBalloonTip("Помощник 1Сника", message, icon);
        }

        #endregion

    }
}
