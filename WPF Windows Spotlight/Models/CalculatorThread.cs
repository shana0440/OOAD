using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models
{
    public class CalculatorThread
    {

        string[] _mathOperators = { "sqrt", "sin", "cos", "tan", "log", "exp", "floor", "abs", "pow" };

        static void Execute()
        {
        }

        string TransformMathOperators(string expression)
        {
            expression = clearAllSpace(expression.ToLower());
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

        string clearAllSpace(string expression)
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
