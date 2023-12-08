
using Microsoft.AspNetCore.Mvc;
using SQLiteWebPdfConverterLib;
using System.Collections.Concurrent;
using WebPdfConverterCommonLib.DTO;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Models
{
    /// <summary>
    /// Performs file conversion scheduling 
    /// </summary>
    public class PdfConvJobScheduller : IConvJobScheduler
    {
        readonly ISettings settings;
        readonly IPdfCreator pdfCreator;
        readonly ConcurrentQueue<FileTR> queue;
        PeriodicTimer timer = null;
        Task longRunningTask;

        const int TIMER_PERIOD_SECONDS = 2;

        public PdfConvJobScheduller(ISettings settings) {
            this.settings = settings;
            queue = new ConcurrentQueue<FileTR>();
            pdfCreator = new PuppeteerPdfCreator(settings.Current.ChromePath);
            longRunningTask = runTimer();
        }

        /// <summary>
        /// Timer method, that will invoke file processing
        /// </summary>
        /// <returns></returns>
        async Task runTimer()
        {
            timer = new PeriodicTimer(TimeSpan.FromSeconds(TIMER_PERIOD_SECONDS));
            while (await timer.WaitForNextTickAsync())
            {
                await processQueue();
            }
        }
        
        public void AddJob(FileTR file)
        {
            queue.Enqueue(file);
        }
        
        public async Task RefreshAsync()
        {
            Journal.Info("Refreshing convert queue...");
            using (var context = new DataContext(settings))
            {
                foreach (var file in await context.Files.SelectAllButStatusAsync(FileStatus.Ready))
                {
                    queue.Enqueue(file);
                }                
            }
        }

        /// <summary>
        /// Processes queue items one by one
        /// </summary>
        /// <returns></returns>
        async Task processQueue()
        {
            if (queue.IsEmpty)
                return;

            var fileManager = new FileManager();

            while(queue.TryDequeue(out var file))
            {
                using (var context = new DataContext(settings))
                {
                    await context.Files.UpdateStatusAsync(file.Name, FileStatus.Converting);
                    Journal.Info("Converting file {fileName}...", file.Name);

                    var filePath = $"{settings.Current.UploadPath}\\{file.Name}";
                    var pdfFileName = file.Name.Replace(Path.GetExtension(file.Name), ".pdf");
                    var pdfPath = filePath.Replace(Path.GetExtension(file.Name), ".pdf");

                    var convTask = pdfCreator.CreateFromHTMLAsync(await fileManager.ReadAllTextAsync(filePath), pdfPath);

                    var timeoutMs = settings.Current.PdfCreationTimeoutSec * 1000;

                    if (await Task.WhenAny(convTask, Task.Delay(timeoutMs)) == convTask)
                    {
                        // success
                        var result = await convTask;
                        if (result.Success)
                        {
                            await context.Files.UpdateWithPDFNameAsync(file.Name, pdfFileName);
                            Journal.Info("File successfully converted {fileName}", file.Name);
                        }
                        else
                        {
                            Journal.Error("PDF creation failed {message}", result.ErrorMessage);
                            await context.Files.UpdateStatusAsync(file.Name, FileStatus.Error);
                        }
                    }
                    else
                    {
                        Journal.Error("PDF creation process timeout occured {fileName} {timeout}", file.Name, settings.Current.PdfCreationTimeoutSec);
                        await context.Files.UpdateStatusAsync(file.Name, FileStatus.Error);
                    }
                }                
            }
        }
    }


    /// <summary>
    /// Performs file conversion scheduling 
    /// </summary>
    public interface IConvJobScheduler
    {
        /// <summary>
        /// Adds file to conversion queue
        /// </summary>
        /// <param name="file"></param>
        void AddJob(FileTR file);

        /// <summary>
        /// Loads queue from database
        /// </summary>
        /// <returns></returns>
        Task RefreshAsync();
    }
}
