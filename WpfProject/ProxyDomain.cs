using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GlueLib;
using Newtonsoft.Json;

namespace WpfProject
{
    class WindowPref
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public WindowStartupLocation Startup { get; set; }
    }
    public class ProxyDomain : MarshalByRefObject, IDisposable

    {
        private IPlugin _pgn;
        private WindowPref _window = new WindowPref();
        public ProxyDomain Initialize
        {
            get
            {
                
                var window = new WindowPref()
                {
                    Height = Application.Current.MainWindow.Height,
                    Width = Application.Current.MainWindow.Width,
                    Startup = Application.Current.MainWindow.WindowStartupLocation,
                    Top = Application.Current.MainWindow.Top,
                    Left = Application.Current.MainWindow.Left
                };
                var domaininfo = new AppDomainSetup
                {
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    //ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                    //ApplicationName = AppDomain.CurrentDomain.SetupInformation.ApplicationName,
                    LoaderOptimization = LoaderOptimization.MultiDomainHost,
                    AppDomainInitializerArguments = new string[] { JsonConvert.SerializeObject(window) }
                };
                var adevidence = AppDomain.CurrentDomain.Evidence;
                var appDomain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);
                return (ProxyDomain)appDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, typeof(ProxyDomain).FullName);
            }
        }

        public void GetPlugin(string assemblyPath)
        {
            try
            {
                _window =
                    JsonConvert.DeserializeObject<WindowPref>(
                        AppDomain.CurrentDomain.SetupInformation.AppDomainInitializerArguments[0]);
                var plugin = Assembly.LoadFrom(assemblyPath);
                var pluginType = plugin.GetTypes().FirstOrDefault(type =>
                {
                    return typeof(IPlugin).IsAssignableFrom(type);
                });
                _pgn = (IPlugin)plugin.CreateInstance(pluginType.FullName);
                _pgn.Init();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
        public void StartPlugin()
        {
            var window = new Window
            {
                Top = _window.Top + _window.Height / 4,
                Left = _window.Left + _window.Width / 4,
                Height = _window.Height / 2,
                Width = _window.Width / 2,
                ResizeMode = ResizeMode.NoResize,
                Content = (UserControl)_pgn
            };
            window.Show();
            _pgn.Execute();
        }
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ProxyDomain() { Dispose(false); }
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            try
            {
                _pgn.Dispose();
            }
            catch
            { }
        }
        #endregion
    }

}
