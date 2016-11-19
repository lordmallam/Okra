using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Okra.Core;
using Okra.ActiveDirectory;
using Okra.File;
using Okra.Mail;
using Okra.Security;
using Okra.UI;


namespace OkraUnitTests
{
    [TestClass]
    public class UnitTestCore
    {
        [TestMethod]
        public void extractIntegerTest()
        {
            Core nCore = new Core();
            long v = nCore.extractInteger("Vol. 3452kls083nbn");
            Assert.AreEqual(3452083, v);
        }
    }
}
