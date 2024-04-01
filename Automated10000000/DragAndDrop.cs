using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace Automated10000000
{
    public struct CursorPoint
    {
        public int X;
        public int Y;
    }

    public static class DragAndDrop
    {
        [DllImport("user32.dll")]
        public static extern bool GetPhysicalCursorPos(ref CursorPoint lpPoint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        public static void Drag(int startX, int startY, int endX, int endY, TimeSpan sleepTimeBetweenOperations)
        {
            CursorPoint cursorPoint = new CursorPoint();
            GetPhysicalCursorPos(ref cursorPoint);
            SetCursorPos(startX, startY);
            Thread.Sleep(sleepTimeBetweenOperations);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(sleepTimeBetweenOperations);
            SetCursorPos(endX, endY);
            //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, outputX, outputY, 0, 0);
            Thread.Sleep(sleepTimeBetweenOperations);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(sleepTimeBetweenOperations);
            SetCursorPos(cursorPoint.X, cursorPoint.Y);
        }
    }
}