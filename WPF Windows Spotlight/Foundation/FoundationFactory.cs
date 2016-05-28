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
            _foundations.Add(new Calculator("計算機"));
            _foundations.Add(new Exchange("匯率換算"));
            _foundations.Add(new Translator("翻譯"));
            _foundations.Add(new SearchEngine("網頁"));
            _foundations.Add(new FileSystem("檔案或資料夾"));
        }

        public List<IFoundation> GetFoundations()
        {
            return _foundations;
        }
    }
}
