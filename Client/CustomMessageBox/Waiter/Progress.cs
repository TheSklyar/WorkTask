using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Helpers.Waiter
{
    static class Progress
    {
        private static Window win;
        public static void Start(Window owner)
        {
            win = new WaitWindow();
            win.Owner = owner;
            win.ShowDialog();
        }

        public static void Finish()
        {
            while (!(win is null))
            {
                try
                {
                    win.Close();
                    win = null;
                }
                catch
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
