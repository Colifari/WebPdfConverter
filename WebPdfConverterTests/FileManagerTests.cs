using Microsoft.AspNetCore.Http;
using System.IO;
using WebPdfConverter.Models;

namespace WebPdfConverterTests
{
    [TestClass]
    public class FileManagerTests
    {
        string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Content";
        FileManager manager = new FileManager();

        [TestMethod]
        public async Task UploadEmptyFile()
        {            
            var formFile = new FormFile(Stream.Null, 0, 0, "myfile.htm", "myfile.htm");            

            Assert.IsFalse(await manager.UploadFileAsync(formFile, path));
        }

        [TestMethod]
        public async Task UploadNonHtmlFile()
        {
            var formFile = new FormFile(Stream.Null, 0, 1024, "myphoto.jpg", "myphoto.jpg");

            Assert.IsFalse(await manager.UploadFileAsync(formFile, path));
        }

        [TestMethod]
        public async Task UploadHtmlFile()
        {
            var formFile = new FormFile(Stream.Null, 0, 1024, "myfile.html", "myfile.html");

            Assert.IsTrue(await manager.UploadFileAsync(formFile, path));

            Assert.IsTrue(File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\myfile.html"));
            manager.Delete("myfile.html", path);
        }

        [TestMethod]
        public async Task UploadHtmlFileWithWrongName()
        {
            var formFile = new FormFile(Stream.Null, 0, 1024, "my^%&*^Dfile.html", "\"my^%&*^Dfile.html");

            Assert.IsFalse(await manager.UploadFileAsync(formFile, path));

            Assert.IsFalse(File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Content\\my^%&*^Dfile.html"));
        }

        [TestMethod]
        public void DeleteFile()
        {
            Assert.IsTrue(manager.Delete("myfile.html", path));
        }

        [TestMethod]
        public async Task ReadFromNonExistFile()
        {
            Assert.IsTrue(await manager.ReadAllTextAsync("nonexistentfile.html") == string.Empty);
        }
    }
}