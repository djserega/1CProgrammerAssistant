using _1CProgrammerAssistant.Additions;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1CProgrammerAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon _taskbarIcon;
        private GlobalHotKeyManager _hotKeyManager = new GlobalHotKeyManager();

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

        public DescriptionsTheMethods.Main DescriptionsTheMethodsMain { get; set; } = new DescriptionsTheMethods.Main();
        public QueryParameters.Main QueryParametersMain { get; set; } = new QueryParameters.Main();
        //    new MethodStore.Class1();



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
                    ShowNotification($"Получены параметры запроса: {QueryParametersMain.NameVariableQueryObject.Trim()}");
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

        private void ButtonCopyResultToClipboard_Click(object sender, RoutedEventArgs e)
        {
            SetResultTextToClipboard();
        }

        private void ShowNotification(string message, BalloonIcon icon = BalloonIcon.None)
        {
            _taskbarIcon.ShowBalloonTip("Помощник 1Сника", message, icon);
        }

        private void ButtonProcessingTextInClipboard_Click(object sender, RoutedEventArgs e)
        {
            ProcessTextWithClipboard();
        }
    }
}
