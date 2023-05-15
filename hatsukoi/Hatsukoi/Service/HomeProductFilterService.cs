using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq;

namespace Hatsukoi.Service
{
    public class HomeProductFilterService : IHomeProductFilterService
    {
        private readonly IRepository _repository;
        private readonly AccountService _accountService;
        private readonly IConfiguration _config;
        private readonly IHomeProductFilterRepository _homeProductFilterRepository;


        public HomeProductFilterService(IRepository repository, AccountService accountService,
            IConfiguration config, IHomeProductFilterRepository homeProductFilterRepository)
        {
            _repository = repository;
            _accountService = accountService;
            _config = config;
            _homeProductFilterRepository = homeProductFilterRepository;
        }

        public IEnumerable<IGrouping<int, HomeProductFilterViewModel>> GetAllCategories()
        {
            var queryString = $@"
                    SELECT
	                c.Id AS CategoryId,c.CategoryName,
	                sc.Id AS SubCategoryId, sc.SubCategoryName
                    FROM [SubCategory] sc
                    INNER JOIN [Category] c ON c.Id = sc.CategoryId";

            var allCategories = _repository.GetSQLQuery<HomeProductFilterViewModel>(queryString).ToList();
            var categoryList = allCategories.GroupBy(x => x.CategoryId);
            return categoryList;

        }

        public HomeProductFilterViewModel ReadBySubCategoryId(int subCategoryId)
        {
            var subCatInfo = _repository.FirstOrDefault<SubCategory>(sc => sc.Id == subCategoryId);
            var catInfo = _repository.FirstOrDefault<Category>(c => c.Id == subCatInfo.CategoryId);
            var subCatList = _repository.ListWhere<SubCategory>(sc => sc.CategoryId == catInfo.Id).Select(sc => sc).ToList();

            var productQuery = from product in _repository.ListWhere<Product>(p => p.SubCategory == subCategoryId)
                               join seller in _repository.ListWhere<Seller>(s => true) on product.SellerId equals seller.Id
                               //join productImg in _repository.ListWhere<ProductImg>(pi => true) on product.Id equals productImg.ProductId
                               //where productImg.ProductId == product.Id
                               where product.ProductStatus == 2
                               select new HomeProductCard
                               {
                                   ProductId = product.Id,
                                   ProductName = product.ProductName,
                                   SellerId = seller.Id,
                                   BrandName = seller.BrandName,
                                   ProductFirstImg = _repository.FirstOrDefault<ProductImg>(x => x.ProductId == product.Id).Img,
                                   Price = product.Price,
                                   ProductStatus = product.ProductStatus,
                                   CreateDate = product.CreateTime,
                                   ViewTimes = (int)product.ViewTimes,
                                   ProductTagList = _repository.ListWhere<ProductTagList>(x => x.ProductId == product.Id)
                                                    .Select(x => x.ProductTagId).ToList()


                               };

            var prodList = productQuery.ToList();


            HomeProductFilterViewModel homeProductFilterViewModel = new HomeProductFilterViewModel
            {
                SubCategoryId = subCatInfo.Id,
                SubCategoryName = subCatInfo.SubCategoryName,
                CategoryId = subCatInfo.CategoryId,
                CategoryName = catInfo.CategoryName,
                SubCategories = subCatList,
                HomeProductCards = prodList,
                 
                
            };


            return homeProductFilterViewModel;
        }

