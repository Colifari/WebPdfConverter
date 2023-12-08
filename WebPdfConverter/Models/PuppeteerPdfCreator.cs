using Microsoft.AspNetCore.Http.HttpResults;
using PuppeteerSharp;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Models
{
    /// <summary>
    /// Puppeteer Sharp wrapper
    /// </summary>
    public class PuppeteerPdfCreator : IPdfCreator
    {
        string chromePath;

        public PuppeteerPdfCreator(string chromePath)
        {
            this.chromePath = chromePath;
        }

        public async Task<PdfCreationResult> CreateFromHTMLAsync(string html, string filePath)
        {
            try
            {                
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = chromePath
                });
                var page = await browser.NewPageAsync();
                await page.SetContentAsync(html);

                var result = await page.GetContentAsync();
                await page.PdfAsync(filePath);

                return PdfCreationResult.Ok(filePath);
            }
            catch(Exception e)
            {
                return PdfCreationResult.Error(e);
            }            
        }
    }

    public interface IPdfCreator
    {
        /// <summary>
        /// Creates PDF file from html
        /// </summary>
        /// <param name="html">Html code</param>
        /// <param name="filePath">Path to save pdf file</param>
        /// <returns></returns>
        Task<PdfCreationResult> CreateFromHTMLAsync(string html, string filePath);
    }

    /// <summary>
    /// Result of Pdf creation
    /// </summary>
    public record PdfCreationResult
    {
        /// <summary>
        /// Flag that job executed successufly
        /// </summary>
        public bool Success { get; init; }
        /// <summary>
        /// Pdf file path
        /// </summary>
        public string PdfPath { get; init; }
        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; init; }

        public static PdfCreationResult Ok(string pdfPath)
        {
            return new PdfCreationResult {Success = true, PdfPath = pdfPath };
        }

        public static PdfCreationResult Error(Exception e)
        {
            return new PdfCreationResult { ErrorMessage  = $"{e.Message} {e.InnerException?.Message}"};
        }
    }

}
