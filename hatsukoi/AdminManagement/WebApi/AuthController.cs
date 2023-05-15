using AdminManagement.BaseModels;
using AdminManagement.Helpers;
using AdminManagement.Models.Dtos;
using AdminManagement.Services;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace AdminManagement
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;
        private readonly LoginService _loginService;

        public AuthController(JwtHelper jwtHelper, LoginService loginService)
        {
            _jwtHelper = jwtHelper;
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto input)
        {
         
            var authResult = AuthenticateUser(input);

            if (authResult.UserName != "")
            {
                var token = _jwtHelper.GenerateToken(authResult.UserName);
                return Ok(new BaseApiResponse(token));
            }
            else
            {
                var result = new BaseApiResponse();
                result.IsSuccess = false;
                return Ok(result);
            }

        }

        private LoginDto AuthenticateUser(LoginDto loginDto)
        {
            var user = _loginService.GetUser(loginDto);

            if (user != null)
            {
                return new LoginDto { UserName = user.UserName, Password = user.Password };
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Check()
        {
            return Ok(new BaseApiResponse(User.Identity.Name));
        }

    }
}


