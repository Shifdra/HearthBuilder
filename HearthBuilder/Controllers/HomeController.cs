using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HearthBuilder.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        //what other paths in /Home/? will we need?
        //Home/contact??
        //I'd prefer to use other controllers:
        //Build/
        //View/
        //View/Deck
        //etc
	}
}