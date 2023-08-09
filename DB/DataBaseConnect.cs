using System.Diagnostics.Eventing.Reader;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using coursework.DB.RequestRoad;
using Org.BouncyCastle.Asn1.Mozilla;

namespace coursework.DB{
    public class DataBaseConnect
    {
        public String login="root";
         private String password="root";
        private String database="coursework";
        String connectionString;
        MySqlConnection connection;
        public Performer _Performer { get;}
      public DataBaseConnect(){

            connectionString = $"Server=localhost;Database={database};Uid={login};Pwd={password};";
            try
            {
                     connection = new MySqlConnection(connectionString);
                     
                    connection.Open();
                    Console.WriteLine("Connection DB true");
                    // Подключение установлено успешно
                
            }
            catch (MySqlException ex)
            {
                    Console.WriteLine(" Connection DB false");
               
            }


        }
        public MySqlConnection getConnection()
        {
            return new MySqlConnection(connectionString);
        }

    }
}
