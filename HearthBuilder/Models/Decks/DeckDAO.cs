﻿using HearthBuilder.Models.Cards;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models.Decks
{
    public sealed class DeckDAO
    {
        private static DeckDAO instance;
        private static object syncRoot = new Object();
        
        public static DeckDAO Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DeckDAO();
                    }
                }
                return instance;
            }
        }

        private DeckDAO()
        {
        }

        public Deck GetDeckById(int id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
                {
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM decks WHERE id = @id", connection);
                    cmd1.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    Deck deck = new Deck();
                    using (MySqlDataReader deckReader = cmd1.ExecuteReader())
                    {
                        if (deckReader.Read())
                        {
                            //map values to deck obj
                            deck = new Deck(id, (PlayerClass)Enum.Parse(typeof(PlayerClass), deckReader.GetString("class"), true));
                            var titleOrdinal = deckReader.GetOrdinal("title");
                            if (!deckReader.IsDBNull(titleOrdinal))
                                deck.Title = deckReader.GetString(titleOrdinal);
                            deck.Likes = deckReader.GetInt32("likes");
                            deck.UserId = deckReader.GetInt32("account_id");
                        }
                    }
                    
                    //now get the cards
                    MySqlCommand cmd2 = new MySqlCommand("SELECT card_id FROM deck_cards WHERE deck_id = @id", connection);
                    cmd2.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader cardsReader = cmd2.ExecuteReader())
                    {
                        //add each card to the deck
                        while (cardsReader.Read())
                        {
                            deck.AddCard(CardCollection.Instance.getById(cardsReader.GetString("card_id")));
                        }
                        
                        return deck;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int CreateNewDeck(Deck deck)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format("INSERT INTO decks (`class`, `account_id`) VALUES (@pClass, @userId)");
                    cmd.Parameters.AddWithValue("@pClass", deck.Class.ToString());
                    cmd.Parameters.AddWithValue("@userId", deck.UserId);
                    cmd.ExecuteNonQuery();
                    int deckid = 0;
                    int.TryParse(cmd.LastInsertedId.ToString(), out deckid); //get the mysql generated last ID

                    return deckid;
                }
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public Deck UpdateDeck(Deck deck)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
            {
                connection.Open();

                if (deck.Id == 0) // if we have a "new" deck
                {
                    deck.Id = CreateNewDeck(deck); //get the new deck id from the DB, after adding a fresh one
                    System.Diagnostics.Debug.WriteLine("UpdateDeck() -> inserting a new Deck: " + deck.Id + " with card count " + deck.Cards.Count);
                }
                else
                    System.Diagnostics.Debug.WriteLine("UpdateDeck() -> updating an old Deck: " + deck.Id + " with card count " + deck.Cards.Count);

                MySqlCommand cmd1 = new MySqlCommand("UPDATE decks SET title = @title WHERE id = @id", connection);
                cmd1.Parameters.AddWithValue("@title", deck.Title);
                cmd1.Parameters.AddWithValue("@id", deck.Id);
                cmd1.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("DELETE FROM deck_cards WHERE deck_id = @id", connection);
                cmd2.Parameters.AddWithValue("@id", deck.Id);
                cmd2.ExecuteNonQuery();

                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    MySqlCommand cmd3 = connection.CreateCommand();
                    cmd3.Transaction = transaction;
                    cmd3.CommandType = System.Data.CommandType.Text;
                    cmd3.CommandText = "INSERT INTO deck_cards (deck_id, card_id) VALUES (@deckid, @cardId);";
                    cmd3.Parameters.Add("@deckId", MySqlDbType.Int32);
                    cmd3.Parameters.Add("@cardId", MySqlDbType.VarChar);
                    try
                    {
                        foreach (Card card in deck.Cards)
                        {
                            cmd3.Parameters[0].Value = deck.Id;
                            cmd3.Parameters[1].Value = card.Id;
                            cmd3.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        connection.Close();
                        throw e;
                    }
                }
            }
            return deck;
        }

        public void DeleteDeck(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd1 = new MySqlCommand("DELETE FROM decks WHERE id = @id", connection);
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.ExecuteNonQuery();

                    MySqlCommand cmd2 = new MySqlCommand("DELETE FROM deck_cards WHERE deck_id = @id", connection);
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    throw e;
                }
                connection.Close();
            }
        }

        public List<Deck> GetAllDecks()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
                {
                    MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM decks", connection);
                    List<Deck> decks = new List<Deck>();
                    connection.Open();
                    using (MySqlDataReader deckReader = cmd1.ExecuteReader())
                    {
                        while (deckReader.Read())
                        {
                            //map values to deck obj
                            Deck deck = new Deck(deckReader.GetInt32("id"), (PlayerClass)Enum.Parse(typeof(PlayerClass), deckReader.GetString("class"), true));
                            var titleOrdinal = deckReader.GetOrdinal("title");
                            if (!deckReader.IsDBNull(titleOrdinal))
                                deck.Title = deckReader.GetString(titleOrdinal);
                            deck.Likes = deckReader.GetInt32("likes");
                            decks.Add(deck);
                        }
                    }

                    foreach(Deck deck in decks)
                    {
                        //now get the cards
                        MySqlCommand cmd2 = new MySqlCommand("SELECT card_id FROM deck_cards WHERE deck_id = @id", connection);
                        cmd2.Parameters.AddWithValue("@id", deck.Id);
                        MySqlDataReader cardsReader = cmd2.ExecuteReader();
                        using (cardsReader)
                        {
                            //add each card to the deck
                            while (cardsReader.Read())
                            {
                                deck.AddCard(CardCollection.Instance.getById(cardsReader.GetString("card_id")));
                            }
                        }
                        cardsReader.Close();
                    }
                    
                    return decks;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}