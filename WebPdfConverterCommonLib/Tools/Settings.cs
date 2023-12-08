using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

[assembly: InternalsVisibleTo("WebPdfConverterTests")]

namespace WebPdfConverterCommonLib.Tools
{
    /// <summary>
    /// Provides alternative setting storage
    /// </summary>
    public class Settings : ISettings
    {
        string filePath;
        public SettingsModel Current { get; set; } = null;

        public Settings()
        {
            filePath = $"{AppDomain.CurrentDomain.BaseDirectory}Settings.yml";
            Current = deserialize();

            replaceHomeDir();
        }

        SettingsModel deserialize()
        {
            var deserializer = new DeserializerBuilder()
                .Build();

            if (!File.Exists(filePath))
                return new SettingsModel();

            SettingsModel settings;

            try
            {
                using (TextReader reader = new StreamReader(filePath))
                {
                    settings = deserializer.Deserialize<SettingsModel>(reader);
                }

                return settings;
            }
            catch(Exception e)
            {
                Journal.Error("Error on reading Settings.yml {message} {innerMessage}", e.Message, e.InnerException?.Message);
            }

            return new SettingsModel();
        }

        internal void replaceHomeDir()
        {
            // replacing ~ symbol to base directory

            if (Current is null)
                return;

            Current.UploadPath = Current.UploadPath.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
            Current.ConnectionString = Current.ConnectionString.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
        }

        void serialize()
        {
            var serializer = new SerializerBuilder()
                .Build();

            try
            {
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer);
                }
            }
            catch (Exception e)
            {
                Journal.Error("Error on writing Settings.yml {message} {innerMessage}", e.Message, e.InnerException?.Message);
            }
        }
    }

    /// <summary>
    /// Model that identify structure of settings file
    /// </summary>
    public class SettingsModel
    {
        /// <summary>
        /// Upload path for files to be converted
        /// </summary>
        public string UploadPath { get; set; }

        /// <summary>
        /// DB Connection string 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Google Chrome browser location 
        /// </summary>
        public string ChromePath { get; set; }

        /// <summary>
        /// Timeout for PDF Creation process (sec.)
        /// </summary>
        public int PdfCreationTimeoutSec { get; set; }
    }

    /// <summary>
    /// Provides alternative setting storage
    /// </summary>
    public interface ISettings
    {
        SettingsModel Current { get; set; }
    }
}
