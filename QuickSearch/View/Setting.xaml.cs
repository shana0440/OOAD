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
using System.Windows.Shapes;

namespace QuickSearch.View
{
    /// <summary>
    /// Setting.xaml 的互動邏輯
    /// </summary>
    public partial class Setting : Window
    {
        private string _theme;
        private HashSet<System.Windows.Forms.Keys> _hotkey = new HashSet<System.Windows.Forms.Keys>();
        Config _config;
        private bool _isSettingHotKey = false;

        public Setting(Config config)
        {
            InitializeComponent();
            _config = config;
        }

        private void PreviewSetHotKey(object sender, KeyEventArgs e)
        {
            if (!_isSettingHotKey)
            {
                _hotkey.Clear();
            }
            _isSettingHotKey = true;
            WinSpaceCheckBox.IsChecked = false;
            var key = e.Key == Key.System
                    ? System.Windows.Forms.Keys.Alt
                    : (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(e.Key);
            _hotkey.Add(key);
            HotKeyTextBox.Text = String.Join(" + ", _hotkey);
            e.Handled = true;
        }

        private void EndSetHotKey(object sender, KeyEventArgs e)
        {
            _isSettingHotKey = false;
        }

        private void UseWinSpace(object sender, RoutedEventArgs e)
        {
            if (HotKeyTextBox != null)
            {
                _hotkey.Add(System.Windows.Forms.Keys.LWin);
                _hotkey.Add(System.Windows.Forms.Keys.Space);
                HotKeyTextBox.Text = "";
            }
        }

        private void Apply(object sender, RoutedEventArgs e)
        {
            _config.SetHotKey(_hotkey);
            _config.SetTheme(_theme);
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeTheme(object sender, SelectionChangedEventArgs e)
        {
            _theme = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content as String;
        }

    }
}
