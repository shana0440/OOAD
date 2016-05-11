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
        private string _expression;
        public AnswerItem(string title, string exprssion)
            : base(title)
        {
            _expression = exprssion;
        }

        public string Expression
        {
            get { return _expression; }
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
