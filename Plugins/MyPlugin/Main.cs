using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GlueLib;

namespace MyPlugin
{
    public class Main : IPlugin
    {
        public void Dispose()
        {
        }

        public Task Init()
        {
            return Task.Run(() =>
            {
                if (!Debugger.IsAttached)
                    Debugger.Launch();
                MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
            });
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                MessageBox.Show("executed");
            });
        }

        public void Call()
        {
        }
    }
}
