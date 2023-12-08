using Microsoft.Data.Sqlite;
using SQLiteWebPdfConverterLib.Tables;
using System.Diagnostics;
using System.Runtime;
using WebPdfConverterCommonLib.DTO;
using WebPdfConverterCommonLib.Tools;

namespace SQLiteWebPdfConverterLib
{
    /// <summary>
    /// Provides access to database
    /// </summary>
    public class DataContext : IDataContext, IDisposable
    {
        readonly ISettings settings;
        public string ConnectionString;

        public DataContext(ISettings settings)
        {
            this.settings = settings;
            ConnectionString = settings.Current.ConnectionString;
            init();
        }
        
        public IFilesTO Files { get; set; }

        /// <summary>
        /// Performs initialization and db creation
        /// </summary>
        void init()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            Files = new FilesTO(ConnectionString, this);            

            createIfNotExist();
        }

        void createIfNotExist()
        {
            // Создание
            Files.Create();
        }

        public void DropDatabase()
        {
            SqliteConnection.ClearAllPools();
            var path = settings.Current.ConnectionString.Replace("data source=", "");
            path = path.Remove(path.Length - 1, 1);
            if(File.Exists(path))
                File.Delete(path);
        }

        public void Dispose()
        {

        }
    }
}
