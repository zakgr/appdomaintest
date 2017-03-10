using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.Shell;

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        // TODO: Make this unique!
        private static readonly string Unique = typeof(App).Namespace;
        private MainWindow _mainWindow;
        
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Handle command line arguments of second instance
            MessageBox.Show("Application already Running");
            var arg = args.Skip(1).ToArray();
            if (arg.Length > 0)_mainWindow.SetArgs(arg);
            return true;
        }
        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            bool result;
            var mutex = new Mutex(true, Unique, out result);
            if (!result)return;

            _mainWindow = new MainWindow();
            _mainWindow.Show();
        }
    }
}
