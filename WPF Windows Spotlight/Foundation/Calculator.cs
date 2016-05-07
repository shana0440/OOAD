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

namespace WPF_Windows_Spotlight.Foundation
{
    public class Calculator : IFoundation
    {
        private string _expression;
        private string _lastResult;

        public Calculator(string expression = "")
        {
            _expression = expression.ToLower() + ";";
        }

        public string Expression
        {
            set { _expression = value.ToLower() + ";"; }
        }

        public void SetKeyword(string keyword)
        {
            _expression = keyword;
        }

        public void TransToFloat ()
        {
            int isFloat = 0;
            int isNumber = 0;
            for (int i = 0; i < _expression.Length; i++)
            {
                if (_expression[i] >= '0' && _expression[i] <= '9')
                {
                    isNumber = 1;
                }
                else if (_expression[i] == '.')
                {
                    isFloat = 1;
                }
                else
                {
                    if (isNumber == 1 && isFloat == 0)
                    {
                        _expression = _expression.Insert(i, ".0");
                        i = i+2;
                    }
                    isNumber = 0;
                    isFloat = 0;
                }
            }

            Console.WriteLine(_expression);
        }

        public void TransformPow ()
        {
            string[] resultString = Regex.Split(_expression, @"\^");
            if (resultString[0] != _expression)
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
                        if (splitLeft[i] == '+' || splitLeft[i] == '-' || splitLeft[i] == '*' || splitLeft[i] == '/' || splitLeft[i] == '^')
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
                        if (splitRight[i] == '+' || splitRight[i] == '-' || splitRight[i] == '*' || splitRight[i] == '/' || splitRight[i] == '^')
                        {
                            rightPosition = i;
                            break;
                        }
                        rightPosition = i;
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

                _expression = "";
                for (int i = 0; i < leftPosition; i++)
                {
                    _expression = _expression + splitLeft[i];
                }
                _expression = _expression + "Math.Pow(" + powLeft + "," + powRight + ")";
                for (int i = rightPosition; i < splitRight.Length; i++)
                {
                    _expression = _expression + splitRight[i];
                }
            }
            
            Console.WriteLine(_expression);
        }

        public void ReplaceSqrt ()
        {
            _expression = _expression.Replace("sqrt", "Math.Sqrt");
            Console.WriteLine(_expression);
        }

        public string GetResult()
        {
            try
            {
                TransToFloat();
                ReplaceSqrt();
                TransformPow();
                string result = Eval(_expression).ToString();
                _lastResult = result;
                return _lastResult;
            }
            catch (Exception e)
            {
                return _lastResult;
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            Item item = new Item(GetResult());
            List<Item> list = new List<Item>();
            list.Add(item);
            e.Result = list;
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
