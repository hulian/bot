using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HT_BOT_State.ipc;
using HT_BOT_State.ipc.impl;
using HT_BOT_State.state;
using HT_BOT_State.state.impl;
using System.Diagnostics;
using System.IO;
using HT_BOT_InputSimulator;

namespace HT_BOT_State
{
    public class BotStateMechine
    {
        private IIpcServer ipcServer;
        private IStateMachine gameModeState;
        private IStateMachine battleState;
        private InputSimulator inputSimulator;

        public BotStateMechine()
        {
              ipcServer = new IpcServer();
              gameModeState = new GameModeState();
              battleState = new BattleState();
              ipcServer.setHandler(handler);
                inputSimulator = new InputSimulator("UnityWndClass", "炉石传说");

        }

        public void start()
        {
            ipcServer.start();
        }

        void handler( string data )
        {
            File.AppendAllText("net.log","handle data:"+ data+Environment.NewLine);
            string[] datas = data.Split(':');
            if(datas[1] == "HUB")
            {
                gameModeState.updateState(datas[1]);
                inputSimulator.moveTo(0, 0);
                inputSimulator.click(0, 0);

            }
        }

        public void stop()
        {
            ipcServer.stop();
        }
    }
}
