using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using HT_BOT_IPC;
using System.ServiceModel.Description;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(GameStatusService)))
            {
               host.AddServiceEndpoint(typeof(IGameStatusService), new WSHttpBinding(), "http://127.0.0.1:9999/calculatorservice");
                   if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                   {
                          ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                                         behavior.HttpGetEnabled = true;
                                         behavior.HttpGetUrl = new Uri("http://127.0.0.1:9999/GameStatusService/metadata");
                                        host.Description.Behaviors.Add(behavior);
                   }
                   host.Opened += delegate
                  {
                  Console.WriteLine("CalculaorService已经启动，按任意键终止服务！");
                  };
                  
             host.Open();
            
           }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
