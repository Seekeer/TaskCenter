using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using Task.Core.Data;
using Task.Core.Model;
using Task.UI.Windows;
using Utils;
using Microsoft.EntityFrameworkCore;
using Utils.WPF.Helpers;
using Task.Core.Communication;
using System.Windows.Threading;
using System.ComponentModel;

namespace Task.UI.VM
{
    public class TasksVM : ViewModelBase
    {
        private readonly AppDbContext _db;
        private readonly TelegramManager _telegramManager;
        private readonly DispatcherTimer _dispatcherTimer;

        public TasksVM(AppDbContext db, IServiceProvider provider, TelegramManager taskRepository,
            AppSettings appSettings)
        {
            _db = db;
            _telegramManager = taskRepository;

            RegisterCommand("Add", x => {
                var vm = provider.GetRequiredService<AddTaskVM>();
                vm.SaveAction = Refresh;
                new AddTaskW(vm).ShowDialog();
                });
            RegisterCommand<GlobalTaskVM>("ShowDetails", x => _ShowDetails(x));
            RegisterCommand("ShowDetailsClick", x => _ShowDetails(SelectedTask));
            RegisterCommand("Refresh", x => Refresh());
            RegisterCommand<GlobalTaskVM>("CancelTask", x => CancelTask(x));

            _dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = appSettings.RefreshUITimeout;
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private async void CancelTask(GlobalTaskVM task)
        {
            await _telegramManager.CancelTask(task.Task);
        }

        private void _ShowDetails(GlobalTaskVM vM)
        {
            var vm = new ReviewTaskVM(vM.Task);
            vm.ShowWindow();
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(FilteredTasks));
        }

        public bool DisplayAll { get; set; }
        public GlobalTaskVM SelectedTask { get; set; }

        public IEnumerable<GlobalTaskVM> FilteredTasks
        {
            get
            {
                IQueryable<GlobalTask> tasksQ = _db.GlobalTasks.Include(task => task.ExecutorTasks);
                if (!DisplayAll)
                    tasksQ = tasksQ.Where(x => x.Status != GlobalTaskStatus.Done && 
                        x.Status != GlobalTaskStatus.Canceled);

                var tasks = tasksQ
                    .Select(x => new GlobalTaskVM(x))
                    .ToList();

                return tasks;
            }
        }
    }

    public class GlobalTaskVM : TotalVM
    {
        public GlobalTaskVM(GlobalTask x):base(x.ExecutorTasks)
        {
            Task = x;
            Caption = x.Caption;
            CreationDate = x.CreatedAt.DisplayUTCTime();
        }

        internal GlobalTask Task { get; }
        public string Caption { get; set; }
        public string CreationDate { get; }

    }
}