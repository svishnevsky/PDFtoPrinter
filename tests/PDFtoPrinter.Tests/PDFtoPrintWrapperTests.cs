using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PDFtoPrinter.Tests
{
    [TestClass]
    public class PDFtoPrintWrapperTests
    {
        [DataTestMethod]
        [DataRow(-15)]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidConcurrencyValue(int maxCuncurrency)
        {
            var wrapper = new PDFtoPrintWrapper(maxCuncurrency);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(99)]
        [DataRow(int.MaxValue)]
        public void ValidConcurrencyValue(int maxCuncurrency)
        {
            var wrapper = new PDFtoPrintWrapper(maxCuncurrency);
            Assert.IsNotNull(wrapper);
        }
    }
}