using System;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CSRF.Web.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AjaxValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {     
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var request = filterContext.HttpContext.Request;

            var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
            var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;
            var formToken = request.Headers["__RequestVerificationToken"];
            AntiForgery.Validate(cookieValue, formToken);
        }
    }
}