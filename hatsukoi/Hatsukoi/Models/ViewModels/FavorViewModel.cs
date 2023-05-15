using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.ViewModels
{
    public class FavorViewModel
    {
        public List<FavProductViewModel> FavProducts { get; set; }
        public List<FavShopViewModel> FavShops { get; set; }
        public class FavProductViewModel
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public int UserId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
            public string ProductImg { get; set; }

            /// <summary>
            /// 1:未上架 2.已上架
            /// </summary>
            public int ProductStatus { get; set; }
            public DateTime Createtime { get; set; }
        }

        public class FavShopViewModel
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int SellerId { get; set; }
            public string ShopName { get; set; }
            public string SellerLogo { get; set; }
            public List<string> ProductImgs { get; set; }
        }



    }
}