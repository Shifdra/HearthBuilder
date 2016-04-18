using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HearthBuilder.Models.Cards
{
    //Singleton Deign
    public sealed class CardCollection
    {
        private static volatile CardCollection instance;
        private static object syncRoot = new Object();

        public static CardCollection Instance 
        {
            get 
            {
                lock (syncRoot) 
                {
                    if (instance == null)
                    {
                        instance = new CardCollection();
                    }
                }
                return instance;
            }
        }

        public List<Card> AllCards { get; private set; }

        private CardCollection()
        {
            //import the data from the db, 
            Dictionary<string, HearthDb.Card> dbCards = HearthDb.Cards.Collectible;

            List<Card> tmpCards = new List<Card>();

            
            foreach (KeyValuePair<string, HearthDb.Card> card in dbCards){
                tmpCards.Add(new Card(card.Value));
            }

            AllCards = new List<Card>();
            //sort Alphabetically by Name, then by Cost
            AllCards = tmpCards.OrderBy(x => x.Cost).ThenBy(x => x.Name).ToList();

        }

        public Card getByName(string name)
        {
            foreach (Card card in AllCards)
            {
                if (card.Name == name)
                    return card;
            }
            return null;
        }

        public Card getById(string id)
        {
            foreach (Card card in AllCards)
            {
                if (card.Id == id)
                    return card;
            }
            return null;
        }
        
    }
}