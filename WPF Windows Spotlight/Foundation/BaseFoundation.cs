using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Foundation
{
    public abstract class BaseFoundation : IFoundation
    {
        private readonly string _name;

        protected BaseFoundation(string name)
        {
            _name = name;
        }
        
        public string Name
        {
            get { return _name; }
        }

        public abstract void DoWork(object sender, DoWorkEventArgs e);
        public abstract void SetKeyword(string keyword);

    }
}
