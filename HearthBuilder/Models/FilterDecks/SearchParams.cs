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
            foreach (PlayerClasses pClass in Enum.GetValues(typeof(PlayerClasses)))
            {
                if (pClass != PlayerClasses.NONE)
                    Classes.Add(new ClassNames(pClass.ToString()));
            }
        }
    }
}