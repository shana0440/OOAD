using System;
using System.Windows;
using System.Windows.Forms;

namespace WPF_Windows_Spotlight.View
{
    class ViewInitialization
    {
        Window _view;
        NotifyIcon _notifyIcon;

        public ViewInitialization(Window view)
        {
            _view = view;
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
            MenuItem notifyIconMenuItem = new MenuItem();
            notifyIconMenuItem.Index = 0;
            notifyIconMenuItem.Text = "結束(E&xit)";
            notifyIconMenuItem.Click += new EventHandler(CloseApp);
            notifyMenu.MenuItems.Add(notifyIconMenuItem);
            _notifyIcon.ContextMenu = notifyMenu;
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
