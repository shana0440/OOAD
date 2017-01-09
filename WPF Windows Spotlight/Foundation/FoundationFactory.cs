using System.Collections.Generic;
using System.Linq;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FoundationFactory
    {
        private List<IFoundation> _foundations = new List<IFoundation>();
        private Dictionary<string, List<Item>> _order = new Dictionary<string, List<Item>>();

        public FoundationFactory()
        {
            //_order.Add("網頁", new List<Item>());

            var keys = _order.Keys.ToArray();

            //_foundations.Add(new SearchEngine(keys[3]));
        }

        public List<IFoundation> GetFoundations()
        {
            return _foundations;
        }

        public Dictionary<string, List<Item>> Order
        {
            get { return _order; }
        }
    }
}
