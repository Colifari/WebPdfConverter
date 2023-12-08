using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPdfConverterCommonLib.DTO;
using WebPdfConverterCommonLib.Tools;

namespace SQLiteWebPdfConverterLib.Tables
{
    /// <summary>
    /// Handles operations with table "Files"
    /// </summary>
    public class FilesTO : DbTable, IFilesTO
    {
        const string CREATE_CMD = @"CREATE TABLE IF NOT EXISTS [Files] (
	            Name TEXT NOT NULL PRIMARY KEY,
	            Timestamp DATETIME NOT NULL,
	            Status INTEGER NOT NULL,
	            PdfName TEXT NULL	                  
            ) WITHOUT ROWID";

        const string INSERT_CMD = @"INSERT INTO [Files]
                (Name,  Timestamp, Status, PdfName)
                VALUES
                (@name, @ts, @status, @pdf)";

        const string SELECT_CMD = @"SELECT Name, Timestamp, Status, PdfName FROM [Files]";
        const string SELECT_WSTATUS_CMD = @"SELECT Name, Timestamp, PdfName FROM [Files] WHERE Status = @status";
        const string SELECT_ABSTATUS_CMD = @"SELECT Name, Timestamp, PdfName FROM [Files] WHERE Status <> @status";
        const string DELETE_FILE_CMD = @"Delete FROM [Files] WHERE Name = @name";
        const string UPDATE_PDF_PATH_CMD = @"Update [Files] SET PdfName = @pdf, Status = @status WHERE Name = @name";
        const string UPDATE_STATUS_CMD = @"Update [Files] SET Status = @status WHERE Name = @name";

        internal FilesTO(string cs, DataContext context) : base(cs, context) { }
        
        public bool Create()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    Journal.Error("SQLIte createIfNotExist(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }

                using (var cmd = new SqliteCommand(CREATE_CMD) { Connection = conn })
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception e)
                    {
                        Journal.Error("FilesTO Create(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                        return false;
                    }
                }                
            }            
        }
        
        public async Task<IEnumerable<FileTR>> SelectAsync()
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO Select(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return Enumerable.Empty<FileTR>();
                }

                var files = new List<FileTR>();

                try
                {
                    using (var cmd = new SqliteCommand(SELECT_CMD, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var file = new FileTR()
                                {
                                    Name = reader.GetString(0),
                                    Timestamp = reader.GetDateTime(1),
                                    Status = (FileStatus)reader.GetInt32(2)
                                };

                                if (!reader.IsDBNull(3))
                                    file.PdfName = reader.GetString(3);

                                files.Add(file);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO Select(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return Enumerable.Empty<FileTR>();
                }

                return files;
            }            
        }
       
        public async Task<IEnumerable<FileTR>> SelectWithStatusAsync(FileStatus status)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO SelectWithStatus(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return Enumerable.Empty<FileTR>();
                }

                var files = new List<FileTR>();

                using (var cmd = new SqliteCommand(SELECT_WSTATUS_CMD, conn))
                {
                    cmd.Parameters.Add(new SqliteParameter("@status", (int)status));

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var file = new FileTR()
                                {
                                    Name = reader.GetString(0),
                                    Timestamp = reader.GetDateTime(1),
                                    Status = status
                                };

                                if (!reader.IsDBNull(2))
                                    file.PdfName = reader.GetString(2);

                                files.Add(file);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Journal.Error("FilesTO SelectWithStatus(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                        return Enumerable.Empty<FileTR>();
                    }
                }

                return files;
            }
        }
        
        public async Task<IEnumerable<FileTR>> SelectAllButStatusAsync(FileStatus status)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO SelectAllButStatusAsync(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return Enumerable.Empty<FileTR>();
                }

                var files = new List<FileTR>();

                using (var cmd = new SqliteCommand(SELECT_ABSTATUS_CMD, conn))
                {
                    cmd.Parameters.Add(new SqliteParameter("@status", (int)status));

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var file = new FileTR()
                                {
                                    Name = reader.GetString(0),
                                    Timestamp = reader.GetDateTime(1),
                                    Status = status
                                };

                                if (!reader.IsDBNull(2))
                                    file.PdfName = reader.GetString(2);

                                files.Add(file);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Journal.Error("FilesTO SelectWithStatus(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                        return Enumerable.Empty<FileTR>();
                    }
                }

                return files;
            }
        }
        
        public async Task<bool> DeleteAsync(string fileName)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO Delete(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }

                try
                {
                    using (var cmd = new SqliteCommand(DELETE_FILE_CMD, conn))
                    {
                        cmd.Parameters.Add(new SqliteParameter("@name", fileName));
                        return await cmd.ExecuteNonQueryAsync() == 1;
                    }
                }
                catch(Exception e)
                {
                    Journal.Error("FilesTO Delete(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                }

                return false;
            }
        }
        
        public async Task<bool> InsertAsync(FileTR file)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO Insert(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }

                try
                {
                    using (var cmd = new SqliteCommand(INSERT_CMD, conn))
                    {
                        cmd.Parameters.Add(new SqliteParameter("@name", file.Name));
                        cmd.Parameters.Add(new SqliteParameter("@ts", file.Timestamp));
                        cmd.Parameters.Add(new SqliteParameter("@status", file.Status));
                        cmd.Parameters.Add(new SqliteParameter("@pdf", file.PdfName is null ? DBNull.Value : file.PdfName));

                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                            return true;
                        }
                        catch (Exception e)
                        {
                            Journal.Error("FilesTO InsertAsync(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                        }

                        return false;
                    }
                }
                catch(Exception e)
                {
                    Journal.Error("FilesTO Insert(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }
                
            }
        }
        
        public async Task<bool> UpdateStatusAsync(string fileName, FileStatus status)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO UpdateWithPDFPath(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }

                try
                {
                    using (var cmd = new SqliteCommand(UPDATE_STATUS_CMD, conn))
                    {
                        cmd.Parameters.Add(new SqliteParameter("@name", fileName));
                        cmd.Parameters.Add(new SqliteParameter("@status", (int)status));
                        return await cmd.ExecuteNonQueryAsync() == 1;
                    }
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO UpdateStatus(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                }

                return false;
            }
        }
        
        public async Task<bool> UpdateWithPDFNameAsync(string fileName, string pdfName)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO UpdateWithPDFPath(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                    return false;
                }

                try
                {
                    using (var cmd = new SqliteCommand(UPDATE_PDF_PATH_CMD, conn))
                    {
                        cmd.Parameters.Add(new SqliteParameter("@name", fileName));
                        cmd.Parameters.Add(new SqliteParameter("@pdf", pdfName));
                        cmd.Parameters.Add(new SqliteParameter("@status", FileStatus.Ready));
                        return await cmd.ExecuteNonQueryAsync() == 1;
                    }
                }
                catch (Exception e)
                {
                    Journal.Error("FilesTO UpdateWithPDFPath(): {message} {innerMessage}", e.Message, e.InnerException?.Message);
                }

                return false;
            }
        }
    }
}
