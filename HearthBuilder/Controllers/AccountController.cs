using HearthBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class AccountController : Controller
    {
        Account account = new Account();
        //
        // GET: /Account/
        public ActionResult Index() //main profile page? redirect /Account/Login if not logged in?
        {
            return View(account);
        }

        public ActionResult Register()
        {
            return View();
        }

	}
}