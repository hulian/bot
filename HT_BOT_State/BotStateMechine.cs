using System;
using HT_BOT_State.ipc;
using HT_BOT_State.ipc.impl;
using HT_BOT_State.state;
using HT_BOT_State.state.impl;
using System.IO;
using HT_BOT_InputSimulator;
using Newtonsoft.Json;
using Hooks;

namespace HT_BOT_State
{
    public class BotStateMechine
    {
        private IIpcServer ipcServer;
        private IStateMachine gameModeState;
        private IStateMachine battleState;
        private InputSimulator inputSimulator;
        private JsonSerializer jsonSerializer;

        public BotStateMechine()
        {
              ipcServer = new IpcServer();
              gameModeState = new GameModeState();
              battleState = new BattleState();
              ipcServer.setHandler(handler);
              jsonSerializer = JsonSerializer.Create();
              inputSimulator = new InputSimulator("UnityWndClass", "炉石传说");

        }

        public void start()
        {
            ipcServer.start();
        }

        void handler( string data )
        {
            File.AppendAllText("net.log","handle data:"+ data+Environment.NewLine);
            HtStatus htStatus = jsonSerializer.Deserialize<HtStatus>(new JsonTextReader( new StringReader(data)));
            if( htStatus==null)
            {
                return;
            }
            switch (htStatus.mode)
            {
                case "HUB":
                    gameModeState.updateState(htStatus.mode);
                    Button button = null;
                    if (htStatus.buttons.TryGetValue("TournamentButton", out button))
                    {
                        inputSimulator.moveAndClik((int)button.x, (int)button.y);
                    }
                    break;

                case "TOURNAMENT":
                    if (htStatus.buttons.TryGetValue("DeckName", out button))
                    {
                        inputSimulator.moveAndClik((int)button.x, (int)button.y);
                    }
                    break;

                default:
                    File.AppendAllText("net.log", "unhandled mode:" + htStatus.mode + Environment.NewLine);
                    break;

            }

        }

        public void stop()
        {
            ipcServer.stop();
        }
    }
}
