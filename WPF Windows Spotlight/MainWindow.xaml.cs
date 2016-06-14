using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Windows_Spotlight.Foundation;
using WPF_Windows_Spotlight.Foundation.ItemType;
using ContextMenu = System.Windows.Forms.ContextMenu;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Forms.MenuItem;

namespace WPF_Windows_Spotlight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Adapter _adapter;
        private LowLevelKeyboardListener _listener;
        private string[] _hotKeyForHide = new string[] { "Escape" };
        private string[] _hotKeyForOpen = new string[] { "LeftCtrl", "Space" };
        private int _hideKeyPointer = 0;
        private int _openKeyPointer = 0;
        private int _windowHieght = 420;
        private int _inputHieght = 70;
        private int _hasResult;
        private NotifyIcon _notifyIcon;
        
        public MainWindow()
        {
            InitializeComponent();
            _adapter = new Adapter();
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_adapter.QueryList);
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            QueryList.ItemsSource = collectionView;
            CenterWindowOnScreen();
            Height = _inputHieght;
            _adapter.UpdateContentHandler += SearchOver;
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += OpenWindow;
            InitNotifyIcon();

            // 在任務列不會出現
            this.ShowInTaskbar = false;
            
            _listener.HookKeyboard();
        }

        // icon 出現在 state bar（右下角那塊）
        private void InitNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = Properties.Resources.icon;
            _notifyIcon.Visible = true;
            
            // Create NotifyIcon Menu (右鍵會出現的東東)
            ContextMenu notifyMenu = new ContextMenu();
            MenuItem notifyIconMenuItem = new MenuItem();
            notifyIconMenuItem.Index = 0;
            notifyIconMenuItem.Text = "結束(E&xit)";
            notifyIconMenuItem.Click += new EventHandler(Exit);
            notifyMenu.MenuItems.Add(notifyIconMenuItem);
            _notifyIcon.ContextMenu = notifyMenu;
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
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
                ResultIcon.Source = null;
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

        private void OpenWindow(object sender, KeyPressedArgs e)
        {
            Console.WriteLine(e.KeyPressed.ToString());
            if (e.KeyPressed.ToString() == _hotKeyForOpen[_openKeyPointer])
                _openKeyPointer++;
            else
                _openKeyPointer = 0;

            if (_openKeyPointer == _hotKeyForOpen.Length)
            {
                this.Show();
                InputTextBox.Text = "";
                _adapter.QueryList.Clear();
                _openKeyPointer = 0;
            }
        }

        // 關閉程式時將keyboard hook解除
        private void ClosedWindow(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _listener.UnHookKeyboard();
        }

        private void LostFocusWindow(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus == null)
            {
                this.Hide();
                //ShowBalloonTip
            }
                
        }

        // 將視窗設定在螢幕中央
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        // 點擊item時開啟檔案
        private void ClickListViewItem(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item != null && item.IsSelected)
            {
                Item selectedItem = _adapter.QueryList[QueryList.SelectedIndex];
                selectedItem.Open();
            }
        }

        private void SearchOver()
        {
            if (_adapter.SelectedIndex < 0)
            {
                if (--_hasResult == 0)
                {
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
                    QueryList.ScrollIntoView(item);
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
                        item.Open();
                    }
                    break;
                default:
                    break;
            }
        }

    }

}
