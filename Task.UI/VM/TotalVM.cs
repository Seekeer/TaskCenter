using System.Linq;
using System.Collections.Generic;
using Task.Core.Model;
using Utils;

namespace Task.UI.VM
{
    public class TotalVM : ViewModelBase
    {
        public TotalVM(ICollection<ExecutorTask> executorTasks)
        {
            Total = executorTasks.Count;
            var doneTasks = executorTasks
                .Where(x => x.LastStatus == TaskStatus.Done)
                .Cast<ExecutorCommentsTask>().ToList();
            _completedTasks = doneTasks.Count();
            _acceptedGroups = executorTasks.Count(x => x.LastStatus == TaskStatus.Accepted) +
                _completedTasks;
        }

        public int Total { get; set; }
        public string CompletedTasksPercentage { get => ReturnPercentage(_completedTasks); }
        public string AcceptedGroupsPercentage { get => ReturnPercentage(_acceptedGroups); }

        protected string ReturnPercentage(int number)
        {
            var percent = Total == 0 ? 0 : number * 100 / Total;

            return $"{percent}%";
        }

        private readonly int _completedTasks;
        private readonly int _acceptedGroups;

    }
}