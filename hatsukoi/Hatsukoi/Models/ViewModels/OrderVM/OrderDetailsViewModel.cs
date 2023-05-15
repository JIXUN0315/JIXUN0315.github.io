namespace Hatsukoi.Models.ViewModels.OrderVM
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }
        public int OrderId { get; set; }

        public int ProductSpecificationId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public string SellerName { get; set; } = null!;

        public string ProductName { get; set; } = null!;


        public string FirstSepcItem { get; set; } = null!;


        public string? SecondSepcItem { get; set; }

        public string ProductImg { get; set; } = null!;

        

    }
}
