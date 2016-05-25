using HearthBuilder.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HearthBuilder.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Session["notifications"] == null)
                Session["notifications"] = new List<Notification>();
        }
    }
}