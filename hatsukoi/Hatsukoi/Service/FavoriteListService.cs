
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.CodeAnalysis;
using Org.BouncyCastle.Asn1.X509;
using System.Data.SqlClient;
using System.Linq;
using static Hatsukoi.Models.ViewModels.FavorViewModel;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Hatsukoi.Service
{
    public class FavoriteListService
    {
        private readonly IRepository _repository;
        private readonly IFavRepository _favRepository;
        
        private readonly IConfiguration _config;
        public FavoriteListService(IRepository repository, IConfiguration config, IFavRepository favRepository)
        {
            _repository = repository;
            _config = config;
            _favRepository = favRepository;

        }

        //存進喜歡商品表 改async
        public async Task<string> CreateFavProduct(int productId, int userId)
        {
            //var favProductId =  _favRepository.GetAll<FavProduct>().Select(x=>x.UserId).ToList().Contains(userId);
            var hadUser = await _favRepository.AnyAsync<FavProduct>(x => x.UserId == userId);
            if (!hadUser)
            {
                var target = new FavProduct
                {
                    ProductId = productId,
                    UserId = userId,
                    Time = DateTime.UtcNow,
                };
                _favRepository.Create(target);
             

                return "已將此商品加入喜歡清單";
            }
            else
            {
                //var FavProduct=  _favRepository.GetAll<FavProduct>().Where(x=>x.UserId==userId).Select(x=>x.ProductId).ToList().Contains(productId);
                var hadProduct = await _favRepository.AnyAsync<FavProduct>(x =>x.UserId == userId && x.ProductId == productId );
                if (!hadProduct)
                {
                    var target = new FavProduct
                    {
                        ProductId = productId,
                        UserId = userId,
                        Time = DateTime.UtcNow,
                    };
                    _favRepository.Create(target);
                 
                    return "已將此商品加入喜歡清單";
                }
                return "";
            }

        }
        //存進喜歡商家表 改async
        public async Task<string> CreateFavShop(int sellerId, int userId)
        {
            //var favShopId = _favRepository.GetAll<FavShop>().Select(x => x.UserId).ToList().Contains(userId);
            var hadUser = await _favRepository.AnyAsync<FavShop>(x => x.UserId == userId);

            if (!hadUser)
            {
                var target = new FavShop
                {
                    SellerId = sellerId,
                    UserId = userId,
                    Time = DateTime.UtcNow,
                };
                _favRepository.Create(target);
                

                return "已關注此品牌";
            }
            else
            {
                //var FavProduct = _favRepository.GetAll<FavShop>().Where(x => x.UserId == userId).Select(x => x.SellerId).ToList().Contains(sellerId);
                var hadShop = await _favRepository.AnyAsync<FavShop>(x => x.UserId == userId && x.SellerId == sellerId);
                if (!hadShop)
                {
                    var target = new FavShop
                    {
                        SellerId = sellerId,
                        UserId = userId,
                        Time = DateTime.UtcNow,
                    };
                    _favRepository.Create(target);
              

                    return "已關注此品牌";

                }

                return "";
            }

        }
        //這邊裡面都要加await ????
        public async Task<FavorViewModel> GetFavorViewModel(int userId)
        {
            var favList = new FavorViewModel();
            //var hadUser = await _favRepository.AnyAsync<FavProduct>(x => x.UserId == userId);


            var favProductList = _repository.ListWhere<FavProduct>(fp => fp.UserId == userId);

            var favProduct = favProductList.Select( fp => new FavProductViewModel
            {
                Id = fp.Id,
                ProductId = fp.ProductId,
                UserId = fp.UserId,
                //ProductName = _repository.GetAll<Product>().First(p => p.Id == fp.ProductId).ProductName,
                ProductName = _repository.FirstOrDefault<Product>(p => p.Id == fp.ProductId).ProductName,
                ProductPrice = _repository.FirstOrDefault<Product>(p => p.Id == fp.ProductId).Price,
                ProductImg = _repository.FirstOrDefault<ProductImg>(pi => pi.ProductId == fp.ProductId && pi.Sort == 1).Img,
                ProductStatus = _repository.FirstOrDefault<Product>(p => p.Id == fp.ProductId).ProductStatus,
            }).ToList();
            
            //已上架
            var favProductCanSell =favProduct.Where(f=>f.ProductStatus == 2).ToList();


            var favShopList = _repository.ListWhere<FavShop>(fp => fp.UserId == userId);
            var sellerIdList = favShopList.Select(fs => fs.SellerId).ToList();
            var productImg = _repository.ListWhere<ProductImg>(x => true);
            var sellerInfos = _repository.ListWhere<Seller>(x => favShopList.Select(c => c.SellerId).Contains(x.Id));
            var product = _repository.ListWhere<Product>(p => favShopList.Select(c => c.SellerId).Contains(p.SellerId));

            //var img = new 
            //var productId = _repository.ListWhere<Product>(p=> p.SellerId == sellerId).Select(p => p.Id).ToList();
            var favShop = favShopList.Select(fs => new FavShopViewModel
            {
                Id = fs.Id,
                UserId=fs.UserId,
                SellerId = fs.SellerId,
                ShopName = sellerInfos.FirstOrDefault(s => s.Id == fs.SellerId).BrandName,
                SellerLogo = sellerInfos.FirstOrDefault(s => s.Id == fs.SellerId).Logo,
                ProductImgs = product.Where(p => p.SellerId == fs.SellerId).Take(3).Select(p => productImg.FirstOrDefault( pi=>pi.ProductId == p.Id).Img).ToList()
            }).ToList();

            favList.FavProducts = favProductCanSell;
            favList.FavShops = favShop;



            return  favList;
        }

        public async Task DeleteFavProduct(int productId, int userId)
        {
            //var hadFavProduct = _favRepository.GetAll<FavProduct>().Where(x => x.UserId == userId).Select(x => x.ProductId).ToList().Contains(productId);
            bool hadProduct = await _favRepository.AnyAsync<FavProduct>(x => x.ProductId == productId && x.UserId == userId);

            if (hadProduct)
            {
                //var favProduct = GetFavProductList();
                //var target = favProduct.FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);
                //var entity = new FavProduct
                //{
                //    Id = target.Id,
                //    ProductId = target.ProductId,
                //    UserId = target.UserId,

                //};

                var target = await _favRepository.FirstOrDefaultAsync<FavProduct>(x => x.ProductId == productId && x.UserId == userId);
                _favRepository.Delete(target);


            }
            

        }

        public async Task DeleteFavShop(int sellerId, int userId)
        {
            //var favShop = GetFavShopList();
            //var target = favShop.FirstOrDefault(x => x.SellerId == shopId && x.UserId == userId);
            //var entity = new FavShop
            //{
            //    Id = target.Id,
            //    SellerId = target.SellerId,
            //    UserId = target.UserId,

            //};

            var hadShop = await _favRepository.AnyAsync<FavShop>(x => x.SellerId == sellerId && x.UserId == userId);

            if (hadShop)
            {
                var target = await _favRepository.FirstOrDefaultAsync<FavShop>(x =>x.UserId == userId && x.SellerId == sellerId );
                _favRepository.Delete(target);
            }
                
        }

        






    }
}
