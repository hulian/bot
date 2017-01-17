
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HT_BOT_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 9900);
            StreamWriter sWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
            int i = 10;
            while ( (i--)>0 )
            {               
                sWriter.WriteLine("mode:HUB"+i);
                sWriter.Flush();
                Thread.Sleep(1000);
            }

            client.Close();
        }
    }
}
