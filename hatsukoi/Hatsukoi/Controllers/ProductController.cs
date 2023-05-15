using Hatsukoi.Models.Dtos.Create;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Mvc;
using static Hatsukoi.Models.ViewModels.ProductViewModel;
using static Hatsukoi.Common.HatsukoiEnum;
using Hatsukoi.Repository.EntityModel;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Hatsukoi.Models.ViewModels.Create;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hatsukoi.Models.Dtos.Product;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.CodeAnalysis.Differencing;
using Hatsukoi.Models;
using Microsoft.AspNetCore.Authorization;
using Hatsukoi.Service.Interface;
using System.Collections.Generic;

namespace Hatsukoi.Controllers
{

    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ShopService _shopService;
        public readonly HomeProductFilterService _homefilterService;
        public ProductController(ProductService productService, ShopService shopService, HomeProductFilterService homefilterService)
        {
            _productService = productService;
            _shopService = shopService;
            _homefilterService = homefilterService;
        }

        public IActionResult Index(int id)
        {
            var product = _productService.GetProduct(id);

            ProductVM vm = new ProductVM
            {
                Product = product,
                SimilarProdList = _productService.GetSimilarProducts(id),
                SellerInfoList = _productService.GetSellerInfo(id),
                ReviewList = _productService.GetReviews(id),
                RecommendShopList = _productService.RecommendShops(id)
            };

            return View(vm);
        }


        public IActionResult Shop([FromQuery] FilterInputData input, int? page)
        {
           
            int pageSize = 8; //一頁有幾個商品
            int pageNumber = (page ?? 1); //當前頁數，如果沒有指定，預設為第1頁

            var shopProductList = _shopService.ShopProductsReadBySellerId(input.Id);
            var shopFilteredCards = _homefilterService.FilterAndSortProductCards(shopProductList, input.SortOrder, input.PriceOrder, input.DateOrder, input.TagOrder);
            
            var totalCount = shopFilteredCards.Count();
            var pagedProducts = shopFilteredCards.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            
            var shopVM = new ShopViewModel
            {
                Shop = _shopService.GetShop(input.Id),
                HotProductList = _shopService.GetHotProducts(input.Id),
                HomeProductCards = pagedProducts,
                SelectedSortOrder = input.SortOrder,
                SelectedPriceOrder = input.PriceOrder,
                SelectedDateOrder = input.DateOrder,
                SelectedTagOrder = input.TagOrder,
                PageSize = pageSize,
                TotalCount = shopFilteredCards.Count(),
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(shopVM);
        }

        [Authorize(Roles = "3")]
        public IActionResult Create()
        {

            var getCategory = _productService.GetProductCategory();
            return View(getCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model, SpecificationDto model1, ProductSubCategoryDto tagModel, ImageDto imageModel)
        {

            var hasName = _productService.CheckName(model.ProductName);
            if (hasName == "")
            {
                var addProductId = _productService.CreateNewProduct(model);
                _productService.AddSpecificationDto(model1, addProductId);
                _productService.AddProductTag(tagModel, addProductId);
                await _productService.AddProductImage(imageModel, addProductId);
                return Json(new { result = "success", IsCreate = "", id = addProductId });
            }
            else
            {
                return Json(new { result = "success", IsCreate = "產品名稱已使用過" });
            }
        }
        public IActionResult Management(int sellerId, int userId)
        {

            ProductVM vm = new ProductVM
            {
                ProductManageList = _productService.GetProductsBySellerId(userId).ToList()
            };

            return View(vm);
        }

        public IActionResult Preview(int id)
        {
            var IsSeller = _productService.IsProduct(id);
            if (IsSeller == false)
            {
                return RediSellerHome();
            }

            var product = _productService.GetNewProduct(id);

            ProductVM vm = new ProductVM
            {
                Product = product,
                SellerInfoList = _productService.GetSeller(id),
            };
            return View(vm);
        }



        public IActionResult Editor(int id)
        {
            var IsSeller = _productService.IsProduct(id);
            if (IsSeller == false)
            {
                return RediSellerHome();
            }
            ViewBag.MonthsLabel = _productService.editt(id);
            var a = _productService.EditProduct(id);
            return View(a);
        }
        [HttpPost]
        public IActionResult Editor(EditDto dto)
        {
            _productService.NewEditProduct(dto);
            return View();
        }


        //讓Layout的前往商家頁面敲過來
        public IActionResult RediHome()
        {
            var id = _productService.GetSellerId();
            return Redirect($"/Shop?id={id}");
        }

        public IActionResult RediSellerHome()
        {
            var id = _productService.GetSellerId();
            return RedirectToAction("Sellerhome", "Seller", new { id });
        }
    }
}
