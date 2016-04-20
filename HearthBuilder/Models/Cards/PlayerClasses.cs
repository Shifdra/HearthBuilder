using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models
{
    public enum PlayerClasses
    {
        [Description("None")]
        NONE,
        [Description("Druid")]
        DRUID,
        [Description("Hunter")]
        HUNTER,
        [Description("Mage")]
        MAGE,
        [Description("Paladin")]
        PALADIN,
        [Description("Priest")]
        PRIEST,
        [Description("Rogue")]
        ROGUE,
        [Description("Shaman")]
        SHAMAN,
        [Description("Warlock")]
        WARLOCK,
        [Description("Warrrior")]
        WARRIOR
    }
}

