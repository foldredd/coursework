using coursework.DB.RequestRoad;
using coursework.Models.DataModels;

namespace coursework.Valid {
    public class LogValid {
        // Метод для перевірки автентифікації клієнта та визначення його ролі
        public User Validate(Client client, Performer performer) {
            List<string> list = new List<string> { "manager", "employee", "Client" };
            string sql;
            int a = 0;

            // Проходження через різні ролі для перевірки автентифікації
            foreach (string role in list)
            {
                sql = $"SELECT * FROM {role} WHERE login = @Login AND password = @Password";
                var user = performer.QueryFirstOrDefault(sql, new { client.Login, client.Password });

                if (user == null)
                {
                    // Автентифікація не вдалася для даної ролі
                }
                else
                {
                    Console.WriteLine(role);
                    user.Role = role;
                    Console.WriteLine($"Emp = {user.Specialty}");
                    return user; // Повертаємо автентифікованого користувача з визначеною роллю
                }
            }

            return null; // Повертаємо null, якщо автентифікація не вдалася для жодної ролі
        }
    }
}
