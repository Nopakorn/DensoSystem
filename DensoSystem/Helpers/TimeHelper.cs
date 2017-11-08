using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DensoSystem.Helpers
{
    public class TimeHelper
    {
        public static IEnumerable<DateTime> DateRange(DateTime fromDate, DateTime toDate)
        {
            return Enumerable.Range(0, toDate.Subtract(fromDate).Days + 1)
                             .Select(d => fromDate.AddDays(d));
        }

        public  static IEnumerable<DateTime> TimeRange(DateTime fromDate)
        {
            DateTime value = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            return Enumerable.Range(0, 24)
                             .Select(d => value.AddHours(d));
        }
    }
}