using HearthBuilder.Models;
using HearthBuilder.Models.Cards;
using HearthBuilder.Models.Decks;
using HearthBuilder.Models.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class DeckController : Controller
    {
        // GET: Deck
        public ActionResult Index() //selecting a new class
        {
            return View();
        }

        public ActionResult New(string id) //generating a new deck from the db
        {
            //check to ensure we have a valid action
            if (id != "Druid" && id != "Hunter" && id != "Mage" && id != "Paladin" && id != "Priest" && id != "Rogue" && id != "Shaman" && id != "Warlock" && id != "Warrior")
            {
                if (Session["notifications"] == null)
                    Session["notifications"] = new List<Notification>();

                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You must select a class!", NotificationType.ERROR));
                return RedirectToAction("Index", "Deck");
            }

            try
            {
                //generating a new deck from the db
                Deck deck = DeckDAO.Instance.CreateNewDeck(id.ToUpper());
                //redirect them to the edit deck page
                return RedirectToAction("Edit", new { id = deck.Id });
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error creating deck! " + e.Message, NotificationType.ERROR));
                return RedirectToAction("Index", "Deck");
            }
            
        }

        public ActionResult Edit(int id = 0) //editing a current deck
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();
            
            try
            {
                //try to pull the deck via ID from the DB
                Deck deck = DeckDAO.Instance.GetDeckById(id);
                if (deck == null)
                {
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Couldn't edit a deck that doesn't exist!", NotificationType.ERROR));
                    return Redirect("/");
                }
                Session["deck"] = deck; //save the deck to the session
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error getting deck! " + e.Message, NotificationType.ERROR));
                return Redirect("/");
            }
            return View();
        }
        
        public string ListAllCards()
        {
            return JsonConvert.SerializeObject(CardCollection.Instance.AllCards);
        }

        public string SessionDeck()
        {
            return JsonConvert.SerializeObject((Deck)Session["deck"]);
        }

        public string GetDeck(int id)
        {
            return JsonConvert.SerializeObject(DeckDAO.Instance.GetDeckById(id));
        }

        [HttpPost]
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
                    deck.AddCard(CardCollection.Instance.getById(id));
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

        [HttpPost]
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
                    deck.RemoveCard(CardCollection.Instance.getById(id));
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

        [HttpPost]
        public ActionResult SaveDeck(string id)
        {
            var result = new List<object>();

            if (Session["deck"] == null)
            {
                //somethings wrong, there should be a deck here...
                result.Add(new { Result = "0", Message = "No deck to save!" });
            }
            else
            {
                Deck deck = (Deck)Session["deck"];
                //add the new title
                deck.Title = id;
                Session["deck"] = deck; //update the session

                try
                {
                    //save the deck
                    DeckDAO.Instance.UpdateDeck(deck);
                    result.Add(new { Result = "1", Message = "Saved deck!" });
                }
                catch (Exception e)
                {
                    result.Add(new { Result = "0", Message = "Couldn't save deck!? " + e.Message});
                }                    
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDeck()
        {
            var result = new List<object>();

            if (Session["deck"] == null)
            {
                result.Add(new { Result = "0", Message = "Couldn't delete deck that doesn't exist!?" });
            }
            else
            {
                try
                {
                    DeckDAO.Instance.DeleteDeck(((Deck)Session["deck"]).Id);
                    result.Add(new { Result = "1", Message = "Deck deleted!" });
                    if (Session["notifications"] == null)
                        Session["notifications"] = new List<Notification>();
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Success!", "The deck has been deleted! ", NotificationType.SUCCESS));
                }
                catch (Exception e)
                {
                    result.Add(new { Result = "0", Message = "Couldn't delete deck! Error: " + e.Message });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
    }
}
