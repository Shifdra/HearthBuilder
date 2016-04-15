using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HearthDb.CardDefs;
using HearthDb.Enums;

namespace HearthBuilder.Models
{
    public class Card
    {
        public string Url 
        {
            get { return "//wow.zamimg.com/images/hearthstone/cards/enus/original/" + Id + ".png"; }
        }
        public string UrlGold
        {
            get { return "//wow.zamimg.com/images/hearthstone/cards/enus/animated/" + Id + "_premium.gif"; }
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public CardClass Class { get; set; }
        public Rarity Rarity { get; set; }

        public Card(string Id, string Name, string Text, CardClass Class, Rarity Rarity)
        {
            this.Id = Id;
            this.Name = Name;
            this.Text = Text;
            this.Class = Class;
            this.Rarity = Rarity;
        }

        public Card(HearthDb.Card card) 
        {
            Id = card.Id;
            Name = card.Name;
            Text = card.Text;
            Class = card.Class;
            Rarity = card.Rarity;
        }


    }
}