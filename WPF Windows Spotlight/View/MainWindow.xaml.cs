using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WPF_Windows_Spotlight.Controller;
using WPF_Windows_Spotlight.Models;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using WPF_Windows_Spotlight.View;
using Xceed.Wpf.Toolkit.Core.Utilities;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace WPF_Windows_Spotlight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int _openKeyPointer = 0;
        LoadingCircle _rotate = new LoadingCircle { Angle = 0 };
        ViewInitialization _viewInitialization;
        bool _isWindowVisible = false;
        SearchService _searchService = new SearchService();
        bool _isSearching = false;

        public MainWindow()
        {
            InitializeComponent();

            _viewInitialization = new ViewInitialization(this);
            _viewInitialization.Init();
            _viewInitialization.SetKeyboardEvent(WatchKeyPressedOfWindowVisible);

            InitRender();
        }

        public void InitRender()
        {
            RenderSearchIcon();
            InitViewHeight();
            InitResultsListSource();
            InitLoadingCircle();
            MakeSearchBarToCenter();
            Application.Current.Resources["SearchbarColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.SearchbarColor));
            Application.Current.Resources["SearchbarBorderColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.SearchbarBorderColor));
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

        private void MakeSearchBarToCenter()
        {
            var searchbarTop = (SystemParameters.PrimaryScreenHeight / 2) - (Config.SearchbarHeight / 2);
            var searchbarLeft = (SystemParameters.PrimaryScreenWidth / 2) - (Config.SearchbarWidth / 2);

            ContainerBorder.Width = Config.SearchbarWidth - 20;
            ContainerBorder.Height = Config.SearchbarHeight - 20;
            Canvas.SetTop(ContainerBorder, searchbarTop);
            Canvas.SetLeft(ContainerBorder, searchbarLeft);
            
            DecorateImage.Width = 300;
            Canvas.SetTop(DecorateImage, searchbarTop - 176);
            Canvas.SetLeft(DecorateImage, searchbarLeft + Config.SearchbarWidth - 195);
        }

        void WatchKeyPressedOfWindowVisible(object sender, KeyPressedArgs args)
        {
            string keyPressed = args.KeyPressed.ToString();
            string watchedKeyPressed = Config.HotKeyForOpen[_openKeyPointer];
            _openKeyPointer = (keyPressed == watchedKeyPressed) ? _openKeyPointer + 1 : 0;

            if (_openKeyPointer == Config.HotKeyForOpen.Length)
            {
                _openKeyPointer = 0;
                if (_isWindowVisible)
                {
                    HideWindow();
                }
                else
                {
                    OpenWindow();
                }
            }
        }

        void HideWindow()
        {
            _isWindowVisible = false;
            _searchService.CancelCurrentSearching();
            ResultIcon.Source = null;
            InputTextBox.Clear();
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
                if (InputTextBox.Text == "")
                {
                    InputTextBoxWatermark.Text = "Quick Search";
                    InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Left;
                    _searchService.CancelCurrentSearching();
                    ResultIcon.Visibility = Visibility.Hidden;
                }
                else
                {
                    InputTextBoxWatermark.Text = "";
                }
                ResizeHeight();
            }
            else
            {
                _isSearching = true;
                ResultIcon.Visibility = Visibility.Visible;
                _rotate.Start();
                ContentView.Children.Clear();
                ResultIcon.Source = BitmapToBitmapImage.Transform(Properties.Resources.loading);
                InputTextBoxWatermark.Text = "";

                _searchService.SubscribeSearchOverEvent(SearchOverEvent);
                _searchService.Search(InputTextBox.Text);
            }
        }

        void SearchOverEvent()
        {
            _rotate.Stop();
            _isSearching = false;
            if (_searchService.ResultList.Count > 0)
            {
                IResultItem item = _searchService.ResultList[0];
                // 不知道為啥要兩個 不用兩個他不會回到最上面
                ResultList.ScrollIntoView(item);
                ResultList.ScrollIntoView(ResultList.SelectedItem);
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

        private void OpenItemResource(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null && item.IsSelected)
            {
                _searchService.OpenItemResource(ResultList.SelectedIndex);
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
                ShowSelectdItemContent(_searchService.ResultList[_searchService.SelectedIndex]);
            }
        }
    }
}
