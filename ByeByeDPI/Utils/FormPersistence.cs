using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Utils
{
    public static class FormPersistence
    {
        public static void ApplyStoredGeometry(Form form)
        {
            var cfg = TempConfigLoader.Current;
            if (cfg.MainFormWidth > 0 && cfg.MainFormHeight > 0)
                form.Size = new Size(cfg.MainFormWidth, cfg.MainFormHeight);

            if (cfg.MainFormX >= 0 && cfg.MainFormY >= 0)
            {
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(cfg.MainFormX, cfg.MainFormY);
            }
        }

        public static void SaveGeometry(Form form)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                var cfg = TempConfigLoader.Current;
                cfg.MainFormWidth = form.Width;
                cfg.MainFormHeight = form.Height;
                cfg.MainFormX = form.Location.X;
                cfg.MainFormY = form.Location.Y;
                TempConfigLoader.Save();
            }
        }
    }
}
