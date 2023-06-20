using SmartMarket.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Tests
{
    public class MockDateProvider : ITestDateProvider
    {
        private DateOnly _currentDate;

        public MockDateProvider()
        {
            _currentDate = DateOnly.FromDateTime(DateTime.Now);
        }

        public DateOnly GetCurrentDate()
        {
            return _currentDate;
        }

        public void SetCurrentDate(DateOnly date)
        {
            _currentDate = date;
        }
    }

}
