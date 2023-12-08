using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPdfConverterCommonLib.DTO
{
    /// <summary>
    /// DTO for File table
    /// </summary>
    public record FileTR
    {
        /// <summary>
        /// File name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// File timestamp
        /// </summary>
        public required DateTime Timestamp { get; init; }

        /// <summary>
        /// File conversion status
        /// </summary>
        public required FileStatus Status { get; init; }
        
        /// <summary>
        /// Pdf file name created from file
        /// </summary>
        public string PdfName { get; set; }
    }

    /// <summary>
    /// File convesion status
    /// </summary>
    public enum FileStatus
    {
        /// <summary>
        /// File uploaded
        /// </summary>
        Uploaded,
        /// <summary>
        /// Conversion in progress
        /// </summary>
        Converting,
        /// <summary>
        /// Error occured while conversion
        /// </summary>
        Error,
        /// <summary>
        /// File uploaded and converted
        /// </summary>
        Ready
    }
}
