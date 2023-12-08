
using SQLiteWebPdfConverterLib;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Models
{
    /// <summary>
    /// Handles file operations
    /// </summary>
    public class FileManager
    {

        /// <summary>
        /// Creates specified file
        /// </summary>
        /// <param name="file">file data from POST</param>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(IFormFile file, string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string ext = Path.GetExtension(file.FileName);
                if(!".htm .html".Contains(ext, StringComparison.CurrentCultureIgnoreCase))
                {
                    Journal.Error("FileManager.UploadFile(): User tried to upload non html file: {filename}", file.FileName);
                    return false;
                }

                if(file.Length == 0)
                {
                    Journal.Error("FileManager.UploadFile(): User tried to upload empty html file: {filename}", file.FileName);
                    return false;
                }

                string fileName = Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                Journal.Error("FileManager.UploadFile(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Performs file deletion
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="path">file location</param>
        /// <returns></returns>
        public bool Delete(string fileName, string path)
        {
            var filePath = $"{path}/{fileName}";
            if (!File.Exists(filePath))
            {
                return true;
            }

            try
            {
                File.Delete(filePath);                
                return true;
            }
            catch(Exception e)
            {
                Journal.Error("FileManager.Delete(): Unable to delete the file: {fileName} {message} {innerMessage}", fileName, e.Message, e.InnerException?.Message);
            }

            return false;
        }

        /// <summary>
        /// Returns content of provided file
        /// </summary>
        /// <param name="fileName">provided file</param>
        /// <returns></returns>
        public async Task<string> ReadAllTextAsync(string fileName)
        {
            if (!File.Exists(fileName))
                return string.Empty;

            try
            {
                return await File.ReadAllTextAsync(fileName);
            }
            catch(Exception e)
            {
                Journal.Error("FileManager.ReadAllText(): Error occured while reading the file {fileName} {message} {innerMessage}", fileName, e.Message, e.InnerException?.Message);
                return string.Empty;
            }
        }
    }    
}
