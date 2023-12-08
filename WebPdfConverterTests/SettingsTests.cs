using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverterTests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void TestHomeReplacement()
        {
            var settings = new Settings
            {
                Current = new SettingsModel()
                {
                    UploadPath = "~\\Content",
                    ConnectionString = "data source=~/Content/database.db;",
                    ChromePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
                    PdfCreationTimeoutSec = 60
                }
            };

            settings.replaceHomeDir();
            Assert.AreEqual(settings.Current.UploadPath, $"{AppDomain.CurrentDomain.BaseDirectory}\\Content");
            Assert.AreEqual(settings.Current.ConnectionString, $"data source={AppDomain.CurrentDomain.BaseDirectory}/Content/database.db;");
            Assert.AreEqual(settings.Current.ChromePath, "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe");
            Assert.AreEqual(settings.Current.PdfCreationTimeoutSec, 60);
        }
    }
}
