using Ats.Commons.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ats.Commons
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;
            return dt.AddDays(1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            int diff = dt.Day - 1;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            int diff = DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day;
            return dt.AddDays(diff).Date;
        }

        public static string GetShortDayInWeekName(this DateTime dt)
        {
            System.Globalization.CultureInfo english = new System.Globalization.CultureInfo("en-US");
            string name = (english.DateTimeFormat.DayNames[(int)dt.DayOfWeek]).Substring(0, 2);
            return name;
        }

        static GregorianCalendar _gc = new GregorianCalendar();
        

        public static List<FirstLastDateOfWeek> GetFirstLastDateOfWeeks(int month, int year)
        {
            var list = new List<FirstLastDateOfWeek>();
            DateTime timeFirtMonth = new DateTime(year, month, 1);
            DateTime timeLastMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int countW = timeLastMonth.GetWeekOfMonth();
            for(int i = 0; i < countW; i++)
            {
                var item = new FirstLastDateOfWeek();
                item.StartDate = timeFirtMonth.StartOfWeek(DayOfWeek.Monday).Month == month? timeFirtMonth.StartOfWeek(DayOfWeek.Monday): timeFirtMonth;
                item.EndDate = timeFirtMonth.EndOfWeek(DayOfWeek.Sunday).Month == month? timeFirtMonth.EndOfWeek(DayOfWeek.Sunday): timeLastMonth;
                item.WeekNumber = i + 1;
                list.Add(item);
                timeFirtMonth = item.EndDate.AddDays(1);
            }
            return list;
        }
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
