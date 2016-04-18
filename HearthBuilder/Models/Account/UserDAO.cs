using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace HearthBuilder.Models.Account
{
    public class UserDAO
    {
        private MySqlConnection connection;
        private MySqlCommand cmd;
        private MySqlDataReader reader;

        public UserDAO() { }

        private void GetConnection()
        {
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            connection.Open();
        }

        public User GetAccountByEmailAndPassword(String email, String password)
        {
            User user = new User();

            try
            {
                GetConnection();
                cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT * FROM account WHERE email = '{0}' AND password = '{1}'", email, password);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //map values to user obj
                    user.ID = Convert.ToInt32(reader.GetString("account_id"));
                    user.Fname = reader.GetString("first_name");
                    user.Lname = reader.GetString("last_name");
                    user.Email = reader.GetString("email");
                    user.Password = reader.GetString("password");
                }
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            reader.Close();
            connection.Close();
            return user;
        }
    }
}