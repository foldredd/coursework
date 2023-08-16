using coursework.DB;
using coursework.DB.RequestRoad;
using coursework.Models;
using coursework.Models.DataModels;
using coursework.Models.TriggerModel;
using coursework.Valid;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Diagnostics;
using System.Text.Json;

namespace coursework.Controllers {
    public class HomeController : Controller {
        // Поле для взаємодії з даними в базі та виконання дій над ними
        public Performer performer;

        // Поле для підключення до бази даних
        private readonly DataBaseConnect _db;

        // Конструктор, який приймає залежності для підключення до бази та виконавця
        public HomeController(DataBaseConnect db, Performer _performer) {
            _db = db;
            performer = _performer;
        }

        // Дія для відображення головної сторінки
        public IActionResult Index() {
            return View();
        }

        // Дія, що обробляє POST-запит на головну сторінку (логін/реєстрація)
        [HttpPost]
        public IActionResult Index(Models.DataModels.Client client, string button) {
            // Створення об'єкту для повідомлення про результат реєстрації/входу
            UserRegist userRegist = new UserRegist();
            userRegist.Result = "Помилка входу";

            switch (button)
            {
                case "Regist":
                    // Перенаправлення на сторінку реєстрації
                    return RedirectToAction("Regist");
                case "Log":
                    if (client.Login != null && client.Password != null)
                    {
                        LogValid validation = new LogValid();
                        var id = validation.Validate(client, performer);
                        if (id != null)
                        {
                            if (id.Login == null)
                            {
                                // Обробка помилки входу
                            }
                            else
                            {
                                // Серіалізація об'єкта користувача та збереження його у сесії
                                var clientJson = JsonSerializer.Serialize(id);
                                HttpContext.Session.SetString("HomeClient", clientJson);

                                // Перенаправлення користувача в залежності від його ролі
                                if (id.Role == "Client")
                                {
                                    return RedirectToAction("Profile", "ClientController");
                                }
                                if (id.Role == "employee")
                                {
                                    return RedirectToAction("Profile", "EmployeeController");
                                }
                                if (id.Role == "manager")
                                {
                                    return RedirectToAction("Profile", "ManagerController");
                                }
                            }
                        }
                    }
                    return View(userRegist);
                default:
                    return View(userRegist);
            }
        }

        // Дія для відображення сторінки реєстрації
        [HttpGet]
        public IActionResult Regist() {
            return View();
        }

        // Дія, що обробляє POST-запит на сторінці реєстрації
        [HttpPost]
        public IActionResult Regist(Models.DataModels.Client client) {
            // SQL-запит для вставки даних користувача
            string sql;
            UserRegist userRegist = new UserRegist();

            // Список ролей користувачів
            List<string> list = new List<string> { "manager", "employee", "client" };

            try
            {
                // Виконання SQL-запиту на додавання користувача
                sql = "INSERT INTO Client (login, password, name, surname, email) VALUES (@Login, @Password, @Name, @Surname, @Email)";
                performer.Execute(sql, new { Login = client.Login, Password = client.Password, Name = client.Name, Surname = client.Surname, Email = client.Email });
                userRegist.Result = "користувач зареєстрований";
                userRegist.IsRegistered = true;
            }
            catch (MySqlException ex)
            {
                // Обробка різних помилок при реєстрації
                if (ex.Number == 1644)
                {
                    userRegist.Result = "Такий користувач вже є";
                    userRegist.IsRegistered = false;
                }
                else
                {
                    userRegist.Result = "помилка реєстрації";
                    userRegist.IsRegistered = false;
                }
            }

            return View(userRegist);
        }

        // Дія для відображення сторінки помилок
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
