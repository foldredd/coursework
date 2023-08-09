namespace coursework.Models.DataModels
{
    public class Employee:User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Specialty { get; set; }
        public string SpecialtyName { get; set; }
       
    }
}
