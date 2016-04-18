using HearthBuilder.Models;
using HearthBuilder.Models.Account;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class AccountController : Controller
    {
        UserDAO userDAO = UserDAO.Instance;
        UserSession userSession = new UserSession();


        //view login page on first visit
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            User userLogin = userDAO.GetAccountByEmailAndPassword(user);
            if (userLogin != null)
                Session["UserSession"] = userLogin;

            return RedirectToAction("Index", "Home");
        }

        //view register page
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            User userRegister = userDAO.RegisterUser(user);
            if (userRegister != null)
                Session["UserSession"] = userRegister;

            return RedirectToAction("Index", "Home");
        }

    }
}