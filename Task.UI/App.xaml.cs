using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Task.UI.VM;
using Task.Core;
using Task.Core.Communication;
using Task.Core.Data;

namespace Task.UI
{
    public class AppSettings
    {
        public TimeSpan RefreshUITimeout { get; set; } = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                         new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            var provider = GetProvider();
            var main = provider.GetRequiredService<MainWindow>();
            main.Show();
        }

        private static IServiceProvider GetProvider()
        {
            //SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
            //{
            //    DataSource = "(local)",
            //    InitialCatalog = "test",
            //    UserID = "sa",
            //    Password = "AF51qOlRehfuP5po"
            //};
            SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
            {
                DataSource = "database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com,1433",
                InitialCatalog = "database-2",
                IntegratedSecurity=false,
                UserID = "admin",
                Password = "AF51qOlRehfuP5po"
            };
            var connectionString = sConnB.ConnectionString;

            //var connectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true";
            //var connectionString = "Server=database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com;Database=database-2;UserId=admin;Password=AF51qOlRehfuP5po;";

            var services = new ServiceCollection().AddCoreServices(connectionString);
            services.AddTransient<TasksVM>();
            services.AddTransient<ExecutorsVM>();
            services.AddTransient<RootVM>();
            services.AddTransient<AddTaskVM>();
            services.AddSingleton<AppSettings>(new AppSettings());

            services.AddTransient<MainWindow>();
            var provider = services.BuildServiceProvider();

            return provider;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
