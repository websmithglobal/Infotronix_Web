using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Josheph.Framework.Common
{
    public interface MyInterface
    {
        bool Insert(object obj);

        bool Update(object obj);

        bool Delete(object obj);
    }
}
