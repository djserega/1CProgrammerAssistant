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

namespace _1CProgrammerAssistant
{
    /// <summary>
    /// Логика взаимодействия для InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox(string label, string label2 = null)
        {
            InitializeComponent();

            LabelDescriptionFirst = label;
            LabelDescriptionSecond = label2;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxDescription.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                SetOKAndCloseForm();
        }

        public bool ClickButtonOK { get; private set; }

        #region DependencyProperty

        public string LabelDescriptionFirst
        {
            get { return (string)GetValue(LabelDescriptionFirstProperty); }
            set { SetValue(LabelDescriptionFirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelDescriptionFirstProperty =
            DependencyProperty.Register("LabelDescription", typeof(string), typeof(InputBox), null);


        public string LabelDescriptionSecond
        {
            get { return (string)GetValue(LabelDescriptionSecondProperty); }
            set { SetValue(LabelDescriptionSecondProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelDescriptionSecond.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelDescriptionSecondProperty =
            DependencyProperty.Register("LabelDescriptionSecond", typeof(string), typeof(InputBox), null);


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(InputBox), null);

        #endregion

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            SetOKAndCloseForm();
        }

        private void TextBoxDescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetOKAndCloseForm();
        }

        private void SetOKAndCloseForm()
        {
            ClickButtonOK = true;
            Close();
        }

    }
}
