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
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Text { get; private set; }
        public int Cost { get; private set; }
        public PlayerClass Class { get; private set; }
        public Rarity Rarity { get; private set; }

        public Card(string Id, string Name, string Text, PlayerClass Class, Rarity Rarity, int Cost)
        {
            this.Id = Id;
            this.Name = Name;
            this.Text = Text;
            this.Class = Class;
            this.Rarity = Rarity;
            this.Cost = Cost;
        }

        public Card(HearthDb.Card card) 
        {
            Id = card.Id;
            Name = card.Name;
            Text = card.Text;

            //use our own Card Class design
            switch (card.Class)
            {
                case CardClass.DRUID:
                    Class = PlayerClass.DRUID;
                    break;
                case CardClass.HUNTER:
                    Class = PlayerClass.HUNTER;
                    break;
                case CardClass.MAGE:
                    Class = PlayerClass.MAGE;
                    break;
                case CardClass.PALADIN:
                    Class = PlayerClass.PALADIN;
                    break;
                case CardClass.PRIEST:
                    Class = PlayerClass.PRIEST;
                    break;
                case CardClass.ROGUE:
                    Class = PlayerClass.ROGUE;
                    break;
                case CardClass.SHAMAN:
                    Class = PlayerClass.SHAMAN;
                    break;
                case CardClass.WARLOCK:
                    Class = PlayerClass.WARLOCK;
                    break;
                case CardClass.WARRIOR:
                    Class = PlayerClass.WARRIOR;
                    break;
                default:
                    Class = PlayerClass.NONE;
                    break;
            }

            Rarity = card.Rarity;
            Cost = card.Cost;
        }


    }
}