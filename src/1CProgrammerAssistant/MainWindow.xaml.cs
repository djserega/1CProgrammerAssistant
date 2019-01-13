﻿using _1CProgrammerAssistant.Additions;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private readonly string[] _namesAddition = new string[2]
        {
            "DescriptionQuery",
            "ModifiedFiles"
        };
        private int? _previousPageID = null;

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

            ListModifiedFiles = new ObservableCollection<ModifiedFiles.Models.File>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangePagesAdditions(0);

            InitializeTaskbarIcon();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_previousPageID != null)
                ChangePagesAdditions((int)_previousPageID);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            else
                Show();
        }

        private void InitializeTaskbarIcon()
        {
            #region menuItemShowMainWindow

            MenuItem menuItemShowMainWindow = new MenuItem()
            {
                Header = "Развернуть окно"
            };
            menuItemShowMainWindow.Click += (object sender, RoutedEventArgs e) => { Show(); WindowState = WindowState.Normal; };

            #endregion

            #region menuItemAutostart

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
                    menuItemAutostart,
                    new Separator(),
                    menuItemExit
                }
            };
        }

        #region Public properties - Additions classes

        public DescriptionsTheMethods.Main DescriptionsTheMethodsMain { get; } = new DescriptionsTheMethods.Main();
        public QueryParameters.Main QueryParametersMain { get; } = new QueryParameters.Main();
        //    new MethodStore.Class1();
        public ModifiedFiles.Main ModifiedFilesMain { get; } = new ModifiedFiles.Main();

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

        private void ButtonDescriptionQuery_Click(object sender, RoutedEventArgs e)
        {
            ChangePagesAdditions(Grid.GetColumn((Button)sender));
        }

        private void ButtonModifiedFiles_Click(object sender, RoutedEventArgs e)
        {
            ChangePagesAdditions(Grid.GetColumn((Button)sender));
        }

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

        #region Changes pages

        private void ChangePagesAdditions(int newColumn)
        {
            if (_previousPageID == null)
            {
                for (int i = 0; i < _namesAddition.Count(); i++)
                {
                    Border findedBorder = GetFindedBorderByPageID(i);
                    DoubleAnimation animation = new DoubleAnimation(0d, new Duration(TimeSpan.FromMilliseconds(0)))
                    {
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                    };
                    findedBorder.BeginAnimation(WidthProperty, animation);
                    findedBorder.Visibility = Visibility.Collapsed;
                }
            }

            Button buttonNewPage = null;
            Button buttonPreviousPage = null;

            Border borderVisible = null;
            Border borderCollapsed = null;

            Grid gridVisible = null;
            Grid gridCollapsed = null;

            for (int i = 0; i < _namesAddition.Count(); i++)
            {
                if (i == newColumn)
                {
                    buttonNewPage = GetFindedButtonByPageID(i);
                    borderVisible = GetFindedBorderByPageID(i);
                    gridVisible = GetFindedGridByPageID(i);
                }
                else if (i == _previousPageID)
                {
                    buttonPreviousPage = GetFindedButtonByPageID(i);
                    borderCollapsed = (Border)FindName(GetNameAdditionsBorder(i));
                    gridCollapsed = GetFindedGridByPageID(i);
                }
            }

            ChangePagesAdditionsInitializeAnimation(buttonNewPage, buttonPreviousPage, borderVisible, borderCollapsed, gridVisible, gridCollapsed);

            _previousPageID = newColumn;
        }

        private void ChangePagesAdditionsInitializeAnimation(
            Button buttonNewPage,
            Button buttonPreviousPage,
            Border borderVisible,
            Border borderCollapsed,
            Grid gridVisible,
            Grid gridCollapsed)
        {
            DoubleAnimation animationVisible = null;
            DoubleAnimation animationCollapsed = null;

            animationVisible = new DoubleAnimation(buttonNewPage.ActualWidth, TimeSpan.FromMilliseconds(500));
            animationCollapsed = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));

            ChangePagesAdditionsBeginAnimation(borderVisible, animationVisible, borderCollapsed, animationCollapsed);

            TimeSpan timeAnimation = TimeSpan.FromMilliseconds(400);
            animationVisible = new DoubleAnimation(GridParent.ActualWidth, timeAnimation);
            animationCollapsed = new DoubleAnimation(0, timeAnimation);

            ChangePagesAdditionsBeginAnimation(gridVisible, animationVisible, gridCollapsed, animationCollapsed);
        }

        private void ChangePagesAdditionsBeginAnimation(
            FrameworkElement borderVisible,
            DoubleAnimation animationVisible,
            FrameworkElement borderCollapsed,
            DoubleAnimation animationCollapsed)
        {
            if (borderCollapsed != null)
            {
                animationCollapsed.Completed += (object sender, EventArgs e) => { borderCollapsed.Visibility = Visibility.Visible; };
                borderCollapsed.BeginAnimation(WidthProperty, animationCollapsed);
            }

            if (borderVisible != null)
            {
                borderVisible.Visibility = Visibility.Visible;
                borderVisible.BeginAnimation(WidthProperty, animationVisible);
            }
        }

        private Button GetFindedButtonByPageID(int i) => (Button)FindName(GetNameAdditionsButton(i));

        private Border GetFindedBorderByPageID(int i) => (Border)FindName(GetNameAdditionsBorder(i));

        private Grid GetFindedGridByPageID(int i) => (Grid)FindName(GetNameAdditionsPage(i));

        private string GetNameAdditionsButton(int i) => "Button" + _namesAddition[i];

        private string GetNameAdditionsBorder(int i) => "Border" + _namesAddition[i];

        private string GetNameAdditionsPage(int i) => "Additions" + _namesAddition[i];

        #endregion

        #region Addition Modified files

        private ModifiedFiles.Models.File _selectedModifiedFile;

        public ObservableCollection<ModifiedFiles.Models.File> ListModifiedFiles
        {
            get { return (ObservableCollection<ModifiedFiles.Models.File>)GetValue(ListModifiedFilesProperty); }
            set { SetValue(ListModifiedFilesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModifiedFiles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListModifiedFilesProperty =
            DependencyProperty.Register("ListModifiedFiles", typeof(ObservableCollection<ModifiedFiles.Models.File>), typeof(MainWindow), null);


        public ObservableCollection<ModifiedFiles.Models.Version> ListModifiedFilesVersion
        {
            get { return (ObservableCollection<ModifiedFiles.Models.Version>)GetValue(ListModifiedFilesVersionProperty); }
            set { SetValue(ListModifiedFilesVersionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ListModifiedFilesVersion.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ListModifiedFilesVersionProperty =
            DependencyProperty.Register("ListModifiedFilesVersion", typeof(ObservableCollection<ModifiedFiles.Models.Version>), typeof(MainWindow), null);


        public ModifiedFiles.Models.File SelectedModifiedFile
        {
            get => _selectedModifiedFile;
            set
            {
                _selectedModifiedFile = value;

                ListModifiedFilesVersion?.Clear();

                List<ModifiedFiles.Models.Version> listVersion = ModifiedFilesMain.GetListVersion(value);
                if (listVersion != null)
                    ListModifiedFilesVersion = new ObservableCollection<ModifiedFiles.Models.Version>(listVersion);
            }
        }

        #region Buttons

        private void ButtonModifiedFilesAddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Внешние обработки|*.epf|Внешние отчёты|*.erf|Внешние обработки и отчёты|*.epf;*.erf",
                Multiselect = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
                Title = "Выбор файлов версионирования"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                    ListModifiedFiles.Add(new ModifiedFiles.Models.File(fileName));

                ModifiedFilesMain.Files = ListModifiedFiles.ToList();
            };
        }

        private void ButtonModifiedFilesRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            ModifiedFiles.Models.File fileInList = ListModifiedFiles.FirstOrDefault(f => f == SelectedModifiedFile);
            if (fileInList != null)
            {
                ListModifiedFiles.Remove(fileInList);

                ModifiedFilesMain.Files = ListModifiedFiles.ToList();
            }
        }

        private void ButtonModifiedFilesChangeVisibility_Click(object sender, RoutedEventArgs e)
        {
            ModifiedFilesChangeVisibilityListVesions();
        }

        private void ButtonOpenFolderVersion_Click(object sender, RoutedEventArgs e)
        {
            ModifiedFilesMain.OpenDirectoryVersion();
        }

        #endregion

        #region Visibility table list version

        private void ModifiedFilesChangeVisibilityListVesions()
        {
            Visibility newVisibility = ReverseValueVisibility(GridSplitterModifiedFiles.Visibility);

            if (newVisibility == Visibility.Visible)
                ModifiedFilesChangeVisibilityListVesionsAnimationOpen(newVisibility);
            else
                ModifiedFilesChangeVisibilityListVesionsAnimationClose(newVisibility);
        }

        private void ModifiedFilesChangeVisibilityListVesionsAnimationOpen(Visibility newVisibility)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                Duration = new Duration(TimeSpan.FromMilliseconds(800)),
                From = 0,
                To = 350
            };
            animation.Completed += (object sender, EventArgs e) =>
            {
            };
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, ColumnDefinitionModifiedFilesVersion);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

            ModifiedFilesChangeVisibilityListVesionsChangeVisibility(newVisibility);


            DoubleAnimation splitterAmimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            GridSplitterModifiedFiles.BeginAnimation(OpacityProperty, splitterAmimation);

            storyboard.Begin();
        }

        private void ModifiedFilesChangeVisibilityListVesionsAnimationClose(Visibility newVisibility)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation
            {
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                From = 350,
                To = 0
            };
            animation.Completed += (object sender, EventArgs e) =>
            {
                DoubleAnimation splitterAmimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                splitterAmimation.Completed += (object senderSplitter, EventArgs eSplitter) =>
                {
                    ModifiedFilesChangeVisibilityListVesionsChangeVisibility(newVisibility);
                };
                GridSplitterModifiedFiles.BeginAnimation(OpacityProperty, splitterAmimation);
            };
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, ColumnDefinitionModifiedFilesVersion);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(ColumnDefinition.MaxWidth)"));

            storyboard.Begin();
        }

        private void ModifiedFilesChangeVisibilityListVesionsChangeVisibility(Visibility newVisibility)
        {
            GridSplitterModifiedFiles.Visibility = newVisibility;

            ColumnDefinitionModifiedFilesFile.Width = new GridLength(DataGridModifiedFiles.ActualWidth, GridUnitType.Auto);
        }

        #endregion

        #endregion

        private Visibility ReverseValueVisibility(Visibility currentVisibility)
            => Visibility.Collapsed == currentVisibility ? Visibility.Visible : Visibility.Collapsed;

    }
}
