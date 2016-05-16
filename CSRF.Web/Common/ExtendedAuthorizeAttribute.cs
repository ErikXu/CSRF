using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CSRF.Web.Common
{
    public class ExtendedAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            PreventCsrf(filterContext);
            base.OnAuthorization(filterContext);
            GenerateUserContext(filterContext);
        }

        /// <summary>
        /// http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// </summary>
        private static void PreventCsrf(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request.HttpMethod.ToUpper() != "POST")
            {
                return;
            }

            var allowAnonymous = HasAttribute(filterContext, typeof(AllowAnonymousAttribute));

            if (allowAnonymous)
            {
                return;
            }

            var bypass = HasAttribute(filterContext, typeof(BypassCsrfValidationAttribute));

            if (bypass)
            {
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                var cookieValue = antiForgeryCookie != null ? antiForgeryCookie.Value : null;
                var formToken = request.Headers["__RequestVerificationToken"];
                AntiForgery.Validate(cookieValue, formToken);
            }
            else
            {
                AntiForgery.Validate();
            }
        }

        private static bool HasAttribute(AuthorizationContext filterContext, Type attributeType)
        {
            return filterContext.ActionDescriptor.IsDefined(attributeType, true) ||
                   filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(attributeType, true);
        }

        private static void GenerateUserContext(AuthorizationContext filterContext)
        {
            var formsIdentity = filterContext.HttpContext.User.Identity as FormsIdentity;

            if (formsIdentity == null || string.IsNullOrWhiteSpace(formsIdentity.Name))
            {
                UserContext.Current = null;
                return;
            }

            UserContext.Current = new WebUserContext(formsIdentity.Name);
        }
    }
}