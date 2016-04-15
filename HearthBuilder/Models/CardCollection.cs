using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models
{
    //Singleton Deign
    public sealed class Cards
    {
        private static volatile Cards instance;
        private static object syncRoot = new Object();

        public static Cards Instance 
        {
            get 
            {
                lock (syncRoot) 
                {
                    if (instance == null)
                    {
                        instance = new Cards();
                    }
                }
                return instance;
            }
        }

        public List<Card> AllCards { get; private set; }

        private Cards()
        {
            //import the data from the db, 
            Dictionary<string, HearthDb.Card> dbCards = HearthDb.Cards.Collectible;

            AllCards = new List<Card>();
            foreach (KeyValuePair<string, HearthDb.Card> card in dbCards){
                AllCards.Add(new Card(card.Value));
            }
        }

        public Card getByName(string name)
        {
            IEnumerable<Card> results = AllCards.Where(card => card.Name == name);

            //if we found something, return it
            if (results.Count() == 1)
                return results.ElementAt(0);
            else
                return null;
        }

        public Card getById(string id)
        {
            IEnumerable<Card> results = AllCards.Where(card => card.Id == id);

            //if we found something, return it
            if (results.Count() != 1)
                return results.ElementAt(0);
            else
                return null;
        }

    }
}