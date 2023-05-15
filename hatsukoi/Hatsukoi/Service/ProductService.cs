using Hatsukoi.Common;
using Hatsukoi.Models.Dtos.Create;
using Hatsukoi.Models.Dtos.Product;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.Create;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service.Interface;
using Microsoft.CodeAnalysis;
using Azure;
using NuGet.Packaging;
using System.Data.Entity.Core.Metadata.Edm;
using static Hatsukoi.Common.HatsukoiEnum;
using static Hatsukoi.Models.ViewModels.ProductViewModel;
using Org.BouncyCastle.Asn1.X509;
using System.Runtime.Intrinsics.X86;

namespace Hatsukoi.Service
{
    public class ProductService
    {
        private readonly IRepository _productRepository;
        private readonly AccountService _accountService;
        private readonly IImageService _imageService;

        public ProductService(IRepository productRepository, AccountService accountService, IImageService imageService)
        {
            _productRepository = productRepository;
            _accountService = accountService;
            _imageService = imageService;

        }

        //瀏覽數
        public void RecordViewData(ViewCountDto viewCountDto)
        {
            var product = _productRepository.FirstOrDefault<Product>(x => x.Id == viewCountDto.ProductId);
            if (product == null) return;

            product.ViewTimes++;
            _productRepository.Save();
        }


        //取得一筆ProductId
        public Product GetProductByProductId(int id)
        {
            return _productRepository.FirstOrDefault<Product>(x => x.Id == id && (x.ProductStatus == (int)ProductStatus.shelve));
        }

        //取得本頁商品的SellerId
        public Seller GetSellerByProductId(int id)
        {
            var product = GetProductByProductId(id);
            if (product == null) { return null; }
            return _productRepository.FirstOrDefault<Seller>(x => x.Id == product.SellerId);
        }

        //取得本頁商品的子類別
        public SubCategory GetSubCategoryByProductId(int id)
        {
            var product = GetProductByProductId(id);
            if (product == null) { return null; }
            var prodSubCategoryId = product.SubCategory;
            return _productRepository.FirstOrDefault<SubCategory>(x => x.Id == prodSubCategoryId);
        }

        //渲染本頁商品
        public ProductViewModel GetProduct(int id)
        {
            var product = GetProductByProductId(id);

            var productImgList = _productRepository.ListWhere<ProductImg>(x => x.ProductId == id).Select(x => x.Img).ToList();

            //找到符合productId的商品標籤
            var tagIds = _productRepository.ListWhere<ProductTagList>(x => x.ProductId == id)
                .Select(x => x.ProductTagId).ToList();

            var tagNames = _productRepository.ListWhere<ProductTag>(x => tagIds.Contains(x.Id))
                .Select(x => x.TagName).ToList();

            var sellerId = GetSellerByProductId(id).Id;
            var sellerName = GetSellerByProductId(id).ShopName;
            var brandName = GetSellerByProductId(id).BrandName;

            var favPeople = _productRepository.ListWhere<FavProduct>(x => x.ProductId == id).Count();
            var favShop = _productRepository.ListWhere<FavShop>(x => x.SellerId == product.SellerId).Count();

            var subcategory = GetSubCategoryByProductId(id);
            var subcategoryId = GetSubCategoryByProductId(id).Id;
            var subcategoryName = GetSubCategoryByProductId(id).SubCategoryName;
            var categoryId = GetSubCategoryByProductId(id).CategoryId;
            var categoryName = _productRepository.FirstOrDefault<Category>(x => x.Id == subcategory.CategoryId).CategoryName;

            //評價
            var reviewAvgandSum = GetShopReviewAvgandSum(id) ?? null;

            //銷售數量(店家)
            var orderIds = _productRepository.ListWhere<OrderDetail>(x => x.SellerName == sellerName).Select(x => x.Id).ToList();
            var salesQtyByShop = _productRepository.ListWhere<OrderDetail>(x => orderIds.Contains(x.OrderId)).Sum(x => x.Quantity);
            //銷售數量(商品)
            var specIds = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id).Select(x => x.Id).ToList();
            var salesQtyByProduct = _productRepository.ListWhere<OrderDetail>(x => specIds.Contains(x.ProductSpecificationId)
            && x.Order.SellerId == sellerId).Sum(x => x.Quantity);

