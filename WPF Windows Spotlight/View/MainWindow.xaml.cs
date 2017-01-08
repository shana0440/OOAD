using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF_Windows_Spotlight.Controller;
using WPF_Windows_Spotlight.Foundation;
using WPF_Windows_Spotlight.Foundation.ItemType;
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
        private Adapter _adapter;
        private string[] _hotKeyForHide = new string[] { "Escape" };
        private string[] _hotKeyForOpen = new string[] { "LeftCtrl", "Space" };
        private int _openKeyPointer = 0;
        private int _windowHieght = 420;
        private int _inputHieght = 70;
        private int _hasResult;
        private LoadingCircle _rotate = new LoadingCircle { Angle = 0 };
        ViewInitialization _viewInitialization;
        private bool _windowVisibleState = false;
        SearchService _searchService = new SearchService();

        public MainWindow()
        {
            InitializeComponent();
            _adapter = new Adapter();
            
            _viewInitialization = new ViewInitialization(this);
            _viewInitialization.Init();
            _viewInitialization.SetKeyboardEvent(WatchKeyPressedOfWindowVisible);

            InitRender();

            _adapter.UpdateContentHandler += SearchOver;
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
            SearchIcon.Source = BitmapToBitmapImage.Transform((Bitmap)Properties.Resources.search);
        }

        void InitViewHeight()
        {
            Height = _inputHieght;
        }

        void InitResultsListSource()
        {
            //ICollectionView collectionView = CollectionViewSource.GetDefaultView(_adapter.QueryList);
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_searchService.ResultList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            QueryList.ItemsSource = collectionView;
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
            _adapter.QueryList.Clear();
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

        // 點擊item時開啟檔案
        private void ClickListViewItem(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null && item.IsSelected)
            {
                Item selectedItem = _adapter.QueryList[QueryList.SelectedIndex];
                this.Hide();
                selectedItem.Open();
            }
        }

        private void SearchOver()
        {
            _rotate.Stop();
            if (_adapter.SelectedIndex < 0)
            {
                if (--_hasResult == 0)
                {
                    ResultIcon.Source = null;
                    Height = _inputHieght;
                    ContentView.Children.Clear();
                    InputTextBoxWatermark.Text = "— No result";
                    InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
            else
            {
                if (_adapter.QueryList.Count > 0)
                {
                    Item item = _adapter.QueryList[_adapter.SelectedIndex];
                    // 不知道為啥要兩個 不用兩個他不會回到最上面
                    QueryList.ScrollIntoView(item);
                    QueryList.ScrollIntoView(QueryList.SelectedItem);
                    ShowDetail(item);
                }
            }
        }

        // 分配要用哪個ShowXXXXDetail來顯示Item的詳細資料
        private void ShowDetail(Item item)
        {
            if (item == null) return;
            ResultIcon.Source = item.Icon;
            Height = _windowHieght;
            item.GenerateContent(ContentView);
        }

        private void SelectItem(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _adapter.SelectItem(_adapter.SelectedIndex - 1);
                    break;
                case Key.Down:
                    _adapter.SelectItem(_adapter.SelectedIndex + 1);
                    break;
                case Key.Enter:
                    if (_adapter.QueryList.Count > 0)
                    {
                        var item = _adapter.QueryList[_adapter.SelectedIndex];
                        this.Hide();
                        item.Open();
                    }
                    break;
                default:
                    break;
            }
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            if (InputTextBox.Text.Trim() == "")
            {
                Height = _inputHieght;
                InputTextBox.Text = "";
            }
            else
            {
                _rotate.Start();
                ContentView.Children.Clear();
                ResultIcon.Source = BitmapToBitmapImage.Transform((Bitmap)Properties.Resources.loading);
                InputTextBoxWatermark.Text = "";
                InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Left;

                _searchService.Search(InputTextBox.Text);
                _searchService.SubscribeSearchOverEvent(SearchOverEvent);

                //_adapter.Search(InputTextBox.Text);
                //_hasResult = _adapter.GetWrokerCount();
            }
        }

        void SearchOverEvent(ObservableCollection<IResultItem> items)
        {
            _rotate.Stop();
            IResultItem item = _searchService.ResultList[0];
            // 不知道為啥要兩個 不用兩個他不會回到最上面
            QueryList.ScrollIntoView(item);
            QueryList.ScrollIntoView(QueryList.SelectedItem);
            ShowSelectdItemContent(item);
        }

        void ShowSelectdItemContent(IResultItem item)
        {
            if (item == null) return;
            ResultIcon.Source = item.Icon;
            Height = _windowHieght;
            item.GenerateContent(ContentView);
        }

    }
}
