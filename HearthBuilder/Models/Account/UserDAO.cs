using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HearthBuilder.Models.Account
{
    public sealed class UserDAO
    {
        private static UserDAO instance;
        private static object syncRoot = new Object();

        private MySqlConnection connection;
        private MySqlCommand cmd;
        private MySqlDataReader reader;

        private UserDAO()
        {
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
        }

        public static UserDAO Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                }
                return instance;
            }
        }

        static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }

        public User GetAccountByEmailAndPassword(User user)
        {
            String passHash = Hash(user.Email + ":" + user.Password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
                {
                    connection.Open();
                    cmd = new MySqlCommand("SELECT * FROM account WHERE email = @email AND password = @passHash", connection);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@passHash", passHash);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //map values to user obj
                            user.ID = Convert.ToInt32(reader.GetString("account_id"));
                            user.Fname = reader.GetString("first_name");
                            user.Lname = reader.GetString("last_name");
                            user.Email = reader.GetString("email");
                            user.Password = reader.GetString("password");

                            return user;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }

        public User RegisterUser(User user)
        {
            String passHash = Hash(user.Email + ":" + user.Password);

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString))
                {
                    connection.Open();
                    cmd = new MySqlCommand("INSERT INTO account (first_name, last_name, email, password) VALUES (@fname, @lname, @email, @passHash)", connection);
                    cmd.Parameters.AddWithValue("@fname", user.Fname);
                    cmd.Parameters.AddWithValue("@lname", user.Lname);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@passHash", passHash);
                    cmd.ExecuteNonQuery();

                    return user;
                }
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }
    }
}