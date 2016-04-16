using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthBuilder.Models;
using Newtonsoft.Json;

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

            PlayerClass pClass = (PlayerClass)Enum.Parse(typeof(PlayerClass), id, true);

            //see if we are resuming an old deck
            if (Session["deck"] == null)
            {
                Session["deck"] = new Deck(pClass);
            }


            ViewData["className"] = id;
            
            return View();
        }

        public string ListCards()
        {
            return JsonConvert.SerializeObject(Cards.Instance.AllCards);
        }

        public ActionResult AddCard(string id)
        {
            var result = new List<object>();
            
            if (Session["deck"] == null)
            {
                //somethings wrong, there should be a deck here...
                result.Add(new { Result = "0", Message = "No deck to add card!" });
            }
            else
            {
                try
                {
                    Deck deck = (Deck)Session["deck"];
                    deck.AddCard(Cards.Instance.getById(id));
                    Session["deck"] = deck;
                    result.Add(new { Result = "1", Message = "Added card to Deck" });
                }
                catch (Exception e)
                {
                    result.Add(new { Result = "0", Message = e.Message, CardId = id, StackTrace = e.StackTrace.ToString() });
                }
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveCard(string id)
        {
            var result = new List<object>();

            if (Session["deck"] == null)
            {
                //somethings wrong, there should be a deck here...
                result.Add(new { Result = "0", Message = "No deck to remove card!" });
            }
            else
            {
                try
                {
                    Deck deck = (Deck)Session["deck"];
                    deck.RemoveCard(Cards.Instance.getById(id));
                    Session["deck"] = deck;
                    result.Add(new { Result = "1", Message = "Card removed from deck!" });
                }
                catch (Exception e)
                {
                    result.Add(new { Result = "0", Message = e.Message, CardId = id, StackTrace = e.StackTrace.ToString() });
                }
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ListDeck()
        {
            return JsonConvert.SerializeObject((Deck)Session["deck"]);
        }
	}
}