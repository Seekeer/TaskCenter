using System;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Task.UI.VM
{
    public class RootVM : ViewModelBase
    {
        public RootVM(TasksVM tasks, ExecutorsVM users)
        {
            users.FullInfoRequired();
            TasksVM = tasks;
            UsersVM = users;
        }

        public TasksVM TasksVM { get; }
        public ExecutorsVM UsersVM { get; }
    }
}
