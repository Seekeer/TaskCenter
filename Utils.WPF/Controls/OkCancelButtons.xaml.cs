using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using Utils.Helpers;

namespace Utils.Controls
{
    /// <summary>
    /// Interaction logic for OkCancelButtons.xaml
    /// </summary>
    public partial class OkCancelButtons : UserControl
    {
        public OkCancelButtons()
        {
            InitializeComponent();
            this.DataContextChanged += 
                new DependencyPropertyChangedEventHandler(_OkCancelButtonsDataContextChanged);
        }

        void _OkCancelButtonsDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var command = DataContext as ICommand;
            if (command != null)
                OkButton.Command = command;
        }

        private void _CloseWindow(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                new Action(() => _CloseParent())
            );
        }

        private void _CloseParent()
        {
            var window = WPFHelper.GetParent(this, typeof(Window)) as Window;

            if (window != null)
                window.Close();
            else
                Debug.Assert(false, "Button must be hosted in a window.");
        }   
    }
}
