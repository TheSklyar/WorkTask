using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Interfaces
{
    public  interface IService : IDisposable
    {
        string Description { get; }

        string Ident { get; }

        bool IsVisibleToUser { get; }

        string Name { get; }
    }
}
