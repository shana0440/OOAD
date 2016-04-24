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
            _calculator.Expression = "1+2*2";
            Assert.AreEqual(5, _calculator.getResult());
        }

        [TestMethod]
        public void TestErrorCalculater()
        {
            _calculator.Expression = "1+2";
            Assert.AreEqual(3, _calculator.getResult());
            _calculator.Expression = "1+2+";
            Assert.AreEqual(3, _calculator.getResult());
        }

    }
}
