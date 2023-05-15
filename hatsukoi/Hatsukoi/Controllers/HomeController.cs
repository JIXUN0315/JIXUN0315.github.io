using Hatsukoi.Models;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Hatsukoi.Models.ViewModels.ProductViewModel;

namespace Hatsukoi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IHomepageService _homepageService;
        public readonly HomeProductFilterService _homeProductFilterService;

        public HomeController(ILogger<HomeController> logger, IHomepageService homepageService, HomeProductFilterService homeProductFilterService)
        {
            _logger = logger;
            _homepageService = homepageService;
            _homeProductFilterService = homeProductFilterService;
        }

        //首頁
        public IActionResult Index()
        {
            
            var rmdProducts = _homepageService.GetRecommendedProducts();            
            var newestProducts = _homepageService.GetNewestProducts();
            var hottestProducts = _homepageService.GetHottestProducts();
            var rmdShops = _homepageService.GetRmdShops();
            var trendingProducts = _homepageService.GetTrendingProducts();
            var banners = _homepageService.GetBanners();


            var homeViewModel = new HomepageProductViewModel
            {
                RmdProductCards = rmdProducts,
                NewestProductCards = newestProducts,
                HottestProductCards = hottestProducts,
                RmdShopCards = rmdShops,
                TrendingProductCards = trendingProducts,
                Banners = banners
            };
            
            return View("Homepage", homeViewModel);
        }

        //子類別商品篩選頁
        public IActionResult ProductFilter([FromQuery] ProductFilterInput input, int? page)
        {
            var allCategories = _homeProductFilterService.ReadBySubCategoryId(input.Id);
            ViewData["showAllCategories"] = allCategories;

            var listBySubCat = allCategories.HomeProductCards;
            var prodCardsBySubCat = _homeProductFilterService.FilterAndSortProductCards(listBySubCat, input.SortOrder, input.PriceOrder, input.DateOrder, input.TagOrder);

            int pageSize = 8; //一頁有幾個商品
            int pageNumber = (page ?? 1); //當前頁數，如果沒有指定，預設為第1頁

            var totalCount = prodCardsBySubCat.Count();
            var pagedProducts = prodCardsBySubCat.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            allCategories.HomeProductCards = pagedProducts;
            allCategories.SelectedSortOrder = input.SortOrder;
            allCategories.SelectedPriceOrder = input.PriceOrder;
            allCategories.SelectedDateOrder = input.DateOrder;
            allCategories.SelectedTagOrder = input.TagOrder;
            allCategories.PageSize = pageSize;
            allCategories.TotalCount = totalCount;
            allCategories.CurrentPage = pageNumber;
            allCategories.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(allCategories);
            
        }


        public IActionResult ProductFilterByCat([FromQuery] ProductFilterInput input, int? page)
        {
            var allCategories = _homeProductFilterService.ReadByCategoryId(input.Id);
            ViewData["showAllCategories"] = allCategories;

            var listByCat = allCategories.HomeProductCards;
            var prodCardsBySubCat = _homeProductFilterService.FilterAndSortProductCards(listByCat, input.SortOrder, input.PriceOrder, input.DateOrder, input.TagOrder);

            int pageSize = 8; //一頁有幾個商品
            int pageNumber = (page ?? 1); //當前頁數，如果沒有指定，預設為第1頁

            var totalCount = prodCardsBySubCat.Count();
            var pagedProducts = prodCardsBySubCat.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            allCategories.HomeProductCards = pagedProducts;
            allCategories.SelectedSortOrder = input.SortOrder;
            allCategories.SelectedPriceOrder = input.PriceOrder;
            allCategories.SelectedDateOrder = input.DateOrder;
            allCategories.SelectedTagOrder = input.TagOrder;
            allCategories.PageSize = pageSize;
            allCategories.TotalCount = totalCount;
            allCategories.CurrentPage = pageNumber;
            allCategories.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(allCategories);
        }

        public IActionResult ProductFilterByKeyword([FromQuery] ProductFilterInput input, int? page)
        {
            var similarProducts = _homeProductFilterService.ReadByKeyword(input.Keyword);
            ViewData["allSimilarProducts"] = similarProducts;

            var listByKeyword = similarProducts.HomeProductCards;
            var prodCardsByKeyword = _homeProductFilterService.FilterAndSortProductCards(listByKeyword, input.SortOrder, input.PriceOrder, input.DateOrder, input.TagOrder);

            int pageSize = 8; //一頁有幾個商品
            int pageNumber = (page ?? 1); //當前頁數，如果沒有指定，預設為第1頁

            var totalCount = prodCardsByKeyword.Count();
            var pagedProducts = prodCardsByKeyword.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            similarProducts.HomeProductCards = pagedProducts;
            similarProducts.SelectedSortOrder = input.SortOrder;
            similarProducts.SelectedPriceOrder = input.PriceOrder;
            similarProducts.SelectedDateOrder = input.DateOrder;
            similarProducts.SelectedTagOrder = input.TagOrder;
            similarProducts.PageSize = pageSize;
            similarProducts.TotalCount = totalCount;
            similarProducts.CurrentPage = pageNumber;
            similarProducts.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(similarProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ProductionError()
        {
            return View();
        }
    }

    

}