using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GlueLib;
using Path = System.IO.Path;

namespace WpfProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EventHandler<string[]> Arguments;

        public static readonly DependencyProperty PluginsProperty = DependencyProperty.Register(
            "Plugins", typeof(ObservableCollection<IPlugin>), typeof(MainWindow), new PropertyMetadata(default(ObservableCollection<IPlugin>)));

        public ObservableCollection<IPlugin> Plugins
        {
            get { return (ObservableCollection<IPlugin>)GetValue(PluginsProperty); }
            set { SetValue(PluginsProperty, value); }
        }

        public MainWindow()
        {
            var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            InitializeComponent();
            if (args.Length > 0) SetArgs(args);
            Plugins = new ObservableCollection<IPlugin>();
        }

        public void SetArgs(string[] args)
        {
            foreach (var arg in args)
            {
                var param = arg.Split('=');
                switch (param[0])
                {
                    case "/minimized":
                        this.WindowState = WindowState.Normal;
                        break;
                    case "plugin":
                        Other(Path.Combine("Plugins", param[1]));
                        break;
                }
            }
        }

        public void Other(string plugpath)
        {
            var proxyDomain = new ProxyDomain().Initialize;
            proxyDomain.GetPlugin(plugpath);
            proxyDomain.StartPlugin();
            proxyDomain.Dispose();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
