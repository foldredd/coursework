using coursework.DB.RequestRoad;
using coursework.Models.DataModels;

namespace coursework.Valid
{
    public class LogValid
    {
        public User Validate(Client client, Performer performer)
        {
            List<string> list = new List<string> { "manager", "employee", "Client" };
            string sql;
            int a=0;
            foreach (string role in list)
            {
                sql = $"SELECT * FROM {role} WHERE login = @Login AND password = @Password";
                var user = performer.QueryFirstOrDefault(sql, new { client.Login, client.Password });



                if (user == null)
                {

                }
                else
                {
                    Console.WriteLine(role);
                    user.Role= role;
                    Console.WriteLine($"Emp = {user.Specialty}");
                    return user;
                }
            }
            return null;

        }
    }
}
