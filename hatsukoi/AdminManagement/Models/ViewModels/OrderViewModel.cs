namespace AdminManagement.Models.ViewModels
{
    public class OrderViewModel
    {

        public int OrderId { get; set; }
        //public int SellerId { get; set; }
        //public string SellerName { get; set; }

        public string Seller { get; set; }
        //public int UserId { get; set; }
        //public string UserName { get; set; }

        public string User { get; set; }
        public decimal TotalPrice { get; set; }

        public int? Payment { get; set; }
        public int Status { get; set; }

        public string CreateTime { get; set; }

        

        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
