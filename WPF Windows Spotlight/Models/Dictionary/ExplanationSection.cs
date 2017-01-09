using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.Dictionary
{
    public class ExplanationSection
    {
        public string PartOfSpeech = "";
        public List<Explanation> Interpretations = new List<Explanation>();
    }
}
