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

        public bool ClickButtonOK { get; private set; }

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


        public string TextBoxDescription
        {
            get { return (string)GetValue(TextBoxDescriptionProperty); }
            set { SetValue(TextBoxDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextBoxDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxDescriptionProperty =
            DependencyProperty.Register("TextBoxDescription", typeof(string), typeof(InputBox), null);


        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            ClickButtonOK = true;
            Close();
        }
    }
}
