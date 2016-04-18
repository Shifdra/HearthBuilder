using HearthBuilder.Models.Cards;
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
        private MySqlConnection connection;

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
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
        }

        public Deck GetDeckById(int id)
        {
            try
            {
                connection.Open();
                using (MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM decks WHERE id = @id", connection))
                {
                    cmd1.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.Read())
                    {
                        //map values to user obj
                        Deck deck = new Deck(id, (PlayerClass)Enum.Parse(typeof(PlayerClass), reader.GetString("class"), true));
                        var titleOrdinal = reader.GetOrdinal("title");
                        if (!reader.IsDBNull(titleOrdinal))
                            deck.Title = reader.GetString(titleOrdinal);
                        deck.Likes = reader.GetInt32("likes");
                        reader.Close();

                        //now get the cards
                        using (MySqlCommand cmd2 = new MySqlCommand("SELECT card_id FROM deck_cards WHERE deck_id = @id", connection))
                        {
                            cmd2.Parameters.AddWithValue("@id", id);
                            MySqlDataReader cardsReader = cmd2.ExecuteReader();

                            //add each card to the deck
                            while (cardsReader.Read())
                            {
                                deck.AddCard(CardCollection.Instance.getById(cardsReader.GetString("card_id")));
                            }
                            cardsReader.Close();
                        }
                        connection.Close();
                        return deck;
                    }
                    reader.Close(); //this is needed again, if we couldnt find a deck
                }
            }
            catch (Exception e)
            {
                connection.Close();
                throw e;
            }
            connection.Close();
            return null;
        }

        public Deck CreateNewDeck(string pClass)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("INSERT INTO decks (`class`) VALUES (@pClass)");
                cmd.Parameters.AddWithValue("@pClass", pClass);
                cmd.ExecuteNonQuery();
                int deckid = 0;
                int.TryParse(cmd.LastInsertedId.ToString(), out deckid); //get the mysql generated last ID

                connection.Close();
                return new Deck(deckid, (PlayerClass)Enum.Parse(typeof(PlayerClass), pClass, true));
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return null;
        }

        public void UpdateDeck(Deck deck)
        {
            connection.Open();
            using (MySqlCommand cmd1 = new MySqlCommand("UPDATE decks SET title = @title WHERE id = @id", connection))
            {
                cmd1.Parameters.AddWithValue("@title", deck.Title);
                cmd1.Parameters.AddWithValue("@id", deck.Id);
                cmd1.ExecuteNonQuery();
            }
            using (MySqlCommand cmd2 = new MySqlCommand("DELETE FROM deck_cards WHERE deck_id = @id", connection))
            {
                cmd2.Parameters.AddWithValue("@id", deck.Id);
                cmd2.ExecuteNonQuery();
            }

            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                using (MySqlCommand cmd3 = connection.CreateCommand())
                {
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
                        throw;
                    }
                }
            }
            connection.Close();
        }

        public void DeleteDeck(int id)
        {
            connection.Open();
            try
            {
                
                using (MySqlCommand cmd1 = new MySqlCommand("DELETE FROM decks WHERE id = @id", connection))
                {
                    cmd1.Parameters.AddWithValue("@id", id);
                    cmd1.ExecuteNonQuery();
                }
                using (MySqlCommand cmd2 = new MySqlCommand("DELETE FROM deck_cards WHERE deck_id = @id", connection))
                {
                    cmd2.Parameters.AddWithValue("@id", id);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw;
            }
            connection.Close();
        }

    }
}