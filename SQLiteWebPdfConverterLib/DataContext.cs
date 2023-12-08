using Microsoft.Data.Sqlite;
using SQLiteWebPdfConverterLib.Tables;
using System.Diagnostics;
using System.Runtime;
using System.Text.RegularExpressions;
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
        readonly Regex regex;

        public DataContext(ISettings settings)
        {
            this.settings = settings;
            ConnectionString = settings.Current.ConnectionString;
            regex = new Regex(@"data source=([A-z0-9:\/\.]+);");
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

            var m = regex.Match(settings.Current.ConnectionString);
            if (m.Success)
            {
                if (m.Groups.Count > 1)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(m.Groups[1].Value));
                }
            }

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
            var m = regex.Match(settings.Current.ConnectionString);
            if(m.Success)
            {
                if (m.Groups.Count > 1 && File.Exists(m.Groups[1].Value))
                    File.Delete(m.Groups[1].Value);
            }
        }

        public void Dispose()
        {

        }
    }
}
