using AdminManagement.BaseModels;
using AdminManagement.Models.Dtos;
using AdminManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{
    public class BannerController : BaseApiController
    {
        private readonly BannerService _bannerService;

        public BannerController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public IActionResult GetBanner()
        {
            var VM = _bannerService.GetBannerImages();
            return Ok(new BaseApiResponse(VM));
        }
        [HttpPost]
        public async Task<IActionResult> CreateBanner(BannerDto dto)
        {
            await _bannerService.CreateBannerImage(dto.BannerImg,dto.SortNum);
            return Ok(new BaseApiResponse());
        }
        [HttpPost]
        public IActionResult DeleteBanner(BannerIdDto dto)
        {
            _bannerService.DeleteBannerImage(dto.Id);
            return Ok(new BaseApiResponse());
        }
        [HttpPost]
        public IActionResult UpdateBanner(UpdateBannerDto dto)
        {
            _bannerService.UpdateBannerImageSort(dto.imgList);
            return Ok(new BaseApiResponse());
        }

    }
}
