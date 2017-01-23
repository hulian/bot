
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

        private JsonSerializer jsonSerializer;

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
            jsonSerializer = JsonSerializer.Create();
            System.Timers.Timer t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
            t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件；   
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；  
            File.WriteAllText("data.log", "init data log" + System.Environment.NewLine);

        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                HtStatus htStatus = new HtStatus();
                GameObject sceneMgr = GameObject.Find("SceneMgr");
                String mode = null;
                if (sceneMgr != null)
                {
                    mode = ((SceneMgr)(sceneMgr.GetComponent<MonoBehaviour>())).GetMode().ToString();
                    htStatus.mode = mode;
                    File.AppendAllText("data.log", "mode" + ":" + mode + System.Environment.NewLine);
                }

                switch (mode)
                {
                    case "HUB":
                        addButtonByName(htStatus, "TournamentButton");
                        break;
                    case "TOURNAMENT":
                        addButtonByName(htStatus, "DeckName");
                        break;
                    default:
                        File.AppendAllText("data.log", "unhandled mode" + ":" + mode + System.Environment.NewLine);
                        break;
                }

                StringWriter sw = new StringWriter();
                jsonSerializer.Serialize(sw, htStatus);
                updateState(sw.ToString());

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
                        }
                    }
                }
            }
            catch(Exception e1)
            {
                File.AppendAllText("data.log", "unhandled mode" + ":" + e1 + System.Environment.NewLine);
            }

        }

        private void addButtonByName( HtStatus htStatus , string name)
        {
            GameObject gameObject = GameObject.Find(name);
            if (gameObject == null)
            {
                File.AppendAllText("data.log", "cannot find game object of name:"+"name" + System.Environment.NewLine);
                return;
            }
            addButton(htStatus, gameObject);
        }

        private void addButton(HtStatus htStatus , GameObject gameObject)
        {
 
            Vector3 position = getGameObjectPosition(gameObject);
            Button button = new Button();
            button.name = gameObject.name;
            button.x = position.x;
            button.y = position.y;
            htStatus.buttons.Add(gameObject.name, button);
            File.AppendAllText("data.log", "button" + ":" + gameObject.name + ":" + position + System.Environment.NewLine);
        }

        private Vector3 getGameObjectPosition(GameObject tournamentButton)
        {
            Vector3 boxPosition = Camera.main.WorldToScreenPoint(tournamentButton.transform.position);
            boxPosition.y = Screen.height - boxPosition.y;
            return boxPosition;
        }

        object OnCall(string typeName, string methodName, object thisObj, object[] args)
		{

            return null;
			
		}

	}
}