        public List<HomeProductCard> FilterAndSortProductCards(List<HomeProductCard> homeProductCards,
            string sortOrder, string priceOrder, string dateOrder, int tagOrder)
        {
            //用價格篩選商品
            var priceRanges = new Dictionary<string, Func<HomeProductCard, bool>>
            {
                { "price300", h => h.Price <= 300 },
                { "price300_500", h => h.Price > 300 && h.Price < 500 },
                { "price500_1000", h => h.Price > 500 && h.Price < 1000 },
                { "price1000_2000", h => h.Price > 1000 && h.Price < 2000 },
                { "price2000_2500", h => h.Price > 2000 && h.Price < 2500 },
                { "price2500_5000", h => h.Price > 2500 && h.Price < 5000 },
                { "price5000", h => h.Price >= 5000 }
            };

            if (!string.IsNullOrEmpty(priceOrder) && priceRanges.TryGetValue(priceOrder, out var filter))
            {
                homeProductCards = homeProductCards.Where(filter).ToList();
            }

            //用上架時間篩選產品
            if (!string.IsNullOrEmpty(dateOrder))
            {
                switch (dateOrder)
                {
                    case "one_week":
                        homeProductCards = homeProductCards.Where(h => h.CreateDate >= (DateTime.Now.AddDays(-7))).ToList();
                        break;
                    case "one_month":
                        homeProductCards = homeProductCards.Where(h => h.CreateDate >= (DateTime.Now.AddMonths(-1))).ToList();
                        break;
                    case "three_months":
                        homeProductCards = homeProductCards.Where(h => h.CreateDate >= (DateTime.Now.AddMonths(-3))).ToList();
                        break;
                    case "one_year":
                        homeProductCards = homeProductCards.Where(h => h.CreateDate >= (DateTime.Now.AddYears(-1))).ToList();
                        break;

                }
            }

            //篩選有這個tag的商品
            if(tagOrder > 0)
            {
                homeProductCards = homeProductCards.Where(h => h.ProductTagList.Contains(tagOrder)).ToList();
            }


            //篩選商品後進行排序
            switch (sortOrder)
            {
                case "viewtimes_desc":
                    homeProductCards = homeProductCards.OrderByDescending(h => h.ViewTimes).ToList();
                    break;
                case "date_desc":
                    homeProductCards = homeProductCards.OrderByDescending(h => h.CreateDate).ToList();
                    break;
                case "price_asc":
                    homeProductCards = homeProductCards.OrderBy(h => h.Price).ToList();
                    break;
                case "price_desc":
                    homeProductCards = homeProductCards.OrderByDescending(h => h.Price).ToList();
                    break;

            }
            

            return homeProductCards;
        }

        

        public HomeProductFilterViewModel ReadByCategoryId(int categoryId)
        {
            var getCatId = _repository.FirstOrDefault<SubCategory>(sc => sc.CategoryId == categoryId);
            var catInfo = _repository.FirstOrDefault<Category>(c => c.Id == getCatId.CategoryId);
            var subCatList = _repository.ListWhere<SubCategory>(sc => sc.CategoryId == categoryId).Select(sc => sc).ToList();
            var subCatIdList = subCatList.Select(s => s.Id).ToList();

            var productQuery = from product in _repository.ListWhere<Product>(p => subCatIdList.Contains(p.SubCategory))
                               join seller in _repository.ListWhere<Seller>(s => true) on product.SellerId equals seller.Id
                               where product.ProductStatus == 2
                               select new HomeProductCard
                               {
                                   ProductId = product.Id,
                                   ProductName = product.ProductName,
                                   SellerId = seller.Id,
                                   BrandName = seller.BrandName,
                                   ProductFirstImg = _repository.FirstOrDefault<ProductImg>(x => x.ProductId == product.Id).Img,
                                   Price = product.Price,
                                   ProductStatus = product.ProductStatus,
                                   CreateDate = product.CreateTime,
                                   ViewTimes = (int)product.ViewTimes,
                                   ProductTagList = _repository.ListWhere<ProductTagList>(x => x.ProductId == product.Id)
                                                    .Select(x => x.ProductTagId).ToList()
                               };

            var prodList = productQuery.ToList();


            HomeProductFilterViewModel homepageProductViewModel = new HomeProductFilterViewModel
            {
                CategoryId = catInfo.Id,
                CategoryName = catInfo.CategoryName,
                SubCategories = subCatList,
                HomeProductCards = prodList

            };


            return homepageProductViewModel;
        }

        public HomeProductFilterViewModel ReadByKeyword(string keyword)
        {
            var keyChars = keyword.ToCharArray();

            var productQuery = from product in _repository.ListWhere<Product>(p => true)
                               join seller in _repository.ListWhere<Seller>(s => true) on product.SellerId equals seller.Id
                               where product.ProductStatus == 2
                               where keyChars.Any(kc => product.ProductName.Contains(kc.ToString()))
                               select new HomeProductCard
                               {
                                   ProductId = product.Id,
                                   ProductName = product.ProductName,
                                   SellerId = seller.Id,
                                   BrandName = seller.BrandName,
                                   ProductFirstImg = _repository.FirstOrDefault<ProductImg>(x => x.ProductId == product.Id).Img,
                                   Price = product.Price,
                                   ProductStatus = product.ProductStatus,
                                   CreateDate = product.CreateTime,
                                   ViewTimes = (int)product.ViewTimes,
                                   ProductTagList = _repository.ListWhere<ProductTagList>(x => x.ProductId == product.Id)
                                                    .Select(x => x.ProductTagId).ToList()
                               };

            var prodList = productQuery.ToList();


            HomeProductFilterViewModel homepageProductViewModel = new HomeProductFilterViewModel
            {                
                HomeProductCards = prodList
            };


            return homepageProductViewModel;
        }

    }






}

