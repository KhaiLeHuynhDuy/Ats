using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lpnet.WebApp.Extentions
{
    public static class HtmlExtensions
    {
        private const string endFieldPattern = "^(.*?)>";

        public static IHtmlContent IsDisabled(this IHtmlHelper htmlHelper, bool disabled)
        {
            string rawString = htmlHelper.ToString();
            if (disabled)
            {
                rawString = Regex.Replace(rawString, endFieldPattern, "$1 disabled=\"disabled\">");
            }

            return new HtmlString(rawString);
        }

        public static IHtmlContent IsReadonly(this IHtmlHelper htmlHelper, bool @readonly)
        {
            string rawString = htmlHelper.ToString();
            if (@readonly)
            {
                rawString = Regex.Replace(rawString, endFieldPattern, "$1 readonly=\"readonly\">");
            }

            return new HtmlString(rawString);
        }        
    }
}



