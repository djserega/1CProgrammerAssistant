using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1CProgrammerAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GlobalHotKeyManager _hotKeyManager = new GlobalHotKeyManager();

        public MainWindow()
        {
            InitializeComponent();

            ProcessTextInClipboardEvents.ProcessTextInClipboardEvent +=
                () =>
                {
                    ProcessTextWithClipboard();
                    SetDescriptionToClipboard();
                };
        }

        public DescriptionsTheMethods.Main DescriptionsTheMethodsMain { get; set; } = new DescriptionsTheMethods.Main();
        //new QueryParameters.Class1();
        //    new MethodStore.Class1();


        private void ProcessTextWithClipboard()
        {
            if (Clipboard.ContainsText())
                DescriptionsTheMethodsMain.DataMethod.StringMethod = Clipboard.GetText();
        }
        private void SetDescriptionToClipboard()
        {
            try
            {
                Clipboard.SetText(DescriptionsTheMethodsMain.DataMethod.Description);
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
    }
}
