using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.Cart;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
using NuGet.Versioning;
using Org.BouncyCastle.Asn1.X509;
using System.Data.Entity;
using Item = FluentEcpay.Item;

using static Org.BouncyCastle.Math.EC.ECCurve;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Service
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly UserService _userService;
        



        public CartService(IRepository repository, IConfiguration config, ICartRepository cartRepository, IAccountService accountService, UserService userService)
        {
            
            _cartRepository = cartRepository;
            _config = config;
            _accountService = accountService;
            _userService = userService;
            
        }


        public async Task<string> CreateCartList(int productId, int userId, int specId, int quantity )
        {
            //var cart = _cartRepository.GetAll<Cart>().Select(x => x.UserId).ToList().Contains(userId);
            var hadCart = await _cartRepository.AnyAsync<Cart>(x => x.UserId == userId);
            //購物車內有沒有此user的東西
            if (!hadCart)//沒有此user的資料
            {
                var target = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    SellerId = await GetSellerId(productId),
                    
                    Quantity = quantity,
                    ProductSpecificationId = specId,


                };
                _cartRepository.Create(target);

                return "save";
            }
            else//有此user的資料
            {
                //有沒有此項商品
                var cartItem = await GetCartItem(productId, userId, specId);

                if (cartItem == null)//沒有此項商品直接存
                {
                    var target = new Cart
                    {
                        UserId = userId,
                        ProductId = productId,
                        SellerId = await GetSellerId(productId),

                        Quantity = quantity,
                        ProductSpecificationId = specId,

                    };
                    _cartRepository.Create(target);

                    return "save";
                }
                else//有此項商品 數量+quantity
                {
                    var targetQty = await GetQuantity(productId, userId, specId);
                    var newQty = targetQty + quantity;
                    
                    cartItem.Quantity = newQty;
                    _cartRepository.Update(cartItem);

                    return "";
                }
                
            }


        }
        public async Task<int> GetSellerId(int productId)
        {
            //var sellerId = _cartRepository.GetAll<Product>().Where(p => p.Id == productId).First().SellerId;
            var cartProduct = await _cartRepository.FirstOrDefaultAsync<Product>(p => p.Id == productId);
            var sellerId = cartProduct.SellerId;

            return sellerId;
        }
        public async Task<int> GetQuantity(int productId, int userId, int specId)
        {
            //var quantity = _cartRepository.GetAll<Cart>().Where(c => c.UserId == userId && c.ProductId == productId).First().Quantity;
            var cartProduct = await _cartRepository.FirstOrDefaultAsync<Cart>(c => c.UserId == userId && c.ProductId == productId && c.ProductSpecificationId == specId);
            var quantity = cartProduct.Quantity;
            return quantity;
        }
        public async Task<Cart> GetCartItem(int productId, int userId, int specId)
        {
            //var cartItem = _cartRepository.GetAll<Cart>().FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
            var cartItem = await _cartRepository.FirstOrDefaultAsync<Cart>(c => c.UserId == userId && c.ProductId == productId && c.ProductSpecificationId == specId);
            return cartItem;
        }



        public List<ShopCart> GetCartByUserId(int userId)
        {
            //var cartList = _cartRepository.GetAll<Cart>().Where(cart => cart.UserId == userId).ToList();
            //var sellerList = _cartRepository.GetAll<Seller>().Where(seller => cartList.Select(cart => cart.SellerId).Contains(seller.Id)).ToList();
            //var productList = _cartRepository.GetAll<Product>().Where(p => cartList.Select(cart => cart.ProductId).Contains(p.Id)).ToList();
            //var productImgList = _cartRepository.GetAll<ProductImg>().Where(pi => productList.Select(p => p.Id).Contains(pi.ProductId)).OrderBy(pi => pi.Sort).ToList();
            var cartList = _cartRepository.ListWhere<Cart>(cart => cart.UserId == userId);
            var sellerList = _cartRepository.ListWhere<Seller>(seller => cartList.Select(cart => cart.SellerId).Contains(seller.Id));
            var productList = _cartRepository.ListWhere<Product>(p => cartList.Select(cart => cart.ProductId).Contains(p.Id));
            var productImgList = _cartRepository.ListWhere<ProductImg>(pi => productList.Select(p => p.Id).Contains(pi.ProductId));
            var specList = _cartRepository.ListWhere<ProductSpecification>(ps => productList.Select(p => p.Id).Contains(ps.ProductId));


            var result = new List<ShopCart>();
            foreach (var seller in sellerList)
            {
                var shop = new ShopCart();

                shop.Id = seller.Id;
                shop.ShopName = seller.ShopName;

                var items = cartList.Where(x => x.SellerId == seller.Id).ToList();
                shop.CartItems = items.Select(x => new CartItem
                {
                    CartId = x.Id,
                    ProductId = x.ProductId,
                    ItemName = productList.FirstOrDefault(p => p.Id == x.ProductId)?.ProductName ?? "產品名稱",
                    ItemImg = productImgList.FirstOrDefault(pi => pi.ProductId == x.ProductId)?.Img ?? "產品照片",
                    Quantity = x.Quantity,
                    //UnitPrice = productList.FirstOrDefault(p => p.Id == x.ProductId)?.Price ?? 0,
                    SpecID = _cartRepository.FirstOrDefault<ProductSpecification>(ps => ps.Id == x.ProductSpecificationId)?.Id ?? 0,
                    SpecName = _cartRepository.FirstOrDefault<ProductSpecification>(ps => ps.Id == x.ProductSpecificationId)?.FirstSepcItem ??  ""+
                               _cartRepository.FirstOrDefault<ProductSpecification>(ps => ps.Id == x.ProductSpecificationId)?.SecondSepcItem ?? "",
                    UnitPrice = _cartRepository.FirstOrDefault<ProductSpecification>(ps => ps.Id == x.ProductSpecificationId)?.UnitPrice ?? productList.FirstOrDefault(p => p.Id == x.ProductId)?.Price ?? 0
                    //SpecList = specList.Where(sp =>sp.ProductId == x.ProductId).Select(sp=>new SpecList
                    //{
                    //    SpecItem = sp.FirstSepcItem + sp.SecondSepcItem,
                    //    UnitPrice = sp.UnitPrice
                    //}).ToList()


                }).ToList();


                result.Add(shop);
            }
            return result;



        }


        public string DeleteCartProduct(int cartId, int userId)
        {
            var target = _cartRepository.FirstOrDefault<Cart>(x => x.UserId == userId && x.Id == cartId);

            if (target != null)
            {
                //var entity = new Cart 
                //{
                //    ProductId = cartId,
                //    UserId = userId,

                //};
                _cartRepository.Delete(target);


                return "delete";
            }
            return "";

        }
        public string DeleteCartShop(int shopId, int userId)
        {
            //購物車裡有沒有存這個商店
            //var hadsShop = _cartRepository.GetAll<Cart>().Where(x => x.UserId == userId).Select(x=>x.UserId).Contains(id);
            var hadsShop = _cartRepository.Any<Cart>(x => x.UserId == userId && x.SellerId == shopId);
            if (hadsShop)
            {
                var targets = _cartRepository.ListWhere<Cart>(x => x.UserId == userId && x.SellerId == shopId).Select(x => x.Id);
                foreach (var target in targets)
                {
                    var cartProduct = _cartRepository.FirstOrDefault<Cart>(x => x.Id == target);
                    //var shop = new Cart();
                    //shop.Id = target;
                    _cartRepository.Delete(cartProduct);

                }

                return "delete";
            }
            return "";
        }


        public void UpdateQuantity(int quantity, int cartId)
        {
            var qty = _cartRepository.FirstOrDefault<Cart>(x => x.Id == cartId);
            qty.Quantity = quantity;
            _cartRepository.Update(qty);
        }

        //取得要結帳的店家資訊
        public ShopCart GetCheckShop(int shopId,int couponId, decimal discountAmount)
        {
            var user = _accountService.GetUser().Id;
            var userCart = GetCartByUserId(user);
            var checkShop = userCart.FirstOrDefault(x => x.Id == shopId);
            if (couponId > 0)
            {
                checkShop.CouponId = couponId;
                //checkShop.DiscountAmount = discountAmount;
            }
            //checkShop.CouponId = 0;
            //checkShop.DiscountAmount = 0;

            //checkShop.CouponId = 0;
            checkShop.DiscountAmount = discountAmount;


            return checkShop;
        }
        

        public List<CartUserCouponViewModel> GetShopCanUseCoupon(int sellerId)
        {
            var userId = _accountService.GetUser().Id;
            var now = DateTime.UtcNow;

            //從coupon清單搜尋此user可使用的優惠券(可使用 2)
            var UserCoupon = _cartRepository.ListWhere<CouponList>(cl => cl.UserId == userId && cl.Status == 2 );
            var seller = _cartRepository.FirstOrDefault<Seller>(seller => seller.Id == sellerId);
            //優惠券使用時間篩選
            var shopCoupon = _cartRepository.ListWhere<Coupon>(c =>UserCoupon.Select(u=>u.CouponId).Contains(c.Id))
                .Where(c=>c.SellerId==sellerId && c.StartTime <= now && now <= c.EndTime).ToList();
            var userShopCoupon = shopCoupon.Select(s => new CartUserCouponViewModel
            {
                CouponId = s.Id,
                sellerId = sellerId,
                Condition = s.Condition,
                CouponNumber = s.PromoCode,
                StartTime = s.StartTime,
                StartTimeStr = s.StartTime.ToString("yyyy/MM/dd"),
                EndTime = s.EndTime,
                EndTimeStr = s.EndTime?.ToString("yyyy/MM/dd"),
                Discount = s.Discount,
                DiscountStr = ((int)(s.Discount * 100)).ToString(),
                Img = seller.ShopBannerSquare,
                SellerName = seller.ShopName
            }).ToList();

            return userShopCoupon;
        }

        public int GetCartAmount(int userId)
        {
           var cartProduct = _cartRepository.ListWhere<Cart>(c=> c.UserId == userId).Count();

            return cartProduct;
        }

    }


}

