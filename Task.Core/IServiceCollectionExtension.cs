using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task.Core.Communication;
using Task.Core.Data;

namespace Task.Core
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, string dbConnectionString)
        {
            services.AddTransient<IExecutorRepository, ExecutorRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddSingleton<TelegramManager>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(dbConnectionString));

            return services;
        }
    }
}
