using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FoundationFactory
    {
        private List<string> _foundations;

        public FoundationFactory()
        {
            _foundations = new List<string>();
            _foundations.Add("Calculator");
            _foundations.Add("FileSystem");
        }

        public IFoundation CreateFoundation(string foundationName, string arg)
        {
            IFoundation foundation;
            switch (foundationName)
	        {
                case "Calculator":
                    foundation = new Calculator(arg);
                    break;
                case "FileSystem":
                    foundation = new FileSystem(arg);
                    break;
		        default:
                    foundation = null;
                    break;
	        }
            return foundation;
        }

        public List<string> GetFoundations()
        {
            return _foundations;
        }
    }
}
