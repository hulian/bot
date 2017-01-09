using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;


namespace Simulator
{
    public class InputSimulator
    {
        // constants for the mouse_input() API function
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        //constants for the SendMessage API 
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCACTIVE = 0x086;
        public const int WM_ACTIVE = 0x006;
        public const int WM_ACTIVATEAPP = 0x01C;

        private IntPtr handle;
        private string classNmae, windowName;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool SetCursorPos(uint x, uint y);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nInex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        public InputSimulator(string classNmae, string windowName)
        {
            this.classNmae = classNmae;
            this.windowName = windowName;
        }


        public static IntPtr MakeLParam(int wLow, int wHigh)
        {
            return (IntPtr)(((short)wHigh << 16) | (wLow & 0xffff));
        }

        private IntPtr getHandle()
        {
            if (handle == null)
            {
                handle = FindWindow(classNmae, windowName);
            }
            return handle;
            
        }

        public void click(int x, int y)
        {
            IntPtr handle =  getHandle();
            SetForegroundWindow(handle);
            SendMessage(handle, WM_LBUTTONDOWN, (IntPtr)0, MakeLParam(x, y));
            SendMessage(handle, WM_LBUTTONUP, (IntPtr)0, MakeLParam(x, y));

        }

        public void moveTo(int x, int y)
        {

            IntPtr handle = getHandle();
            SetForegroundWindow(handle);
            SendMessage(handle, WM_NCHITTEST, (IntPtr)0, MakeLParam(x, y));
            SendMessage(handle, 0x20, handle, (IntPtr)0x2000001);
            SendMessage(handle, WM_MOUSEMOVE, (IntPtr)0, MakeLParam(x, y));

            //Debug.WriteLine(x + ":" + y);
            if (isMouseInWindow(handle))
            {
                Point p = new Point(x,y);
                 ClientToScreen(handle, ref p);
                 Debug.WriteLine(p.X+ ":" + p.Y);
                 SetCursorPos((uint)p.X, (uint)p.Y);
            }

        }

        private bool isMouseInWindow(IntPtr handle)
        {
            IntPtr activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }
            return activatedHandle == handle;

        }


    }
}
