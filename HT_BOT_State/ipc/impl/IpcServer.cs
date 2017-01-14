using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace HT_BOT_State.ipc.impl
{
    class IpcServer : IIpcServer
    {
        private int port;
        private string ip;
        private TcpListener tcpListener;
        private Action<byte[]> action;
        private bool isUp;


        public IpcServer()
        {
            if (port == 0)
            {
                port = 9900;
            }
            tcpListener = new TcpListener( IPAddress.Any, port);
        }

        public void setHandler( Action<byte[]> action )
        {
            this.action = action;
        }

        public void start()
        {
            Debug.WriteLine("start server");
            this.tcpListener.Start();
            isUp = true;
            Byte[] bytes = new Byte[256];
            while (isUp)
            {
                if (!isUp)
                {
                    Debug.WriteLine("stop server");
                    break;
                }
                Debug.WriteLine("Waiting for a connection... ");

                TcpClient client =  this.tcpListener.AcceptTcpClient();
                Debug.WriteLine("client conneted :"+client);

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {

                    Debug.WriteLine("Received: {0} byte", i);

                }

                action.Invoke(bytes);
            }
            
        }

        public void stop()
        {
            isUp = false;
        }
    }
}
