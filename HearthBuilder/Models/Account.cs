using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HearthBuilder.Models
{
    public class Account
    {
        public int ID { get; set; }
        public String Fname { get; set; }
        public String Lname { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }

        private MySqlConnection connection;

        public Account()
        {
            GetConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM user";

                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    String fName = reader.GetString("first_name");
                    reader.Close();
                }
                catch (MySqlException e)
                {
                }
            }
            catch (MySqlException e)
            {
            }

            connection.Close();
        }

    private void GetConnection()
        {
            connection = new MySqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;

            //if (db_manage_connnection.DB_Connect.OpenTheConnection(connection))
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
            }
        }
    }
}