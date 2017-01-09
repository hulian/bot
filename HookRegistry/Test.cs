using System.Reflection;
using bgs;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

namespace Hooks
{
	[RuntimeHook]
	class Test
	{
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;	
        private const int MOUSEEVENTF_MOVE    =    0x0001;
        private const int MOUSEEVENTF_ABSOLUTE  =  0x8000;


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);   
        
		public Test()
		{
			HookRegistry.Register(OnCall);
		}

		object OnCall(string typeName, string methodName, object thisObj, object[] args)
		{
            try
            {
                if ("OnPowerHistory".Equals(methodName))
                {
                    GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
                    foreach (GameObject go in allObjects)
                        if (go.name.Contains("cardId"))
                        {
                            File.AppendAllText("data.log", methodName+":"+go.tag+":"+go.name + ":" + Camera.main.WorldToScreenPoint(go.transform.position).ToString() + System.Environment.NewLine);
                        }

                } if ("SetNextMode".Equals(methodName))
                {
                    File.AppendAllText("data.log", SceneManager.GetActiveScene().name + ":" + methodName + ":" + (SceneMgr.Mode)args[0] + System.Environment.NewLine);

                }
                else
                {
                    GameObject tournamentButton = GameObject.Find("TournamentButton");
                    Vector3 P = Camera.main.WorldToScreenPoint(tournamentButton.transform.position);
                    // "Flip" it into screen coordinates
                    P.y = Screen.height - P.y;
                    File.AppendAllText("data.log", SceneManager.GetActiveScene().name + ":" + methodName + ":" + tournamentButton.tag + ":" + tournamentButton.name + ":" + P + System.Environment.NewLine);
                   
                    //MonoBehaviour[] allObjects = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
                    //foreach (MonoBehaviour go in allObjects)
                    //{
                    //    if (go.gameObject.activeInHierarchy && go.name.Contains("Button"))
                    //    {
                    //        Vector3 boxPosition = Camera.main.WorldToScreenPoint(go.transform.position);

                    //        // "Flip" it into screen coordinates
                    //        boxPosition.y = Screen.height - boxPosition.y;
                    //        File.AppendAllText("data.log", SceneManager.GetActiveScene().name + ":" + methodName + ":" + go.tag + ":" + go.name + ":" + boxPosition + System.Environment.NewLine);
                    //    }
                    //}


                }

                
            }
            catch(Exception e)
            {
                File.AppendAllText("data.log", JsonConvert.SerializeObject(e) + "/t/t");
            }

            return null;
			
		}

	}
}
