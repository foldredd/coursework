using Dapper;
using MySql.Data.MySqlClient;
using coursework.Models.DataModels;
using System.Text.Json.Serialization;
using System.Data.Common;
using System.Data;

namespace coursework.DB.RequestRoad {
    [Serializable]
    public class Performer {
        private MySqlConnection _connection;
        private DataBaseConnect _db;

        public MySqlConnection Connection {
            get { return _connection; }
            set { _connection = value; }
        }

        public string id { get; set; }

        // Конструктор, який приймає з'єднання з базою даних
        [JsonConstructor]
        public Performer(DataBaseConnect db) {
            _db = db;
            _connection = _db.getConnection();
        }

        // Метод для виконання запиту до бази даних та отримання результатів у вигляді списку об'єктів
        public IEnumerable<T> Query<T>(string sql, object param = null) {
            return _connection.Query<T>(sql, param);
        }

        // Метод для виконання SQL-запиту, який не повертає дані (INSERT, UPDATE, DELETE)
        public int Execute(string sql, object param = null) {
            return _connection.Execute(sql, param);
        }

        // Метод для отримання першого результату запиту, використовуючи Dapper
        public User QueryFirstOrDefault(string sql, object param = null) {
            User client = _connection.QueryFirstOrDefault<User>(sql, param);
            return client;
        }

        // Метод для отримання списку перших результатів запиту, які представлені рядками
        public List<string> ListQueryFirstOrDefault(string sql, object param = null) {
            List<string> strings = _connection.Query<string>(sql).AsList();
            return strings;
        }

        // Метод для отримання списку замовлень з бази даних з можливістю передачі параметрів
        public List<Order> ListOrder(string sql, object param = null) {
            return _connection.Query<Order>(sql, param).ToList();
        }

        // Метод для отримання першого замовлення з бази даних з можливістю передачі параметрів
        public Order TakeOrder(string sql, object param = null) {
            return _connection.QueryFirstOrDefault<Order>(sql, param);
        }

        // Метод для виконання SQL-запиту для видалення даних
        public void Delete(string sql, object param = null) {
            _connection.Execute(sql, param);
        }

        // Метод для отримання першого замовлення з бази даних з можливістю передачі параметрів
        public Order GetOrder(string sql, object param = null) {
            return _connection.QueryFirstOrDefault<Order>(sql, param);
        }

        // Метод для отримання списку всіх співробітників з бази даних з можливістю передачі параметрів
        public List<Employee> AllEmp(string sql, object param = null) {
            return _connection.Query<Employee>(sql, param).ToList();
        }

        // Метод для отримання співробітника з бази даних за його ідентифікатором
        public Employee getEmp(string sql, object param = null) {
            Employee employee = _connection.QueryFirstOrDefault<Employee>(sql, param);
            return employee;
        }

        // Метод для виклику збереженої процедури в базі даних та отримання результатів
        public CompliteOrdEmp ReportsEmpOrd(int empId, DateTime start, DateTime end) {
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", empId, DbType.Int32);
            parameters.Add("@startDateTime", start, DbType.DateTime);
            parameters.Add("@endDateTime", end, DbType.DateTime);

            var result = _connection.QueryFirstOrDefault<CompliteOrdEmp>("Complite_Orders_Employee", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        // Метод для отримання звіту про всі замовлення за певний період
        public CompliteOrdEmp AllOrders(DateTime start, DateTime end) {
            var parameters = new DynamicParameters();
            parameters.Add("@startDateTime", start, DbType.DateTime);
            parameters.Add("@endDateTime", end, DbType.DateTime);
            var result = _connection.QueryFirstOrDefault<CompliteOrdEmp>("AllOrders", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        // Метод для отримання списку рейтингів спеціалізацій
        public List<RatingSpec> GetRatingSpec(string sql) {
            return _connection.Query<RatingSpec>(sql).ToList();
        }

        // Метод для отримання списку замовлень клієнтів
        public List<ClientOrder> GetClientOrder(string sql) {
            return _connection.Query<ClientOrder>(sql).ToList();
        }

        // Метод для виконання SQL-запиту для додавання нових даних
        public void AddSpec(string sql, object param = null) {
            _connection.Execute(sql, param);
        }
    }
}
