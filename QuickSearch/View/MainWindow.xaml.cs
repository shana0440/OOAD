using FMUtils.KeyboardHook;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using QuickSearch.Controller;
using QuickSearch.Models;
using QuickSearch.Models.ResultItemsFactory;
using QuickSearch.View;
using Xceed.Wpf.Toolkit.Core.Utilities;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using System.Threading;
using System.Windows.Media;

namespace QuickSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoadingCircle _rotate = new LoadingCircle { Angle = 0 };
        ViewInitialization _viewInitialization;
        bool _isWindowVisible = false;
        SearchService _searchService;
        bool _isSearching = false;
        HookKeyMatch _hookKeyMatch = new HookKeyMatch();
        Theme _theme;

        public MainWindow()
        {
            InitializeComponent();

            _viewInitialization = new ViewInitialization(this);
            _viewInitialization.Init();
            _searchService = new SearchService();
            _hookKeyMatch.PlusKeyDownEvent(SearchbarVisable);
            Application.Current.Resources["DecorateImage"] = BitmapToBitmapImage.Transform((System.Drawing.Bitmap)System.Drawing.Image.FromFile(@"images\DecorateImage2.png"));
            _searchService.SubscribeSearchOverEvent(() => {
                this.Dispatcher.Invoke(SearchOverEvent);
            });

            InitRender();
        }

        void SearchOverEvent()
        {
            _rotate.Stop();
            _isSearching = false;
            if (_searchService.ResultList.Count > 0)
            {
                IResultItem item = _searchService.ResultList[0];
                VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(ResultList)?.ScrollToTop();
                _searchService.SelectItem(0);
                ShowSelectdItemContent(item);
            }
            else
            {
                ResultIcon.Visibility = Visibility.Hidden;
                ResizeHeight();
                ContentView.Children.Clear();
                InputTextBoxWatermark.Text = "— 沒有結果";
                InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        void ShowSelectdItemContent(IResultItem item)
        {
            if (item == null) return;
            if (!_isSearching)
            {
                ResultIcon.Source = item.Icon;
            }
            ResizeHeight();
            item.GenerateContent(ContentView);
        }

        public void InitRender()
        {
            RenderSearchIcon();
            InitViewHeight();
            InitResultsListSource();
            InitLoadingCircle();
            MakeSearchBarToCenter();
            ApplyTheme("Dark");
        }

        void RenderSearchIcon()
        {
            SearchIcon.Source = BitmapToBitmapImage.Transform(Properties.Resources.search);
        }

        void InitViewHeight()
        {
            ResizeHeight();
        }

        void ResizeHeight()
        {
            if (_searchService.ResultList.Count > 0)
            {
                Application.Current.Resources["BorderClipRect"] = new Rect(0, 0, 680, 400);
            }
            else
            {
                Application.Current.Resources["BorderClipRect"] = new Rect(0, 0, 680, 52);
            }
        }

        void InitResultsListSource()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_searchService.ResultList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            ResultList.ItemsSource = collectionView;
        }

        void InitLoadingCircle()
        {
            DataContext = _rotate;
        }

        void MakeSearchBarToCenter()
        {
            var searchbarTop = (SystemParameters.PrimaryScreenHeight / 2) - (Config.SearchbarHeight / 2);
            var searchbarLeft = (SystemParameters.PrimaryScreenWidth / 2) - (Config.SearchbarWidth / 2);

            ContainerBorder.Width = Config.SearchbarWidth - 20;
            ContainerBorder.Height = Config.SearchbarHeight - 20;
            Canvas.SetTop(ContainerBorder, searchbarTop);
            Canvas.SetLeft(ContainerBorder, searchbarLeft);
            
            DecorateImage.Width = 300;
            Canvas.SetTop(DecorateImage, searchbarTop - 176);
            Canvas.SetLeft(DecorateImage, searchbarLeft + Config.SearchbarWidth - 196);
        }

        void ApplyTheme(string themeName)
        {
            _theme = new Theme(themeName);
            var fields = _theme.GetType().GetFields();
            foreach (var field in fields)
            {
                var test = Application.Current.Resources[field.Name] = _theme.GetType().GetField(field.Name).GetValue(_theme);
            }
        }

        void SearchbarVisable(KeyboardHookEventArgs args)
        {
            if (_hookKeyMatch.MatchKey(Config.KeyForOpenAndHide))
            {
                if (_isWindowVisible)
                {
                    HideWindow();
                }
                else
                {
                    OpenWindow();
                }
            }
            else if (_hookKeyMatch.MatchKey(Config.KeyForHide))
            {
                HideWindow();
            }
        }

        void HideWindow()
        {
            _isWindowVisible = false;
            _searchService.AbortSearchThread();
            _searchService.ResultList.Clear();
            ResultIcon.Source = null;
            InputTextBox.Clear();
            ResizeHeight();
            Hide();
        }

        void OpenWindow()
        {
            _isWindowVisible = true;
            Activate();
            Dispatcher.BeginInvoke((Action)delegate
            {
                FocusManager.SetFocusedElement(this, InputTextBox);
                Keyboard.Focus(InputTextBox);
            }, DispatcherPriority.Render);
            Show();
        }
        
        void LostFocusWindow(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus == null)
            {
                HideWindow();
            }
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            if (InputTextBox.Text.Trim() == "")
            {
                ClearSearchText();
            }
            else
            {
                _isSearching = true;
                ResultIcon.Visibility = Visibility.Visible;
                _rotate.IfStopThenStart();
                ContentView.Children.Clear();
                ResultIcon.Source = _theme.LoadingImage;
                InputTextBoxWatermark.Text = "";

                _searchService.Search(InputTextBox.Text);
            }
        }

        private void ClearSearchText()
        {
            if (InputTextBox.Text == "")
            {
                InputTextBoxWatermark.Text = "Quick Search";
                InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Left;
                _searchService.AbortSearchThread();
                _searchService.ResultList.Clear();
                ResultIcon.Visibility = Visibility.Hidden;
            }
            else
            {
                InputTextBoxWatermark.Text = "";
            }
            ResizeHeight();
        }

        private void OpenItemResource(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null && item.IsSelected)
            {
                _searchService.OpenSelectedItemResource();
                HideWindow();
            }
        }

        private void SelectItem(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _searchService.SelectItem(_searchService.SelectedIndex - 1);
                    if (_searchService.SelectedIndex - 1 < 0)
                    {
                        VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(ResultList)?.ScrollToTop();
                    }
                    break;
                case Key.Down:
                    _searchService.SelectItem(_searchService.SelectedIndex + 1);
                    break;
                case Key.Enter:
                    _searchService.OpenSelectedItemResource();
                    HideWindow();
                    break;
            }
            if (_searchService.ResultList.Count > 0)
            {
                ResultList.ScrollIntoView(ResultList.SelectedItem);
                if (_searchService.SelectedIndex < _searchService.ResultList.Count)
                {
                    ShowSelectdItemContent(_searchService.ResultList[_searchService.SelectedIndex]);
                }
            }
        }
    }
}
