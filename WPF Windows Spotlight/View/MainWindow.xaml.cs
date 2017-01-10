using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        private string[] _hotKeyForHide = new string[] { "Escape" };
        private string[] _hotKeyForOpen = new string[] { "LeftCtrl", "Space" };
        private int _openKeyPointer = 0;
        private int _windowHieght = 420;
        private int _inputHieght = 70;
        private LoadingCircle _rotate = new LoadingCircle { Angle = 0 };
        ViewInitialization _viewInitialization;
        private bool _windowVisibleState = false;
        SearchService _searchService = new SearchService();

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
            string watchedKeyPressed = _hotKeyForOpen[_openKeyPointer];
            _openKeyPointer = (keyPressed == watchedKeyPressed) ? _openKeyPointer + 1 : 0;

            if (_openKeyPointer == _hotKeyForOpen.Length)
            {
                _openKeyPointer = 0;
                if (_windowVisibleState)
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
            _windowVisibleState = true;
            FocusManager.SetFocusedElement(InputTextBox, InputTextBox);
            Show();
        }

        void HideWindow()
        {
            _windowVisibleState = false;
            _searchService.ResultList.Clear();
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
                InputTextBox.Text = "";
                InputTextBoxWatermark.Text = "Quick Search";
                InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                ResultIcon.Visibility = Visibility.Visible;
                _rotate.Start();
                ContentView.Children.Clear();
                ResultIcon.Source = BitmapToBitmapImage.Transform(Properties.Resources.loading);
                InputTextBoxWatermark.Text = "";

                _searchService.SubscribeSearchOverEvent(SearchOverEvent);
                _searchService.Search(InputTextBox.Text);
            }
        }

        void SearchOverEvent(ObservableCollection<IResultItem> items)
        {
            _rotate.Stop();
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
            ResultIcon.Source = item.Icon;
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
