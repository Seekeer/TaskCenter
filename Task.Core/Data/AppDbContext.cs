using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Task.Core.Model;

namespace Task.Core.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=database-2.cerjgpevkqbg.eu-central-1.rds.amazonaws.com,1433;Initial Catalog=database-2;Integrated Security=True;User ID=admin;Password=AF51qOlRehfuP5po;Trusted_Connection=False");
            //optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=test;User ID=sa;Password=AF51qOlRehfuP5po");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Executor>()
                .HasMany(c => c.ExecutorTasks)
                .WithOne(e => e.Executor);

            modelBuilder.Entity<Task.Core.Model.GlobalTask>()
                .HasMany(c => c.ExecutorTasks)
                .WithOne(e => e.Task);
        }

        public DbSet<Executor> Executors { get; set; }

        public DbSet<GlobalTask> GlobalTasks { get; set; }
        public DbSet<CommentsTask> CommentsTasks { get; set; }

        public DbSet<ExecutorTask> ExecutorTasks { get; set; }
        public DbSet<ExecutorCommentsTask> ExecutorCommentsTasks { get; set; }
    }
}