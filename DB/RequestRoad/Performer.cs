using Dapper;
using MySql.Data.MySqlClient;
using coursework.Models.DataModels;
using System.Text.Json.Serialization;
using System.Data.Common;
using System.Data;

namespace coursework.DB.RequestRoad
{
    [Serializable]
    public class Performer
    {
        private MySqlConnection _connection;
        private DataBaseConnect _db;
        public MySqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
        public string id { get; set; }

        [JsonConstructor]
        public Performer(DataBaseConnect db)
        {
            _db = db;
            _connection = _db.getConnection();
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return _connection.Query<T>(sql, param);
        }

        public int Execute(string sql, object param = null)
        {
            return _connection.Execute(sql, param);
        }
        public User QueryFirstOrDefault(string sql, object param = null)
        {
            User client = _connection.QueryFirstOrDefault<User>(sql, param);

            return client;
        }
        public List<string> ListQueryFirstOrDefault(string sql, object param = null)
        {
            List<string> strings = _connection.Query<string>(sql).AsList();
            return strings;
        }
        public List<Order> ListOrder(string sql,object param = null)
        {
            return _connection.Query<Order>(sql, param).ToList();
        }
        public Order TakeOrder(string sql,object param = null)
        {
           return _connection.QueryFirstOrDefault<Order>(sql, param);
        }
        public void Delete(string sql,object param = null)
        {
            _connection.Execute(sql, param);
        }
        public Order GetOrder(string sql,object param = null)
        {
            return _connection.QueryFirstOrDefault<Order>(sql,param);
        }
        public List<Employee> AllEmp(string sql, object param=null)
        {

            return _connection.Query<Employee>(sql, param).ToList();
        }

        public Employee getEmp(string sql, object param = null)
        {
            Employee employee = _connection.QueryFirstOrDefault<Employee>(sql, param);
            return employee;
        }
        public CompliteOrdEmp ReportsEmpOrd(int empId, DateTime start, DateTime end)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", empId, DbType.Int32);
            parameters.Add("@startDateTime", start, DbType.DateTime);
            parameters.Add("@endDateTime", end, DbType.DateTime);

            var result = _connection.QueryFirstOrDefault<CompliteOrdEmp>("Complite_Orders_Employee", parameters, commandType: CommandType.StoredProcedure);

            return result;
        }
        public CompliteOrdEmp AllOrders(DateTime start, DateTime end)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDateTime", start, DbType.DateTime);
            parameters.Add("@endDateTime", end, DbType.DateTime);
            var result = _connection.QueryFirstOrDefault<CompliteOrdEmp>("AllOrders", parameters, commandType: CommandType.StoredProcedure);
            return result;

        }
        public List<RatingSpec> GetRatingSpec(string sql)
        {
            return _connection. Query<RatingSpec>(sql).ToList();
        }
        public List<ClientOrder> GetClientOrder(string sql)
        {
            return _connection.Query<ClientOrder>(sql).ToList();
        }
        public void AddSpec(string sql, object param =null)
        {
            _connection.Execute(sql,param);
        }

    }


}