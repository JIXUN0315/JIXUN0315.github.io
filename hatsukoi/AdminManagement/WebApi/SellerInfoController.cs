using AdminManagement.BaseModels;
using AdminManagement.Models.Dtos;
using AdminManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{
    public class SellerInfoController : BaseApiController
    {
        private readonly SellerService _sellerService;

        public SellerInfoController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public IActionResult GetReviewer()
        {
            var VM = _sellerService.GetReviewerViewModel();
            return Ok(new BaseApiResponse(VM));
        }
        [HttpPost]
        public IActionResult RejectReview(RejectReviewDto dto)
        {
            _sellerService.RejectReviewer(dto);
            return Ok(new BaseApiResponse());
        }
        [HttpPost]
        public IActionResult ApplyReview(ReviewerDto dto)
        {
            _sellerService.ApplyReviewer(dto.ReviewerId);
            return Ok(new BaseApiResponse());
        }
        [HttpGet]
        public IActionResult GetSllSeller()
        {
            var VM = _sellerService.GetAllSeller();
            return Ok(new BaseApiResponse(VM));
        }
        [HttpPost]
        public IActionResult Suspension(RejectReviewDto dto)
        {
            _sellerService.SuspensionSeller(dto);
            return Ok(new BaseApiResponse());
        }
        [HttpPost]
        public IActionResult Restoration(ReviewerDto dto)
        {
            _sellerService.RestorationSuspension(dto.ReviewerId);
            return Ok(new BaseApiResponse());
        }
    }
}
