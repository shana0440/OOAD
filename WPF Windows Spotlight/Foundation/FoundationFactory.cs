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
            _foundations.Add("Translator");
        }

        public IFoundation CreateFoundation(string foundationName, string arg = "")
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
                case "Translator":
                    foundation = new Translator(arg);
	                break;
		        default:
                    foundation = null;
                    break;
	        }
            return foundation;
        }

        public List<IFoundation> GetFoundations()
        {
            List<IFoundation> foundations = new List<IFoundation>();
            foreach (string foundationName in _foundations)
            {
                foundations.Add(CreateFoundation(foundationName));
            }
            return foundations;
        }
    }
}
