using System.Windows.Forms;

namespace ByeByeDPI.Utils
{
    public static class ScreenHelper
    {
        public static void CenterFormManually(Form form)
        {
            if (form == null || form.IsDisposed)
                return;
            Screen screen = Screen.FromControl(form);
            int newX = (screen.WorkingArea.Width - form.Width) / 2;
            int newY = (screen.WorkingArea.Height - form.Height) / 2;
            form.Location = new System.Drawing.Point(newX, newY);
        }
    }
}
