using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel.Description;
using HT_BOT_State;
using HT_BOT_State.state.impl;
using System.Threading;

namespace WindowsFormsApplication1
{
    static class Program
    {
        private static BotStateMechine botStateMechine;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
           // new Thread(new ParameterizedThreadStart(null)).Start();
            botStateMechine = new BotStateMechine();
            botStateMechine.start();



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form = new Form1();
            form.FormClosing += onFormClosing;
            Application.Run(form);
        }

        private static void onFormClosing(object sender, FormClosingEventArgs e)
        {
            botStateMechine.stop();
        }
    }
}
