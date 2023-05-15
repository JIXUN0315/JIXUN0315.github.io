using Hatsukoi.Common;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Hatsukoi.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Controllers
{
    [Authorize(Roles = "3")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly IAccountService _iAccountService;

        public OrderController(OrderService orderService, IAccountService iAccountService)
        {
            _orderService = orderService;
            _iAccountService = iAccountService;
        }
//int page,int pageSize=15
        public IActionResult Management()
        {

            //var userId = _iAccountService.GetUser().Id;
            //var sellerId = _orderService.GetSellerIdByUserId(userId);
            //var orders = _orderService.GetOrdersBySeller(sellerId);
            //var notPayOrders = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.NotPay);
            //var notShippedOrders = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.NotShipped);
            //var toBeReceivedOrders = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.ToBeReceived);
            //var completedOrders = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.Completed);
            //var cancelledOrders = _orderService.GetOrdersByStatus(orders, HatsukoiEnum.OrderStatus.Cancelled);

            //var viewModel = new OrderStatusViewModel
            //{
            //    //Page=page,
            //    //PageSize=pageSize,
            //    NotPayOrders = notPayOrders,
            //    NotShippedOrders = notShippedOrders,
            //    ToBeReceivedOrders = toBeReceivedOrders,
            //    CompletedOrders = completedOrders,
            //    CancelledOrders = cancelledOrders
            //};

            //return View(viewModel);
            return View();
        }


        public IActionResult OrderDetails(string orderNum)
        {
            var orderViewModel = _orderService.GetOrderDetailsByOrderNum(orderNum);

            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }


        public IActionResult Evaluate()
        {
            return View();
        }


    }
}
