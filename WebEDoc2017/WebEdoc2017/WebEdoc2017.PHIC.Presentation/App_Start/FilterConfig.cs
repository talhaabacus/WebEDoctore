using System.Web;
using System.Web.Mvc;

namespace WebEdoc2017.PHIC.Presentation
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
