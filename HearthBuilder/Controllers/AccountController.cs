using HearthBuilder.Models;
using HearthBuilder.Models.Account;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class AccountController : Controller
    {
        UserDAO userDAO = new UserDAO();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            //login page
            User user = userDAO.GetAccountByEmail("trevor166933@hotmail.com");

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(user);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

	}
}