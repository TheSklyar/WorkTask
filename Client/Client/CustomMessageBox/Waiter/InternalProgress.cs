using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers.Waiter
{

    internal class InternalProgress : IDisposable
    {
        private static WaitWindow _form;
        private static Thread _thread;

        public InternalProgress(Action onFinishAction)
        {
            _onFinishAction = onFinishAction;
        }

        public void Dispose()
        {
            if (_onFinishAction != null)
            {
                _onFinishAction();
            }

            Finish();
        }

        public static void Start()
        {
            while (State == ProgressState.Closing)
            {
                Thread.Sleep(1);
            }

            State = ProgressState.Starting;
            _thread = new Thread(DoWork);
            _thread.Start();
        }

        public static ProgressState State { get; set; }

        private static void DoWork(Object obj)
        {
            _form = new WaitWindow();
            _form.ShowDialog();

            _form = null;
            _thread = null;

            State = ProgressState.Idle;
        }

        public static void Finish()
        {
            if (!(_form is null))
            {
                while (State == ProgressState.Starting)
                {
                    Thread.Sleep(1);
                }

                State = ProgressState.Closing;

                _form.Dispatcher.BeginInvoke(new MethodInvoker(() => _form.Close()));
            }

        }

        private readonly Action _onFinishAction;
    }
}
