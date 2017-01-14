using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HT_BOT_InputSimulator;
using System.Threading;

namespace HT_BOT_UnitTest
{
    [TestClass]
    public class InputSimulatorTest
    {
        [TestMethod]
        public void TestInputSimulator()
        {
            InputSimulator s = new InputSimulator("UnityWndClass", "炉石传说");
            int x = 0;
            int y = 0;
            while (true)
            {
                s.moveTo(x, y);
                s.click(x, y);
                x += 10;
                y += 10;
                if (x > 1000)
                {
                    break;
                }
                Thread.Sleep(100);
            }
        }
    }
}
