using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.Calculator
{
    public class Calculator
    {
        string[] _mathOperators = { "sqrt", "sin", "cos", "tan", "log", "exp", "floor", "abs", "pow" };

        public string Execute(string expression)
        {
            expression = TransformMathOperators(expression);
            try
            {
                var result = Eval.Execute(expression);
                return result.ToString();
            }
            catch (Exception e)
            {
                throw new ExecutionEngineException("算式有誤");
            }

        }

        string TransformMathOperators(string expression)
        {
            expression = ClearAllSpace(expression.ToLower());
            int index = 0;
            while (index < _mathOperators.Length)
            {
                var targetOperator = _mathOperators[index];
                var transformedOperator = String.Format("Math.{0}", FirstCharToUpper(targetOperator));
                expression = expression.Replace(targetOperator, transformedOperator);
                index++;
            }
            return expression;
        }

        string ClearAllSpace(string expression)
        {
            expression = expression.Replace(" ", string.Empty);
            return expression;
        }

        string FirstCharToUpper(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
