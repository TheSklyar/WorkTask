using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helpers
{
    public static class CtmMessageBox
    {
        public static void Show(string caption, string text, string stacktrace)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(caption, text, stacktrace);
            msg.Topmost = true;
            msg.ShowDialog();
        }

        public static void Show(string caption, string text, string stacktrace, Window owner)
        {

            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(caption, text, stacktrace);
            msg.Owner = owner;
            msg.Topmost = true;
            msg.ShowDialog();
        }
    }
}
