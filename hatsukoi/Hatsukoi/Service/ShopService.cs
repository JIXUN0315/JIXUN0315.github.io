using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Core.Types;
using static Hatsukoi.Common.HatsukoiEnum;
using static Hatsukoi.Models.ViewModels.ProductViewModel;
using static Hatsukoi.Models.ViewModels.ShopViewModel;

namespace Hatsukoi.Service
{
    public class ShopService
    {
        private readonly IRepository _shopRepository;
        
        public ShopService(IRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public Product GetProductByProductId(int id)
        {
            return _shopRepository.FirstOrDefault<Product>(x => x.Id == id && (x.ProductStatus == (int)ProductStatus.shelve));
        }

        //取得本頁商品的SellerId
        public Seller GetSellerByProductId(int id)
        {
            var product = GetProductByProductId(id);
            if (product == null) { return null; }

            var prodSellerId = product.SellerId;
            return _shopRepository.FirstOrDefault<Seller>(x => x.Id == prodSellerId);
        }
        public Seller GetSellerBySellerId(int sellerId)
        {
            var seller = _shopRepository.FirstOrDefault<Seller>(x => x.Id == sellerId);
            return seller;
        }

        //取得本頁商店的資訊
        public ShopViewModel GetShop(int id)
        {
            var seller = GetSellerBySellerId(id);
            var shopName = seller.ShopName;
           
            //銷售數量(店家)
            var orderIds = _shopRepository.ListWhere<OrderDetail>(x => x.SellerName == shopName).Select(x => x.Id).ToList();
            var salesQtyByShop = _shopRepository.ListWhere<OrderDetail>(x => orderIds.Contains(x.OrderId)).Sum(x => x.Quantity);

            //商店的總商品數量
            var productQty = _shopRepository.ListWhere<Product>(x => x.SellerId == id).Count();

            //商店關注人數
            var favShop = _shopRepository.ListWhere<FavShop>(x => x.SellerId == id).Count();

            //評價
            var evaluateAvgandSum = GetShopReviewAvgandSum(id) ?? null;

            var shopInfo = _shopRepository
                .ListWhere<Seller>(x => x.Id == id)
                .Select(x => new ShopViewModel
                {
                    Id = x.Id,
                    BrandName = x.BrandName,
                    ShopBannerRect = x.ShopBannerRect,
                    Logo = x.Logo,
                    ProductOrigin = x.ProductOrigin,
                    ShopName = x.ShopName,
                    Introduction = x.Introduction,
                    Year = x.CreateDate.ToString("yyyy"),
                    SalesQtyByShop = salesQtyByShop,
                    FavShop = favShop,
                    ProductQty = productQty,
                    EvaluateAvg = evaluateAvgandSum.EvaluateAvg,
                    EvaluateSum = evaluateAvgandSum.EvaluateSum
                }).FirstOrDefault();

            return shopInfo;
        }

        //取得商店的評價
        public ShopViewModel GetShopReviewAvgandSum(int id)
        {

            var orders = _shopRepository
                .ListWhere<Order>(o => o.SellerId == id && o.Evaluate.HasValue)
                .ToList();

            var evaluateAvg = Math.Ceiling(orders.Average(r => r.Evaluate) ?? 0);
            var evaluateSum = orders.Count;

            return new ShopViewModel
            {
                EvaluateAvg = Convert.ToInt32(evaluateAvg),
                EvaluateSum = evaluateSum
            };
        }

        //取得熱門商品
        public List<HotProductData>? GetHotProducts(int id)
        {
            var seller = GetSellerBySellerId(id);

            var hotProducts = _shopRepository
                .ListWhere<Product>(p => p.SellerId == seller.Id && p.ProductStatus == (int)ProductStatus.shelve)
                .OrderByDescending(p => p.ViewTimes).Take(4)
                .Select(x => new HotProductData
                {
                    ProductId = x.Id,
                    ProductName = x.ProductName,
                    ProductFirstImg = _shopRepository.FirstOrDefault<ProductImg>(y => y.ProductId == x.Id).Img,
                    Price = x.Price,
                    ViewTimes = x.ViewTimes
                }).ToList();

            return hotProducts;

        }

        //組成商店的商品卡
        public List<HomeProductCard> ShopProductsReadBySellerId(int id)
        {
            var seller = GetSellerBySellerId(id);
            var shopProducts = _shopRepository
                .ListWhere<Product>(p => p.SellerId == seller.Id && p.ProductStatus == (int)ProductStatus.shelve)
                .Select(x => new HomeProductCard
                {
                    ProductId = x.Id,
                    ProductName = x.ProductName,
                    ProductFirstImg = _shopRepository.FirstOrDefault<ProductImg>(y => y.ProductId == x.Id).Img,
                    Price = x.Price,
                    ProductTagList = _shopRepository.ListWhere<ProductTagList>(x => x.ProductId == x.Id)
                                                    .Select(x => x.ProductTagId).ToList()
                }).ToList();    
            return shopProducts;
        }
    }
}
