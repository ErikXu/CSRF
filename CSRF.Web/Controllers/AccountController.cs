using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CSRF.Web.Models;
using Newtonsoft.Json;

namespace CSRF.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\data.txt");
        private readonly List<User> _users;

        public AccountController()
        {
            var data = System.IO.File.ReadAllText(_dataPath);

            if (!string.IsNullOrWhiteSpace(data))
            {
                _users = JsonConvert.DeserializeObject<List<User>>(data);
            }
        }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOn(LogOnForm form)
        {
            if (!_users.Any(n => n.Username == form.Username && n.Password == form.Password))
            {
                return View();
            }

            FormsAuthentication.SetAuthCookie(form.Username, false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn", "Account");
        }
	}
}