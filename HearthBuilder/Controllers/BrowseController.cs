using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Card() //viewing a card
        {
            return View();
        }
	}
}