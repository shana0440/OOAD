using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class CalculatorTest
    {
        private Calculator _calculator;

        [TestInitialize]
        public void Init()
        {
            _calculator = new Calculator();
        }

        [TestMethod]
        public void TestCalculater()
        {
            _calculator.Expression = "1+sqrt(4)-2";
            _calculator.replaceSqrt();
            //_calculator.Expression = "(2+3)^(1+1)";
            //_calculator.transformWord("(2+3)^(1+1)");
            //_calculator.Expression = "1.1+2+(1*2.2)";
            //Assert.AreEqual(5.3, _calculator.getResult());
            //_calculator.Expression = "1+2-3*4/5";
            //Assert.AreEqual(0.6, _calculator.getResult());
            //_calculator.Expression = "100+2*2";
            //Assert.AreEqual(104, _calculator.getResult());
            _calculator.Expression = "1+2*2";
            Assert.AreEqual("5", _calculator.GetResult());
        }

        [TestMethod]
        public void TestErrorCalculater()
        {
            _calculator.Expression = "1+2";
            Assert.AreEqual("3", _calculator.GetResult());
            _calculator.Expression = "1+2+";
            Assert.AreEqual("3", _calculator.GetResult());
        }

    }
}
