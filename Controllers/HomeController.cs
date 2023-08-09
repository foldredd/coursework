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

namespace coursework.Controllers
{

    public class HomeController : Controller
    {
        public Performer performer;
        private readonly DataBaseConnect _db;

        public HomeController(DataBaseConnect db, Performer _performer)
        {
            _db = db;
            performer = _performer;
            //performer.Connection = _db.getConnection();

        }


        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Models.DataModels.Client client,string button) {
            UserRegist userRegist = new UserRegist();
            userRegist.Result = "Помилка входу";
            switch (button) {
                case "Regist":
                    return RedirectToAction("Regist");
                case "Log":
                    if (client.Login!=null && client.Password != null){
                        LogValid validation = new LogValid();
                        var id=validation.Validate(client, performer);
                        if (id !=null)
                        {


                            if (id.Login == null)
                            {

                            }
                            else
                            {
                                var clientJson = JsonSerializer.Serialize(id);
                                HttpContext.Session.SetString("HomeClient", clientJson);
                                if (id.Role == "Client") {

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
        
       
        [HttpGet]
        public IActionResult Regist(){
            return View();
        }




        [HttpPost]
        public IActionResult Regist(Models.DataModels.Client client)
        {
            string sql;
            UserRegist userRegist = new UserRegist();
            List<string> list = new List<string> { "manager", "employee", "client" };
                try
                {
                    sql = "INSERT INTO Client (login, password, name, surname, email) VALUES (@Login, @Password, @Name, @Surname, @Email)";
                    performer.Execute(sql, new { Login = client.Login, Password = client.Password, Name = client.Name, Surname = client.Surname, Email = client.Email });
                userRegist.Result = "користувач зареєстрований";
                userRegist.IsRegistered= true;
                   
                }
            catch (MySqlException ex)
            {
                if (ex.Number == 1644) 
                {
                    userRegist.Result = "Такий користувач вже є";
                    userRegist.IsRegistered = false;
                    Console.WriteLine("Authentication failed");
                }
                else
                {
                    userRegist.Result = "помилка реєстрації";
                    userRegist.IsRegistered = false;
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }


            return View(userRegist);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}