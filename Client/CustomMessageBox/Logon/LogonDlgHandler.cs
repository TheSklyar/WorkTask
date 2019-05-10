using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helpers.Logon
{
    public static class LogonDlgHandler
    {
        public static bool Show(
      ref string userName,
      ref string password,
      ref string connectionStr,
      ref string fileNameStr,
      ref string aliasStr,
      string appname,
      Window owner)
        {
            var logd = new LogonDlg();
            logd.Owner = owner;
            logd.username.Text = userName;
            logd.path.Text = fileNameStr;
            logd.Alias.Text = aliasStr;
            logd.AppName = appname;
            bool? result = logd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                password = logd.password.Password;
                userName = logd.username.Text;
                connectionStr = logd.connectionString;
                fileNameStr = logd.path.Text;
                aliasStr = logd.Alias.Text;

                return true;
            }
            password = string.Empty;
            connectionStr = string.Empty;
            return false;
        }
    }
}
