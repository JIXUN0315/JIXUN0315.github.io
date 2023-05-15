using Hatsukoi.Models.ViewModels;

namespace Hatsukoi.Service.Interface
{
    public interface IHomepageService
    {
        public IEnumerable<HomapageBanner> GetBanners();
        public IEnumerable<HomepageProductCard> GetRecommendedProducts();
        public IEnumerable<HomepageProductCard> GetNewestProducts();
        public IEnumerable<HomepageProductCard> GetHottestProducts();
        public IEnumerable<HomepageProductCard> GetTrendingProducts();
        public IEnumerable<HomepageShopCard> GetRmdShops();
    }
}
