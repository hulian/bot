
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

        public void updateState(string data)
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 9900);
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
            sWriter.WriteLine(data);
            sWriter.Flush();
            client.Close();
        }
       
		public GameStatusWatcher()
		{
			HookRegistry.Register(OnCall);
            File.AppendAllText("data.log", "init data log" + System.Environment.NewLine);
        }

		object OnCall(string typeName, string methodName, object thisObj, object[] args)
		{
            try
            {
                if ("OnPowerHistory".Equals(methodName))
                {
                    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject go in allObjects)
                    {
                        if (go.activeInHierarchy)
                        {
                            if (go.name.Contains("cardId"))
                            {
                                Vector3 boxPosition = Camera.main.WorldToScreenPoint(go.transform.position);
                                boxPosition.y = Screen.height - boxPosition.y;
                                File.AppendAllText("data.log", "CARD" + ":" + go.name + ":" + boxPosition + System.Environment.NewLine);
                            }
                        }
                    }

                } if ("SetNextMode".Equals(methodName))
                {
                    updateState("mode:" + args[0]);
                    File.AppendAllText("data.log", "MODE"+":"+ (SceneMgr.Mode)args[0] + System.Environment.NewLine);
                }
                else
                {
                    GameObject tournamentButton = GameObject.Find("TournamentButton");
                    if(tournamentButton != null)
                    { 
                        Vector3 P = Camera.main.WorldToScreenPoint(tournamentButton.transform.position);
                        P.y = Screen.height - P.y;
                        updateState("button"+":"+tournamentButton.name + ":" + P + System.Environment.NewLine);
                        File.AppendAllText("data.log","BUTTON" + ":" + tournamentButton.name + ":" + P + System.Environment.NewLine);
                    }

                }

                
            }
            catch(Exception e)
            {
                File.AppendAllText("data.log", JsonConvert.SerializeObject(e) + System.Environment.NewLine);
            }

            return null;
			
		}

	}
}
