using System.Diagnostics.Eventing.Reader;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using coursework.DB.RequestRoad;
using Org.BouncyCastle.Asn1.Mozilla;

namespace coursework.DB {
    // Клас для з'єднання з базою даних
    public class DataBaseConnect {
        // Параметри для з'єднання з базою даних
        public String login = "root";            // Логін користувача бази даних
        private String password = "root";        // Пароль користувача бази даних
        private String database = "coursework";  // Назва бази даних

        String connectionString;                  // Рядок підключення до бази даних
        MySqlConnection connection;               // З'єднання з базою даних

        public Performer _Performer { get; }      // Об'єкт для виконання запитів до бази даних

        // Конструктор класу
        public DataBaseConnect() {
            // Формування рядка підключення до бази даних
            connectionString = $"Server=localhost;Database={database};Uid={login};Pwd={password};";
            try
            {
                // Спроба встановити з'єднання з базою даних
                connection = new MySqlConnection(connectionString);
                connection.Open(); // Відкриття з'єднання
                Console.WriteLine("Connection DB true");
                // Виведення повідомлення, що з'єднання встановлено успішно
            }
            catch (MySqlException ex)
            {
                // Виведення повідомлення про невдале з'єднання
                Console.WriteLine("Connection DB false");
            }
        }

        // Метод для отримання з'єднання до бази даних
        public MySqlConnection getConnection() {
            return new MySqlConnection(connectionString);
        }
    }
}
