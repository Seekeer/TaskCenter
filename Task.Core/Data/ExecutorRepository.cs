using System;
using System.Collections.Generic;
using System.Linq;
using Task.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Task.Core.Data
{
    public interface IExecutorRepository
    {
        Executor FindExecutor(int id);
        IEnumerable<Executor> GetAllExecutors(bool isFullInfoRequired);
        Executor RegisterExecutor(int id, string firstName, string lastName);
        void Update(Executor user);
    }

    public class ExecutorRepository : IExecutorRepository
    {

        public ExecutorRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public Executor RegisterExecutor(int id, string firstName, string lastName)
        {
            var executor = new Executor { TelegramId = id, FirstName = firstName, LastName = lastName };
            _db.Executors.Add(executor);
            _db.SaveChanges();

            return executor;
        }

        public Executor FindExecutor(int id)
        {
            return _db.Executors.FirstOrDefault(x => x.TelegramId == id);
        }

        public void Update(Executor user)
        {
            _db.Executors.Update(user);
            _db.SaveChanges();
        }

        public IEnumerable<Executor> GetAllExecutors(bool isFullInfoRequired)
        {
            return _db.Executors.Include(x => x.ExecutorTasks).Where(x => x.Id > 0);
        }

        private AppDbContext _db;
    }
}