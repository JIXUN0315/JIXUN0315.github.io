
using Hatsukoi.Common;
using Hatsukoi.Models.Dtos.User;
using Hatsukoi.Models.ViewModels.OrderVM;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Service
{
    public class OrderService
    {
        private readonly IRepository _dBRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IRepository dBRepository, IOrderRepository orderRepository)
        {
            _dBRepository = dBRepository;
            _orderRepository = orderRepository;
        }

        //取到賣家身分
        public int GetSellerIdByUserId(int userId)
        {
            var sellerId = _dBRepository.ListWhere<Seller>(s => s.UserId == userId)
                .Select(s => s.Id).First();

            return sellerId;
        }
        //取到訂單
        public List<OrderViewModel> GetOrdersBySeller(int sellerId)
        {
            var orderSource = _dBRepository.ListWhere<Order>(o => o.SellerId == sellerId);
            var orderUserSource = orderSource.Select(o => o.UserId).ToList();
            var userSource = _dBRepository.ListWhere<User>(u => orderUserSource.Contains(u.Id));
            var orderIdSource = orderSource.Select(o => o.Id).ToList();
            var orderDetailSource = _dBRepository.ListWhere<OrderDetail>(od => orderIdSource.Contains(od.OrderId));

            //foreach (var order in orderSource)
            //{
            //    var OrderDetails = _dBRepository.ListWhere<OrderDetail>(o => o.OrderId == order.Id)
            //        .ToList();
            //}

            // 取得該賣家的所有訂單
            var orders = orderSource.Select(o =>
            {

                var user = userSource.First<User>(u => u.Id == o.UserId);

                return new OrderViewModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    Status = (OrderStatus)o.Status,
                    StatusText = GetOrderStatusText((OrderStatus)o.Status),
                    CouponId = o.CouponId,
                    RecipientAddress = o.RecipientAddress,
                    RecipientName = o.RecipientName,
                    RecipientPhone = o.RecipientPhone,
                    TotalPrice = o.TotalPrice,
                    Evaluate = o.Evaluate,
                    EvaluateDate = o.EvaluateDate?.AddHours(8).ToString("yyyy-MM-dd"),
                    EvaluateText = o.EvaluateText,
                    CreateTime = o.CreateTime.AddHours(8).ToString("yyyy-MM-dd HH:mm"),
                    Name = user.Name,
                    Photo = user.Photo,
                    TotalQuantity = o.OrderDetails.Sum(od => od.Quantity),
                    OrderDetails = orderDetailSource.Where(od => od.OrderId == o.Id).Select(od => new OrderDetailsViewModel
                    {
                        Id = od.Id,
                        OrderId = od.OrderId,
                        ProductSpecificationId = od.ProductSpecificationId,
                        Quantity = od.Quantity,
                        UnitPrice = (decimal)od.UnitPrice,
                        ProductName = od.ProductName,
                        FirstSepcItem = od.FirstSepcItem,
                        SecondSepcItem = od.SecondSepcItem,
                        ProductImg = od.ProductImg,
                    }).ToList()
                };
            }).ToList();

            return orders;
        }
        //依狀態取到該狀態的訂單
        public List<OrderViewModel> GetOrdersByStatus(List<OrderViewModel> orders, HatsukoiEnum.OrderStatus status)
        {
            // 取得該賣家的所有訂單
            var orderWithStatus = orders.Where(o => o.Status == status)
                .OrderByDescending(o => o.CreateTime)
                .ToList();

            return orderWithStatus;
        }
        //取訂單明細
        public OrderViewModel GetOrderDetailsByOrderNum(string orderNum)
        {
            decimal originalPrice = 0;
            decimal discountNum = 0;
            decimal discount = 0;
            var order = _dBRepository.FirstOrDefault<Order>(o => o.OrderNumber == orderNum); ;
            var orderUser = _dBRepository.FirstOrDefault<User>(u => u.Id == order.UserId);
            var orderDetail = _dBRepository.ListWhere<OrderDetail>(od => od.OrderId == order.Id);
            var specSource = orderDetail.Select(o => o.ProductSpecificationId).ToList();
            var specification = _dBRepository.ListWhere<ProductSpecification>(p => specSource.Contains(p.Id));
            
            if (order.CouponId != 0)
            {

                var coupon = _dBRepository.FirstOrDefault<Coupon>(c => c.Id == order.CouponId);
                discount = coupon.Discount;
                originalPrice = Math.Round( (order.TotalPrice / discount),0);
                discountNum = Math.Round((1 - discount) * originalPrice, 0);

            }
            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                Status = (OrderStatus)order.Status,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                StatusText = GetOrderStatusText((OrderStatus)order.Status),
                RecipientAddress = order.RecipientAddress,
                RecipientName = order.RecipientName,
                RecipientPhone = order.RecipientPhone,
                TotalPrice = order.TotalPrice,
                Discount = discount,
                DiscountNum=discountNum,
                OriginalPrice=originalPrice,
                CreateTime = order.CreateTime.AddHours(8).ToString("yyyy-MM-dd hh:mm"),
                PayTime = order.PayTime?.AddHours(8).ToString("yyyy-MM-dd hh:mm"),
                StatusSendTime = order.StatusSendTime?.AddHours(8).ToString("yyyy-MM-dd hh:mm"),
                StatusCancelTime = order.StatusCancelTime?.AddHours(8).ToString("yyyy-MM-dd hh:mm"),
                StatusFinishTime = order.StatusFinishTime?.AddHours(8).ToString("yyyy-MM-dd hh:mm"),
                Name = orderUser.Name,
                Photo = orderUser.Photo,
                MemberLevel = GetMemberLevelText((MemberLevel)orderUser.MemberLevel),
                TotalQuantity = _dBRepository.ListWhere<OrderDetail>(od => od.OrderId == order.Id).Sum(od => od.Quantity),
                Memo = order.Memo,

                OrderDetails = orderDetail.Select(od =>
                {
                    var spec = specification.FirstOrDefault(s => s.Id == od.ProductSpecificationId);
                    return new OrderDetailsViewModel
                    {
                        Id = od.Id,
                        OrderId = od.OrderId,
                        ProductId = spec.ProductId,
                        ProductSpecificationId = od.ProductSpecificationId,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                        ProductName = od.ProductName,
                        FirstSepcItem = od.FirstSepcItem,
                        SecondSepcItem = od.SecondSepcItem,
                        ProductImg = od.ProductImg,
                        
                    };
                }).ToList()

            };

            return orderViewModel;
        }
        //轉換訂單文字
        private string GetOrderStatusText(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.NotPay:
                    return "待付款";
                case OrderStatus.NotShipped:
                    return "待出貨";
                case OrderStatus.ToBeReceived:
                    return "已出貨";
                case OrderStatus.Completed:
                    return "已完成";
                case OrderStatus.Cancelled:
                    return "已取消";
                default:
                    return "";
            }
        }
        //轉換會員文字
        private string GetMemberLevelText(MemberLevel level)
        {
            switch (level)
            {
                case MemberLevel.Blue:
                    return "品藍會員";
                case MemberLevel.Silver:
                    return "白銀會員";
                case MemberLevel.Gold:
                    return "黃金會員";
                case MemberLevel.Diamond:
                    return "鑽石會員";
                case MemberLevel.Monarch:
                    return "尊爵會員";
                default:
                    return "";
            }
        }
        //取指定訂單
        public Order GetOrderByOrderNumber(string orderNum)
        {
            return _dBRepository.FirstOrDefault<Order>(x => x.OrderNumber == orderNum);
        }
        //訂單出貨
        public void ShipOrderByOrderNumber(string orderNum)
        {
            var order = GetOrderByOrderNumber(orderNum);
            order.Status = (int)HatsukoiEnum.OrderStatus.ToBeReceived;
            order.StatusSendTime = DateTime.UtcNow;
            _dBRepository.Update(order);
        }
        //取消訂單
        public void CancelOrderByOrderNumber(string orderNum, string cancelReason)
        {
            var order = GetOrderByOrderNumber(orderNum);
            order.Status = (int)HatsukoiEnum.OrderStatus.Cancelled;
            order.Memo = cancelReason;
            order.StatusCancelTime = DateTime.UtcNow;
            _dBRepository.Update(order);
        }
        //取有評價的訂單
        public List<EvaluateViewModel> GetOrderHasEvaluate(List<OrderViewModel> orders)
        {
            var evaluateList = orders.Where(o => o.Evaluate != null).ToList();
            return evaluateList.Select(e => new EvaluateViewModel
            {
                Evaluate = e.Evaluate,
                EvaluateDate = e.EvaluateDate,
                EvaluateText = e.EvaluateText,
                UserImg = e.Photo,
                UserName = e.Name,
                OrderNumber = e.OrderNumber

            }).ToList();
        }

        public int OrderCount(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime seven = DateTime.Now.AddDays(-7);
            var sevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(y => y.StatusFinishTime);
            int count = 0;
            foreach (var s in sevenDay)
            {
                if (s < dt && s > seven)
                {
                    count++;
                }
            }
            return count;
        }
        public decimal OrderAvgTotlePrice(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime seven = DateTime.Now.AddDays(-7);
            var sevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in sevenDay.Select(x => x.StatusFinishTime))
            {
                if (s < dt && s > seven)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return decimal.Round(total/7,2);
        }
        public decimal OrderTotlePrice(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime seven = DateTime.Now.AddDays(-7);
            var sevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in sevenDay.Select(x => x.StatusFinishTime))
            {
                if (s < dt && s > seven)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return total;
        }
        public int DoubleSevenOrderCount(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime doubleSeven = DateTime.Now.AddDays(-14);
            var doubleSevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(y => y.StatusFinishTime);
            int count = 0;
            foreach (var s in doubleSevenDay)
            {
                if (s < dt && s > doubleSeven)
                {
                    count++;
                }
            }
            return count;
        }
        public decimal DoubleSevenOrderAvgTotlePrice(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime seven = DateTime.Now.AddDays(-14);
            var sevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in sevenDay.Select(x => x.StatusFinishTime))
            {
                if (s < dt && s > seven)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return decimal.Round(total / 7, 2);
        }
        public decimal DoubleSevenOrderTotlePrice(int sellerId)
        {
            DateTime dt = DateTime.Now;
            DateTime seven = DateTime.Now.AddDays(-14);
            var sevenDay = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in sevenDay.Select(x => x.StatusFinishTime))
            {
                if (s < dt && s > seven)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return total;
        }
        public string ChangeDate(int a)
        {
            var seven = DateTime.Now.AddDays(a).ToShortDateString();
            return seven;
        }
        public string SevenDay()
        {
            int number = -7;
            DateTime dt = DateTime.Now;
            var seven = ChangeDate(number);
            var sevenDay = $"{seven} - {dt.ToShortDateString()}";
            return sevenDay;
        }
        public string DoubleSevenDay()
        {
            int number = -14;
            DateTime dt = DateTime.Now;
            var seven = ChangeDate(number);
            var sevenDay = $"{seven} - {dt.ToShortDateString()}";
            return sevenDay;
        }
        public string LastMonth()
        {
            var lastMonthFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();

            var lastMonthLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
            var lastMonth = $"{lastMonthFirstDay} - {lastMonthLastDay}";
            return lastMonth;
        }

        public int LastMonthOrderCount(int sellerId)
        {
            var lastMonthFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);

            var lastMonthLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(y => y.StatusFinishTime);
            int count = 0;
            foreach (var s in day)
            {
                if (s < lastMonthLastDay && s > lastMonthFirstDay)
                {
                    count++;
                }
            }
            return count;
        }
        public decimal LastMonthOrderAvgTotlePrice(int sellerId)
        {
            var lastMonthFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
            var lastMonthLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            TimeSpan avgDay = lastMonthLastDay.Subtract(lastMonthFirstDay);
            var avgDayCount = avgDay.Days + 1;
            decimal total = 0;
            foreach (var s in day.Select(x => x.StatusFinishTime))
            {
                if (s < lastMonthLastDay && s > lastMonthFirstDay)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return decimal.Round(total / avgDayCount, 2);
        }

        public decimal LastMonthOrderTotlePrice(int sellerId)
        {
            var lastMonthFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
            var lastMonthLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in day.Select(x => x.StatusFinishTime))
            {
                if (s < lastMonthLastDay && s > lastMonthFirstDay)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return total;
        }

        public string LastYear()
        {
            var LastYearFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToShortDateString();
            var LastYearLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1).ToShortDateString();
            var lastYear = $"{LastYearFirstDay} - {LastYearLastDay}";
            return lastYear;
        }
        public int LastYearOrderCount(int sellerId)
        {
            var LastYearFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
            var LastYearLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(y => y.StatusFinishTime);
            int count = 0;
            foreach (var s in day)
            {
                if (s < LastYearLastDay && s > LastYearFirstDay)
                {
                    count++;
                }
            }
            return count;
        }
        public decimal LastYearOrderAvgTotlePrice(int sellerId)
        {
            var LastYearFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
            var LastYearLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            TimeSpan avgDay = LastYearLastDay.Subtract(LastYearFirstDay);
            var avgDayCount = avgDay.Days + 1;
            decimal total = 0;
            foreach (var s in day.Select(x => x.StatusFinishTime))
            {
                if (s < LastYearLastDay && s > LastYearFirstDay)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return decimal.Round(total / avgDayCount, 2);
        }
        public decimal LastYearOrderTotlePrice(int sellerId)
        {
            var LastYearFirstDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1);
            var LastYearLastDay = DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1);
            var day = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId);
            var order = _orderRepository.ListWhere<Order>(x => x.SellerId == sellerId).Select(x => x.TotalPrice);
            decimal total = 0;
            foreach (var s in day.Select(x => x.StatusFinishTime))
            {
                if (s < LastYearLastDay && s > LastYearFirstDay)
                {
                    var a = _orderRepository.FirstOrDefault<Order>(x => x.StatusFinishTime == s).TotalPrice;
                    total += a;
                }
            }
            return total;
        }
    }

}
