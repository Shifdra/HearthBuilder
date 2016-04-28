using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HearthDb.CardDefs;
using HearthDb.Enums;

namespace HearthBuilder.Models.Cards
{
    public class Card
    {
        public string Url 
        {
            get { return "/Content/Images/cards/" + Id + ".png"; }
        }
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Text { get; private set; }
        public int Cost { get; private set; }
        public PlayerClasses Class { get; private set; }
        public Rarity Rarity { get; private set; }

        public Card(string Id, string Name, string Text, PlayerClasses Class, Rarity Rarity, int Cost)
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
                    Class = PlayerClasses.DRUID;
                    break;
                case CardClass.HUNTER:
                    Class = PlayerClasses.HUNTER;
                    break;
                case CardClass.MAGE:
                    Class = PlayerClasses.MAGE;
                    break;
                case CardClass.PALADIN:
                    Class = PlayerClasses.PALADIN;
                    break;
                case CardClass.PRIEST:
                    Class = PlayerClasses.PRIEST;
                    break;
                case CardClass.ROGUE:
                    Class = PlayerClasses.ROGUE;
                    break;
                case CardClass.SHAMAN:
                    Class = PlayerClasses.SHAMAN;
                    break;
                case CardClass.WARLOCK:
                    Class = PlayerClasses.WARLOCK;
                    break;
                case CardClass.WARRIOR:
                    Class = PlayerClasses.WARRIOR;
                    break;
                default:
                    Class = PlayerClasses.NONE;
                    break;
            }

            Rarity = card.Rarity;
            Cost = card.Cost;
        }


    }
}