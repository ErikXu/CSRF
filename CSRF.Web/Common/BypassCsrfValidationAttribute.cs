using System;

namespace CSRF.Web.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public class BypassCsrfValidationAttribute : Attribute
    {

    }
}