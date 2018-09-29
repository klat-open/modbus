using System;
using System.Windows.Forms;

namespace Code4Bugs.Utils.WinForms
{
    public static class ControlUtils
    {
        public static void Iterate(this Control control, Action<Control> actionOnChild)
        {
            actionOnChild(control);
            foreach (Control child in control.Controls)
            {
                Iterate(child, actionOnChild);
            }
        }

        public static void Iterate(this Control control, Func<Control, bool> actionOnChild)
        {
            if (!actionOnChild(control)) return;
            foreach (Control child in control.Controls)
            {
                Iterate(child, actionOnChild);
            }
        }
    }
}