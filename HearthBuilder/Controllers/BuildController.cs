using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Deck() //this will be actually building the deck 
        {
            return View();
        }
	}
}