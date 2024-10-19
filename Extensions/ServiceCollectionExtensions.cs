using AiDetector.Configurations;
using AiDetector.Interfaces;
using AiDetector.Models;
using AiDetector.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AiDetector.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            return services;
        }

        public static IServiceCollection AddApiService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection("Api"));
            services.AddHttpClient<IApiService, ApiService>();
            return services;
        }
    }
}
