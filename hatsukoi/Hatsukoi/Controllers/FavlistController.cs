
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Hatsukoi.Controllers
{
    [Authorize]
    public class FavlistController : Controller
    {
        private readonly FavoriteListService _favoriteListService;
        private readonly IAccountService _accountService;
        

        public FavlistController(FavoriteListService favoriteListService, IAccountService accountService)
        {
            _favoriteListService = favoriteListService;
            _accountService = accountService;
            
        }
        
        public async Task<IActionResult> Index()
        {
            var favList = await _favoriteListService.GetFavorViewModel(_accountService.GetUser().Id);
            //var favProductList = _favoriteListService.GetProduct(_accountService.GetUser().Id);
            //var favShopList = _favoriteListService.GetShop(_accountService.GetUser().Id);
            //var favList = _favoriteListService.GetProduct(_accountService.GetUser().Id);
            //var sellerImg = _favoriteListService.GetShopImg(_accountService.GetUser().Id);
            //ViewData["favProductList"] = favProductList;
            //ViewData["favShopList"] = favShopList;
            //ViewData["sellerImg"] = sellerImg;
            return View(favList);
        }

        
        //預設是[GET]
        //加進喜歡清單
        [HttpPost]
        public async Task< IActionResult> AddfavProduct(int productId)
        {
           var msg = await _favoriteListService.CreateFavProduct(productId, _accountService.GetUser().Id);
            //var hadProduct = _favoriteListService.CheckProduct(productId, _accountService.GetUser().Id);
            if ( msg == "已將此商品加入喜歡清單")
            {
                await _favoriteListService.CreateFavProduct(productId, _accountService.GetUser().Id);
                return Json(new { result = "success", IsCreate = "已將此商品加入喜歡清單" });
                
            }
            else
            {
                await _favoriteListService.DeleteFavProduct(productId, _accountService.GetUser().Id);

                return Json(new { result = "success", IsCreate = "已將商品移除" });
                
            }
            
        }
        //加進喜歡商家
        [HttpPost]
        public async Task< IActionResult> AddfavShop(int sellerId)
        {
            var msg = await _favoriteListService.CreateFavShop(sellerId, _accountService.GetUser().Id);
            if( msg == "已關注此品牌")
            {
                await _favoriteListService.CreateFavShop(sellerId, _accountService.GetUser().Id);
                return Json(new { result = "success", IsCreate = "已關注此品牌" });
            }
            else
            {
                await _favoriteListService.DeleteFavShop(sellerId, _accountService.GetUser().Id);
                return Json(new { result = "success", IsCreate = "已取消關注" });
            }

            
        }

    }
}
