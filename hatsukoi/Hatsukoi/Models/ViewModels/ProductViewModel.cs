using Microsoft.Build.Framework;


namespace Hatsukoi.Models.ViewModels
{
#nullable disable
    public class ProductViewModel
    {
        public int Id { get; set; }
        public int SellerId { get; set; }
        public string ShopName { get; set; }
        public string BrandName { get; set; }
        public string ProductFirstImg { get; set; }
        public IEnumerable<string> ProductImg { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int ProductStatus { get; set; }
        public string MadeCountry { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId{ get; set; }
        public string SubCategoryName{ get; set; }
        public string CategoryName { get; set; }
        public string Tag { get; set; }
        public string Editor { get; set; }
        public int ViewTimes { get; set; }

        //Tag
        public IEnumerable<string> TagNames { get; set; }

        //商品收藏人數
        public string FavPeople { get; set; }
        //商品關注人數
        public string FavShop { get; set; }

        //Order - SalesQty
        public string SalesQtyByShop { get; set; }
        public string SalesQtyByProduct { get; set; }

        //商店評論
        public int EvaluateAvg { get; set; } //商店評論平均分數
        public int EvaluateSum { get; set; } //商店評論總數量

        public IEnumerable<SpecData> SpecList { get; set; }

        public class ProductVM
        {
            public ProductViewModel Product { get; set; }
            public List<SimilarProductData> SimilarProdList { get; set; }
            public List<ReviewData> ReviewList { get; set; }
            public List<SellerData> SellerInfoList { get; set; }
            public List<ProductManageData> ProductManageList { get; set; }
            public List<RecommendShopData> RecommendShopList { get; set; }
        }

        //訂單評論用
        public class ReviewData 
        {
            public string ShopIcon { get; set; }
            public int UserId { get; set; }
            public string Account { get; set; } //使用者名稱
            public int Evaluate { get; set; } //單一訂單評分
            public DateTime EvaluateDate { get; set; }
            public string EvaluateDateCalculate { get; set; }
            public string EvaluateText { get; set; }
            public string ProductName { get; set; }
        }
        public class SimilarProductData
        {
            public int ProductId { get; set; }
            public string ProductFirstImg { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public string BrandName { get; set; }
            public int SubCategory { get; set; }
            public int SellerId { get; set; }

        }
        public class SpecData
        {
            public int SpecId { get; set; }
            public string FirstSpec { get; set; }
            public string FirstSpecItem { get; set; }
            public string SecondSpec { get; set; }
            public string SecondSpecItem { get; set; }
            public decimal UnitPrice { get; set; }
        }
        public class SellerData
        {
            public int Id { get; set; }
            public string ShopBannerRect { get; set; } = null!;
            public string Icon { get; set; } = null!;
            public string Logo { get; set; } = null!;
            public string BrandName { get; set; } = null!;
            public string ProductOrigin { get; set; } = null!;
            public string ShopName { get; set; } = null!;
            public string TaxIdNumber { get; set; }
        }
        public class ProductManageData
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductFirstImg { get; set; }
            public decimal Price { get; set; }
            public int ProductStatus { get; set; }
        }
        public class RecommendShopData
        {
            public int SellerId { get; set;}
            public string BrandName { get; set;}
            public double EvaluateAvg { get; set;}
            public int FavShop { get; set;}
            public string ShopBannerSquare { get; set;}
            public string Icon { get; set;}
        }

    }


}
