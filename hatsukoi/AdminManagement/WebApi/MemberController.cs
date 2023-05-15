using AdminManagement.BaseModels;
using AdminManagement.Services;
using Hatsukoi.Repository.Interface;
using Hatsukoi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminManagement.WebApi
{

    
    public class MemberController : BaseApiController
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService sellerService)
        {
            _memberService = sellerService;
        }

        [HttpGet]
        public IActionResult GetReviewer()
        {
            var VM = _memberService.GetAllUser();
            return Ok(new BaseApiResponse(VM));
        }
    }
}
