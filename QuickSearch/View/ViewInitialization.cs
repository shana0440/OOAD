using System;
using System.Windows;
using System.Windows.Forms;

namespace QuickSearch.View
{
    class ViewInitialization
    {
        Window _view;
        NotifyIcon _notifyIcon;
        Config _config;

        public ViewInitialization(Window view, Config config)
        {
            _view = view;
            _config = config;
        }

        public void Init()
        {
            InitStateBar();
            MakeWindowToFullScreen();
            HideOfTaskBar();
        }

        void InitStateBar()
        {
            InitStateBarIcon();
            InitStateBarMenu();
        }

        void InitStateBarIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = Properties.Resources.icon;
            _notifyIcon.Visible = true;
        }

        void InitStateBarMenu()
        {
            ContextMenu notifyMenu = new ContextMenu();

            MenuItem setting = new MenuItem();
            setting.Index = 0;
            setting.Text = "設定";
            setting.Click += new EventHandler(OpenSettingWindow);
            notifyMenu.MenuItems.Add(setting);

            MenuItem exit = new MenuItem();
            exit.Index = 1;
            exit.Text = "結束(E&xit)";
            exit.Click += new EventHandler(CloseApp);
            notifyMenu.MenuItems.Add(exit);

            _notifyIcon.ContextMenu = notifyMenu;
        }

        private void OpenSettingWindow(object sender, EventArgs e)
        {
            var window = new Setting(_config);
            window.Show();
        }

        void CloseApp(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _view.Close();
        }

        void MakeWindowToFullScreen()
        {
            _view.Width = SystemParameters.PrimaryScreenWidth;
            _view.Height = SystemParameters.PrimaryScreenHeight;
            _view.Left = 0;
            _view.Top = 0;
        }

        void HideOfTaskBar()
        {
            // 在任務列(alt + tab)不會出現
            _view.ShowInTaskbar = false;
        }

    }
}
