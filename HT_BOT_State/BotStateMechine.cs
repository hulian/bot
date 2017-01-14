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

namespace HT_BOT_State
{
    public class BotStateMechine
    {
        private IIpcServer ipcServer = new IpcServer();
        private IStateMachine gameModeState = new GameModeState();
        private IStateMachine battleState = new BattleState();

        public BotStateMechine()
        {
            ipcServer.setHandler(handler);
            
        }

        void handler( byte[] data )
        {
            Debug.WriteLine("handle data:"+ System.Text.Encoding.Default.GetString(data));
        }

    }
}
