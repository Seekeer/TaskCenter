using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;
using Task.Core.Data;
using Task.Core.Model;
using Utils;

namespace Task.UI.VM
{
    public class ExecutorVM : TotalVM
    {
        public ExecutorVM(Executor x):base(x.ExecutorTasks)
        {
            this.Executor = x;
        }
        public bool IsSelected { get; set; }

        public Executor Executor { get; set; }
    }

    public class ExecutorsVM : ViewModelBase
    {
        private readonly IExecutorRepository _repository;
        private bool _isAllSelected;

        public DispatcherTimer _dispatcherTimer { get; }

        public ExecutorsVM(IExecutorRepository executorRep, AppSettings appSettings)
        {
            _repository = executorRep;
            _isAllSelected = Filtered.All(x => x.IsSelected);
            Refresh();

            //_dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //_dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //_dispatcherTimer.Interval = appSettings.RefreshUITimeout;
            //_dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            var tasks = _repository.GetAllExecutors(_isFullInfoRequired);
            var filtered = tasks.Select(x => new ExecutorVM(x)).ToList();

            Filtered.Clear();
            filtered.ForEach(x => Filtered.Add(x));
        }

        public void FullInfoRequired()
        {
            _isFullInfoRequired = true;
        }

        private bool _isFullInfoRequired;

        public bool IsAllSelected
        {
            get { return _isAllSelected; }
            set 
            { 
                _isAllSelected = value;
                Filtered.ToList().ForEach(x => x.IsSelected = _isAllSelected);
            }
        }

        public BindingList<ExecutorVM> Filtered { get; } = new BindingList<ExecutorVM>();

        public IEnumerable<Executor> Selected
        {
            get
            {
                return Filtered.Where(x => x.IsSelected).Select(x => x.Executor);
            }
        }
    }
}