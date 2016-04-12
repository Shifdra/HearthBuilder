using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthDb;

namespace HearthBuilder.Controllers
{
    public class BrowseController : Controller
    {
        //
        // GET: /Browse/
        public ActionResult Index() //this is the main page to browse/search decks
        {
            return View();
        }

        public ActionResult Deck() //viewing an individual deck
        {
            return View();
        }

        public ActionResult Card(String name) //viewing a card
        {
            //look for a card using the name

            Card card = Cards.GetFromName(name, HearthDb.Enums.Language.enUS, true);

            ViewData["searchName"] = name;

            ViewData["card"] = card;

            return View();
        }
	}
}