using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Utils
{
    public static class ResizeHitTestHelper
    {
        public static IntPtr GetHitTest(
            Form form,
            Point cursor,
            int gripSize = 8)
        {
            if (cursor.X <= gripSize && cursor.Y <= gripSize)
                return (IntPtr)13; // HTTOPLEFT

            if (cursor.X >= form.Width - gripSize && cursor.Y <= gripSize)
                return (IntPtr)14; // HTTOPRIGHT

            if (cursor.X <= gripSize && cursor.Y >= form.Height - gripSize)
                return (IntPtr)16; // HTBOTTOMLEFT

            if (cursor.X >= form.Width - gripSize && cursor.Y >= form.Height - gripSize)
                return (IntPtr)17; // HTBOTTOMRIGHT

            if (cursor.X <= gripSize)
                return (IntPtr)10; // HTLEFT

            if (cursor.X >= form.Width - gripSize)
                return (IntPtr)11; // HTRIGHT

            if (cursor.Y <= gripSize)
                return (IntPtr)12; // HTTOP

            if (cursor.Y >= form.Height - gripSize)
                return (IntPtr)15; // HTBOTTOM

            return IntPtr.Zero;
        }
    }
}
