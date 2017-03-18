using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.Calculator;

namespace QuickSearchTest
{
    [TestClass]
    public class CalculatorTests
    {

        PrivateObject _obj;

        [TestInitialize]
        public void Initialize()
        {
            _obj = new PrivateObject(new Calculator());
        }

        [TestMethod]
        public void TestExecute()
        {
            object[] param = { "2 * 1 + sqrt(100) + pow(20, 2)" };
            var answer = _obj.Invoke("Execute", param);
            Assert.AreEqual("412", answer);
        }

        [TestMethod]
        [ExpectedException(typeof(ExecutionEngineException), "算式有誤")]
        public void TestExecuteError()
        {
            object[] param = { "2 * 1 + sqrt(100, 2) + pow(20, 2)" };
            var answer = _obj.Invoke("Execute", param);
            Assert.Fail();
        }

        [TestMethod]
        public void TestTransformMathOperators()
        {
            object[] param = { "sin(30) + sqrt(2, 3) + sqrt(2, 3)" };
            var expression = _obj.Invoke("TransformMathOperators", param);
            Assert.AreEqual("Math.Sin(30)+Math.Sqrt(2,3)+Math.Sqrt(2,3)", expression);
        }

        [TestMethod]
        public void TestClearAllSpace()
        {
            object[] param = { "1 + 2 + 3" };
            var expression = _obj.Invoke("ClearAllSpace", param);
            Assert.AreEqual("1+2+3", expression);
        }

        [TestMethod]
        public void TestFirstCharToUpper()
        {
            object[] param = { "sin" };
            var txt = _obj.Invoke("FirstCharToUpper", param);
            Assert.AreEqual("Sin", txt);
        }
    }
}
