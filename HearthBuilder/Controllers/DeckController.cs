using HearthBuilder.Models;
using HearthBuilder.Models.Account;
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
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            if (Session["UserSession"] == null)
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You are not logged in and as a result, will be unable to save decks! <a href='/Account'>Log in</a> or <a href='/Account/Register'>Register</a>", NotificationType.WARNING));

            return View();
        }
        
        //@id is either an number or the name of a class
        public ActionResult Edit(string id = "") //editing a current deck
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();
            
            try
            {
                int nId = 0;
                Deck deck;

                if (Session["UserSession"] == null)
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You are not logged in and as a result, will be unable to save decks! <a href='/Account'>Log in</a> or <a href='/Account/Register'>Register</a>", NotificationType.WARNING));

                //do we have an existing deck?
                if (int.TryParse(id, out nId))
                {
                    //try to pull the deck via ID from the DB
                    deck = DeckDAO.Instance.GetDeckById(nId);
                    Session["deck"] = deck; //save the deck to the session
                    if (deck == null)
                    {
                        ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You can't edit a deck that doesn't exist!", NotificationType.ERROR));
                        return Redirect("/");
                    }
                    //check deck ownership before allowing editing
                    if (Session["UserSession"] == null || deck.UserId != ((User)Session["UserSession"]).ID)
                    {
                        ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You can't edit a deck that is not yours!", NotificationType.ERROR));
                        return Redirect("/");
                    }
                }
                else //we have a new deck
                {
                    //check to ensure we have a valid action
                    if (id != "Druid" && id != "Hunter" && id != "Mage" && id != "Paladin" && id != "Priest" && id != "Rogue" && id != "Shaman" && id != "Warlock" && id != "Warrior")
                    {
                        ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You must select a class!", NotificationType.ERROR));
                        return RedirectToAction("Index", "Deck");
                    }

                    //are they refreshing the page with a new deck?
                    if (Session["deck"] == null || ((Deck)Session["deck"]).Id != 0 || ((Deck)Session["deck"]).ClassStr != id)
                    {
                        deck = new Deck((PlayerClasses)Enum.Parse(typeof(PlayerClasses), id, true));
                        Session["deck"] = deck; //save the deck to the session
                    }
                }
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error getting deck! " + e.Message, NotificationType.ERROR));
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Redirect("/");
            }
            return View();
        }

        public ActionResult View(int id) //editing a current deck
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            try
            {
                //try to pull the deck via ID from the DB
                ViewBag.deck = DeckDAO.Instance.GetDeckById(id);
                if (ViewBag.deck == null)
                {
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Couldn't edit a deck that doesn't exist!", NotificationType.ERROR));
                    return Redirect("/");
                }

                ViewBag.user = UserDAO.Instance.GetUserbyId(((Deck)ViewBag.deck).UserId);
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error getting deck! " + e.Message, NotificationType.ERROR));
                System.Diagnostics.Debug.WriteLine(e.Message);
                return Redirect("/");
            }
            return View();
        }

        public string ListAllCards()
        {
            return JsonConvert.SerializeObject(CardCollectionFactory.Instance.AllCards);
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
                result.Add(new { Result = "0", Message = "Your session has expired and you will have to create a new deck... Sorry!" });
            }
            else
            {
                try
                {
                    Deck deck = (Deck)Session["deck"];

                    //check deck ownership before allowing adding a card (malicious bypass)
                    if (Session["UserSession"] != null && deck.UserId != ((User)Session["UserSession"]).ID && deck.UserId != 0)
                    {
                        result.Add(new { Result = "0", Message = "You can't add a card to a deck that isn't yours!" + deck.UserId });
                    }
                    else
                    {
                        deck.AddCard(CardCollectionFactory.Instance.getById(id));
                        Session["deck"] = deck;
                        result.Add(new { Result = "1", Message = "Card added." });
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
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
                result.Add(new { Result = "0", Message = "Your session has expired and you will have to create a new deck... Sorry!" });
            }
            else
            {
                try
                {
                    Deck deck = (Deck)Session["deck"];

                    //check deck ownership before allowing adding a card (malicious bypass)
                    if (Session["UserSession"] != null && deck.UserId != ((User)Session["UserSession"]).ID && deck.UserId != 0)
                    {
                        result.Add(new { Result = "0", Message = "You can't add a card to a deck that isn't yours!" });
                    }
                    else
                    {
                        deck.RemoveCard(CardCollectionFactory.Instance.getById(id));
                        Session["deck"] = deck;
                        result.Add(new { Result = "1", Message = "Card removed." });
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    result.Add(new { Result = "0", Message = e.Message, CardId = id, StackTrace = e.StackTrace.ToString() });
                }
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDeck(string id)
        {
            var result = new List<object>();
            bool shouldRedirect = false;

            if (Session["deck"] == null)
            {
                //somethings wrong, there should be a deck here...
                result.Add(new { Result = "0", Message = "Your session has expired and you will have to create a new deck... Sorry!" });
            }
            else if (Session["UserSession"] == null)
            {
                //somethings wrong, there should be a user here... HAXORS
                result.Add(new { Result = "0", Message = "You cannot save if you're not logged in!" });
            }
            else if (((User)Session["UserSession"]).ID != ((Deck)Session["deck"]).UserId && ((Deck)Session["deck"]).UserId != 0)
            {
                result.Add(new { Result = "0", Message = "This is not your deck to save!" });
            }
            else
            {
                Deck deck = (Deck)Session["deck"];
                deck.UserId = ((User)Session["UserSession"]).ID;
                System.Diagnostics.Debug.WriteLine("SaveDeck() " + deck.Id + " card count " + deck.Cards.Count + " to user " + ((User)Session["UserSession"]).ID);

                //add the new title
                deck.Title = id;
                
                try
                {
                    if (deck.Id == 0)
                        shouldRedirect = true;

                    //save the deck
                    deck = DeckDAO.Instance.UpdateDeck(deck);
                    result.Add(new { Result = "1", Message = "Saved deck!", ShouldRedirect = shouldRedirect, NewId = deck.Id });

                    Session["deck"] = deck; //update the session
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    result.Add(new { Result = "0", Message = "Couldn't save deck!? " + e.Message});
                }                    
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDeck()
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            var result = new List<object>();

            if (Session["deck"] == null)
            {
                result.Add(new { Result = "0", Message = "Your session has expired and you will have to create a new deck... Sorry!" });
            }
            else
            {
                try
                {
                    if ((Session["UserSession"]) == null && ((Deck)Session["deck"]).UserId > 0 ||
                        Session["UserSession"] != null && ((User)Session["UserSession"]).ID != ((Deck)Session["deck"]).UserId)
                    {
                        result.Add(new { Result = "0", Message = "This is not your deck to delete!" });
                    }
                    else
                    {
                        Session["deck"] = null;
                        result.Add(new { Result = "1", Message = "Deck deleted!" });
                        ((List<Notification>)Session["notifications"]).Add(new Notification("Success!", "The deck has been deleted! ", NotificationType.SUCCESS));

                        if (((Deck)Session["deck"]).UserId > 0)
                        {
                            DeckDAO.Instance.DeleteDeck(((Deck)Session["deck"]).Id);
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    result.Add(new { Result = "0", Message = "Couldn't delete deck! Error: " + e.Message });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
    }
}
