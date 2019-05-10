using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    public abstract class Service : IService
    {
        public string Description { get; protected set; }

        public string Ident { get; protected set; }

        public bool IsVisibleToUser { get; protected set; }

        public string Name { get; protected set; }

        public override string ToString()
        {
            return Name + "\t|\t" + Description;
        }

        public void Dispose()
        {

        }

        public virtual bool Execute()
        {
            throw new NotImplementedException();
        }

        protected void ExecutionShield(Action action, Action onFinally = null)
        {
            try
            {
                action();
            }
            catch (SqlException ex)
            {
                CtmMessageBox.Show(@"Ошибка SQL запуска сервиса", ex.Message, ex.StackTrace);

            }
            catch (Exception ex)
            {
                CtmMessageBox.Show(@"Ошибка запуска сервиса", ex.Message, ex.StackTrace);

            }
            finally
            {
                if (onFinally != null)
                {
                    onFinally();
                }
            }
        }

        protected TResult ExecutionShield<TResult>(Func<TResult> action, Action onFinally = null)
        {
            try
            {
                return action();
            }
            catch (SqlException ex)
            {
                CtmMessageBox.Show(@"Ошибка SQL запуска сервиса", ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                CtmMessageBox.Show(@"Ошибка запуска сервиса", ex.Message, ex.StackTrace);
            }
            finally
            {
                if (onFinally != null)
                {
                    onFinally();
                }
            }

            return default(TResult);
        }


        protected Service()
        {
        }

    }
}
