using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GlueLib;

namespace MyUserControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IPlugin
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public Task Init()
        {
            return Task.Run(() =>
            {
                if (!Debugger.IsAttached)
                    Debugger.Launch();
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show(AppDomain.CurrentDomain.FriendlyName));
            });
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show("executed"));
            });
        }

        public void Call()
        {
        }
    }
}
