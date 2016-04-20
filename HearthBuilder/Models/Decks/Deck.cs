using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HearthDb.CardDefs;
using HearthDb.Enums;
using HearthBuilder.Models.Cards;

namespace HearthBuilder.Models.Decks
{
    public class Deck
    {
        public int Id { get; set; }
        public List<Card> Cards { get; private set; }
        public PlayerClasses Class { get; private set; }
        public string ClassStr { get { return char.ToUpper(Class.ToString().ToLower()[0]) + Class.ToString().ToLower().Substring(1); } } //converts "CLASS" to "Class"
        public string Title { get; set; }
        public int Likes { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        
        public Deck()
        {
            Id = 0;
            Cards = new List<Card>();
            Class = PlayerClasses.NONE;
            Title = "";
        }

        public Deck(PlayerClasses pClass)
        {
            Id = 0;
            Cards = new List<Card>();
            Class = pClass;
            Title = "";
        }

        public Deck(int id, PlayerClasses pClass)
        {
            Id = id;
            Cards = new List<Card>();
            Class = pClass;
            Title = "";
        }

        public void AddCard(Card card)
        {
            if (card == null)
                throw new DeckException("The card cannot be null!");

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

            //resort them
            SortCards();
        }

        public void RemoveCard(Card card)
        {
            if (card == null)
                throw new DeckException("The card cannot be null!");

            if (Cards.Count == 0)
                throw new DeckException("There are no cards in this deck to remove.");

            //remove the card
            foreach (Card aCard in Cards)
            {
                if (aCard.Id == card.Id) //we found a match
                {
                    Cards.Remove(card);
                    return; //dont keep looping (we dont want to remove other occurances, just the first)
                }
            }
        }

        private void SortCards()
        {
            //sort Alphabetically by Name, then by Cost
            List<Card> tmpCards = Cards.OrderBy(x => x.Cost).ThenBy(x => x.Name).ToList();
            Cards = tmpCards;
        }
        
    }
}