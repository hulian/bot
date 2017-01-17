
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;

namespace Hooks
{
	[RuntimeHook]
	class GameStatusWatcher
	{
        private TcpClient client;

        private StreamWriter sWriter;

        private bool connectIpc()
        {
            if(client==null || client.Connected == false)
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 9900);
                if (client.Connected)
                {
                    sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
                }
                else
                {
                    return false;
                }              
            }

            return true;
        }

        public void updateState(string data)
        {
            if (connectIpc())
            {
                sWriter.WriteLine(data);
                sWriter.Flush();
            }
        }
       
		public GameStatusWatcher()
		{
			HookRegistry.Register(OnCall);
            System.Timers.Timer t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   
            File.WriteAllText("data.log", "init data log" + System.Environment.NewLine);

        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {

            GameObject sceneMgr = GameObject.Find("SceneMgr");
            if (sceneMgr != null )
            {
                String mode = ((SceneMgr)(sceneMgr.GetComponent<MonoBehaviour>())).GetMode().ToString();
                File.AppendAllText("data.log", "MODE" + ":" + mode + System.Environment.NewLine);
                updateState("mode:" + mode);
            }

            File.AppendAllText("data.log", "Timer scan game object ......" + System.Environment.NewLine);
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.activeInHierarchy)
                {
                    if (go.activeInHierarchy)
                    {
                        Vector3 boxPosition = Camera.main.WorldToScreenPoint(go.transform.position);
                        boxPosition.y = Screen.height - boxPosition.y;
                        File.AppendAllText("data.log", "CARD" + ":" + go.name + ":" + boxPosition + System.Environment.NewLine);
                        updateState("CARD" + ":" + go.name + ":" + boxPosition);
                    }
                }
            }
  
        }

        object OnCall(string typeName, string methodName, object thisObj, object[] args)
		{

            return null;
			
		}

	}
}
