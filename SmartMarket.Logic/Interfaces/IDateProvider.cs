using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic.Interfaces
{
    public interface IDateProvider
    {
        DateOnly GetCurrentDate();
    }
}
