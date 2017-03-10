using System;
using System.Threading.Tasks;

namespace GlueLib
{

    public interface IPlugin: IDisposable
    {
        //IMainScreenButton MainButton { get; set; }
        Task Init();
        Task Execute();
        void Call();
        //void OpenItem(ServerItem item);
    }
}
