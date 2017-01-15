using System;
using System.Collections.Generic;
using System.IO;

namespace HT_BOT_State.state.impl
{
    public class GameModeState : IStateMachine
    {
        private string model;

        private List<Card> handCards = new List<Card>();

        public GameModeState( )
        {

        }

        public void start()
        {
            throw new NotImplementedException();
        }

        public void stop()
        {
            throw new NotImplementedException();
        }

        public void updateState(string state)
        {
            this.model = state;
        }
    }
}
