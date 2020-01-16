using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Task.Core;
using Task.Core.Data;
using Task.Core.Model;
using Utils;
using Task.UI.Windows;

namespace Task.UI.VM
{
    public class ReviewTaskVM : ViewModelBase
    {
        public ReviewTaskVM(GlobalTask task)
        {
            Caption = task.Caption;
            Status = task.Status;
            Executors = task.ExecutorTasks.Select(x => new ExecutorTaskVM(x));
        }

        public string Caption { get; set; }
        public GlobalTaskStatus Status { get; }
        public IEnumerable<ExecutorTaskVM> Executors { get; }
        public string Urls { get; set; }

        internal void ShowWindow()
        {
            new ReviewTaskWindow(this).ShowDialog();
        }

    }   

    public class ExecutorTaskVM
    {
        public ExecutorTask Task { get; set; }
        public Executor Executor { get; set; }
        
        public TaskStatusData Status { get; }

        public ExecutorTaskVM(ExecutorTask x)
        {
            Task = x;
            Executor = Task.Executor;
            Status = x.Statuses.LastOrDefault();
        }
    }
}
