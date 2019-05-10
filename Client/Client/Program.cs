using Castle.Windsor;
using System;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                using (var container = new WindsorContainer())
                {
                    var bootstrapper = new Bootstrapper(container);
                    bootstrapper.Install();
                    bootstrapper.RunApplication();
                }
            }
            catch (Exception ex)
            {
                CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace);
            }
        }
    }
}
