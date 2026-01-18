using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ByeByeDPI.Utils
{
    public static class NativeMethods
    {
        private const byte VK_CONTROL = 0x11;
        private const byte VK_V = 0x56;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static bool IsCurrentProcessFocused()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero) return false;

            GetWindowThreadProcessId(foregroundWindow, out uint foregroundProcessId);
            return foregroundProcessId == (uint)Process.GetCurrentProcess().Id;
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void SendCtrlV()
        {
            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_V, 0, 0, 0);

            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
