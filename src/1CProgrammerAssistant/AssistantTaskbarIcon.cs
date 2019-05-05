using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace _1CProgrammerAssistant
{
    internal static class AssistantTaskbarIcon
    {
        private static readonly TaskbarIcon _taskbarIcon;

        static AssistantTaskbarIcon()
        {
            _taskbarIcon = new TaskbarIcon
            {
                IconSource = new BitmapImage(new Uri("pack://application:,,,/Помощник 1Сника;component/" + "1CProgrammerAssistant.ico")),
                ToolTipText = "Помощник 1Сника"
            };
        }

        internal static void InitializeTaskbarIcon(Action ActionShowMainWindow,
                                                   Action ActionTopmostWindow)
        {
            #region menuItemShowMainWindow

            MenuItem menuItemShowMainWindow = new MenuItem()
            {
                Header = "Развернуть окно"
            };
            menuItemShowMainWindow.Click += (object sender, RoutedEventArgs e) => { ActionShowMainWindow(); };

            #endregion

            #region menuItemTopmostInWindow

            MenuItem menuItemTopmostInWindow = new MenuItem()
            {
                HeaderTemplate = new DataTemplate()
                {
                    DataType = typeof(MenuItem),
                    VisualTree = CreateHeaderTemplate_VisualTree("Поверх всех окон")
                },
                IsChecked = Properties.Settings.Default.IsTopmost,
                IsCheckable = true
            };
            menuItemTopmostInWindow.Click += (object sender, RoutedEventArgs e) => { ActionTopmostWindow(); };

            #endregion

            #region menuItemAutostart

            ImageSource IconShield = Imaging.CreateBitmapSourceFromHBitmap(
               SystemIcons.Shield.ToBitmap().GetHbitmap(),
               IntPtr.Zero,
               Int32Rect.Empty,
               BitmapSizeOptions.FromEmptyOptions());

            MenuItem menuItemAutostart = new MenuItem()
            {
                HeaderTemplate = new DataTemplate()
                {
                    DataType = typeof(MenuItem),
                    VisualTree = CreateHeaderTemplate_VisualTree("Запускать при старте системы", IconShield)
                },
                ToolTip = "Для изменения значения требуются права администратора",
                IsChecked = Permission.GetStatusAutostart(),
                IsCheckable = true
            };
            menuItemAutostart.Click += (object sender, RoutedEventArgs e) => { Permission.SetRemoveAutostart(menuItemAutostart.IsChecked); };

            #endregion

            #region menuItemExit

            MenuItem menuItemExit = new MenuItem()
            {
                Header = "Выход"
            };
            menuItemExit.Click += (object sender, RoutedEventArgs e) => { Application.Current.Shutdown(); };

            #endregion

            _taskbarIcon.ContextMenu = new ContextMenu()
            {
                Items =
                {
                    menuItemShowMainWindow,
                    new Separator(),
                    menuItemTopmostInWindow,
                    menuItemAutostart,
                    new Separator(),
                    menuItemExit
                }
            };
        }

        private static FrameworkElementFactory CreateHeaderTemplate_VisualTree(string header, ImageSource image = null)
        {
            FrameworkElementFactory elementFactoryAutostartTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            elementFactoryAutostartTextBlock.SetValue(TextBlock.TextProperty, header);
            elementFactoryAutostartTextBlock.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 5, 0));

            FrameworkElementFactory elementFactoryAutostart = new FrameworkElementFactory(typeof(StackPanel));
            elementFactoryAutostart.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            elementFactoryAutostart.AppendChild(elementFactoryAutostartTextBlock);

            if (image != null)
            {
                FrameworkElementFactory elementFactoryAutostartIcon = new FrameworkElementFactory(typeof(Image));
                elementFactoryAutostartIcon.SetValue(Image.SourceProperty, image);
                elementFactoryAutostartIcon.SetValue(FrameworkElement.WidthProperty, 14.0);
                elementFactoryAutostartIcon.SetValue(FrameworkElement.HeightProperty, 14.0);

                elementFactoryAutostart.AppendChild(elementFactoryAutostartIcon);
            }

            return elementFactoryAutostart;
        }

        internal static void ShowNotification(string message, BalloonIcon icon = BalloonIcon.None)
        {
            _taskbarIcon.ShowBalloonTip("Помощник 1Сника", message, icon);
        }
    }
}
