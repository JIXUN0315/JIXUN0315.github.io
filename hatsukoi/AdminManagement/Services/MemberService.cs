using AdminManagement.Models.ViewModels;
using CloudinaryDotNet;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using isRock.LineBot;

namespace AdminManagement.Services
{
    public class MemberService
    {
        private readonly IRepository _repository;
        public MemberService(IRepository repository)
        {
            _repository = repository;

        }

        public List<MemberViewModel> GetAllUser()
        {
 
          var  allUser = _repository.ListWhere<User>(x => true).Select(y => new MemberViewModel
            {
                Id = y.Id,
                Name = y.Name,
                Email = y.Email,
                Account = y.Account == null ? "第三方登陸" : y.Account,
                Gender = y.Gender == null ? "性別未設定" : y.Gender == true ? "男" : "女",
                Price = _repository.ListWhere<Order>(x =>x.UserId == y.Id).Sum(z =>z.TotalPrice),

            }).OrderByDescending(x =>x.Price).ToList();

            return allUser;

        }
    }
}
