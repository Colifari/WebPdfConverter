using SQLiteWebPdfConverterLib;
using WebPdfConverter.Models;
using WebPdfConverter.Services;
using WebPdfConverterCommonLib.Tools;

namespace WebPdfConverter.Extensions
{
    public static class AplicationServicesExtension
    {
        /// <summary>
        /// Adds specified services and features for web app
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebPdfConverterAppApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSingleton<ISettings, Settings>();
            services.AddSingleton<IConvJobScheduler, PdfConvJobScheduller>();
            services.AddHostedService<TimedHostedService>();
            services.AddSingleton<IDataContext, DataContext>();   // sqlite

            services.AddCors(opt =>
            {
                // adding any CORS from localhost:3000
                opt.AddPolicy("ClientAppCorsPolicy", policy =>
                {
                    policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000");
                });
            });

            return services;
        }
    }
}
