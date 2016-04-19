using HearthBuilder.Models;
using HearthBuilder.Models.Account;
using HearthBuilder.Models.Notifications;
using System;
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
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            if (ModelState.IsValid)
            {
                try
                {
                    User userLogin = userDAO.GetAccountByEmailAndPassword(user);
                    if (userLogin != null)
                        Session["UserSession"] = userLogin;

                    ((List<Notification>)Session["notifications"]).Add(new Notification("Great!", "You have logged in successfully!", NotificationType.SUCCESS));

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", e.Message, NotificationType.ERROR));
                }
            }
            else
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Could not log in. See the below messages", NotificationType.ERROR));
            }
            return View();
        }

        //view register page
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            if (ModelState.IsValid)
            {
                try
                {
                    User userRegister = userDAO.RegisterUser(user);
                    if (userRegister != null)
                        Session["UserSession"] = userRegister;

                    ((List<Notification>)Session["notifications"]).Add(new Notification("Great!", "You have registered in successfully! You have been logged in.", NotificationType.SUCCESS));

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", e.Message, NotificationType.ERROR));
                    
                }
            }
            else
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Could not register. See the below messages", NotificationType.ERROR));
            }
            return View();
        }

        public ActionResult Logout()
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            if (Session["UserSession"] != null)
            {
                Session["UserSession"] = null;
                ((List<Notification>)Session["notifications"]).Add(new Notification("Great!", "You have been logged out.", NotificationType.SUCCESS));
            }
            else
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Woah!", "You weren't logged in, so there was nothing to log out of...", NotificationType.WARNING));
            }

            return RedirectToAction("Index", "Home");
        }

    }
}