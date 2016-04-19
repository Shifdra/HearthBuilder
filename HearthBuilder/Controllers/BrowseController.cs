using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthBuilder.Models.FilterDecks;
using HearthBuilder.Models.Cards;
using HearthBuilder.Models.Decks;
using HearthBuilder.Models.Notifications;

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
        public ActionResult Index(SearchParams searchParams)
        {
            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();

            DeckDAO deckDAO = DeckDAO.Instance;
            List<Deck> allDecks = deckDAO.GetAllDecks();

            List<Deck> filteredDecks = new List<Deck>();

            for(int i = 0; i < allDecks.Count; i++)
            {
                for (int j = 0; j < searchParams.Types.Count; j++)
                {
                    if (searchParams.Types[j].Checked && allDecks[i].Class.ToString() == searchParams.Types[j].Name.ToUpper())
                    {
                        filteredDecks.Add(allDecks[i]);
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
    }
}