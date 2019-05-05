using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace _1CProgrammerAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AssistantObjects _assistantObjects = new AssistantObjects();
        private readonly GlobalHotKeyManager _hotKeyManager = new GlobalHotKeyManager();
        private readonly ActionClipboard _actionClipboard;

        private readonly string[] _namesAddition = new string[3]
        {
            "DescriptionQuery",
            "ModifiedFiles",
            "MethodStore"
        };
        private int? _previousPageID = null;

        #region Window event

        public MainWindow()
        {
            InitializeComponent();

            _actionClipboard = new ActionClipboard(ref _assistantObjects);

            _actionClipboard.ChangedSourceTextEvents += (string value) => { SourceText = value; };
            _actionClipboard.ChangedResultTextEvents += (string value) => { ResultText = value; };

            ListModifiedFiles = new ObservableCollection<ModifiedFiles.Models.File>();

            ModifiedFiles.Events.CreateNewVersionEvent.NewVersionCreatedEvents += (FileInfo modifiedFile) =>
            {
                Dispatcher.Invoke(new ThreadStart(delegate
                {
                    LoadVersionSelectedModifiedFiles();
                    AssistantTaskbarIcon.ShowNotification(
                        $"{modifiedFile.Name}\n{modifiedFile.LastWriteTime.ToString("HH:mm:ss")}",
                        BalloonIcon.Info);
                }));
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChangePagesAdditions(0);

            InitializeTaskbarIcon();

            LoadListModifiedFiles();

            Topmost = Properties.Settings.Default.IsTopmost;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveListModifiedFiles();
            _assistantObjects.ViewerFilesMain.Dispose();
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

        #endregion

        #region DependencyProperty

        public string SourceText
        {
            get { return (string)GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value); }
        }
        public static readonly DependencyProperty SourceTextProperty =
            DependencyProperty.Register("SourceText", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public string ResultText
        {
            get { return (string)GetValue(ResultTextProperty); }
            set { SetValue(ResultTextProperty, value); }
        }

        public static readonly DependencyProperty ResultTextProperty =
            DependencyProperty.Register("ResultText", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        #endregion

        #region Button

        private void ButtonDescriptionQuery_Click(object sender, RoutedEventArgs e) => ClickButtonsAdditions(sender);
        private void ButtonModifiedFiles_Click(object sender, RoutedEventArgs e) => ClickButtonsAdditions(sender);
        private void ButtonMethodStore_Click(object sender, RoutedEventArgs e) { ClickButtonsAdditions(sender); InitializeMethodStore(); }
        private void ClickButtonsAdditions(object sender) => ChangePagesAdditions(Grid.GetColumn((Button)sender));
        private void ButtonProcessingTextInClipboard_Click(object sender, RoutedEventArgs e) => _actionClipboard.ProcessTextWithClipboard(true);
        private void ButtonCopyResultToClipboard_Click(object sender, RoutedEventArgs e) => _actionClipboard.SetResultTextToClipboard(true);

        #endregion
        
        #region Changes pages

        private void ChangePagesAdditions(int newColumn)
        {
            if (_previousPageID == null)
                CollapsedPages();

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

        private void CollapsedPages()
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

        private void ChangePagesAdditionsInitializeAnimation(
            Button buttonNewPage,
            Button buttonPreviousPage,
            Border borderVisible,
            Border borderCollapsed,
            Grid gridVisible,
            Grid gridCollapsed)
        {
            DoubleAnimation animationVisible;
            DoubleAnimation animationCollapsed;

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

        #region DependencyProperty

        public ObservableCollection<ModifiedFiles.Models.File> ListModifiedFiles
        {
            get { return (ObservableCollection<ModifiedFiles.Models.File>)GetValue(ListModifiedFilesProperty); }
            set { SetValue(ListModifiedFilesProperty, value); }
        }

        public static readonly DependencyProperty ListModifiedFilesProperty =
            DependencyProperty.Register("ListModifiedFiles", typeof(ObservableCollection<ModifiedFiles.Models.File>), typeof(MainWindow), null);


        public ObservableCollection<ModifiedFiles.Models.Version> ListModifiedFilesVersion
        {
            get { return (ObservableCollection<ModifiedFiles.Models.Version>)GetValue(ListModifiedFilesVersionProperty); }
            set { SetValue(ListModifiedFilesVersionProperty, value); }
        }

        public static readonly DependencyProperty ListModifiedFilesVersionProperty =
            DependencyProperty.Register("ListModifiedFilesVersion", typeof(ObservableCollection<ModifiedFiles.Models.Version>), typeof(MainWindow), null);


        public ModifiedFiles.Models.Version ListModifiedFilesVersionSelectedItem
        {
            get { return (ModifiedFiles.Models.Version)GetValue(ListModifiedFilesVersionSelectedItemProperty); }
            set { SetValue(ListModifiedFilesVersionSelectedItemProperty, value); }
        }

        public static readonly DependencyProperty ListModifiedFilesVersionSelectedItemProperty =
            DependencyProperty.Register("ListModifiedFilesVersionSelectedItem", typeof(ModifiedFiles.Models.Version), typeof(MainWindow), null);

        #endregion

        #region DataGridModifiedFiles

        private void DataGridModifiedFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EnterDescriptionSelectedModifiedFile(sender);
        }

        private void DataGridModifiedFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
                EnterDescriptionSelectedModifiedFile(sender);
        }

        private void DataGridModifiedFiles_Drop(object sender, DragEventArgs e)
        {
            string[] dropFiles = (string[])e.Data.GetData("FileDrop");
            foreach (string path in dropFiles)
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Extension == ".epf" || fileInfo.Extension == ".erf")
                    ModifiedFilesAddFileByPath(path);
            }
        }

        #endregion

        private void EnterDescriptionSelectedModifiedFile(object sender)
        {
            if (((DataGrid)sender).CurrentColumn?.SortMemberPath == "Description"
                && SelectedModifiedFile != null)
            {
                InputBox inputBox = new InputBox("Описание внешнего файла:", SelectedModifiedFile.FileName)
                {
                    Description = SelectedModifiedFile.Description ?? string.Empty
                };
                inputBox.ShowDialog();

                if (inputBox.ClickButtonOK)
                {
                    SelectedModifiedFile.Description = inputBox.Description;
                    if (ListModifiedFilesVersion.Count > 0)
                        _assistantObjects.ModifiedFilesMain.SetDescriptionLastVersion(SelectedModifiedFile, ListModifiedFilesVersion);
                }
                LoadVersionSelectedModifiedFiles();

                DataGridModifiedFiles.Items.Refresh();
            }
        }

        public ModifiedFiles.Models.File SelectedModifiedFile
        {
            get => _selectedModifiedFile;
            set
            {
                _selectedModifiedFile = value;

                LoadVersionSelectedModifiedFiles();
            }
        }

        private readonly List<ModifiedFiles.Models.Version> _selectedVersions = new List<ModifiedFiles.Models.Version>();

        private void LoadVersionSelectedModifiedFiles()
        {
            ListModifiedFilesVersion?.Clear();

            string lastComment = string.Empty;
            List<ModifiedFiles.Models.Version> listVersion = _assistantObjects.ModifiedFilesMain.GetListVersion(_selectedModifiedFile);
            if (listVersion != null)
            {
                ListModifiedFilesVersion = new ObservableCollection<ModifiedFiles.Models.Version>(listVersion);
                DataGridModifiedFilesVersion.Items.SortDescriptions.Add(new SortDescription("DateVersion", ListSortDirection.Descending));

                ModifiedFiles.Models.Version lastCommentedVersion = ListModifiedFilesVersion.LastOrDefault(f => !string.IsNullOrWhiteSpace(f.Description));
                if (lastCommentedVersion != null)
                    lastComment = lastCommentedVersion.Description;
            }

            if (_selectedModifiedFile != null)
            {
                _selectedModifiedFile.Description = lastComment;
                //DataGridModifiedFiles.Items.Refresh();
            }
        }

        private void LoadListModifiedFiles()
        {
            if (Properties.Settings.Default.ListModifiedFiles == null)
                Properties.Settings.Default.ListModifiedFiles = new System.Collections.Specialized.StringCollection();

            foreach (string path in Properties.Settings.Default.ListModifiedFiles)
                ListModifiedFiles.Add(new ModifiedFiles.Models.File(path));

            _assistantObjects.ModifiedFilesMain.Files = ListModifiedFiles.ToList();
        }

        private void SaveListModifiedFiles()
        {
            Properties.Settings.Default.ListModifiedFiles.Clear();

            foreach (ModifiedFiles.Models.File file in _assistantObjects.ModifiedFilesMain.Files)
                Properties.Settings.Default.ListModifiedFiles.Add(file.Path);

            Properties.Settings.Default.Save();
        }

        private void MenuItemCompareVersion_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedVersions.Count > 1)
                _assistantObjects.ViewerFilesMain.CompareFilesVersion(_selectedVersions[0].Path, _selectedVersions[_selectedVersions.Count - 1].Path);
            else
                MessageBox.Show("Для сравнения версий нужно выделить более одного файла версии.");
        }

        private void DataGridModifiedFilesVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedVersions.Clear();

            foreach (ModifiedFiles.Models.Version itemVersion in ((DataGrid)sender).SelectedItems)
                _selectedVersions.Add(itemVersion);
        }

        #region Buttons

        private void ButtonModifiedFilesAddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Внешние обработки и отчёты|*.epf;*.erf|Внешние обработки|*.epf|Внешние отчёты|*.erf",
                Multiselect = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true,
                Title = "Выбор файлов версионирования"
            };
            if (openFileDialog.ShowDialog() == true)
                foreach (string fileName in openFileDialog.FileNames)
                    ModifiedFilesAddFileByPath(fileName);
        }

        private void ButtonModifiedFilesRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            ModifiedFiles.Models.File fileInList = ListModifiedFiles.FirstOrDefault(f => f == SelectedModifiedFile);
            if (fileInList != null)
            {
                ListModifiedFiles.Remove(fileInList);

                _assistantObjects.ModifiedFilesMain.Files = ListModifiedFiles.ToList();
            }
        }

        private void ButtonModifiedFilesChangeVisibility_Click(object sender, RoutedEventArgs e)
        {
            ModifiedFilesChangeVisibilityListVesions();
        }

        private void ButtonOpenFolderVersion_Click(object sender, RoutedEventArgs e)
        {
            _assistantObjects.ModifiedFilesMain.OpenDirectoryVersion();
        }

        private void MenuItemVersionOpenFile(object sender, RoutedEventArgs e)
        {
            if (ListModifiedFilesVersionSelectedItem == null)
                return;

            _assistantObjects.ViewerFilesMain.OpenFileVersion(ListModifiedFilesVersionSelectedItem.Path);
        }

        #endregion

        private void ModifiedFilesAddFileByPath(string path)
        {
            if (ListModifiedFiles.FirstOrDefault(f => f.Path == path) == null)
            {
                ListModifiedFiles.Add(new ModifiedFiles.Models.File(path));

                _assistantObjects.ModifiedFilesMain.Files = ListModifiedFiles.ToList();
            }
        }

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

        #region Addition Method store

        #region DependencyProperty

        public ObservableCollection<MethodStore.Models.ElementStore> MethodStoreListMethod
        {
            get { return (ObservableCollection<MethodStore.Models.ElementStore>)GetValue(MethodStoreListMethodProperty); }
            set { SetValue(MethodStoreListMethodProperty, value); }
        }

        public static readonly DependencyProperty MethodStoreListMethodProperty =
            DependencyProperty.Register("MethodStoreListMethod", typeof(ObservableCollection<MethodStore.Models.ElementStore>), typeof(MainWindow), null);


        public MethodStore.Models.ElementStore MethodStoreListMethodSelectedItem
        {
            get { return (MethodStore.Models.ElementStore)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MethodStoreListMethodSelectedItem", typeof(MethodStore.Models.ElementStore), typeof(MainWindow), null);

        #endregion

        #region Button

        private void ButtonMethodStoreAdd_Click(object sender, RoutedEventArgs e)
        {
            var viewElementStore = new Views.MethodStore.ElementStore
            {
                Left = Left + 20,
                Top = Top + 20,
                Owner = this
            };
            viewElementStore.ShowDialog();
        }

        private void ButtonMethodStoreUpdateList_Click(object sender, RoutedEventArgs e)
        {
            InitializeMethodStore();
        }

        private void InitializeMethodStore()
        {
            _assistantObjects.MethodStoreMain.LoadMethod();

            MethodStoreListMethod = _assistantObjects.MethodStoreMain.ListMethods;
        }

        #endregion

        #endregion

        private void InitializeTaskbarIcon()
        {
            AssistantTaskbarIcon.InitializeTaskbarIcon(
                
                () =>
                {
                    Show(); WindowState = WindowState.Normal;
                },

                () =>
                {
                    bool newValueIsTopmost = !Properties.Settings.Default.IsTopmost;
                    Properties.Settings.Default.IsTopmost = newValueIsTopmost;
                    Topmost = newValueIsTopmost;
                }  
            );
        }

        private Visibility ReverseValueVisibility(Visibility currentVisibility)
            => Visibility.Collapsed == currentVisibility ? Visibility.Visible : Visibility.Collapsed;

    }
}
