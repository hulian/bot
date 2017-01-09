using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT_BOT_IPC
{
    public class GameStatusService : IGameStatusService
    {
        public void updateGameStatus(int type, string value)
        {
            Debug.WriteLine(type+":"+value);
        }
    }
}
