using Hatsukoi.Service.Interface;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hatsukoi.Models;
using Hatsukoi.Models.Dtos.OrderDto;
using Microsoft.AspNetCore.Authorization;
using Hatsukoi.Common;
using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IAccountService _iAccountService;
        private readonly ReportService _reportService;

        public OrderController(OrderService orderService, IAccountService iAccountService, ReportService  reportService)
        {
            _orderService = orderService;
            _iAccountService = iAccountService;
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult GetOrderList()
        {
           
            try
            {
                var userId = _iAccountService.GetUser().Id;
                var sellerId = _orderService.GetSellerIdByUserId(userId);
                var orders = _orderService.GetOrdersBySeller(sellerId);

                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = string.Empty,
                    Result = orders
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = ex.Message,
                    //Result = new List<string>()
                });
            }
        }

        [HttpPost]
        public IActionResult CancelOrder(CancelOrderDto dto)
        {
            try
            {
                _orderService.CancelOrderByOrderNumber(dto.orderNum, dto.cancelReason);
                return Ok(new APIBaseResponse()
                {
                    Status = APIStatus.Success,
                    Result = new { result = "success", isSuccess = "已成功更新" }
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = ex.Message,
                    //Result = new List<string>()
                });
            }
        }
        [HttpPost]
        public IActionResult ShipOrder(ShipOrderNumDto dto)
        {
            try
            {
                _orderService.ShipOrderByOrderNumber(dto.orderNum);
                return Ok(new APIBaseResponse()
                {
                    Status = APIStatus.Success,
                    Result = new { result = "success", isSuccess = "已成功更新" }
                });
            }
            catch
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤"
                });
            }

        }
        [HttpPost]
        public IActionResult ShipOrders(ShipOrdersDto dto)
        {
            try
            {
                for (int i = 0; i < dto.orderNums.Count; i++)
                {
                    _orderService.ShipOrderByOrderNumber(dto.orderNums[i]);
                }
                return Ok(new APIBaseResponse()
                {
                    Status = APIStatus.Success,
                    Result = new { result = "success", isSuccess = "已成功更新" }
                });
            }
            catch
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤"
                });
            }


        }
        [HttpGet]
        public IActionResult GetEvaluateList()
        {

            try
            {
                var userId = _iAccountService.GetUser().Id;
                var sellerId = _orderService.GetSellerIdByUserId(userId);
                var orders = _orderService.GetOrdersBySeller(sellerId);
                var evaluateList = _orderService.GetOrderHasEvaluate(orders);

                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Success,
                    ErrorMsg = string.Empty,
                    Result = evaluateList
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = ex.Message,
                    //Result = new List<string>()
                });
            }
        }

        //取得賣家首頁訊息的未讀總數
        [HttpGet]
        public IActionResult GetOrderCount()
        {
            try
            {
                var userId = _iAccountService.GetUser().Id;
                var sellerId = _orderService.GetSellerIdByUserId(userId);
                var orders = _orderService.GetOrdersBySeller(sellerId);
                var notPayOrderCount = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.NotPay).Count;
                var notShippedOrderCount = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.NotShipped).Count;
                
                return Ok(new APIBaseResponse()
                {
                    Status = APIStatus.Success,
                    Result = new { result = "success", notPayCount = notPayOrderCount, notShippedCount = notShippedOrderCount }
                });
            }
            catch
            {
                return Ok(new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "傳遞資料發生錯誤"
                });
            }
        }

        [HttpGet]
        public IActionResult GetSellerHome()
        {
            var userId = _iAccountService.GetUser().Id;
            var sellerId = _orderService.GetSellerIdByUserId(userId);
            var orderCount =_orderService.OrderCount(sellerId);
            var orderAvgTotlePrice = _orderService.OrderAvgTotlePrice(sellerId);
            var orderTotalPrice=_orderService.OrderTotlePrice(sellerId);
            var sevenDay = _orderService.SevenDay();
            var doubleSevenDay =_orderService.DoubleSevenDay();
            var doubleSevenOrderCount = _orderService.DoubleSevenOrderCount(sellerId);
            var doubleSevenOrderTotlePrice = _orderService.DoubleSevenOrderTotlePrice(sellerId);
            var doubleSevenOrderAvgTotlePrice = _orderService.DoubleSevenOrderAvgTotlePrice(sellerId);
            var lastMonth =_orderService.LastMonth();
            var lastMonthOrderCount = _orderService.LastMonthOrderCount(sellerId);
            var lastMonthOrderAvgTotlePrice = _orderService.LastMonthOrderAvgTotlePrice(sellerId);
            var lastMonthOrderTotlePrice = _orderService.LastMonthOrderTotlePrice(sellerId);
            var lastYear = _orderService.LastYear();
            var lastYearOrderCount = _orderService.LastYearOrderCount(sellerId);
            var lastYearOrderAvgTotlePrice = _orderService.LastYearOrderAvgTotlePrice(sellerId);
            var lastYearOrderTotlePrice = _orderService.LastYearOrderTotlePrice(sellerId);
            return Ok(new APIBaseResponse()
            {
                Result = new { result = "success", orderCount = orderCount, orderAvgTotlePrice= orderAvgTotlePrice, orderTotalPrice = orderTotalPrice , sevenDay = sevenDay , doubleSevenDay = doubleSevenDay , doubleSevenOrderCount  = doubleSevenOrderCount, doubleSevenOrderAvgTotlePrice= doubleSevenOrderAvgTotlePrice, doubleSevenOrderTotlePrice = doubleSevenOrderTotlePrice , lastMonth = lastMonth , lastMonthOrderCount = lastMonthOrderCount , lastMonthOrderAvgTotlePrice = lastMonthOrderAvgTotlePrice , lastMonthOrderTotlePrice = lastMonthOrderTotlePrice , lastYear = lastYear , lastYearOrderCount = lastYearOrderCount , lastYearOrderAvgTotlePrice = lastYearOrderAvgTotlePrice, lastYearOrderTotlePrice= lastYearOrderTotlePrice }
            });
        }

        
    }
}
