using Coravel.Cache.Interfaces;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Hatsukoi.Service.CacheServices
{
    public class RedisHomepageCacheService : IHomepageService
    {
        private static readonly string _bannerKey = "banner";
        private static readonly string _rmdProdsKey = "rmdProds";
        private static readonly string _newestProdsKey = "newestProds";
        private static readonly string _hottestProdsKey = "hottestProds";
        private static readonly string _trendingProdsKey = "trendingProds";
        private static readonly string _rmdShopsKey = "rmdShops";
        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(15);
        private readonly HomepageService _homepageservice;
        private readonly IDistributedCache _cache;

        public RedisHomepageCacheService(HomepageService homepageservice, IDistributedCache cache)
        {
            _homepageservice = homepageservice;
            _cache = cache;
        }

        public IEnumerable<HomapageBanner> GetBanners()
        {
            //var cacheItems = ByteArrayToObj<IEnumerable<HomapageBanner>>(_cache.Get(_bannerKey));
            //if (cacheItems is not null)
            //{
            //    return (IEnumerable<HomapageBanner>)cacheItems;
            //}

            //var realItems = _homepageservice.GetBanners();
            //var byteArrResult = ObjectToByteArray(realItems);
            //_cache.Set(_bannerKey, byteArrResult, new DistributedCacheEntryOptions()
            //{
            //    SlidingExpiration = _defaultCacheDuration,
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            //});

            //return realItems;

            var banners = _homepageservice.GetBanners();
            return banners;
        }

        public IEnumerable<HomepageProductCard> GetRecommendedProducts()
        {
            var cacheItems = ByteArrayToObj<IEnumerable<HomepageProductCard>>(_cache.Get(_rmdProdsKey));
            if (cacheItems is not null)
            {
                return (IEnumerable<HomepageProductCard>)cacheItems;
            }

            var realItems = _homepageservice.GetRecommendedProducts();
            var byteArrResult = ObjectToByteArray(realItems);
            _cache.Set(_rmdProdsKey, byteArrResult, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = _defaultCacheDuration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(25)
            });

            return realItems;
        }

        public IEnumerable<HomepageProductCard> GetNewestProducts()
        {
            var cacheItems = ByteArrayToObj<IEnumerable<HomepageProductCard>>(_cache.Get(_newestProdsKey));
            if (cacheItems is not null)
            {
                return (IEnumerable<HomepageProductCard>)cacheItems;
            }

            var realItems = _homepageservice.GetNewestProducts();
            var byteArrResult = ObjectToByteArray(realItems);
            _cache.Set(_newestProdsKey, byteArrResult, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = _defaultCacheDuration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return realItems;
        }

        public IEnumerable<HomepageProductCard> GetHottestProducts()
        {
            //取快取資料
            var cacheItems = ByteArrayToObj<IEnumerable<HomepageProductCard>>(_cache.Get(_hottestProdsKey));
            //有快取資料
            if (cacheItems is not null)
            {
                return (IEnumerable<HomepageProductCard>)cacheItems;
            }

            //無快取資料
            var realItems = _homepageservice.GetHottestProducts();
            var byteArrResult = ObjectToByteArray(realItems);
            //存快取資料
            _cache.Set(_hottestProdsKey, byteArrResult, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = _defaultCacheDuration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(35)
            });

            return realItems;
        }

        public IEnumerable<HomepageProductCard> GetTrendingProducts()
        {
            var cacheItems = ByteArrayToObj<IEnumerable<HomepageProductCard>>(_cache.Get(_trendingProdsKey));
            if (cacheItems is not null)
            {
                return (IEnumerable<HomepageProductCard>)cacheItems;
            }

            var realItems = _homepageservice.GetTrendingProducts();
            var byteArrResult = ObjectToByteArray(realItems);
            _cache.Set(_trendingProdsKey, byteArrResult, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = _defaultCacheDuration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(36)
            });

            return realItems;
        }

        public IEnumerable<HomepageShopCard> GetRmdShops()
        {
            var cacheItems = ByteArrayToObj<IEnumerable<HomepageShopCard>>(_cache.Get(_rmdShopsKey));
            if (cacheItems is not null)
            {
                return (IEnumerable<HomepageShopCard>)cacheItems;
            }

            var realItems = _homepageservice.GetRmdShops();
            var byteArrResult = ObjectToByteArray(realItems);
            _cache.Set(_rmdShopsKey, byteArrResult, new DistributedCacheEntryOptions()
            {
                SlidingExpiration = _defaultCacheDuration,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(38)
            });

            return realItems;
        }

        private byte[] ObjectToByteArray(object obj)
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        private T ByteArrayToObj<T>(byte[] byteArr) where T : class
        {
            return byteArr is null ? null : JsonSerializer.Deserialize<T>(byteArr);
        }

        
    }
}
