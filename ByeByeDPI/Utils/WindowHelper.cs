using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Utils
{
    public static class WindowHelper
    {
        public static int CalculateSnap(int pos, int size, int screenStart, int screenEnd, int distance, int padding = 0)
        {
            // Sol veya Üst kenara yapışma (Padding eklenmiş hali)
            if (Math.Abs(pos - (screenStart + padding)) < distance)
                return screenStart + padding;

            // Sağ veya Alt kenara yapışma (Padding çıkarılmış hali)
            if (Math.Abs((pos + size) - (screenEnd - padding)) < distance)
                return screenEnd - padding - size;

            return pos;
        }

        public static Point GetSnappedLocation(Form form, int snapDistance = 20, int padding = 10)
        {
            var screen = Screen.FromControl(form).WorkingArea;

            int newX = CalculateSnap(form.Left, form.Width, screen.Left, screen.Right, snapDistance, padding);
            int newY = CalculateSnap(form.Top, form.Height, screen.Top, screen.Bottom, snapDistance, padding);

            return new Point(newX, newY);
        }

    }
}