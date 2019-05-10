using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    public interface IGenericFactory : IDisposable
    {
        T Create<T>();

        T[] CreateAll<T>();
    }
}
