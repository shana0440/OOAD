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
            Assert.AreEqual("1", _calculator.GetResult());
            _calculator.Expression = "1+sqrt(4+5)-2";
            Assert.AreEqual("2", _calculator.GetResult());
            _calculator.Expression = "1^2";
            Assert.AreEqual("1", _calculator.GetResult());
            _calculator.Expression = "2^2";
            Assert.AreEqual("4", _calculator.GetResult());
            _calculator.Expression = "2+3^(1+1)";
            Assert.AreEqual("11", _calculator.GetResult());
            _calculator.Expression = "1.1+2+(1*2.2)";
            Assert.AreEqual("5.3", _calculator.GetResult());
            _calculator.Expression = "1.0+2.0-3.0*4.0/5.0";
            Assert.AreEqual("0.6", _calculator.GetResult());
            _calculator.Expression = "1.0+2-3*4/5+1.0";
            Assert.AreEqual("1.6", _calculator.GetResult());
            _calculator.Expression = "100+2*2";
            Assert.AreEqual("104", _calculator.GetResult());
        }

        [TestMethod]
        public void TestTransToFloat()
        {
            string testExp; 
            testExp = "sqrt(1^2)";
            Assert.AreEqual("sqrt(1.0^2.0)", _calculator.TransToFloat(testExp));
            testExp = "sqrt(1^2)";
            testExp = _calculator.TransformPow(testExp);
            Assert.AreEqual("sqrt(Math.Pow(1.0,2.0))", _calculator.TransToFloat(testExp));
        }

        [TestMethod]
        public void TestTransformPow()
        {
            string testExp;
            testExp = "sqrt(1^2)";
            testExp = _calculator.TransToFloat(testExp);
            Assert.AreEqual("sqrt(Math.Pow(1.0,2.0))", _calculator.TransformPow(testExp));
        }

        [TestMethod]
        public void TestErrorCalculater()
        {
            _calculator.Expression = "1+2";
            Assert.AreEqual("3", _calculator.GetResult());
            _calculator.Expression = "1+2+";
            Assert.AreEqual("3", _calculator.GetResult());
        }

        [TestMethod]
        public void TestReplaceSqrt ()
        {
            string testExp = "sqrt(1^2)";
            Assert.AreEqual("Math.Sqrt(1^2)", _calculator.ReplaceSqrt(testExp));
        }

    }
}
