using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CSRF.Web.Common;
using CSRF.Web.Models;
using Newtonsoft.Json;

namespace CSRF.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\data.txt");
        private readonly List<User> _users;

        public HomeController()
        {
            var data = System.IO.File.ReadAllText(_dataPath);

            if (!string.IsNullOrWhiteSpace(data))
            {
                _users = JsonConvert.DeserializeObject<List<User>>(data);
            }
        }
        public ActionResult Index()
        {
            var view = new HomeView
            {
                CurrentUser = UserContext.Current.Username,
                Users = _users
            };
            return View(view);
        }

        public ActionResult Transfer()
        {
            var view = new TransferView
            {
                CurrentUser = UserContext.Current.Username
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Transfer(TransferForm form)
        {
            var currentUser = _users.Single(n => n.Username == UserContext.Current.Username);
            var targetUser = _users.Single(n => n.Username == form.TargetUser);

            currentUser.Account -= form.Amount;
            targetUser.Account += form.Amount;

            var data = JsonConvert.SerializeObject(_users);
            System.IO.File.WriteAllText(_dataPath, data);
            return RedirectToAction("Index");
        }

        public ActionResult Transfer2()
        {
            var view = new TransferView
            {
                CurrentUser = UserContext.Current.Username
            };
            return View(view);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxValidateAntiForgeryToken]
        public ActionResult Transfer2(TransferForm form)
        {
            var currentUser = _users.Single(n => n.Username == UserContext.Current.Username);
            var targetUser = _users.Single(n => n.Username == form.TargetUser);

            currentUser.Account -= form.Amount;
            targetUser.Account += form.Amount;

            var data = JsonConvert.SerializeObject(_users);
            System.IO.File.WriteAllText(_dataPath, data);
            return Json(true);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}