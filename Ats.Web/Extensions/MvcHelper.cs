using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ats.Web.Extensions
{
    public static class MvcHelper
    {
        public static SelectList GetAvailableYears(int startYear)
        {
            int yearNow = DateTime.Now.Year;
            var years = new List<int>();
            int length = yearNow - startYear;
            for (int i = 0; i <= length; i++)
            {
                years.Add(yearNow);
                yearNow--;
            }

            return new SelectList(years);
        }

        public static String GetCheckinStyle(DateTime workingTime, DateTime checkinTime)
        {
            DateTime checkTime = new DateTime(checkinTime.Year, checkinTime.Month, checkinTime.Day, workingTime.Hour, workingTime.Minute, workingTime.Second);
            TimeSpan diff = checkinTime.Subtract(checkTime);

            String style = String.Empty;

            if(diff.TotalMinutes <= 0)
            {
                style = "background-color:#5aa469; color: white; outline: 1px solid #cccccc;";
            } else if (diff.TotalMinutes <= 30)
            {
                style = "background-color:#f8d49d; color: white; outline: 1px solid #cccccc;";
            }
            else {
                style = "background-color:#d35d6e; color: white; outline: 1px solid #cccccc;";
            }
                return style;
        }
    }
}
