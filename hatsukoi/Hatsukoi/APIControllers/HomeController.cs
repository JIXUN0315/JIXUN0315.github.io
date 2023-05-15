using Hatsukoi.Models;
using Hatsukoi.Repository.Migrations;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> _logger;
        public readonly HomepageService _homepageService;
        public readonly HomeProductFilterService _homeProductFilterService;

        public HomeController(ILogger<HomeController> logger, HomepageService homepageService, HomeProductFilterService homeProductFilterService)
        {
            _logger = logger;
            _homepageService = homepageService;
            _homeProductFilterService = homeProductFilterService;
        }

        //[HttpGet]
        //public APIBaseResponse ProductFilter([FromQuery]int id, [FromQuery]string sortOrder, [FromQuery]string priceOrder, [FromQuery]string dateOrder, [FromQuery]int tagOrder)
        //{
        //    try
        //    {
        //        var listBySubCat = _homeProductFilterService.ReadBySubCategoryId(id).HomeProductCards;
        //        var prodCardsBySubCat = _homeProductFilterService.FilterAndSortProductCards(listBySubCat, sortOrder, priceOrder, dateOrder, tagOrder);
        //        var response = new APIBaseResponse(prodCardsBySubCat);
        //        return response;
        //    }
        //    catch
        //    {
        //        return new APIBaseResponse
        //        {
        //            Status = APIStatus.Fail,
        //            ErrorMsg = "系統錯誤",
                    
        //        };
        //    }
        //}

        

    }
}
