using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class AnswerItem : Item
    {
        public AnswerItem(string title) : base(title)
        {

        }

        public override void Open()
        {
            if (_content != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(_content);
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine("無法開啟");
                }
            }
        }
    }
}
