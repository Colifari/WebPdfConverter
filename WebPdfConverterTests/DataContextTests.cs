using Microsoft.AspNetCore.Http;
using SQLiteWebPdfConverterLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverterTests
{
    [TestClass]
    public class DataContextTests
    {
        IDataContext dataContext;
        ISettings settings;
        string dbPath;

        public DataContextTests()
        {
            settings = new Settings
            {
                Current = new SettingsModel()
                {
                    UploadPath = "~\\Content",
                    ConnectionString = "data source=~/Content/database.db;",
                    ChromePath = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
                    PdfCreationTimeoutSec = 60
                }
            };

            ((Settings)settings).replaceHomeDir();

            dbPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\database.db";
        }

        [TestInitialize]
        public void Initialize()
        {
            dataContext = new DataContext(settings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            dataContext.DropDatabase();
        }

        [TestMethod]
        public async Task DbCreation()
        {
            Assert.IsTrue(File.Exists(dbPath));

            var info = new FileInfo(dbPath);

            Assert.AreNotEqual(0, info.Length);
        }
    }
}
