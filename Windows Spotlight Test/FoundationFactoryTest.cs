using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FoundationFactoryTest
    {
        FoundationFactory _factory;

        [TestInitialize]
        public void TestInit()
        {
            _factory = new FoundationFactory();
        }
    }
}
