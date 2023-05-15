namespace Hatsukoi.Models.ViewModels
{
    public class HomepageProductViewModel
    {
        public IEnumerable<HomepageProductCard>? RmdProductCards { get; set; }
        public IEnumerable<HomepageProductCard>? NewestProductCards { get; set; }
        public IEnumerable<HomepageProductCard>? HottestProductCards { get; set; }
        public IEnumerable<HomepageShopCard>? RmdShopCards { get; set; }
        public IEnumerable<HomepageProductCard>? TrendingProductCards { get; set; }
        public IEnumerable<HomapageBanner>? Banners { get; set; }
        
    }

    public class HomapageBanner
    {
        public int Id { get; set; }
        public string? ImgUrl { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
    }


    public class HomepageProductCard
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string? ProductFirstImg { get; set; }
        public int SubCategoryId { get; set; }
        public decimal Price { get; set; }
        public int ProductStatus { get; set; }
        public int SellerId { get; set; }
        public int ViewTimes { get; set; }
        public DateTime? CreateDate { get; set; }
        public int BoughtTimes { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }

        public List<int>? ProductTagList { get; set; }
    }

    public class HomepageShopCard
    {
        public int Id { get; set; }
        public string? BrandName { get; set; }
        public string? ShopBannerSquare { get; set; }
        public string? Logo { get; set; }
        public int AvgEvaluate { get; set; }
        public int Followers { get; set; }
        public int BoughtTimes { get; set; }
    }

    public class StringBuilder
    {
        public string? QueryRmdProds { get; set; }
        public string? QueryNewestProds { get; set; }
        public string? QueryHottestProds { get; set; }
        public string? QueryTrendingProds { get; set; }
        public string? QueryRmdShops { get; set; }


    }
}
