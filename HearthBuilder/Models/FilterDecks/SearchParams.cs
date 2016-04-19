using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.FilterDecks
{
    public class SearchParams
    {
        public List<ClassTypes> Types { get; set; }
        public String DeckName { get; set; }


        public SearchParams()
        {
            Types = new List<ClassTypes>
            {
                new ClassTypes { Name = "Druid" },
                new ClassTypes { Name = "Hunter" },
                new ClassTypes { Name = "Mage" },
                new ClassTypes { Name = "Paladin" },
                new ClassTypes { Name = "Priest" },
                new ClassTypes { Name = "Rogue" },
                new ClassTypes { Name = "Shaman" },
                new ClassTypes { Name = "Warlock" },
                new ClassTypes { Name = "Warrior" },
            };
        }
    }
}