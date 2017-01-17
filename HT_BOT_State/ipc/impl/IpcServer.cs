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
        private TcpClientState tcpClientState;


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
                File.AppendAllText("net.log", "stop accept client"+Environment.NewLine);
                return;
            }

            TcpClient client = this.tcpListener.EndAcceptTcpClient(iar);

            File.AppendAllText("net.log", "client conneted :" + client + Environment.NewLine);

            tcpClientState = new TcpClientState(client);

            tcpClientState.stream.BeginRead(tcpClientState.buffer, 0, tcpClientState.buffer.Length, new AsyncCallback(receiveReadData), tcpClientState);

            tcpListener.BeginAcceptTcpClient(new AsyncCallback(acceptClient), this.tcpListener);
        }

        private void receiveReadData(IAsyncResult ar)
        {
            TcpClientState state = (TcpClientState)ar.AsyncState;

            int bytesRead = state.stream.EndRead(ar);

            if (bytesRead > 0)
            {

                File.AppendAllText("net.log", "received data" + bytesRead + Environment.NewLine);

                MemoryStream ms = new MemoryStream(tcpClientState.buffer);
                ms.Seek(0, 0);
                StreamReader sr = new StreamReader(ms);

                 action.Invoke(sr.ReadLine());
                
            }

            Thread.Sleep(10);

            if (state.tcpClient != null && !state.tcpClient.Connected)
            {
                File.AppendAllText("net.log", "client closed" + Environment.NewLine);
                state.stream.Close();
                state.tcpClient.Close();
                return;
            }

            if (!isUp)
            {
                File.AppendAllText("net.log", "stop receive data" + Environment.NewLine);
                return;
            }

            state.stream.BeginRead(state.buffer, 0, state.buffer.Length, new AsyncCallback(receiveReadData), state);
            
        }

        public void stop()
        {
            isUp = false;
            this.tcpListener.Stop();
        }
    }
}
