using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FoundationFactory
    {
        public IFoundation CreateFoundation(string foundationName, string arg)
        {
            IFoundation foundation;
            switch (foundationName)
	        {
                case "calculator":
                    foundation = new Calculator(arg);
                    break;
		        default:
                    foundation = null;
                    break;
	        }
            return foundation;
        }
    }
}
