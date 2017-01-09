using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace HT_BOT_IPC
{
    [ServiceContract(Name = "IGameStatusService", Namespace = "http://www.htbot.com/")]
    public interface IGameStatusService
    {
        [OperationContract]
        void updateGameStatus(int type , String value);
    }
}
