using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthBuilder.Models;

namespace HearthBuilder.Controllers
{
    public class BuildController : Controller
    {
        //
        // GET: /Build/
        public ActionResult Index() //this will be select a class
        {
            return View();
        }

        public ActionResult Deck(string id) //this will be actually building the deck 
        {
            //check to ensure we have a valid action
            if (id != "Druid" && id != "Hunter" && id != "Mage" && id != "Paladin" && id != "Priest" && id != "Rogue" && id != "Shaman" && id != "Warlock" && id != "Warrior") {
                if (Session["notifications"] != null)
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You must select a class!", NotificationType.ERROR));
                else
                {
                    List<Notification> notifications = new List<Notification>();
                    notifications.Add(new Notification("Error!", "You must select a class!", NotificationType.ERROR));
                    Session["notifications"] = notifications;
                }

                return RedirectToAction("Index", "Build");
            }

            //see if we are resuming an old deck
            if (Session["deck"] == null)
            {
                
            }


            ViewData["class"] = id;
            
            return View();
        }

        public string ListCards()
        {
            return Cards.Instance.AsJSON();
        }
	}
}