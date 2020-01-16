using System;
using System.Collections.Generic;
using System.Text;
using Task.Core;
using Task.Core.Data;
using Utils;
using System.Linq;
using PropertyChanged;

namespace Task.UI.VM
{
    public class AddTaskVM : ViewModelBase
    {
        public AddTaskVM(ExecutorsVM executorsVM, ITaskRepository taskRepository)
        {
            ExecutorsVM = executorsVM;
            _repo = taskRepository;

            RegisterCommand("Add", x => _AddTask());
            executorsVM.Filtered.ListChanged += Filtered_ListChanged;
        }

        private void Filtered_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            //DT  TODO.
            OnPropertyChanged(nameof(LimitStr));
        }

        private void _AddTask()
        {
            var task = Solver.CreateCommentTask(ExecutorsVM.Selected, Urls, Caption);
            _repo.AddCommentTask(task);
            SaveAction();
        }

        public string Caption { get; set; }

        [AlsoNotifyFor(nameof(LimitStr))]
        public string Urls { get; set; }

        public string LimitStr
        {
            get
            {
                var taskPower = string.IsNullOrEmpty(Urls) ? 0 : SplitByNewLine(Urls).Count();
                var usersPower = ExecutorsVM.Selected.Sum(x => x.Power);
                return $"Groups count: {taskPower} users will take: {usersPower}";
            }
        }

        public ExecutorsVM ExecutorsVM { get; set; }

        public Action SaveAction { get; internal set; }

        private static IEnumerable<string> SplitByNewLine( string input)
        {
            var lines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrWhiteSpace(x));
            return lines;
        }
        private readonly ITaskRepository _repo;
    }
}
