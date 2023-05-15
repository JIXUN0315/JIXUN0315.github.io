using Hatsukoi.Common;
using Hatsukoi.Models.ViewModels.Cart;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using static Hatsukoi.Common.HatsukoiEnum;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Item = FluentEcpay.Item;

namespace Hatsukoi.Service
{
    public class PaymentService
    {

        private readonly IRepository _repository;
        private readonly PaymentRepository _paymentRepository;
        

        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly CartService _cartService;

        public PaymentService(IRepository repository, IConfiguration config, IAccountService accountService,CartService cartService, PaymentRepository paymentRepository)
        {
            _repository = repository;
            _config = config;
            _accountService = accountService;
            _cartService = cartService;
            _paymentRepository = paymentRepository;
        }

        // 生成訂單編號(原16碼ex:202304031330SDFG) 改20碼ex:202304031330SDFG1234
        public string CreateOrderNumber()
        {
            // 取得當前時間，並生成時間字串
            var now = DateTime.Now;
            var timeString = now.ToString("yyyyMMddHHmm");

            // 隨機生成4個英文字母
            var random = new Random();
            var letter1 = (char)random.Next('A', 'Z' + 1);
            var letter2 = (char)random.Next('A', 'Z' + 1);
            var letter3 = (char)random.Next('A', 'Z' + 1);
            var letter4 = (char)random.Next('A', 'Z' + 1);

            // 隨機生成4位數字
            //var digits = random.Next(10000).ToString("D4");

            // 將時間字串、4個英文字母和4位數字合併成訂單編號
            //var orderNumber = timeString + letter1 + letter2 + letter3 + letter4 + digits;

            //// 將時間字串和4個英文字母合併成訂單編號
            var orderNumber = timeString + letter1 + letter2 + letter3 + letter4;

            return orderNumber;
        }




        public int CreateOrderAndOrderDetail(int userId, CreateOrderViewModel input)
        {
            //結帳商品資訊
            var checkCartShopName = _cartService.GetCheckShop(input.SellerId,input.CouponId, input.DiscountAmount).ShopName;
            var checkCartProduct = _cartService.GetCheckShop(input.SellerId, input.CouponId, input.DiscountAmount).CartItems;

            

            var targetOrder = new Order
            {

                CouponId = input.CouponId,
                UserId = _accountService.GetUser().Id,
                CreateTime = DateTime.UtcNow,
                OrderNumber = CreateOrderNumber(),//16碼
                Payment = input.Payment,
                RecipientName = input.RecipientName,
                RecipientPostCode = input.RecipientPostCode,
                RecipientAddress = input.RecipientAddress,
                RecipientCity = input.RecipientCity,
                RecipientPhone = input.RecipientPhone,
                GreenPayId = "",
                SellerId = input.SellerId,
                Status = input.Status,
                TotalPrice = input.TotalPrice,

            };

            //_repository.Create(targetOrder);
            

            List<OrderDetail> orderDetailList = new List<OrderDetail>();
            foreach (var product in checkCartProduct)
            {
                var targetOrderDetail = new OrderDetail
                {
                    OrderId = 0,
                    //不用存商品ID???
                    ProductName = product.ItemName,
                    ProductImg = product.ItemImg,
                    //UnitPrice = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.UnitPrice ?? product.UnitPrice ?? 0,
                    UnitPrice = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.UnitPrice ?? 0,
                    Quantity = product.Quantity,
                    //規格跟商品關係
                    ProductSpecificationId = product?.SpecID ?? 0,
                    FirstSpecification = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.FirstSpecification ?? "",
                    FirstSepcItem = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.FirstSepcItem ?? "",
                    SecondSpecification = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.SecondSpecification ?? "",
                    SecondSepcItem = _repository.FirstOrDefault<ProductSpecification>(ps => ps.Id == product.SpecID)?.SecondSepcItem ?? "",
                    SellerName = checkCartShopName,

                };
                orderDetailList.Add(targetOrderDetail);
                //_repository.Create(targetOrderDetail);

            }

            
            //if (input.CouponId > 0)
            //{
            //    var UsedCouponId = input.CouponId;
            //    var UsedCoupon = _repository.FirstOrDefault<CouponList>(cl => cl.CouponId == UsedCouponId);
            //    UsedCoupon.Status = (int)HatsukoiEnum.CouponStatus.Used;
            //    _repository.Update(UsedCoupon);
            //}
            var targetCart = _repository.ListWhere<Cart>(x => x.UserId == userId && x.SellerId == input.SellerId).Select(x => x.Id);
            List<Cart> cartList = new List<Cart>();
            foreach (int target in targetCart)
            {
                var cartProduct = _repository.FirstOrDefault<Cart>(x => x.Id == target);
                cartList.Add(cartProduct);
                //_repository.Delete(cartProduct);
            }
            _paymentRepository.CreateOrderAndOrderDetail(targetOrder, orderDetailList, cartList);
            var orderId = targetOrder.Id; 
            return orderId;
        }

        public List<Item> GetOrderDetailItems(int orderId)
        {
            var orderDetailList = _repository.ListWhere<OrderDetail>(od => od.OrderId == orderId);
            var items = orderDetailList.Select(od => new Item
            {
                Name = od.ProductName,
                Price = (int)od.UnitPrice,
                Quantity = od.Quantity,
            }).ToList();

            return items;
        }
        public string GetOrderNumber(int orderId)
        {
            var order = _repository.FirstOrDefault<Order>(o => o.Id == orderId);
            var orderNumber = order.OrderNumber;
            return orderNumber;
        }
        public decimal GetOrderTotal(int orderId)
        {
            var totalPrice = _repository.FirstOrDefault<Order>(o => o.Id == orderId).TotalPrice;
            return totalPrice;
        }

        public void UpdateOrder(string orderId, string tradeNo, string paymentDate)
        {
            //付款完成更新 綠界號碼 訂單狀態 付款時間
            var order = _repository.FirstOrDefault<Order>(o => o.Id.ToString() == orderId);
           
            if (paymentDate != "")
            {
                order.Status = (int)OrderStatus.NotShipped;
                order.GreenPayId = tradeNo;
                //付款時間綠界回傳string
                //先用這方法取得時間替代
                //DateTime.UtcNow加8怎麼用???
                order.PayTime = DateTime.UtcNow;
                //重新付款時orderNumber要重新產生才能進綠界交易
                //order.OrderNumber = orderNumber;//16碼

            }
            else
            {
                order.Status = (int)OrderStatus.NotPay;
                order.GreenPayId = "";
                
            }
            _repository.Update(order);

            //if(couponIdInt > 0)
            //{
            //    var coupon = _repository.FirstOrDefault<CouponList>(cl => cl.CouponId == couponIdInt && cl.UserId == order.UserId);
            //    coupon.Status = (int)CouponStatus.Used;
            //    _repository.Update(coupon);
            //}
        }

        public int GetOrderId(string ordernumber)
        {
            var orderId = _repository.FirstOrDefault<Order>(o => o.OrderNumber == ordernumber).Id;
            return orderId;
        }

       

    }
}
