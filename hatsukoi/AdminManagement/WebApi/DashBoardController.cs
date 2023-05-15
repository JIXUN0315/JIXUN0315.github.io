using AdminManagement.BaseModels;
using AdminManagement.Models.Dtos;
using AdminManagement.Models.ViewModels;
using AdminManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{
    public class DashBoardController: BaseApiController
    {
        private readonly SalesDataService _salesDataService;
        private readonly RevenueService _revenueService;

        public DashBoardController(SalesDataService salesDataService, RevenueService revenueService)
        {
            _salesDataService = salesDataService;
            _revenueService = revenueService;
        }

        [HttpPost]
        public IActionResult GetSalseData(MonthDto dto)
        {
            var VM = _salesDataService.GetSalesDataByCategory(dto.Month);
            return Ok(new BaseApiResponse(VM));
        }

        [HttpGet]
        public ActionResult<RevenueDto> GetRevenue(int year, int month)
        {
            var revenueDto = _revenueService.GetThisYearRevenue(year, month);
            return revenueDto;
        }

        [HttpGet]
        public ActionResult<List<RevenueDto>> GetRevenueByMonth(int year)
        {
            var revenueByMonth = _revenueService.GetRevenueByMonth(year);
            return revenueByMonth;
        }




        [HttpGet]
        public IActionResult GetUserCount()
        {
            var userCount = _revenueService.GetUserCount();
            
            return Ok(new BaseApiResponse(userCount));
        }

        [HttpGet]
        public IActionResult GetIncreasedUserData()
        {
            var year = DateTime.Now.Year;
            var increasedUserData = _revenueService.GetIncreasedUserData(year);
            return Ok(new BaseApiResponse(increasedUserData));
        }

    }

}
