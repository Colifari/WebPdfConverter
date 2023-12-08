using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPdfConverterCommonLib.DTO
{
    public interface IFilesTO
    {
        //// <summary>
        /// Creates table structure
        /// </summary>
        /// <returns></returns>
        bool Create();

        /// <summary>
        /// Returns all rows
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<FileTR>> SelectAsync();

        /// <summary>
        /// Returns all rows with provided status
        /// </summary>
        /// <param name="status">File status</param>
        /// <returns></returns>
        Task<IEnumerable<FileTR>> SelectWithStatusAsync(FileStatus status);

        /// <summary>
        /// Returns all rows which status differs from provided
        /// </summary>
        /// <param name="status">File status</param>
        /// <returns></returns>
        Task<IEnumerable<FileTR>> SelectAllButStatusAsync(FileStatus status);

        /// <summary>
        /// Performs deletion of provided row
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string fileName);

        /// <summary>
        /// Creates provided row in db
        /// </summary>
        /// <param name="file">file</param>
        /// <returns></returns>
        Task<bool> InsertAsync(FileTR file);

        /// <summary>
        /// Performs status updation
        /// </summary>
        /// <param name="fileName">file</param>
        /// <param name="status">new status</param>
        /// <returns></returns>
        Task<bool> UpdateStatusAsync(string fileName, FileStatus status);

        /// <summary>
        /// Performs Pdf file name updation
        /// </summary>
        /// <param name="fileName">file</param>
        /// <param name="pdfName">Pdf file name</param>
        /// <returns></returns>
        Task<bool> UpdateWithPDFNameAsync(string fileName, string pdfPath);


        
    }
}
