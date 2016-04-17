using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace HearthBuilder.Models.Account
{
    public class UserDAO
    {
        private MySqlConnection connection;

        public UserDAO() { }

        private void GetConnection()
        {
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            connection.Open();
        }

        public User GetAccountByEmail(String email)
        {
            User user = new User();

            try
            {
                GetConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("SELECT * FROM account WHERE email = '{0}'", email);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    //map values to user obj
                    user.ID = Convert.ToInt32(reader.GetString("account_id"));
                    user.Fname = reader.GetString("first_name");
                    user.Lname = reader.GetString("last_name");
                    user.Email = reader.GetString("email");
                    user.Password = reader.GetString("password");
                }
                else
                {
                    //did not return anything
                }
                reader.Close();
                connection.Close();
            }
            catch (MySqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return user;
        }
    }
}