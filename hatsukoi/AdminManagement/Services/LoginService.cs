using AdminManagement.Models.Dtos;
using Hatsukoi.Common;
using Hatsukoi.Models.ViewModel;
using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;

namespace AdminManagement.Services
{
    public class LoginService
    {
        private readonly IRepository _repository;

        public LoginService(IRepository repository)
        {
            _repository = repository;
        }
        public UserDto GetUser(LoginDto loginDto)
        {
            var admin = _repository
                .FirstOrDefault<Admin>(x => x.Account == loginDto.UserName && x.Password == loginDto.Password);
            
            if (admin != null)
            {
                return new UserDto 
                { 
                    UserName = admin.Account, 
                    Password = admin.Password
                };
            }
            else
            {
                return new UserDto
                {
                    Id = 0,
                    UserName = ""
                };
            };
        }

    }
}
