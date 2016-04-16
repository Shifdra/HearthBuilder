using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HearthBuilder.Models;

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

        public ActionResult CardName(String id) //viewing a card
        {
            //look for a card using the name
            Card card = Cards.Instance.getByName(id);// Cards.GetFromName(id HearthDb.Enums.Language.enUS, true);
            
            ViewData["card"] = card;
            ViewData["searchName"] = id;

            return View();
        }

        public ActionResult CardId(String id) //viewing a card
        {
            //look for a card using the name
            Card card = Cards.Instance.getById(id);// Cards.GetFromName(id HearthDb.Enums.Language.enUS, true);

            ViewData["card"] = card;
            ViewData["searchName"] = id;

            return View();
        }
    }
}