using coursework.DB.RequestRoad;
using coursework.Models.DataModels;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Data;
using coursework.Models.TriggerModel;
using MySql.Data.MySqlClient;

namespace coursework.Valid
{
    public interface Validation
    {
        public static  List<Order> ValidateOrder(Employee employee,Performer performer)
        {
            string sql = "SELECT * From `order` WHERE specialty=@specialty";
           List<Order> list= performer.ListOrder(sql, new {specialty=employee.Specialty});
            foreach (Order order in list )
            {
                sql = "Select * FROM client WHERE id=@id";
                    User client = performer.QueryFirstOrDefault(sql, new {id=order.Id_client });
                order.NameClient = client.Name;
                order.SurnameClient = client.Surname;
            }
            return list;
        }

        public static void TakeOrder(Employee employee,Performer performer,int idOrder) {
            string sql = "SELECT * FROM `order` WHERE id=@Id";
            Order order = performer.TakeOrder(sql,new {Id=idOrder});
            
            sql = "DELETE FROM `order` WHERE id=@Id";
            performer.Delete(sql, new {Id=idOrder});
            order.id_employee = employee.Id;
            Console.WriteLine($"Spec={order.Specialty}");
            if (order.Specialty==1)
            {
                order.Cost= 400;
            }
            order.Date= DateTime.Now;
            sql = "INSERT INTO orders_in_progress(id_client, id_employee, specialty, description, cost, date) VALUES (@Id_client, @id_employee, @Specialty, @Description, @Cost, @Date)";
           
            performer.Execute(sql, order);
        }
        public  static List<Order> GetYourOrder(Employee employee, Performer performer) {
            string sql = "SELECT * FROM orders_in_progress_with_client_info WHERE id_employee=@Id";
            List<Order> list = performer.ListOrder(sql, new {Id=employee.Id});
            return list;
        }
        public static Order getOrder(int orderId,Performer performer)
        {
            string sql = "SELECT * From `order` WHERE id=@Id";
            return performer.GetOrder(sql, new {Id=orderId});
        }

        public static void CompleteOrder(Order order, Performer performer)
        {
            string sql = "DELETE FROM orders_in_progress WHERE id=@Id";
            Console.WriteLine($"ORDERID={order.Id}");
            performer.Delete(sql, new { Id = order.Id });
            sql = "INSERT INTO completed_orders(id_client,id_employee,specialty,description,cost,date) VALUES(@IdClient,@IdEmployee,@Specialty,@Description,@Cost,@Date)";
            performer.Execute(sql, new { IdClient = order.Id_client, IdEmployee = order.id_employee, Specialty = order.Specialty, Description = order.Description, Cost = order.Cost, Date=DateTime.Now });
        }
        public static Order getOrderInProgress(int orderId, Performer performer)
        {
            string sql = "SELECT * From orders_in_progress WHERE id=@Id";
            return performer.GetOrder(sql, new { Id = orderId });
        }
        public static UserRegist CreateEmployee(Employee employee,Performer performer)
        {
           UserRegist userRegist = new UserRegist();
            Console.WriteLine($"IDSPECEMP={employee.Email}");
            string sql = "INSERT INTO Employee (login,password,name,surname,email,specialty) VALUES (@Login,@Password,@Name,@Surname,@Email,@Specialty)";
            try
            {
                performer.Execute(sql, employee);
                userRegist.Result = "користувач зареєстрований";
                userRegist.IsRegistered = true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1644) // Код ошибки для сигнала
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
            return userRegist;

        }
        public static List<Employee> AllEmp(Performer performer)
        {
            string sql = "Select * FROM Employee";
            return performer.AllEmp(sql);
        }
        public static Employee getEmp(int empId,Performer performer) {
            string sql = "SELECT * FROM Employee WHERE id=@Id";
            Employee employee= performer.getEmp(sql, new { Id = empId });
            return employee;
        }
        public static CompliteOrdEmp ReportsEmpOrd(int employeeId, DateTime startDateTime, DateTime endDateTime, Performer performer)
        {
            string sql = "Complite_Orders_Employee";
            var parameters = new
            {
                employeeId,
                startDateTime,
                endDateTime
            };
            Console.WriteLine($"parameters={parameters.employeeId}+{parameters.startDateTime}+{parameters.endDateTime}");
            var result = performer.ReportsEmpOrd(employeeId, startDateTime, endDateTime);
            return result;
        }
        public static CompliteOrdEmp AllOrders(DateTime startDateTime, DateTime endDateTime, Performer performer)
        {
            string sql = "AllOrders";
            var parameters = new
            {
                startDateTime,
                endDateTime
            };
            CompliteOrdEmp result= performer.AllOrders(startDateTime,endDateTime);
            return result;
        }
        public static List<RatingSpec> GetRatingSpec(Performer performer)
        {
            string sql = "SELECT * FROM specialty_rating_view";
            return performer.GetRatingSpec(sql);
        }
        public static List<ClientOrder> GetClientOrder(Performer performer)
        {
            string sql = "SELECT * FROM client_order_view";
            return performer.GetClientOrder(sql);

        }
        public static void AddSpec(string spec,Performer performer)
        {
            string sql = "INSERT INTO specialty(specialty) VALUES(@Spec)";
            performer.AddSpec(sql, new {Spec=spec});
        }
    }
}
