using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Helpers.Common;

namespace Order.Data
{
    public class OrdersFilter : INotifyPropertyChanged
    {
        ConnectionSettings _connectionSettings;
        public Dictionary<int, string> _filters;
        public OrdersFilter(ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _filters = new Dictionary<int, string>
            {
                {1, ""},
                {2, ""},
                {3, ""}
            };
        }

        public Dictionary<int, string> TableSourceForSort = new Dictionary<int, string>
        {
            {1,@"tor.ID" },
            {2,@"tor.[Date]" },
            {3,@"tor.[Summ]" },
            {4,@"tor.[SummPayed]" }
        };

        private int _NumberFilterText;
        public string NumberFilterText
        {
            get => _NumberFilterText==0?"":_NumberFilterText.ToString();
            set
            {
                _NumberFilterText = Convert.ToInt32(string.IsNullOrWhiteSpace(value)?"0":value);
                NumberFilterTextForm();
                OnPropertyChanged();
            }
        }
        public void NumberFilterTextForm()
        {
            if (!string.IsNullOrWhiteSpace(NumberFilterText))
            {
                _filters[1] = "tor.[ID] = " + _NumberFilterText + "";
            }
            else
            {
                _filters[1] = "";
            }
        }

        private decimal _SummFilterText;
        public string SummFilterText
        {
            get => _SummFilterText == 0 ? "" : _SummFilterText.ToString();
            set
            {
                _SummFilterText = Convert.ToDecimal(Extensions.PrepareStringToConvert(string.IsNullOrEmpty(value) ? "0" : value));
                SummFilterTextForm();
                OnPropertyChanged();
            }
        }
        public void SummFilterTextForm()
        {
            if (_SummFilterText != 0)
            {
                _filters[3] = "tor.[Summ] = " + _SummFilterText.ToString().Replace(',', '.') + "";
            }
            else
            {
                _filters[3] = "";
            }
        }

        private decimal _SummPayedFilterText;
        public string SummPayedFilterText
        {
            get => _SummPayedFilterText == 0 ? "" : _SummPayedFilterText.ToString();
            set
            {
                _SummPayedFilterText = Convert.ToDecimal(Extensions.PrepareStringToConvert(string.IsNullOrEmpty(value) ? "0" : value));
                SummPayedFilterTextForm();
                OnPropertyChanged();
            }
        }
        public void SummPayedFilterTextForm()
        {
            if (_SummPayedFilterText != 0)
            {
                _filters[4] = "tor.[SummPayed] = " + _SummPayedFilterText.ToString().Replace(',', '.') + "";
            }
            else
            {
                _filters[4] = "";
            }
        }
        
        private int _DateFilterType;

        public int DateFilterType
        {
            get { return _DateFilterType; }
            set
            {
                _DateFilterType = value;
                RecalculateFilterDate();
                OnPropertyChanged();
            }
        }

        private Visibility _DateTime2Visibility;

        public Visibility DateTime2Visibility
        {
            get { return _DateTime2Visibility; }
            set
            {
                _DateTime2Visibility = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DateTime1;

        public DateTime? DateTime1
        {
            get => _DateTime1;
            set
            {
                _DateTime1 = value;
                RecalculateFilterDate();
                OnPropertyChanged();
            }
        }

        private DateTime? _DateTime2;

        public DateTime? DateTime2
        {
            get => _DateTime2;
            set
            {
                _DateTime2 = value;
                RecalculateFilterDate();
                OnPropertyChanged();
            }
        }

        private void RecalculateFilterDate()
        {
            if (DateFilterType == 3)
            {
                DateTime2Visibility = Visibility.Visible;
                if (DateTime1.HasValue && DateTime2.HasValue)
                {
                    if (DateTime1 > DateTime2)
                    {
                        MessageBox.Show("Начальная дата не может быть позже конечной!");
                        _filters[2] = "";
                    }
                    else
                    {
                        _filters[2] = "(tor.[Date] >= Convert(datetime,'" + DateTime1.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "') and tor.[Date] <= Convert(datetime,'" + DateTime2.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "'))";
                    }
                }
                else
                {
                    _filters[2] = "";
                }
            }
            else
            {
                DateTime2Visibility = Visibility.Collapsed;
                if (DateTime1.HasValue)
                {
                    switch (DateFilterType)
                    {
                        case 0:
                            _filters[2] = "tor.[Date] = Convert(datetime,'" + DateTime1.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "')";
                            break;
                        case 1:
                            _filters[2] = "tor.[Date] >= Convert(datetime,'" + DateTime1.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "')";
                            break;
                        case 2:
                            _filters[2] = "tor.[Date] <= Convert(datetime,'" + DateTime1.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "')";
                            break;
                    }
                }
                else
                {
                    _filters[2] = "";
                }
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
