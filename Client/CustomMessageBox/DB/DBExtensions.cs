using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers.DB
{
    public static class DBExtensions
    {
        public static TReturn ExecuteAutoOpenClose<TReturn>(this IDbConnection connection, Func<TReturn> action)
        {
            Guard.CheckNotNull(connection, "connection");
            Guard.CheckNotNull(action, "action");

            connection.OpenIfClosed();

            try
            {
                return action();
            }
            finally
            {
                connection.Close();
            }
        }
        private static void OpenIfClosed(this IDbConnection connection)
        {
            Guard.CheckNotNull(connection, "connection");

            connection
                .IfNot(x => x.State == ConnectionState.Open)
                .Do(x => x.Open());
        }

        public static TSource IfNot<TSource>(this TSource source, Func<TSource, bool> condition)
            where TSource : class
        {
            if ((source != default(TSource)) && (condition(source) == false))
            {
                return source;
            }

            return default(TSource);
        }

        public static TSource Do<TSource>(this TSource source, Action<TSource> action)
            where TSource : class
        {
            if (source != default(TSource))
            {
                action(source);
            }

            return source;
        }

        public static T InvokeWithRetriesThrows<T>(
            Func<T> func,
            uint maxRepeats = 0U,
            uint waitMsec = 0,
            Action<Exception> err = null)
        {
            uint retryCount = 0;

            do
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    if (++retryCount >= maxRepeats)
                    {
                        throw;
                    }

                    ProcessError(err, ex);
                    Wait(waitMsec);
                }
            } while (true);
        }

        private static void ProcessError(Action<Exception> err, Exception ex)
        {
            if (err != null)
            {
                err(ex);
            }
        }

        private static void Wait(uint waitsms)
        {
            if (waitsms > 0)
            {
                Thread.Sleep(Convert.ToInt32(waitsms));
            }
        }
    }
}
