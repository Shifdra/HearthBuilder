using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.FilterDecks
{
    public class ClassNames
    {
        //public PlayerClass Class { get; private set; }
        public PlayerClasses PlayerClass { get; set; }
        public String PlayerClassStr { get { return EnumHelper<PlayerClasses>.GetDisplayValue(PlayerClass); } }
        public bool Checked { get; set; }

        public ClassNames(PlayerClasses PlayerClass)
        {
            this.PlayerClass = PlayerClass;
            Checked = false;
        }
    }
}