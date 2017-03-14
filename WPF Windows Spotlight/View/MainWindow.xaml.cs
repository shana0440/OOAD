using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WPF_Windows_Spotlight.Controller;
using WPF_Windows_Spotlight.Models;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using WPF_Windows_Spotlight.View;
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
        int _windowHieght = 420;
        int _inputHieght = 70;
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
        }

        void RenderSearchIcon()
        {
            SearchIcon.Source = BitmapToBitmapImage.Transform(Properties.Resources.search);
        }

        void InitViewHeight()
        {
            Height = _inputHieght;
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

        void HideWindow()
        {
            _isWindowVisible = false;
            _searchService.CancelCurrentSearching();
            ResultIcon.Source = null;
            InputTextBox.Clear();
            Hide();
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
                Height = _inputHieght;
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
                Height = _inputHieght;
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
            Height = _windowHieght;
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
