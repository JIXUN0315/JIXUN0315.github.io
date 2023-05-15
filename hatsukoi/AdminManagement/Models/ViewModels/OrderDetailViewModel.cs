namespace AdminManagement.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set;}
        public string ProductName { get; set;}

        public string Spec { get; set;}

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
