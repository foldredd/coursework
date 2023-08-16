using Microsoft.AspNetCore.Mvc;
using coursework.Models.DataModels;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using coursework.DB.RequestRoad;
using System.Collections.Generic;
using coursework.DB;

namespace coursework.Controllers {
    public class ClientController : Controller {
        // Поле для представлення даних клієнта
        private Client client;

        // Статичне поле для зберігання списку замовлень
        private static List<Order> orders;

        // Залежності для підключення до бази даних та виконавця
        private readonly DataBaseConnect _db;
        private readonly Performer performer;

        // Статичне поле для зберігання списку спеціалізацій
        private static List<string> specialties;

        // Конструктор, який приймає залежності для підключення до бази та виконавця
        public ClientController(DataBaseConnect db, Performer _performer) {
            _db = db;
            performer = _performer;
        }

        // Дія для відображення профілю клієнта (GET-запит)
        [HttpGet]
        [Route("ClientController/Profile")]
        public IActionResult Profile() {
            Console.WriteLine("getProf");

            // Перевірка, чи дані клієнта вже отримані з сесії
            if (client == null)
            {
                // Отримання даних клієнта з сесії та десеріалізація їх
                var clientJson = HttpContext.Session.GetString("HomeClient");
                client = JsonSerializer.Deserialize<Client>(clientJson);
                Console.WriteLine(client.Id);
            }

            // Передача об'єкту клієнта до представлення
            return View(client);
        }

        // Дія для відображення профілю клієнта (POST-запит)
        [HttpPost]
        [Route("ClientController/Profile")]
        public IActionResult Profile(string button) {
            // Отримання даних клієнта з сесії та десеріалізація їх
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);
            Console.WriteLine(client.Id);

            switch (button)
            {
                case "Add":
                    // Перенаправлення на сторінку додавання замовлення
                    return RedirectToAction("AddOrder");
                case "View":
                    // Перенаправлення на сторінку перегляду замовлень
                    return RedirectToAction("ViewOrders");
                default:
                    Console.WriteLine("def");
                    // Відображення профілю клієнта
                    return View(client);
            }
        }

        // Дія для відображення сторінки додавання замовлення (GET-запит)
        [HttpGet]
        [Route("ClientController/AddOrder")]
        public IActionResult AddOrder() {
            // SQL-запит для отримання спеціалізацій
            string sql = "SELECT specialty FROM specialty";
            specialties = performer.ListQueryFirstOrDefault(sql);
            // Передача списку спеціалізацій до представлення
            return View(specialties);
        }

        // Дія для відправлення замовлення (POST-запит)
        [HttpPost]
        [Route("ClientController/AddOrder")]
        public IActionResult AddOrder(string specialty, string button, string description) {
            // Отримання даних клієнта з сесії та десеріалізація їх
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);

            // Перевірка вибраної спеціалізації
            if (!string.IsNullOrEmpty(specialty))
            {
                int i = 0;
                // Знаходження індексу вибраної спеціалізації у списку
                foreach (var spec in specialties)
                {
                    i++;
                    if (spec == specialty)
                    {
                        break;
                    }
                }

                // SQL-запит на додавання замовлення
                string sql = "INSERT INTO `Order` (id_client, specialty, description,`date`) VALUES (@IdClient, @Specialty, @Description,@Date)";
                performer.QueryFirstOrDefault(sql, new { IdClient = client.Id, Specialty = i, Description = description, Date = DateTime.Now });

                // Перенаправлення на сторінку профілю клієнта
                return RedirectToAction("Profile");
            }
            return View();
        }

        // Дія для відображення сторінки перегляду замовлень
        [HttpGet]
        [Route("ClientController/ViewOrders")]
        public IActionResult ViewOrders() {
            // Створення списку для зберігання замовлень
            orders = new List<Order>();

            // Отримання даних клієнта з сесії та десеріалізація їх
            var clientJson = HttpContext.Session.GetString("HomeClient");
            client = JsonSerializer.Deserialize<Client>(clientJson);

            // SQL-запит на отримання замовлень
            string sql = "SELECT * FROM `order` WHERE id_client=@client";
            orders.AddRange(performer.ListOrder(sql, new { client = client.Id }));

            // SQL-запит на отримання замовлень у процесі виконання
            sql = "SELECT * FROM orders_in_progress WHERE id_client=@client";
            orders.AddRange(performer.ListOrder(sql, new { client = client.Id }));

            // Передача списку замовлень до представлення
            return View(orders);
        }
    }
}
