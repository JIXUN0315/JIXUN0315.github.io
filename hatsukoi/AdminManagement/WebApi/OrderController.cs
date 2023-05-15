using AdminManagement.BaseModels;
using AdminManagement.Models.ViewModels;
using AdminManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{
    public class OrderController : BaseApiController
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult GetOrderPage(PageDto input)
        {
           
            try
            {
                
                var orderData = _orderService.GetOrderList(input);
                var orderCount = _orderService.GetOrderCount();//總共有幾筆
                int perPage = input.Perpage; //一頁有10筆資料
                int currentPage = input.CurrentPage;//當前頁面

                //var pagedProducts = orderData.Skip((currentPage - 1) * perPage).Take(perPage).ToList();
                var count = orderCount / perPage;
                var totalPage = orderCount % perPage == 0 ? count : count + 1;

                var vm = new OrderListVM
                {
                    OrderList = orderData,
                    TotalPage = totalPage,
                    CurrentPage = currentPage,
                    TotalCount = orderCount
                };
               
                

                return Ok(new BaseApiResponse(orderData));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }

    public class OrderListVM
    {
        public List<OrderViewModel> OrderList { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }

        public int TotalCount { get; set; }

    }
    public class GetOrderDetailVM
    {
        public int OrderId { get; set; }
    }
    public class PageDto
    {
        public int CurrentPage { get; set; }
        public int Perpage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


}
