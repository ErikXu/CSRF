using System.Collections.Generic;

namespace CSRF.Web.Models
{
    public class HomeView
    {
        public string CurrentUser { get; set; }
        public List<User> Users { get; set; } 
    }
}