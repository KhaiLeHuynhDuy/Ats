using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Ats.Helper
{
    public static class HtmlHelperExtention
    {
        public static HtmlString YesNo(this HtmlHelper htmlHelper, bool yesNo)
        {
            var text = yesNo ? "Yes" : "No";
            return new HtmlString(text);
        }

        public static IHtmlContent Yes(this IHtmlHelper htmlHelper, bool yesNo, string yes)
        {
            if (htmlHelper == null)
            {
                throw new System.ArgumentNullException(nameof(htmlHelper));
            }

            var text = yesNo ? yes : "";
            return new HtmlString(text);
        }

        public static IHtmlContent Yes(this IHtmlHelper html, bool? yesNo, string yes, string no)
        {
            var text = yesNo.HasValue ? yesNo.Value ? yes : no : "";
            return new HtmlString(text);
        }

    }
}