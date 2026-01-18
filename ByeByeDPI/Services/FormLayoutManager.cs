using ByeByeDPI.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByeByeDPI.Services
{
    /// <summary>
    /// Manages the layout, persistence, and behavior of the main form.
    /// Handles window geometry, snapping, and synchronization with detail windows.
    /// </summary>
    public class FormLayoutManager
    {
        private readonly Form _form;
        private bool _isLoaded;
        public bool IsLoaded => _isLoaded;

        public FormLayoutManager(Form form)
        {
            _form = form ?? throw new ArgumentNullException(nameof(form));
        }

        #region Lifecycle
        public void OnLoad()
        {
            _form.StartPosition = FormStartPosition.Manual;
            FormPersistence.ApplyStoredGeometry(_form);
            _isLoaded = true;
        }
        public void OnShown()
        {
            // Placeholder for future logic upon form display
        }
        public void OnClosing()
        {
            if (_isLoaded)
            {
                FormPersistence.SaveGeometry(_form);
            }
        }
        #endregion

        #region Window Events
        public void OnResize()
        {
            if (!_isLoaded)
                return;
            if (_form.WindowState == FormWindowState.Normal)
            {
                FormPersistence.SaveGeometry(_form);
            }
        }
        public void OnMove()
        {
            if (!_isLoaded)
                return;
            if (_form.WindowState != FormWindowState.Normal)
                return;
            _form.Location = WindowHelper.GetSnappedLocation(_form);
            FormPersistence.SaveGeometry(_form);
        }
        #endregion

        #region Helpers
    
        public void ResetToDefault()
        {
            _form.Size = new Size(480, 720);
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.WindowState = FormWindowState.Normal;
        }
        #endregion
    }
}