using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Windows_Spotlight.Foundation;
using WPF_Windows_Spotlight.Foundation.ItemType;

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
        private int _windowHieght = 400;
        private int _inputHieght = 50;
        
        public MainWindow()
        {
            _adapter = new Adapter();
            InitializeComponent();
            QueryList.ItemsSource = _adapter.QueryList;
            CenterWindowOnScreen();
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += OpenWindow;
            
            _listener.HookKeyboard();
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            if (Input.Text.Trim() == "")
            {
                Height = _inputHieght;
            }
            else
            {
                _adapter.Search(Input.Text);
                Height = _windowHieght;
            }
        }

        private void SelectItem(object sender, SelectionChangedEventArgs e)
        {
            //ListBox list = (ListBox)sender;
            //if (list.SelectedIndex < _adapter.QueryList.Count && list.SelectedIndex != -1)
            //{
            //    Item selectedItem = _adapter.QueryList[list.SelectedIndex];
            //    selectedItem.Open();
            //}
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
                Input.Text = "";
                _adapter.QueryList.Clear();
                this.Focus();
                _openKeyPointer = 0;
            }
        }

        // 關閉程式時將keyboard hook解除
        private void ClosedWindow(object sender, EventArgs e)
        {
            _listener.UnHookKeyboard();
        }

        private void LostFocusWindow(object sender, KeyboardFocusChangedEventArgs e)
        {
            //if (e.NewFocus == null)
            //    this.Hide();
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
                ShowFileDetail((FileItem)selectedItem);
            }
        }

        private void ShowFileDetail(FileItem file)
        {
            ContentView.Children.Clear();
            Image img = new Image();
            img.Source = file.Icon;
            Thickness imgMargin = new Thickness(150, 50, 150, 10);
            img.Margin = imgMargin;
            img.Width = 100;
            ContentView.Children.Add(img);

            Label title = new Label();
            title.Content = file.Title;
            title.FontSize = 24;
            title.HorizontalContentAlignment = HorizontalAlignment.Center;
            ContentView.Children.Add(title);

            Border hr = new Border();
            Color hrColor = (Color)ColorConverter.ConvertFromString("#FFD7D7D7");
            hr.Height = 1;
            hr.BorderBrush = new SolidColorBrush(hrColor);
            hr.BorderThickness = new Thickness(0, 1, 0, 0);
            hr.Width = ContentView.Width - 30;
            hr.Margin= new Thickness(15, 40, 15, 15);
            hr.VerticalAlignment = VerticalAlignment.Top;
            hr.HorizontalAlignment = HorizontalAlignment.Left;
            ContentView.Children.Add(hr);

            List<KeyValuePair<string, string>> propertys = file.GetProperty();
            foreach (KeyValuePair<string, string> property in propertys)
            {
                WrapPanel wrap = new WrapPanel();
                Label name = new Label();
                name.Content = property.Key;
                Label value = new Label();
                value.Content = property.Value;
                name.Width = value.Width = ContentView.Width / 2;
                name.HorizontalContentAlignment = HorizontalAlignment.Right;
                value.HorizontalContentAlignment = HorizontalAlignment.Left;
                name.Foreground = Brushes.Gray;

                wrap.Children.Add(name);
                wrap.Children.Add(value);
                ContentView.Children.Add(wrap);
            }
        }
    }

}
