using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using HearthDb;

namespace HearthBuilder
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Card leeroy = Cards.GetFromName("Leeroy Jenkins", HearthDb.Enums.Language.enUS, true);

            //throw new Exception("Card: " + leeroy.ToString());
        }
    }
}