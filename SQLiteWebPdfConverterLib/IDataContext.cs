using SQLiteWebPdfConverterLib.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPdfConverterCommonLib.DTO;

namespace SQLiteWebPdfConverterLib
{
    public interface IDataContext
    {
        /// <summary>
        /// Files table
        /// </summary>
        IFilesTO Files { get; set; }

        /// <summary>
        /// Performs database drop
        /// </summary>
        void DropDatabase();
    }
}
