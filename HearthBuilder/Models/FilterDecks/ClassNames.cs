using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.FilterDecks
{
    public class ClassNames
    {
        //public PlayerClass Class { get; private set; }
        public string PlayerClass { get; set; }
        public bool Checked { get; set; }

        public ClassNames(string PlayerClass)
        {
            this.PlayerClass = PlayerClass;
            Checked = false;
        }

        public ClassNames()
        {

        }
    }
}