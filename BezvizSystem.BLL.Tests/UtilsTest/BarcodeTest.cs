using System;
using BezvizSystem.BLL.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BezvizSystem.BLL.Tests.UtilsTest
{
    [TestClass]
    public class BarcodeTest
    {
        [TestMethod]
        public void GetText_Test()
        {
            Barcode barcode = new Barcode();
            string result1 = barcode.GetText(134);
            string result2 = barcode.GetText(1234567);

            Assert.AreEqual("BRESTVISAFREE 1234567", result2);
        }

        [TestMethod]
        public void GetText_ChildClass_Test()
        {
            Barcode barcode = new BarcodeChild();
            string result = barcode.GetText(134);

            Assert.AreEqual("Hello", result);
        }
    }
}
