using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alquran.connection
{
    interface IRequestWeb
    {
        string GetReleases(string url);
    }
}
