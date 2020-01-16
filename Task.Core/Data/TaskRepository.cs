using System;
using System.Diagnostics;
using System.Linq;
using Task.Core.Model;
using Telegram.Bot.Types;

namespace Task.Core.Data
{
    public interface ITaskRepository
    {
        ExecutorTask TaskAccepted(string id);
        ExecutorTask TaskDone(string id);
        void TaskSent(ExecutorTask task, Message message);
        void TaskCanceled(ExecutorTask task);
        void AddCommentTask(CommentsTask task);
    }

    public class TaskRepository : ITaskRepository
    {
        public TaskRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ExecutorTask TaskDone(string id)
        {
            var task = _AddStatus(int.Parse(id), TaskStatus.Done);

            _dbContext.Entry(task).Reference(t => t.Task).Load();
            if (task.Task.ExecutorTasks.All(x => x.LastStatus == TaskStatus.Done))
            {
                task.Task.Status = GlobalTaskStatus.Done;
                _dbContext.SaveChanges();
            }

            return task;
        }

        public ExecutorTask TaskAccepted(string id)
        {
            return _AddStatus(int.Parse(id), TaskStatus.Accepted);
        }

        public void TaskSent(ExecutorTask task, Message message)
        {

            task.AddStatus(TaskStatus.Sent);
            task.TelegramMessageId = message.MessageId;

            _dbContext.SaveChanges();
        }

        public void TaskCanceled(ExecutorTask task)
        {
            task.AddStatus(TaskStatus.Canceled);

            _dbContext.Entry(task).Reference(t => t.Task).Load();
            if (task.Task.ExecutorTasks.All(x => x.LastStatus == TaskStatus.Canceled))
                task.Task.Status = GlobalTaskStatus.Canceled;

            _dbContext.SaveChanges();
        }

        private ExecutorTask _AddStatus(int id, TaskStatus status)
        {
            var task = _dbContext.ExecutorTasks.FirstOrDefault(x => x.Id == id);
            task.AddStatus(status, true);
            _dbContext.SaveChanges();

            return task;
        }

        public void AddCommentTask(CommentsTask task)
        {
            _dbContext.CommentsTasks.Add(task);
            _dbContext.SaveChanges();
        }

        private readonly AppDbContext _dbContext;
    }
}
