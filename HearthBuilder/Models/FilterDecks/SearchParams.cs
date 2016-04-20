using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HearthBuilder.Models.Cards;

namespace HearthBuilder.Models.FilterDecks
{
    public class SearchParams
    {
        public List<ClassNames> Classes { get; set; }
        public String DeckName { get; set; }

        public SearchParams()
        {
            Classes = new List<ClassNames>
            {
                new ClassNames { Name = "Druid" },
                new ClassNames { Name = "Hunter" },
                new ClassNames { Name = "Mage" },
                new ClassNames { Name = "Paladin" },
                new ClassNames { Name = "Priest" },
                new ClassNames { Name = "Rogue" },
                new ClassNames { Name = "Shaman" },
                new ClassNames { Name = "Warlock" },
                new ClassNames { Name = "Warrior" },
            };
        }
    }
}