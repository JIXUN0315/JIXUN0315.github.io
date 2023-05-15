using Microsoft.Build.Framework;

namespace Hatsukoi.Models.ViewModels
{
#nullable disable
    public class ShopViewModel
    {
        public int Id { get; set; }
        //上方商店資訊
        public string ShopName { get; set; }
        public string BrandName { get; set; }
        public string ShopBannerRect { get; set; }
        public string Introduction { get; set; }
        public string Logo { get; set; }
        public string ProductOrigin { get; set; }
        public string Year { get; set; }
        public int ViewTimes { get; set; }
        public int ProductQty { get; set; }

        //商品關注人數
        public int FavShop { get; set; }

        //Order - SalesQty
        public int SalesQtyByShop { get; set; }

        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }

        //評價
        public int Evaluate { get; set; } //單一訂單評分
        public int EvaluateAvg { get; set; } //商店評論平均分數
        public int EvaluateSum { get; set; } //總評論數量

        //---篩選用--------

        //總共幾個商品
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<HomeProductCard> PagedProducts { get; set; }
        public List<int> ProductTagList { get; set; }

        //radio button點擊後的狀態
        public string SelectedSortOrder { get; set; }
        public string SelectedPriceOrder { get; set; }
        public string SelectedDateOrder { get; set; }
        public int SelectedTagOrder { get; set; }

        public List<HotProductData> HotProductList { get; set; }
        public List<HomeProductCard> HomeProductCards { get; set; }
        public List<FilterInputData> FilterInputList { get; set; }
        public ShopViewModel Shop { get; internal set; }
    }

    public class HotProductData
    {
        //熱銷商品
        public int ProductId { get; set; }
        public string ProductFirstImg { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int? ViewTimes { get; set; }
    }
    public class FilterInputData
    {
        public int Id { get; set; }
        public string SortOrder { get; set; }
        public string PriceOrder { get; set; }
        public string DateOrder { get; set; }
        public int TagOrder { get; set; }
    }
   
}



