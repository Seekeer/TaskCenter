using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace Task.Core.Model
{
    public class Executor
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Power { get; set; } = 5;
        public int LastPower { get; set; } = 5;

        public ICollection<ExecutorTask> ExecutorTasks { get; set; } = new List<ExecutorTask>();
        public long TelegramId { get; internal set; }
    }
}
