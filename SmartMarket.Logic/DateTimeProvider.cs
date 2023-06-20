using SmartMarket.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Logic
{
    public class DateTimeProvider : IDateProvider
    {
        public DateOnly GetCurrentDate()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
