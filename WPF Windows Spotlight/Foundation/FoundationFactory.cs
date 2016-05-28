using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FoundationFactory
    {
        private List<IFoundation> _foundations;

        public FoundationFactory()
        {
            _foundations = new List<IFoundation>();
            _foundations.Add(new Calculator());
            _foundations.Add(new FileSystem());
            _foundations.Add(new Translator());
            _foundations.Add(new SearchEngine());
            _foundations.Add(new Exchange());
        }

        public List<IFoundation> GetFoundations()
        {
            return _foundations;
        }
    }
}
