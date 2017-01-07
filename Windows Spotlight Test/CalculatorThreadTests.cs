using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class CalculatorThreadTests
    {

        PrivateObject _obj;

        [TestInitialize]
        public void Initialize()
        {
            _obj = new PrivateObject(new CalculatorThread());
        }

        [TestMethod]
        public void TestTransformMathOperators()
        {
            object[] param = { "sin(30) + sqrt(2, 3) + sqrt(2, 3)" };
            var expression = _obj.Invoke("TransformMathOperators", param);
            Assert.AreEqual("Math.Sin(30)+Math.Sqrt(2,3)+Math.Sqrt(2,3)", expression);
        }
    }
}
