using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Task.Core.Model
{
    public enum TaskType
    {
        Comment,
    }
    public enum TaskStatus
    {
        Created,
        Sent,
        Canceled,
        Accepted,
        Done
    }
    public enum GlobalTaskStatus
    {
        Created,
        InWork,
        Canceled,
        Done
    }

    public abstract class GlobalTask
    {
        public int Id { get; internal set; }
        public string Caption { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ExecutorTask> ExecutorTasks { get; set; } = new List<ExecutorTask>();
        public GlobalTaskStatus Status { get; set; }
    }

    public class TaskStatusData
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public TaskStatus Status { get; set; }
        public bool IsUserSet { get; set; }
    }

    public abstract class ExecutorTask
    {
        protected ExecutorTask()
        {
            AddStatus(TaskStatus.Created);
        }

        public void AddStatus(TaskStatus status, bool isUserSet = false)
        {
            if (Statuses.Any(x => x.Status == status))
            {
                Debug.Assert(false);
                return;
            }

            LastStatus = status;
            Statuses.Add(new TaskStatusData { Status = status, IsUserSet = isUserSet});
        }

        public int Id { get; internal set; }
        public GlobalTask Task { get; set; }
        public Executor Executor { get; set; }
        public TaskStatus LastStatus { get; set; }

        public ICollection<TaskStatusData> Statuses { get; set; } = new List<TaskStatusData>();
        public int TelegramMessageId { get; internal set; }
    }

    public class ExecutorCommentsTask : ExecutorTask
    {
        public string Groups { get; set; }
    }

    public class CommentsTask : GlobalTask
    {
        public IEnumerable<ExecutorCommentsTask> GetTasks()
        {
            return ExecutorTasks.Cast<ExecutorCommentsTask>();
        }

        internal void AddExecutorTasks(ExecutorTask executorTask)
        {
            executorTask.Task = this;
            ExecutorTasks.Add(executorTask);
        }
    }
}
