using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthBuilder.Models.FilterDecks;
using HearthBuilder.Models.Cards;
using HearthBuilder.Models.Decks;
using HearthBuilder.Models.Notifications;
using HearthBuilder.Models.Account;

namespace HearthBuilder.Controllers
{
    public class BrowseController : Controller
    {
        //
        // GET: /Browse/
        public ActionResult Index() //this is the main page to browse/search decks
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            try
            {
                //try to pull the decks from the DB
                ViewBag.decks = DeckDAO.Instance.GetAllDecks();
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error getting decks! " + e.Message, NotificationType.ERROR));
            }

            return View(new SearchParams());
        }

        [HttpPost]
        public ActionResult Index(SearchParams searchParams) //this page will filter down the number of decks displayed
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            DeckDAO deckDAO = DeckDAO.Instance;
            List<Deck> allDecks = deckDAO.GetAllDecks();

            List<Deck> filteredDecks = new List<Deck>();

            //filter down decks here
            Boolean classFilter = false;
            for (int i = 0; i < allDecks.Count; i++)
            {
                //check if there should be a class filter
                for (int j = 0; j < searchParams.Classes.Count; j++)
                {
                    if (searchParams.Classes[j].Checked)
                    {
                        classFilter = true;
                    }
                }
                //there is a deck name and class filter
                if (searchParams.DeckName != null && classFilter)
                {
                    for (int j = 0; j < searchParams.Classes.Count; j++)
                    {
                        if (searchParams.Classes[j].Checked && allDecks[i].Class.ToString() == searchParams.Classes[j].Name.ToUpper() && searchParams.DeckName == allDecks[i].Title)
                        {
                            filteredDecks.Add(allDecks[i]);
                        }
                    }
                }
                //there is a deck name, no class filter
                else if (searchParams.DeckName != null && !classFilter && searchParams.DeckName == allDecks[i].Title)
                {
                    filteredDecks.Add(allDecks[i]);
                }
                //there is no deck name, but a class filter
                else if (searchParams.DeckName == null && classFilter)
                {
                    for (int j = 0; j < searchParams.Classes.Count; j++)
                    {
                        if (searchParams.Classes[j].Checked && allDecks[i].Class.ToString() == searchParams.Classes[j].Name.ToUpper())
                        {
                            filteredDecks.Add(allDecks[i]);
                        }
                    }
                }
            }

            try
            {
                if (filteredDecks.Count > 0)
                {
                    //only show filtered decks
                    ViewBag.decks = filteredDecks;
                }
                else {
                    //pull decks from the DB
                    ViewBag.decks = DeckDAO.Instance.GetAllDecks();
                    //display notification
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Woops!", "We couldn't find any decks matching your filter!", NotificationType.WARNING));
                } 
            }
            catch (Exception e)
            {
                ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "Unexpected error getting decks! " + e.Message, NotificationType.ERROR));
            }

            return View(new SearchParams());
        }

        public ActionResult Deck() //viewing an individual deck
        {
            return View();
        }

        public ActionResult CardName(String id) //viewing a card
        {
            //look for a card using the name
            Card card = CardCollection.Instance.getByName(id);// Cards.GetFromName(id HearthDb.Enums.Language.enUS, true);
            
            ViewData["card"] = card;
            ViewData["searchName"] = id;

            return View();
        }

        public ActionResult CardId(String id) //viewing a card
        {
            //look for a card using the name
            Card card = CardCollection.Instance.getById(id);// Cards.GetFromName(id HearthDb.Enums.Language.enUS, true);

            ViewData["card"] = card;
            ViewData["searchName"] = id;

            return View();
        }

        public ActionResult UserDecks(string id)
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            int uId = 0;
            
            //do we have an existing user?
            if (int.TryParse(id, out uId))
            {
                //try to pull the decks via user ID from the DB
                List<Deck> decks = DeckDAO.Instance.GetDecksByUser(uId);

                ViewBag.decks = decks;
            }
            else if (id == "Mine") //our deck
            {
                if (Session["UserSession"] == null)
                {
                    ((List<Notification>)Session["notifications"]).Add(new Notification("Error!", "You aren't logged in!", NotificationType.ERROR));
                    return Redirect("/");
                }

                //try to pull the decks via user ID from the DB
                List<Deck> decks = DeckDAO.Instance.GetDecksByUser(((User)Session["UserSession"]).ID);

                ViewBag.decks = decks;
            }

            return View();
        }
    }
}