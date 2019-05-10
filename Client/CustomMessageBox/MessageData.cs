using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class MessageData : INotifyPropertyChanged
    {
        private string _Title_Caption;

        public string Title_Caption
        {
            get
            {
                return _Title_Caption;

            }
            set
            {
                _Title_Caption = value;
                OnPropertyChanged();
            }
        }

        private string _Text_Caption;

        public string Text_Caption
        {
            get
            {
                return _Text_Caption;

            }
            set
            {
                _Text_Caption = value;
                OnPropertyChanged();
            }
        }
        private string _Text_Message;

        public string Text_Message
        {
            get
            {
                return _Text_Message;

            }
            set
            {
                _Text_Message = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
