using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ByeByeDPI.Services
{
    /// <summary>
    /// Provides system-wide hotkey registration by wrapping Win32 API calls.
    /// Uses a hidden NativeWindow to intercept WM_HOTKEY messages.
    /// </summary>
    public class HotkeyService : IDisposable
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Windows Message ID for hotkey events
        private const int WM_HOTKEY = 0x0312;
        private readonly HotkeyWindow _window;
        private readonly int _hotkeyId = 1001;

        public bool IsRegistered { get; private set; }
        public event Action HotkeyPressed;

        public HotkeyService()
        {
            _window = new HotkeyWindow();
            _window.HotkeyReceived += () => HotkeyPressed?.Invoke();
        }

        /// <summary>
        /// Registers a global hotkey with the specified modifiers and key.
        /// </summary>
        /// <param name="modifiers">Modifier keys (Control, Alt, Shift, Win).</param>
        /// <param name="key">The main key to be pressed.</param>
        /// <returns>True if registration was successful.</returns>
        public bool Register(Keys modifiers, Keys key)
        {
            // Clear existing registration before assigning a new one
            Unregister();

            // Map WinForms Keys flags to Win32 Modifier flags
            uint modFlags = 0;
            if (modifiers.HasFlag(Keys.Control)) modFlags |= 0x0002; // MOD_CONTROL
            if (modifiers.HasFlag(Keys.Alt)) modFlags |= 0x0001;     // MOD_ALT
            if (modifiers.HasFlag(Keys.Shift)) modFlags |= 0x0004;   // MOD_SHIFT
            if (modifiers.HasFlag(Keys.LWin) || modifiers.HasFlag(Keys.RWin)) modFlags |= 0x0008; // MOD_WIN

            IsRegistered = RegisterHotKey(_window.Handle, _hotkeyId, modFlags, (uint)key);
            return IsRegistered;
        }

        /// <summary>
        /// Unregisters the current hotkey from the system.
        /// </summary>
        public void Unregister()
        {
            if (IsRegistered && _window.Handle != IntPtr.Zero)
            {
                UnregisterHotKey(_window.Handle, _hotkeyId);
                IsRegistered = false;
            }
        }

        public void Dispose()
        {
            Unregister();
            _window.Dispose();
        }

        /// <summary>
        /// A hidden window implementation to listen for system-wide window messages (WndProc).
        /// </summary>
        private class HotkeyWindow : NativeWindow, IDisposable
        {
            public event Action HotkeyReceived;

            public HotkeyWindow()
            {
                // Create an invisible window handle to capture messages
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                // Intercept the WM_HOTKEY message
                if (m.Msg == WM_HOTKEY)
                    HotkeyReceived?.Invoke();

                base.WndProc(ref m);
            }

            public void Dispose() => DestroyHandle();
        }
    }
}