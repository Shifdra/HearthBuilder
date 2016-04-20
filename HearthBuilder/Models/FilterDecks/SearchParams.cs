using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.FilterDecks
{
    public class SearchParams
    {
        public List<PlayerClass> Classes { get; set; }
        public String DeckName { get; set; }

        public SearchParams() { }
    }
}