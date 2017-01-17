using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HT_BOT_State.ipc.impl
{
    class TcpClientState
    {
        private TcpClient client;

        public TcpClientState(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
            this.buffer = new byte[4096];
        }

        public TcpClient tcpClient { get; set; }
        public NetworkStream stream { get; set; }
        public byte[] buffer { get ; set ;}
    }
}
