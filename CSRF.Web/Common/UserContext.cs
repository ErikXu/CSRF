namespace CSRF.Web.Common
{
    public abstract class UserContext
    {
        public static UserContext Current { get; set; }

        public abstract string Username { get; }
    }
}