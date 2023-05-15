using AdminManagement.Models.ViewModels;
using AdminManagement.WebApi;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Repository.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Globalization;

namespace AdminManagement.Services
{
    public class OrderService
    {
        private readonly IRepository _repository;

        public OrderService(IRepository repository)
        {
            _repository = repository;
        }

        public List<OrderViewModel> GetOrderList(PageDto input)
        {
            int perPage = input.Perpage; //一頁有10筆資料
            int currentPage = input.CurrentPage;//當前頁面

            DateTime startDate =input.StartDate;
            DateTime endDate = input.EndDate;
            var orderList = _repository.ListWhere<Order>(o => o.CreateTime >= startDate && o.CreateTime <= endDate);
            //var orderDetail = _repository.ListWhere<OrderDetail>(od => true);
            var orderDetail = _repository.ListWhere<OrderDetail>(od => orderList.Select(o=>o.Id).Contains(od.OrderId));
            var sellerList = _repository.ListWhere<Seller>(s => orderList.Select(o => o.SellerId).Contains(s.Id));
            var UserList = _repository.ListWhere<User>(u => orderList.Select(o => o.UserId).Contains(u.Id));
            var result = orderList
                .Select(o=> new OrderViewModel
                {
                    OrderId = o.Id,
                    //SellerId = o.SellerId,
                    //SellerName = _repository.FirstOrDefault<Seller>(s => s.Id == o.SellerId).ShopName,
                    //Seller = o.SellerId + "" + _repository.FirstOrDefault<Seller>(s => s.Id == o.SellerId).ShopName,
                    Seller = o.SellerId + "" + sellerList.FirstOrDefault(s => s.Id == o.SellerId)?.ShopName ?? string.Empty,
                    //UserId = o.UserId,
                    //User = o.UserId + "" +_repository.FirstOrDefault<User>(s => s.Id == o.UserId).Name,
                    User = o.UserId + "" + UserList.FirstOrDefault(s => s.Id == o.UserId)?.Name ?? string.Empty,
                    //UserName = _repository.FirstOrDefault<User>(s => s.Id == o.UserId).Name,
                    Payment = o.Payment,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    CreateTime = o.CreateTime.AddHours(8).ToString("yyyy/MM/dd-HH:mm"),
                    OrderDetails = orderDetail.Where(od=>od.Id == o.Id).Select(od => new OrderDetailViewModel
                        {
                            OrderId = od.OrderId,
                            ProductName = od.ProductName,
                            Quantity = od.Quantity,
                            UnitPrice = od.UnitPrice,
                            Spec = od.FirstSepcItem + "" + od.SecondSepcItem,
                        }).ToList(),

                }).ToList();
            return result;
        }
        

        public int GetOrderCount()
        {
            var orderCount = _repository.ListWhere<Order>(o => true).Count();
            return orderCount;
        }

    }
}
