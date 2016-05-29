using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Calculator : IFoundation
    {
        private string _expression;
        private string _lastResult;
        private Bitmap _icon;
        private string _orignalExp;
        private readonly string _name;

        public Calculator(string name = "")
        {
            _icon = (Bitmap)WPF_Windows_Spotlight.Properties.Resources.calculator_icon;
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Expression
        {
            set 
            {
                _expression = value.Replace(" ", "").ToLower(); 
            } 
        }

        public void SetKeyword(string keyword)
        {
            _expression = keyword.Replace(" ", "").ToLower();
        }

        public string TransToFloat (string inputExpression)
        {
            int isFloat = 0;
            int isNumber = 0;
            int end = inputExpression.Length;
            int i = 0;
            while (i < end)
            {
                if (inputExpression[i] >= '0' && inputExpression[i] <= '9')
                {
                    isNumber = 1;
                }
                else if (inputExpression[i] == '.')
                {
                    isFloat = 1;
                }
                else
                {
                    if (isNumber == 1 && isFloat == 0)
                    {
                        inputExpression = inputExpression.Insert(i, ".0");
                        i = i+2;
                        end = end + 2;
                    }
                    isNumber = 0;
                    isFloat = 0;
                }
                i++;
            }

            return inputExpression;
        }

        public string TransformPow (string inputExpression)
        {
            string[] resultString = Regex.Split(inputExpression, @"\^");
            if (resultString[0] != inputExpression)
            {
                string splitLeft = resultString[0];
                string splitRight = resultString[1];
                int leftBracketsCount = 0;
                int leftPosition = 0;
                int rightBracketsCount = 0;
                int rightPosition = 0;
                string powLeft = "";
                string powRight = "";
                
                //指數符號左邊 尋找括號
                if (splitLeft[splitLeft.Length - 1] == ')')
                {
                    for (int i = splitLeft.Length - 1; i >= 0; i--)
                    {
                        if (splitLeft[i] == '(')
                        {
                            leftBracketsCount--;
                        }
                        else if (splitLeft[i] == ')')
                        {
                            leftBracketsCount++;
                        }

                        if (leftBracketsCount == 0)
                        {
                            leftPosition = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = splitLeft.Length - 1; i >= 0; i--)
                    {
                        if (splitLeft[i] == '+' || splitLeft[i] == '-' || splitLeft[i] == '*' || splitLeft[i] == '/' || splitLeft[i] == '^' || splitLeft[i] == '(')
                        {
                            leftPosition = i + 1;
                            break;
                        }
                    }
                }
                //右邊尋找括號
                if (splitRight[0] == '(')
                {
                    for (int i = 0; i < splitRight.Length; i++)
                    {
                        if (splitRight[i] == ')')
                        {
                            rightBracketsCount--;
                        }
                        else if (splitRight[i] == '(')
                        {
                            rightBracketsCount++;
                        }

                        if (rightBracketsCount == 0)
                        {
                            rightPosition = i + 1;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < splitRight.Length; i++)
                    {
                        if (splitRight[i] == '+' || splitRight[i] == '-' || splitRight[i] == '*' || splitRight[i] == '/' || splitRight[i] == '^' || splitRight[i] == ')')
                        {
                            rightPosition = i;
                            break;
                        }
                        rightPosition = i + 1;
                    }
                    
                }
                //轉換成 pow
                for (int i = leftPosition; i < splitLeft.Length; i++)
                {
                    powLeft = powLeft + splitLeft[i];
                }

                for (int i = 0; i < rightPosition; i++)
                {
                    powRight = powRight + splitRight[i];
                }

                inputExpression = "";
                for (int i = 0; i < leftPosition; i++)
                {
                    inputExpression = inputExpression + splitLeft[i];
                }
                inputExpression = inputExpression + "Math.Pow(" + powLeft + "," + powRight + ")";
                for (int i = rightPosition; i < splitRight.Length; i++)
                {
                    inputExpression = inputExpression + splitRight[i];
                }
            }

            return inputExpression;
        }

        public string ReplaceSqrt (string inputExpression)
        {
            return inputExpression.Replace("sqrt", "Math.Sqrt");
        }

        public string GetResult()
        {
            _orignalExp = _expression;
            try
            {
                int expLength = _expression.Length;

                if (_orignalExp[expLength-1] >= '0' &&  _orignalExp[expLength-1] <= '9')
                {
                    _orignalExp = _orignalExp + "=";
                }

                _expression = TransToFloat(_expression);
                _expression = ReplaceSqrt(_expression);
                _expression = TransformPow(_expression);
                string result = Eval(_expression).ToString();
                _lastResult = result;
                return _lastResult;
            }
            catch (Exception e)
            {
                string tempExp = _expression;
                int expLength = tempExp.Length;
                if (_expression[expLength - 1] == '+' || _expression[expLength - 1] == '-')
                {
                    tempExp = tempExp + '0';
                }else if (_expression[expLength - 1] == '*' || _expression[expLength - 1] == '/'  || _expression[expLength - 1] == '^')
                {
                    tempExp = tempExp + '1';
                }
              
                try
                {   
                    string result = Eval(tempExp).ToString();
                    return _lastResult;
                }
                catch
                {
                    return null;
                }
                
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var answer = GetResult();
            var item = new AnswerItem(answer, _orignalExp, _name);
            double n;
            var bg = sender as BackgroundWorker;
            if (bg.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            var list = new List<Item>();
            if (double.TryParse(answer, out n) && answer != null)
            {
                item.SetIcon(_icon);
                list.Add(item);
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, list);
        }

        private static object Eval(string sCSCode)
        {
            CSharpCodeProvider c = new CSharpCodeProvider();
            ICodeCompiler icc = c.CreateCompiler();
            CompilerParameters cp = new CompilerParameters();

            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("system.windows.forms.dll");
            cp.ReferencedAssemblies.Add("system.drawing.dll");

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            StringBuilder sb = new StringBuilder("");
            sb.Append("using System;\n");
            sb.Append("using System.Xml;\n");
            sb.Append("using System.Data;\n");
            sb.Append("using System.Data.SqlClient;\n");
            sb.Append("using System.Windows.Forms;\n");
            sb.Append("using System.Drawing;\n");

            sb.Append("namespace CSCodeEvaler{ \n");
            sb.Append("public class CSCodeEvaler{ \n");
            sb.Append("public object EvalCode(){\n");
            sb.Append("return " + sCSCode + "; \n");
            sb.Append("} \n");
            sb.Append("} \n");
            sb.Append("}\n");

            CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
            if (cr.Errors.Count > 0)
            {
                Console.WriteLine("ERROR: " + cr.Errors[0].ErrorText,
                   "Error evaluating cs code");
                return null;
            }

            System.Reflection.Assembly a = cr.CompiledAssembly;
            object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("EvalCode");

            object s = mi.Invoke(o, null);
            return s;
        }
    }
}
