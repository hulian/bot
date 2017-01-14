using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT_BOT_State.ipc
{
    interface IIpcServer
    {
        void start();
        void setHandler(Action<byte[]> action);
        void stop();

    }
}
