using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT_BOT_State.state
{
    interface IStateMachine
    {
        void start();
        void stop();
        void updateState(String state);
    }
}
