using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HT_BOT_State.ipc.impl
{
    class IpcServer : IIpcServer
    {
        private int port=0;
        private string ip;
        private TcpListener tcpListener;
        private Action<String> action;
        private bool isUp=true;
        private Thread thread;


        public IpcServer()
        {
            if (port == 0)
            {
                port = 9900;
            }
            tcpListener = new TcpListener( IPAddress.Any, port);
        }

        public void setHandler( Action<string> action )
        {
            this.action = action;
        }

        public void start()
        {
            thread = new Thread(new ThreadStart(WorkThreadFunction));
            thread.Start();
            
        }

        private void WorkThreadFunction()
        {
            File.AppendAllText("net.log","start server"+Environment.NewLine);
            this.tcpListener.Start();

           

            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(acceptClient), this.tcpListener);

            isUp = true;
            while (isUp)
            {
                Thread.Sleep(1000);
            }
        }

        private void acceptClient( IAsyncResult iar )
        {

            if (!isUp)
            {
                File.AppendAllText("net.log", "server stoped"+Environment.NewLine);
                return;
            }

            using (TcpClient client = this.tcpListener.EndAcceptTcpClient(iar))
            {
                File.AppendAllText("net.log", "client conneted :" + client + Environment.NewLine);

                NetworkStream stream = client.GetStream();

                Byte[] bytes = new Byte[256];
                int i;

                while (stream.DataAvailable)
                {
                    StreamReader sr = new StreamReader(stream);
                    string data = sr.ReadLine();
                    File.AppendAllText("net.log", "Received: " + sr.ReadLine()+ Environment.NewLine);
                    if (data.Length > 0)
                    {
                        action.Invoke(data);
                    }

                }
            }

            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(acceptClient), this.tcpListener);
        }

        public void stop()
        {
            isUp = false;
            this.tcpListener.Stop();
        }
    }
}
