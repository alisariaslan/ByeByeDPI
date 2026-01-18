using ReaLTaiizor.Controls;
using System.Reflection;

namespace ByeByeDPI.Utils
{
    public static class PoisonDropDownButtonExtensions
    {
        private static MethodInfo showMethod = typeof(PoisonDropDownButton)
            .GetMethod("ShowContextMenuStrip", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void OpenDropDown(this PoisonDropDownButton button)
        {
            showMethod?.Invoke(button, null);
        }
    }

}
