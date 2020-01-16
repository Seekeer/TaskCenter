using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Task.UI.VM;

namespace Task.UI.Windows
{
    /// <summary>
    /// Interaction logic for AddTaskW.xaml
    /// </summary>
    public partial class AddTaskW : Window
    {
        public AddTaskW(AddTaskVM addTaskVM)
        {
            DataContext = addTaskVM;
            InitializeComponent();
        }
    }
}
