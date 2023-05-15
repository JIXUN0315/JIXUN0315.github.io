namespace AdminManagement.Models.ViewModels
{
    public class MemberViewModel
    {
#nullable disable
        public int Id { get; set; }

        public string Gender { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Account { get; set; }

        //public List<decimal> Price { get; set; }
        public decimal  Price { get; set; }
    }
}
