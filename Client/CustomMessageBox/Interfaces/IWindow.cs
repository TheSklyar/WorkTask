using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helpers.Interfaces
{
    public interface IWindow
    {
        Window parent { get; }
    }
}
