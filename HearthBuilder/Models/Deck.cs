using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HearthDb.CardDefs;
using HearthDb.Enums;

namespace HearthBuilder.Models
{
    public class Deck
    {
        public List<Card> Cards { get; private set; }
        public PlayerClass Class { get; private set; }
        public string Title { get; set; }

        public Deck(PlayerClass pClass)
        {
            Class = pClass;
        }

        public Deck(List<Card> cards, PlayerClass pClass, string title)
        {
            Cards = cards;
            Class = pClass;
            Title = title;
        }

        public void AddCard(Card card)
        {
            //check size limit
            if (Cards.Count >= 30) 
                throw new DeckException("You cannot have more than 30 cards in a deck.");

            //calculate occurrances of this card in the deck
            int occurrance = 0;

            foreach (Card aCard in Cards)
            {
                if (aCard.Id == card.Id) //we found a match
                    occurrance++;
            }

            //we can only have 1 legendary max
            if (card.Rarity == Rarity.LEGENDARY && occurrance == 1) 
                throw new DeckException("You cannot have more than 1 of the same legendary in a deck.");

            //we can only have 2 of any card max
            if (card.Rarity != Rarity.LEGENDARY && occurrance == 2)
                throw new DeckException("You cannot have more than 2 of the same card in a deck.");

            //add the card to the deck
            Cards.Add(card);

        }
        
    }
}