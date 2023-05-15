using Hatsukoi.Models;
using Hatsukoi.Models.Dtos.Create;
using Hatsukoi.Models.Dtos.Product;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Models.ViewModels.Create;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.APIControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public APIBaseResponse GetProducts()
        {
            try
            {
                var sellerId = _productService.GetSellerIdWithoutParameter();
                var productManageList = _productService.GetProductsBySellerId(sellerId);
                var response = new APIBaseResponse(productManageList);
                return response;
            }
            catch (Exception ex)
            {
                return new APIBaseResponse
                {
                    Status = APIStatus.Fail,
                    ErrorMsg = "資料傳送失敗",
                    Result = ex.Message
                };
            }
        }

        //瀏覽數
        [HttpPost]
        public IActionResult RecordViewCount([FromBody]ViewCountDto viewCountDto) 
        {
            _productService.RecordViewData(viewCountDto);
            return Ok();
        }

        //刪除
        [HttpPost]
        public IActionResult DeleteProduct([FromBody] ProductManageIdListDto productIds)
        {
            _productService.DeleteProductStatus(productIds);
            return Ok(new APIBaseResponse() { Status = APIStatus.Success });
        }

        //重新上架
        [HttpPost]
        public IActionResult ShelveProduct([FromBody] ProductManageIdListDto productIds)
        {
            _productService.ShelveProduct(productIds);
            return Ok(new APIBaseResponse() { Status = APIStatus.Success });
        }

        //下架
        [HttpPost]
        public IActionResult OffShelveProduct([FromBody] ProductManageIdListDto productIds)
        {
            _productService.OffShelveProduct(productIds);
            return Ok(new APIBaseResponse() { Status = APIStatus.Success });
        }


        [HttpPost]
        public IActionResult ReadEdit(PreviewDto dto)
        {

            var a = _productService.EditProduct(dto.Id);
            return Ok(new APIBaseResponse(a));
        }
        [HttpPost]
        public IActionResult UpEdit(EditDto dto)
        {
            _productService.NewEditProduct(dto);
            _productService.EditProductImage(dto);
            return Ok(new APIBaseResponse());
        }

        [HttpPost]
        public IActionResult ReadPreview(PreviewDto dto)
        {
            _productService.UpProduct(dto.Id);
            return Ok(new APIBaseResponse());
        }

    }
}
