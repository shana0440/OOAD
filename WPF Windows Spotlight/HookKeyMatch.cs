using FMUtils.KeyboardHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPF_Windows_Spotlight
{
    public class HookKeyMatch
    {
        Hook _keyboardHook;
        HashSet<Keys> _pressKeys = new HashSet<Keys>();

        public HookKeyMatch()
        {
            _keyboardHook = new Hook("Global Action Hook");
            _keyboardHook.KeyDownEvent += StorePressKey;
            _keyboardHook.KeyUpEvent += ReleasePressKey;
        }

        void StorePressKey(KeyboardHookEventArgs e)
        {
            if (e.Key != Keys.None)
            {
                _pressKeys.Add(e.Key);
            }

            if (e.isLCtrlPressed)
            {
                _pressKeys.Add(Keys.Control);
            }
        }

        void ReleasePressKey(KeyboardHookEventArgs e)
        {
            if (e.Key != Keys.None)
            {
                _pressKeys.Remove(e.Key);
            }

            if (e.isLCtrlPressed)
            {
                _pressKeys.Remove(Keys.Control);
            }
        }

        public bool MatchKey(HashSet<Keys> hotKeys)
        {
            if (hotKeys.Count == _pressKeys.Count)
            {
                return hotKeys.SequenceEqual(_pressKeys);
            }
            return false;
        }

        public void PlusKeyDownEvent(Hook.KeyDownEventDelegate handler)
        {
            _keyboardHook.KeyDownEvent += handler;
        }

    }
}
