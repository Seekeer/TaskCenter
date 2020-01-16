using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Task.Core;
using Task.Core.Communication;
using Task.Core.Data;

namespace TaskCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = GetProvider();
            var executorRepository = provider.GetRequiredService<IExecutorRepository>();
            var telegram = provider.GetRequiredService<TelegramManager>();
            var user = executorRepository.RegisterExecutor(3, "First name", "Last name");
            var taskRepository = provider.GetRequiredService<ITaskRepository>();

            var db = provider.GetRequiredService<AppDbContext>();
            var tasks = db.ExecutorTasks.Where(x => x.Id > 0).ToList();
            tasks.ForEach(x => taskRepository.TaskDone(x.Id.ToString()));

            //var task = Solver.CreateCommentTask(executorRepository.GetAllExecutors(), "asdasdasd", "capt");
            //taskRepository.AddCommentTask(task);
            //telegram.SendTask(task.ExecutorTasks).Wait();
            //telegram.CancelTask(task.ExecutorTasks.First());

            Console.ReadLine();
        }

        private static IServiceProvider GetProvider()
        {
            SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
            {
                DataSource = "(local)",
                InitialCatalog = "test",
                UserID = "sa",
                Password = "AF51qOlRehfuP5po"
            };
            var connectionString = sConnB.ConnectionString;

            //SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
            //{
            //    DataSource = "database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com",
            //    InitialCatalog = "-",
            //    UserID = "admin",
            //    Password = "AF51qOlRehfuP5po"
            //};

            //var connectionString = "Server=database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com;Database=database-2;UserId=admin;Password=AF51qOlRehfuP5po;";
            //var connectionString = "Server=database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com;Database=database-2;UserId=admin;Password=AF51qOlRehfuP5po;";

            var provider = (new ServiceCollection().AddCoreServices(connectionString)).BuildServiceProvider();

            return provider;
        }
    }
}
