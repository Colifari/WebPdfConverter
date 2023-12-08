using WebPdfConverter.Models;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Services
{
    /// <summary>
    /// Hosted service that runs within the app 
    /// </summary>
    public class TimedHostedService : IHostedService
    {
        readonly ILogger<TimedHostedService> logger;
        readonly IConvJobScheduler scheduler;
        Task longRunningTask;        

        public TimedHostedService(ILogger<TimedHostedService> logger, IConvJobScheduler scheduler)
        {
            this.logger = logger;
            this.scheduler = scheduler;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service running.");
            longRunningTask = scheduler.RefreshAsync();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Timed Hosted Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
