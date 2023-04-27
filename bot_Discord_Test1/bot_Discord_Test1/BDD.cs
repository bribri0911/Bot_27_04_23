using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace bot_Discord_Test1
{
    
    internal class BDD
    {
        private string connectionString = "server=localhost;port=3306;database=brieu2017601;uid=brieu2017601;password=wA7$@92UjVKQSEj;";

        public void ConnectionBDD()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM BBD_Lien";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                string test = "";
                while (reader.Read())
                {
                    test += reader["Lien"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }



    }
}
