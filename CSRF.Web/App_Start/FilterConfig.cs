using System.Web.Mvc;
using CSRF.Web.Common;

namespace CSRF.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExtendedAuthorizeAttribute());
        }
    }
}
