namespace coursework.Models.DataModels
{
    public class Order
    {
        public int Id { get; set; }
        public int Id_client { get; set; }
        public int? id_employee { get; set; } // nullable
        public int Specialty { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; } // nullable
        public string NameClient { get; set; }
        public string NameEmployee { get; set; }
        public string SurnameClient { get; set; }
        public string SurnameEmployee { get; set; }
        public string Email { get; set; }
        public double? Cost { get; set; } // nullable
    }
}
