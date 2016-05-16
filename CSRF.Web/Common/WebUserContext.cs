using System.Web;

namespace CSRF.Web.Common
{
    public class WebUserContext : UserContext
    {
        private const string ContextKey = "Current-User";

        public WebUserContext(string username)
        {
            HttpContext.Current.Items[ContextKey] = username;
        }

        public override string Username
        {
            get
            {
                var name = HttpContext.Current.Items[ContextKey] as string;
                return name;
            }
        }
    }
}