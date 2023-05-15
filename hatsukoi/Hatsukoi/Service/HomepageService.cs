using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using NuGet.Protocol.Core.Types;

namespace Hatsukoi.Service
{
    public class HomepageService : IHomepageService
    {
        private readonly IRepository _repository;
        private readonly AccountService _accountService;
        private readonly IConfiguration _config;

        public HomepageService(IRepository repository, AccountService accountService, IConfiguration config)
        {
            _repository = repository;
            _accountService = accountService;
            _config = config;
        }

        
        public IEnumerable<HomapageBanner> GetBanners()
        {
            var banners = _repository.ListWhere<Banner>(b => b.Status == 1).OrderBy(b=>b.Sort).Select(b => new HomapageBanner
            {
                Id = b.Id,
                ImgUrl = b.ImgUrl,
                Status = b.Status,
                Sort = (int)b.Sort
            });

            return banners;
            
        }

        StringBuilder stringBuilder = new StringBuilder 
        { 
            QueryRmdProds = $@"
                    SELECT * FROM
                        (SELECT TOP 18
                            p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                            (SELECT TOP 1 Img FROM [ProductImg] WHERE ProductId = p.Id) AS ProductFirstImg,
                            s.Id AS SellerId, s.BrandName, p.Price, p.CreateTime AS CreateDate
                            FROM [Product] p
                            INNER JOIN [Seller] s ON s.Id = p.SellerId
                            WHERE p.ProductStatus = 2
                            ORDER BY p.ViewTimes DESC
                        ) t
                    WHERE t.ProductStatus = 2",

            QueryNewestProds = $@"
                    SELECT * FROM
                        (SELECT TOP 18
                            p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                            (SELECT TOP 1 Img FROM [ProductImg] WHERE ProductId = p.Id ORDER BY CreateTime DESC) AS ProductFirstImg,
                            s.Id AS SellerId, s.BrandName, p.Price, p.CreateTime AS CreateDate
                            FROM [Product] p
                            INNER JOIN [Seller] s ON s.Id = p.SellerId
                            WHERE p.ProductStatus = 2
                            ORDER BY p.CreateTime DESC) t",

            QueryHottestProds = $@"
                    SELECT TOP 18
                        p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                        (SELECT TOP 1 Img FROM [ProductImg] WHERE ProductId = p.Id) AS ProductFirstImg,
                        s.Id AS SellerId, s.BrandName, p.Price, p.CreateTime AS CreateDate,
                        COALESCE((
                            SELECT SUM(od.Quantity)
                            FROM [OrderDetail] od
                            INNER JOIN [Order] o ON o.Id = od.OrderId
                            WHERE od.ProductSpecificationId IN (
                                SELECT Id
                                FROM [ProductSpecification]
                                WHERE ProductId = p.Id
                            ) AND o.Status = 4
                        ), 0) AS BoughtTime
                    FROM [Product] p
                    INNER JOIN [Seller] s ON s.Id = p.SellerId
                    WHERE p.ProductStatus = 2
                    GROUP BY 
                        p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                        s.Id, s.BrandName, p.Price, p.CreateTime
                    ORDER BY BoughtTime DESC;",

            QueryTrendingProds = $@"
                    SELECT TOP 18
                        p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                        (SELECT TOP 1 Img FROM [ProductImg] WHERE ProductId = p.Id) AS ProductFirstImg,
                        s.Id AS SellerId, s.BrandName, p.Price, p.CreateTime AS CreateDate,
                        COALESCE((
                            SELECT SUM(od.Quantity)
                            FROM [OrderDetail] od
                            INNER JOIN [Order] o ON o.Id = od.OrderId
                            WHERE od.ProductSpecificationId IN (
                                SELECT Id
                                FROM [ProductSpecification]
                                WHERE ProductId = p.Id
                            ) AND o.Status = 4
                        ), 0) AS BoughtTime
                    FROM [Product] p
                    INNER JOIN [Seller] s ON s.Id = p.SellerId
                    WHERE p.ProductStatus = 2 AND p.CreateTime >= DATEADD(month, -3, GETDATE())
                    GROUP BY 
                        p.Id, p.ProductName, p.ProductStatus, p.ViewTimes,
                        s.Id, s.BrandName, p.Price, p.CreateTime
                    ORDER BY BoughtTime DESC;",

            QueryRmdShops = $@"
                    SELECT TOP 9
                        s.Id, s.BrandName, s.ShopBannerSquare, s.Logo,
                        ISNULL(AVG(o.Evaluate),0) AS AvgEvaluate,
                        COUNT(DISTINCT fs.Id) AS Followers,
	                    SUM(DISTINCT CASE WHEN o.Status = 4 THEN o.SellerId ELSE 0 END) AS BoughtTimes
                    FROM
                        [Seller] s
                        LEFT JOIN [Order] o ON o.SellerId = s.Id
                        LEFT JOIN [FavShop] fs ON fs.SellerId = s.Id
                    GROUP BY
                        s.Id, s.BrandName, s.ShopBannerSquare, s.Logo
                    ORDER BY BoughtTimes DESC;"

        };
        
        //編輯推薦 > 被看過最多次的商品
        public IEnumerable<HomepageProductCard> GetRecommendedProducts()
        {
            var rmdProducts = _repository.GetSQLQuery<HomepageProductCard>(stringBuilder.QueryRmdProds).ToList();
            return rmdProducts;

        }

        //新品上架
        public IEnumerable<HomepageProductCard> GetNewestProducts()
        {
            var newestProducts = _repository.GetSQLQuery<HomepageProductCard>(stringBuilder.QueryNewestProds).ToList();
            return newestProducts;
        }

        //熱門商品 > 被買過最多次的商品
        public IEnumerable<HomepageProductCard> GetHottestProducts()
        {
            var hottestProducts = _repository.GetSQLQuery<HomepageProductCard>(stringBuilder.QueryHottestProds).ToList();
            return hottestProducts;
        }

        //近期熱銷 > 三個月內被買過最多次的商品
        public IEnumerable<HomepageProductCard> GetTrendingProducts()
        {
            var trendingProducts = _repository.GetSQLQuery<HomepageProductCard>(stringBuilder.QueryTrendingProds).ToList();
            return trendingProducts;
        }

        //店家探索 > 被買過最多次的店家
        public IEnumerable<HomepageShopCard> GetRmdShops()
        {
            var rmdShops = _repository.GetSQLQuery<HomepageShopCard>(stringBuilder.QueryRmdShops).ToList();
            return rmdShops;
        }

    }




}

