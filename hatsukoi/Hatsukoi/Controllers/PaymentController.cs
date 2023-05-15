using ECPay.Payment.Integration;
using Hatsukoi.Models;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using FluentEcpay;
using Item = FluentEcpay.Item;
using Hatsukoi.Models.ViewModels.Cart;
using Hatsukoi.Repository.EntityModel;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Hatsukoi.Models.Dtos;

namespace Hatsukoi.Controllers
{
    //[Route("[controller]/[action]")]
    //[ApiController]
    public class PaymentController : Controller
    {
        //要改設定檔 要放在appsettings
        // 設定 ECpay 金流參數
        //const string _MerchantID = "2000132";
        //const string _HashKey = "5294y06JbISpM5x9";
        //const string _HashIV = "v77hoKGq4kWxNNIS";

        //const string _ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";


        //要記得改網址
        const string _ReturnUrl = "/payment/callback"; // 付款完成後的返回頁面
        const string _NotifyUrl = "/payment/success"; // 付款完成後的通知頁面

        private readonly PaymentService _paymentService;
        private readonly IConfiguration _configuration;
        

        public PaymentController(PaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            
        }

        [HttpPost]
        public IActionResult CheckOut([FromForm] CreateOrderViewModel input, [FromForm] int userId)
        {
            var origin = Request.Headers.Origin.ToString();


            int orderId;
            List<Item> items;
            int totalPrice;
            string orderNumber;

            // 隨機生成4位數字
            var random = new Random();
            var digits = random.Next(10000).ToString("D4");

            if (input.OrderNumber == "0")
            {
                //OrderNumber若=="0"是新訂單
                orderId = _paymentService.CreateOrderAndOrderDetail(userId, input);
                orderNumber = _paymentService.GetOrderNumber(orderId)+ digits;//給綠界的20碼
            }
            else
            {
                //OrderNumber若有值是重新付款，用此值找出訂單
                orderNumber = input.OrderNumber;
                orderId =_paymentService.GetOrderId(orderNumber);
                //orderNumber要再加4碼亂數給綠界交易
                orderNumber = orderNumber + digits;//給綠界的20碼
            }

            items = _paymentService.GetOrderDetailItems(orderId);
            totalPrice = (int)_paymentService.GetOrderTotal(orderId);
            var service = new
            {
                Url = _configuration["ECPay:ServiceURL"],
                MerchantId = _configuration["ECPay:MerchantID"],
                HashKey = _configuration["ECPay:HashKey"],
                HashIV = _configuration["ECPay:HashIV"],
                ServerUrl = origin + _ReturnUrl,
                ClientUrl = origin + _NotifyUrl
            };
            var transaction = new
            {
                No = orderNumber,//訂單編號
                Description = "測試購物系統",
                Date = DateTime.UtcNow,
                Method = EPaymentMethod.Credit,
                Items = items,
            };

            
            IPayment payment = new PaymentConfiguration()
                .Send.ToApi(
                    url: service.Url)
                .Send.ToMerchant(
                    service.MerchantId)
                .Send.UsingHash(
                    key: service.HashKey,
                    iv: service.HashIV)
                .Return.ToServer(
                    url: service.ServerUrl)
                .Return.ToClient(
                    url: service.ClientUrl,
                     needExtraPaidInfo: true)
                //這裡要加needExtraPaidInfo: true
                .Transaction.New(
                    no: transaction.No,//需要20碼!!!
                    description: transaction.Description,
                    date: transaction.Date)
                .Transaction.UseMethod(
                    method: transaction.Method)
                .Transaction.WithItems(
                    items: transaction.Items,
                    amount: totalPrice)
                .Transaction.WithCustomFields(
                    //自訂欄位
                    field1: orderId.ToString(),
                    field2: input.CouponId.ToString())
                .Generate();

            return View(payment);
        }

        [HttpPost]
        public IActionResult Callback(PaymentResult result)
        {
            var hashKey = _configuration["ECPay:HashKey"];  
            var hashIV = _configuration["ECPay:HashIV"];

            // 務必判斷檢查碼是否正確。
            if (!CheckMac.PaymentResultIsValid(result, hashKey, hashIV)) return BadRequest();

            // 處理後續訂單狀態的更動等等...。
            
            return Ok("1|OK");

        }

        [HttpPost]
        public async Task<IActionResult> success()
        {
            var form = await Request.ReadFormAsync();
            var merchantTradeNo = form["MerchantTradeNo"].ToString();//orderNumber 20碼
            var paymentDate = form["PaymentDate"];
            var rtnMsg = form["RtnMsg"];
            var tradeNo = form["TradeNo"];//綠界編號
            var tradeAmt = form["TradeAmt"];
            var orderId = form["CustomField1"];//訂單id
            var couponId = form["CustomField2"];//優惠券id
            var rtnCode = form["RtnCode"];

            var displayOrdernumber = merchantTradeNo.Substring(0, merchantTradeNo.Length - 4);//16碼

            var orderNumber = new SuccessDto
            {
                OrderNumber = displayOrdernumber
            };
            if (rtnCode == "1")
            {
                
                _paymentService.UpdateOrder(orderId, tradeNo, paymentDate);
                return View(orderNumber);
            }
            else
            {
                _paymentService.UpdateOrder(orderId, "", "");

                //return RedirectToAction("Order", "User");

                return Redirect($"/User/OrderDetail?orderNum={orderNumber.OrderNumber}");
                
            }
            

        }


    }
}
