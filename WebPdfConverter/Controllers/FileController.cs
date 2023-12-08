using Microsoft.AspNetCore.Mvc;
using SQLiteWebPdfConverterLib;
using WebPdfConverter.Models;
using WebPdfConverterCommonLib.DTO;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Controllers
{
    /// <summary>
    /// Controller that handles file operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly ISettings settings;
        readonly IConvJobScheduler scheduler;

        public FileController(ILogger<FileController> logger, ISettings settings, IConvJobScheduler scheduler)
        {
            _logger = logger;
            this.settings = settings;
            this.scheduler = scheduler;
        }

        /// <summary>
        /// Returns list of files
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetFiles")]        
        public async Task<ContentResult> Get()
        {
            IEnumerable<FileTR> files;
            using (var context = new DataContext(settings))
            {
                files = await context.Files.SelectAsync();
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(files), "application/json");
        }

        /// <summary>
        /// Returns particular file
        /// </summary>
        /// <param name="fileName">file to return</param>
        /// <returns></returns>
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var path = $"{settings.Current.UploadPath}\\{fileName}";
            if(!System.IO.File.Exists(path))
                return NotFound();

            string ext = Path.GetExtension(fileName);

            if(ext == ".html")
                return PhysicalFile(path, "text/html");
            if (ext == ".htm")
                return PhysicalFile(path, "text/html");
            else if(ext == ".pdf")
                return PhysicalFile(path, "application/pdf");

            return NotFound();
        }

        /// <summary>
        /// Upload file to server
        /// </summary>
        /// <param name="fileDetails"></param>
        /// <returns></returns>
        [HttpPost(Name = "PostMultipleFile")]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = Int64.MaxValue)]
        [RequestSizeLimit(Int64.MaxValue)]
        public async Task<ActionResult> PostMultiFileAsync(List<IFormFile> fileDetails)
        {
            var files = HttpContext.Request.Form.Files;

            if (files is null || !files.Any())
                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(false), "application/json");

            var fileManager = new FileManager();

            foreach (IFormFile file in files)
            {
                var result = await fileManager.UploadFileAsync(file, settings.Current.UploadPath);
                if(result)
                {
                    var fileDto = new FileTR { Name = file.FileName, Status = FileStatus.Uploaded, Timestamp = DateTime.Now };
                    using (var context = new DataContext(settings))
                    {
                        if(await context.Files.InsertAsync(fileDto))
                            scheduler.AddJob(fileDto);
                    }                    
                }
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(true), "application/json");
        }

        /// <summary>
        /// Delete file from server
        /// </summary>
        /// <param name="fileName">File to delete</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{fileName}")]
        public async Task<ActionResult> Delete(string fileName)
        {
            var fileManager = new FileManager();

            fileManager.Delete(fileName.Replace(Path.GetExtension(fileName), ".pdf"), settings.Current.UploadPath);
            
            var result = fileManager.Delete(fileName, settings.Current.UploadPath);
            if (result)
            {
                using (var context = new DataContext(settings))
                {
                    if(await context.Files.DeleteAsync(fileName))
                        return Content(Newtonsoft.Json.JsonConvert.SerializeObject(true), "application/json");
                }
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(false), "application/json");
        }
    }
}
