using HearthBuilder.Models;
using HearthBuilder.Models.Account;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class AccountController : Controller
    {
        UserDAO userDAO = new UserDAO();

        //view login page on first visit
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            /*if (ModelState.IsValid)
            {
                if (users.userName == "NAAM" && users.userPassword == "WACHTWOORD")
                {
                    FormsAuthentication.SetAuthCookie(users.userName, false);
                    return RedirectToAction("", "Home");
                }
                {
                    ModelState.AddModelError("", "Invalid username and/or password");
                }
            }*/
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

	}
}