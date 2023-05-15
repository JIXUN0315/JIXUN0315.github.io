using Hatsukoi.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Service.Interface
{
    public interface IAccountService
    {
        public UserSimpleModel GetUser();
        public User GetUserById(int id);
    }
}