            //規格/價格
            var specs = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id)
                .GroupBy(x => x.FirstSpecification).ToList();

            List<SpecData> SpecList = new List<SpecData>();

            var prodPrice = GetProductByProductId(id).Price;

            foreach (var first in specs)
            {
                foreach (var second in first)
                {
                    // 儲存每個規格的價格
                    decimal specPrice = 0;
                    if (second.UnitPrice == null)
                    {
                        specPrice = prodPrice;
                    }
                    else
                    {
                        specPrice = (decimal)second.UnitPrice;
                    }

                    SpecList.Add(new SpecData
                    {
                        SpecId = second.Id,
                        FirstSpecItem = second.FirstSepcItem,
                        SecondSpecItem = second.SecondSepcItem,
                        UnitPrice = specPrice,
                    });
                }
            }


            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                BrandName = brandName,
                Editor = product.Editor, //富文本
                ProductImg = productImgList,
                SpecList = SpecList,
                Price = prodPrice,
                ViewTimes = (int)product.ViewTimes,
                SubCategoryName = subcategoryName,
                CategoryName = categoryName,
                CategoryId = categoryId,
                SubCategoryId = subcategoryId,
                MadeCountry = product.MadeCountry,
                Description = product.Description,
                SalesQtyByShop = salesQtyByShop.ToString(),
                SalesQtyByProduct = salesQtyByProduct.ToString(),
                FavPeople = favPeople.ToString(),
                TagNames = tagNames,
                EvaluateAvg = reviewAvgandSum.EvaluateAvg,
                EvaluateSum = reviewAvgandSum.EvaluateSum,
                SellerId = sellerId
            };
        }

        //取得相似的商品(相同子類別的六個產品)
        public List<SimilarProductData> GetSimilarProducts(int id)
        {
            var product = GetProductByProductId(id);
            var seller = GetSellerByProductId(id);

            var similarProducts = _productRepository
                .ListWhere<Product>(x => x.SubCategory == product.SubCategory && x.Id != id
                && (x.ProductStatus == (int)ProductStatus.shelve))
                .Take(6)
                .Select(x => new SimilarProductData
                {
                    ProductName = x.ProductName,
                    ProductFirstImg = _productRepository.FirstOrDefault<ProductImg>(y => y.ProductId == x.Id)?.Img,
                    BrandName = _productRepository.FirstOrDefault<Seller>(y => y.Id == x.SellerId).BrandName,
                    Price = x.Price,
                    SellerId = x.SellerId,
                    ProductId = x.Id
                }).ToList();

            return similarProducts;
        }

        //取得本頁商品的Seller資訊
        public List<SellerData> GetSellerInfo(int id)
        {
            var product = GetProductByProductId(id);

            var sellerInfoList = _productRepository
                .ListWhere<Seller>(x => x.Id == product.SellerId)
                .Select(x => new SellerData
                {
                    ShopBannerRect = x.ShopBannerRect,
                    Icon = x.Icon,
                    Logo = x.Logo,
                    BrandName = x.BrandName,
                    ProductOrigin = x.ProductOrigin,
                    ShopName = x.ShopName,
                    TaxIdNumber = x.TaxIdNumber
                }).ToList();

            return sellerInfoList;
        }

        //取得推薦的設計館
        public List<RecommendShopData> RecommendShops(int id)
        {
            var product = GetProductByProductId(id);
            var seller = GetSellerByProductId(id);

            // 推薦其他店家
            var recommendShops = _productRepository
                .ListWhere<Seller>(x => x.Id != seller.Id)
                .Take(3)
                .Select(x => new RecommendShopData
                {
                    SellerId = x.Id,
                    Icon = x.Icon,
                    BrandName = x.BrandName,
                    ShopBannerSquare = x.ShopBannerSquare,
                    EvaluateAvg = Math.Ceiling(_productRepository
                        .ListWhere<Order>(o => o.SellerId == x.Id && o.Evaluate.HasValue)
                        .Average(r => r.Evaluate) ?? 0),
                    FavShop = _productRepository.ListWhere<FavShop>(f => f.SellerId == x.Id).Count(),
                }).ToList();

            return recommendShops;
        }

        //取得最近的五筆訂單評價
        public List<ReviewData> GetReviews(int id)
        {
            var product = GetProductByProductId(id);
            var orders = _productRepository
                .ListWhere<Order>(o => o.SellerId == product.SellerId && o.Evaluate.HasValue)
                .OrderByDescending(o => o.EvaluateDate).Take(5).ToList();

            var userIds = orders.Select(o => o.UserId).ToList();
            var users = _productRepository.ListWhere<User>(x => userIds.Contains(x.Id)).ToList();
            var orderIds = orders.Select(o => o.Id).ToList();
            var orderDetail = _productRepository.ListWhere<OrderDetail>(od => orderIds.Contains(od.OrderId)).ToList();

            var reviews = orders.Select(o => new ReviewData
            {
                Account = users.FirstOrDefault(x => x.Id == o.UserId)?.Account.MaskedAccount(),
                Evaluate = o.Evaluate.HasValue ? o.Evaluate.Value : 0,
                EvaluateText = o.EvaluateText,
                EvaluateDate = (DateTime)o.EvaluateDate,
                ProductName = orderDetail.FirstOrDefault(od => od.OrderId == o.Id)?.ProductName
            }).OrderByDescending(o => o.EvaluateDate).ToList();

            var result = CountDate(reviews);
            return result;
        }

        //取得商店的評價(平均和總數)
        public ProductViewModel GetShopReviewAvgandSum(int id)
        {
            var product = GetProductByProductId(id);
            var orders = _productRepository
                .ListWhere<Order>(o => o.SellerId == product.SellerId && o.Evaluate.HasValue)
                .ToList();

            var evaluateAvg = Math.Ceiling(orders.Average(r => r.Evaluate) ?? 0);
            var evaluateSum = orders.Count;
        
            return new ProductViewModel
            {
                EvaluateAvg = Convert.ToInt32(evaluateAvg),
                EvaluateSum = evaluateSum
            };
        }

        //計算評論的日期是多久以前
        private List<ReviewData> CountDate(List<ReviewData> list)
        {
            foreach (var review in list)
            {
                var diff = DateTime.Today - review.EvaluateDate;
                if (diff.TotalDays >= 365)
                {
                    review.EvaluateDateCalculate = $"{(int)(diff.TotalDays / 365)} 年前";
                }
                else if (diff.TotalDays >= 30)
                {
                    review.EvaluateDateCalculate = $"{(int)(diff.TotalDays / 30)} 個月前";
                }
                else
                {
                    review.EvaluateDateCalculate = $"{(int)diff.TotalDays} 天前";
                }
            }
            return list;
        }

        //Management - 判斷 UserId 屬於哪個 SellerId
        public int GetSellerIdByUserId(int userId)
        {
            var user = _accountService.GetUser();
            return _productRepository.FirstOrDefault<Seller>(s => s.UserId == user.Id).Id;
        }
        public bool IsProduct(int id)
        {
            var user = _accountService.GetUser();
            var seller = _productRepository.FirstOrDefault<Seller>(x => x.UserId == user.Id);
            var product = _productRepository.FirstOrDefault<Product>(x => x.Id == id).SellerId;
            return seller.Id == product;
        }

        //Management - 取得該賣家的所有商品
        public IEnumerable<ProductManageData> GetProductsBySellerId(int userId)
        {
            var sellerId = GetSellerIdByUserId(userId);

            var productCards = _productRepository
                .ListWhere<Product>(p => p.SellerId == sellerId)
                .Select(p => new ProductManageData
                {
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ProductFirstImg = _productRepository.FirstOrDefault<ProductImg>(x => x.ProductId == p.Id)?.Img,
                    ProductStatus = p.ProductStatus,
                    ProductId = p.Id
                }).ToList();

            return productCards;
        }

        //Management - 下架商品
        public void OffShelveProduct(ProductManageIdListDto productIds)
        {
            foreach (var productDto in productIds.productManageDtos)
            {
                var product = _productRepository.FirstOrDefault<Product>(p => p.Id == productDto.ProductId);
                product.ProductStatus = (int)ProductStatus.offShelve;

                _productRepository.Update(product);
            }
        }

        //Management - 上架商品
        public void ShelveProduct(ProductManageIdListDto productIds)
        {
            foreach (var productDto in productIds.productManageDtos)
            {
                var product = _productRepository.FirstOrDefault<Product>(p => p.Id == productDto.ProductId);
                product.ProductStatus = (int)ProductStatus.shelve;
                _productRepository.Update(product);
            }
        }

        //Management - 刪除商品(改狀態)
        public void DeleteProductStatus(ProductManageIdListDto productIds)
        {

            foreach (var productDto in productIds.productManageDtos)
            {
                var product = _productRepository.FirstOrDefault<Product>(p => p.Id == productDto.ProductId);
                product.ProductStatus = (int)ProductStatus.delete;
                _productRepository.Update(product);
            }
        }

        public int GetSellerIdWithoutParameter()
        {
            var userId = _accountService.GetUser().Id;
            return _productRepository.FirstOrDefault<Seller>(x => x.UserId == userId).Id;
        }

        //建立商品
        public int CreateNewProduct(ProductViewModel newProduct)
        {
            var subCategory = _productRepository.FirstOrDefault<SubCategory>(x => x.SubCategoryName == newProduct.SubCategoryName.Trim());
            var sellerId = GetSellerId();
            var target = new Product
            {
                ProductName = newProduct.ProductName,
                Price = newProduct.Price,
                SubCategory = subCategory.Id,
                Description = newProduct.Description,
                MadeCountry = newProduct.MadeCountry,
                Editor = newProduct.Editor,
                ProductStatus = 1,
                SellerId = sellerId,
                ViewTimes = 0,
                CreateTime = DateTime.UtcNow,

            };
            _productRepository.Create(target);
            var productId = target.Id;
            return productId;
        }

        //建立規格
        //要再改價格 如果 價格不輸入的話
        public void AddSpecificationDto(SpecificationDto newSpecification, int productId)
        {
            if (newSpecification.SecondSepcItem == null)
            {
                for (int i = 0; i < newSpecification.FirstSepcItem.Length; i++)
                {
                    var target = new ProductSpecification
                    {
                        FirstSpecification = newSpecification.FirstSpecification,
                        SecondSpecification = newSpecification.SecondSpecification,
                        FirstSepcItem = newSpecification.FirstSepcItem[i],
                        //SecondSepcItem = newSpecification.SecondSepcItem[j],
                        UnitPrice = newSpecification.UnitPrice[i],
                        ProductId = productId,
                    };
                    _productRepository.Create(target);
                }
            }
            else
            {
                for (int i = 0; i < newSpecification.FirstSepcItem.Length; i++)
                {
                    for (int j = 0; j < newSpecification.SecondSepcItem.Length; j++)
                    {
                        var target = new ProductSpecification
                        {
                            FirstSpecification = newSpecification.FirstSpecification,
                            SecondSpecification = newSpecification.SecondSpecification,
                            FirstSepcItem = newSpecification.FirstSepcItem[i],
                            SecondSepcItem = newSpecification.SecondSepcItem[j],
                            UnitPrice = newSpecification.UnitPrice[i],
                            ProductId = productId,
                        };
                        _productRepository.Create(target);
                    }
                }
            }

        }

        //建立產品標籤
        public void AddProductTag(ProductSubCategoryDto newTag, int productId)
        {
            var ProductId = _productRepository.ListWhere<Product>(x => x.Id == productId).Select(z => z.Id).FirstOrDefault();

            for (int i = 0; i < newTag.TagName.Length; i++)
            {
                var tag = newTag.TagName[i];
                var ProductTagId = _productRepository.ListWhere<ProductTag>(x => x.TagName == tag).Select(y => y.Id).ToList();
                var target = new ProductTagList
                {
                    ProductId = productId,
                    ProductTagId = int.Parse(string.Join("", ProductTagId)),
                };
                _productRepository.Create(target);
            }
        }

        //建立圖片
        public async Task AddProductImage(ImageDto newImg, int productId)
        {
            var imgList = await _imageService.GetImageUrlList(newImg.Img, 900, 900);

            foreach (var i in imgList)
            {
                var target = new ProductImg
                {
                    ProductId = productId,
                    Img = i,
                    Sort = 1
                };
                _productRepository.Create(target);
            }
        }
        public string CheckName(string productName)
        {
            var user = _accountService.GetUser();
            //抓到使用者是賣家
            var sellerId = _productRepository.FirstOrDefault<Seller>(x => x.UserId == user.Id);
            var a = _productRepository.ListWhere<Product>(x => x.SellerId == sellerId.Id).Select(y => y.ProductName).Contains(productName.Trim());
            if (a)
            {
                return "已經有相同商品名稱";
            }
            return "";
        }

        //編輯圖片 待修改
        public List<string> editt(int id)
        {
            var imgList = _productRepository.ListWhere<ProductImg>(x => x.ProductId == id).Select(y => y.Img).ToList();
            return imgList;
        }
        //渲染編輯
        public CreateViewModel EditProduct(int id)
        {
            var sellerId = GetSellerId();
            var imgList = _productRepository.ListWhere<ProductImg>(x => x.ProductId == id).Select(y => y.Img).ToList();
            var name = _productRepository.FirstOrDefault<Product>(x => x.Id == id).ProductName;
            var category = _productRepository.ListWhere<Category>(x => true).Select(y => y.CategoryName).ToList();
            var subCategory = _productRepository.ListWhere<SubCategory>(x => true).Select(y => y.SubCategoryName).ToList();
            var subcategoryStatus = _productRepository.ListWhere<Product>(x => x.SellerId == sellerId).Select(y =>
            {
                var a = _productRepository.ListWhere<Product>(z => z.Id == id).Select(o => o.SubCategory);
                var b = _productRepository.ListWhere<SubCategory>(s => s.Id == int.Parse(string.Join("", a))).Select(item => item.SubCategoryName);
                return string.Join("", b);
            }).Take(1);
            var categoryStatus = _productRepository.ListWhere<Product>(x => x.SellerId == sellerId).Select(y =>
            {
                var a = _productRepository.ListWhere<Product>(z => z.Id == id).Select(o => o.SubCategory);
                var b = _productRepository.ListWhere<SubCategory>(s => s.Id == int.Parse(string.Join("", a))).Select(q => q.CategoryId);
                var c = _productRepository.ListWhere<Category>(d => d.Id == int.Parse(string.Join("", b))).Select(item => item.CategoryName);
                return string.Join("", c);
            }).Take(1);


            var price = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Price;

            var ff = _productRepository.FirstOrDefault<ProductSpecification>(x => x.ProductId == id).FirstSpecification;

            var fItem = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id).Select(y => y.FirstSepcItem).Distinct().ToArray();

            var ss = _productRepository.FirstOrDefault<ProductSpecification>(x => x.ProductId == id).SecondSpecification;

            var sItem = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id).Select(y => y.SecondSepcItem).Distinct().ToArray();

            var unitPrice = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id).Select(y => y.UnitPrice).ToArray();

            var made = _productRepository.FirstOrDefault<Product>(x => x.Id == id).MadeCountry;
            var tagList = _productRepository.ListWhere<ProductTagList>(x => x.ProductId == id).Select(y =>
            {
                var a = _productRepository.ListWhere<ProductTag>(z => z.Id == y.ProductTagId);
                var b = string.Join("", a.Select(zz => zz.TagName));
                return string.Join("", b);
            }).ToList();
            var tagName = _productRepository.ListWhere<ProductTag>(x => true).Select(z => z.TagName).ToList();
            var descr = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Description;
            var edit = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Editor;

            var subAndCatId = _productRepository.ListWhere<SubCategory>(x => true).Select(y =>
            {
                var a = _productRepository.ListWhere<Category>(z => z.Id == y.CategoryId).Select(item => item.Id);
                return int.Parse(string.Join("", a));
            }).ToList();
            List<string> Specification = new List<string>();

            foreach (var fitem in fItem)
            {
                foreach (var sitem in sItem)
                {
                    Specification.Add($"{fitem} + {sitem}");
                }
            }
            var target = new CreateViewModel
            {
                Img = imgList,
                ProductName = name,
                Price = price,
                FirstSpecification = ff,
                SecondSpecification = ss,
                FirstSepcItem = fItem,
                SecondSepcItem = sItem,
                MadeCountry = made,
                Tag = tagName,
                Description = descr,
                Editor = edit,
                CategoryName = category,
                SubCategoryName = subCategory,
                TagStatus = tagList,
                SubcategoryStatus = string.Join("", subcategoryStatus),
                CategoryStatus = string.Join("", categoryStatus),
                SubCategoryId = subAndCatId,
                UnitPrice = unitPrice,
                SpecificationMix = Specification.ToArray()
            };
            return target;

        }

        //編輯
        public void NewEditProduct(EditDto editModel)
        {
            var subCategory = _productRepository.FirstOrDefault<SubCategory>(x => x.SubCategoryName == editModel.SubCategoryName.Trim());
            var product = _productRepository.FirstOrDefault<Product>(x => x.Id == editModel.Id);

            //product表
            product.ProductName = editModel.ProductName;
            product.Price = editModel.Price;
            product.Description = editModel.Description;
            product.Editor = editModel.Editor;
            product.MadeCountry = editModel.MadeCountry;
            product.SubCategory = subCategory.Id;
            _productRepository.Update(product);


            //規格表
            //取出這個商品的所有規格
            var specification = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == editModel.Id);

            var specificationPriceList = editModel.UnitPrice;
            for (int i = 0; i < specificationPriceList.Length; i++)
            {
                specification[i].UnitPrice = specificationPriceList[i];
                _productRepository.Update(specification[i]);
            };

            //標籤
            var tagDb = _productRepository.ListWhere<ProductTagList>(x => x.ProductId == editModel.Id);
            foreach (var t in tagDb)
            {
                _productRepository.Delete(t);
            }

            for (int i = 0; i < editModel.TagName.Length; i++)
            {
                var tag = editModel.TagName[i];
                var productTagNameId = _productRepository.ListWhere<ProductTag>(x => x.TagName == tag).Select(y => y.Id);
                var target = new ProductTagList
                {
                    ProductId = editModel.Id,
                    ProductTagId = int.Parse(string.Join("", productTagNameId))
                };
                _productRepository.Create(target);
            }
            //圖片
            var imgDb = _productRepository.ListWhere<ProductImg>(x => x.ProductId == editModel.Id);

            foreach (var i in imgDb)
            {
                _productRepository.Delete(i);
            }
        }

        //編輯圖片
        public async Task EditProductImage(EditDto editModel)
        {
            var check = new List<string>();

            foreach (var img in editModel.Img)
            {
                var url = "";
                if (!img.Contains("https://"))
                {
                    url = await _imageService.GetImageUrl(img);
                }
                else
                {
                    url = img;
                }
                check.Add(url);
            }
            foreach (var i in check)
            {
                var target = new ProductImg
                {
                    ProductId = editModel.Id,
                    Img = i,
                    Sort = 1
                };
                _productRepository.Create(target);
            }

        }


        //渲染預覽
        public ProductViewModel GetNewProduct(int id)
        {
            //var productId = GetProductId(id);
            var sellerId = GetSellerId();

            var productId = _productRepository.FirstOrDefault<Product>(x => x.SellerId == sellerId).Id;

            var name = _productRepository.FirstOrDefault<Product>(x => x.Id == id).ProductName;
            var edit = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Editor;
            var descr = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Description;
            var madeCountry = _productRepository.FirstOrDefault<Product>(x => x.Id == id).MadeCountry;

            var productImgList = _productRepository.ListWhere<ProductImg>(x => x.ProductId == id).Select(x => x.Img).ToList();


            ProductViewModel evaluateAvg = new ProductViewModel();
            var reviewAvgandSum = evaluateAvg.EvaluateAvg;

            //評價
            //完全沒訂單
            var nullAvgand = _productRepository.FirstOrDefault<Order>(x => x.SellerId == sellerId) == null;
            if (nullAvgand)
            {
                reviewAvgandSum = 0;
            }
            //有訂單沒評價
            //有訂單有評價
            bool avgand;
            if (!nullAvgand)
            {
                avgand = _productRepository.FirstOrDefault<Order>(x => x.SellerId == sellerId).Evaluate == null;
                if (avgand)
                {
                    reviewAvgandSum = GetShopAvgand(id).EvaluateAvg;
                }
                else
                {
                    reviewAvgandSum = GetShopAvgandTwo(id).EvaluateAvg;
                }
            }


            //找到符合productId的商品標籤
            var tagIds = _productRepository.ListWhere<ProductTagList>(x => x.ProductId == id)
                .Select(x => x.ProductTagId).ToList();

            var tagNames = _productRepository.ListWhere<ProductTag>(x => tagIds.Contains(x.Id))
                .Select(x => x.TagName).ToList();

            var brandName = _productRepository.FirstOrDefault<Seller>(x => x.Id == sellerId).BrandName;

            var favPeople = _productRepository.ListWhere<FavProduct>(x => x.ProductId == id).Count();
            var favShop = _productRepository.ListWhere<FavShop>(x => x.SellerId == sellerId).Count();


            var subcategoryName = _productRepository.ListWhere<Product>(x => x.SellerId == sellerId).Select(y =>
            {
                var a = _productRepository.ListWhere<Product>(z => z.Id == id).Select(o => o.SubCategory);
                var b = _productRepository.ListWhere<SubCategory>(s => s.Id == int.Parse(string.Join("", a))).Select(item => item.SubCategoryName);
                return string.Join("", b);
            }).Take(1);

            var categoryName = _productRepository.ListWhere<Product>(x => x.SellerId == sellerId).Select(y =>
            {
                var a = _productRepository.ListWhere<Product>(z => z.Id == id).Select(o => o.SubCategory);
                var b = _productRepository.ListWhere<SubCategory>(s => s.Id == int.Parse(string.Join("", a))).Select(q => q.CategoryId);
                var c = _productRepository.ListWhere<Category>(d => d.Id == int.Parse(string.Join("", b))).Select(item => item.CategoryName);
                return string.Join("", c);
            }).Take(1);



            //規格&價格
            var specs = _productRepository.ListWhere<ProductSpecification>(x => x.ProductId == id)
                .GroupBy(x => x.FirstSpecification).ToList();

            List<SpecData> SpecList = new List<SpecData>();

            var prodPrice = _productRepository.FirstOrDefault<Product>(x => x.Id == id).Price;

            foreach (var first in specs)
            {
                foreach (var second in first)
                {
                    // 儲存每個規格的價格
                    decimal specPrice = 0;
                    if (second.UnitPrice == null)
                    {
                        specPrice = prodPrice;
                    }
                    else
                    {
                        specPrice = (decimal)second.UnitPrice;
                    }

                    SpecList.Add(new SpecData
                    {
                        FirstSpecItem = second.FirstSepcItem,
                        SecondSpecItem = second.SecondSepcItem,
                        UnitPrice = specPrice,
                    });
                }
            }


            return new ProductViewModel
            {
                Id = productId,
                ProductName = name,
                BrandName = brandName,
                Editor = edit,
                ProductImg = productImgList,
                SpecList = SpecList,
                Price = prodPrice,
                SubCategoryName = string.Join("", subcategoryName),
                CategoryName = string.Join("", categoryName),
                MadeCountry = madeCountry,
                Description = descr,
                FavPeople = favPeople.ToString(),
                TagNames = tagNames,
                EvaluateAvg = reviewAvgandSum,
            };

        }
        //渲染預覽的seller
        public List<SellerData> GetSeller(int id)
        {
            var productId = GetProductId(id);
            var sellerId = GetSellerId();
            var sellerInfoList = _productRepository
                .ListWhere<Seller>(x => x.Id == sellerId)
                .Select(x => new SellerData
                {
                    ShopBannerRect = x.ShopBannerRect,
                    Icon = x.Icon,
                    Logo = x.Logo,
                    BrandName = x.BrandName,
                    ProductOrigin = x.ProductOrigin,
                    ShopName = x.ShopName,
                    TaxIdNumber = x.TaxIdNumber
                }).ToList();

            return sellerInfoList;
        }
        //渲染預覽的評價
        public ProductViewModel GetShopAvgand(int id)
        {

            var sellerId = GetSellerId();
            var orders = _productRepository
                .ListWhere<Order>(o => o.SellerId == sellerId && o.Evaluate == null)
                .ToList();

            var evaluateAvg = Math.Ceiling(orders.Average(r => r.Evaluate) ?? 0);
            var evaluateSum = orders.Count;

            return new ProductViewModel
            {
                EvaluateAvg = Convert.ToInt32(evaluateAvg),
                EvaluateSum = evaluateSum
            };
        }
        //渲染預覽的評價2
        public ProductViewModel GetShopAvgandTwo(int id)
        {

            var sellerId = GetSellerId();
            var orders = _productRepository
                .ListWhere<Order>(o => o.SellerId == sellerId && o.Evaluate.HasValue)
                .ToList();

            var evaluateAvg = Math.Ceiling(orders.Average(r => r.Evaluate) ?? 0);
            var evaluateSum = orders.Count;

            return new ProductViewModel
            {
                EvaluateAvg = Convert.ToInt32(evaluateAvg),
                EvaluateSum = evaluateSum
            };
        }
        public int GetProductId(int id)
        {
            return _productRepository.FirstOrDefault<Product>(x => x.Id == id).Id;
        }
        //預覽更新上架
        public void UpProduct(int id)
        {
            var sellerId = GetSellerId();
            var status = _productRepository.FirstOrDefault<Product>(x => x.Id == id);
            status.ProductStatus = 2;
            _productRepository.Update(status);
        }

        //渲染前端 select 主副類別
        public List<ProductSubCategoryDto> GetProductCategory()
        {
            var tag = _productRepository.ListWhere<ProductTag>(x => true).Select(y => y.TagName);
            var categoryList = _productRepository.ListWhere<SubCategory>(y => true).Select(x => new ProductSubCategoryDto
            {
                CategoryName = _productRepository.FirstOrDefault<Category>(y => y.Id == x.CategoryId).CategoryName,
                CategoryId = x.Id,
                SubCategoryName = x.SubCategoryName,
                SubCategoryId = x.Id,
                TagName = tag.ToArray(),

            });
            return categoryList.ToList();
        }
        public int GetSellerId()
        {
            var userId = _accountService.GetUser().Id;
            var seller = _productRepository.FirstOrDefault<Seller>(x => x.UserId == userId);
            return seller.Id;
        }

    }





}