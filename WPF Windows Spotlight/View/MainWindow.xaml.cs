using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF_Windows_Spotlight.Foundation.ItemType;
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
        private int _hideKeyPointer = 0;
        private int _openKeyPointer = 0;
        private int _windowHieght = 420;
        private int _inputHieght = 70;
        private int _hasResult;
        private LoadingCircle _rotate = new LoadingCircle { Angle = 0 };
        ViewInitialization _viewInitialization;
        
        public MainWindow()
        {
            InitializeComponent();
            _adapter = new Adapter();
            
            _viewInitialization = new ViewInitialization(this);
            _viewInitialization.Init();

            Render();

            _adapter.UpdateContentHandler += SearchOver;
        }

        public void Render()
        {
            InitViewHeight();
            InitResultsListSource();
            InitLoadingCircle();
        }

        void InitViewHeight()
        {
            Height = _inputHieght;
        }

        void InitResultsListSource()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_adapter.QueryList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            QueryList.ItemsSource = collectionView;
        }

        private void InitLoadingCircle()
        {
            DataContext = _rotate;
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
                ResultIcon.Source = _rotate.SearchImage;
                InputTextBoxWatermark.Text = "";
                InputTextBoxWatermark.HorizontalAlignment = HorizontalAlignment.Left;
                _adapter.Search(InputTextBox.Text);
                _hasResult = _adapter.GetWrokerCount();
            }
        }

        private void HideWindow(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == _hotKeyForHide[_hideKeyPointer])
                _hideKeyPointer++;
            else
                _hideKeyPointer = 0;

            if (_hideKeyPointer == _hotKeyForHide.Length)
            {
                this.Hide();
                _hideKeyPointer = 0;
            }
        }
        
        // 關閉程式時將keyboard hook解除
        private void ClosedWindow(object sender, EventArgs e)
        {
            //_notifyIcon.Visible = false;
            //_listener.UnHookKeyboard();
        }

        private void LostFocusWindow(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus == null)
            {
                this.Hide();
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

    }
}
