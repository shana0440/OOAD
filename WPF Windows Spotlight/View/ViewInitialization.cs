using System;
using System.Windows;
using System.Windows.Forms;

namespace WPF_Windows_Spotlight.View
{
    class ViewInitialization
    {
        Window _view;
        NotifyIcon _notifyIcon;
        LowLevelKeyboardListener _keyboardListener;
        string[] _hotKeyForHide = new string[] { "Escape" };
        string[] _hotKeyForOpen = new string[] { "LeftCtrl", "Space" };
        int _hideKeyPointer = 0;
        int _openKeyPointer = 0;


        public ViewInitialization(Window view)
        {
            _view = view;
        }

        public void Init()
        {
            InitStateBar();
            MakeWindowOnCenterOfScreen();
            HookKeyboard();
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
            _keyboardListener.UnHookKeyboard();
            _view.Close();
        }

        void MakeWindowOnCenterOfScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = _view.Width;
            double windowHeight = _view.Height;
            _view.Left = (screenWidth / 2) - (windowWidth / 2);
            _view.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        void HookKeyboard()
        {
            _keyboardListener = new LowLevelKeyboardListener();
            _keyboardListener.HookKeyboard();
        }

        void HideOfTaskBar()
        {
            // 在任務列(alt + tab)不會出現
            _view.ShowInTaskbar = false;
        }

        public void SetKeyboardEvent(EventHandler<KeyPressedArgs> handler)
        {
            _keyboardListener.OnKeyPressed += handler;
        }
    }
}
