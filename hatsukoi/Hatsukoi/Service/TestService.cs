using Hatsukoi.Repository;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Configuration;
using System.Data.Entity;

namespace Hatsukoi.Service
{
    public class TestService
    {
        private readonly AccountService _accountService;
        private readonly IRepository _dBRepository;
        public TestService(AccountService accountService, IRepository dBRepository)
        {
            _accountService= accountService;
            _dBRepository= dBRepository;
        }
        public void Delete()
        {
            var user = _accountService.GetUser();
            var target = _dBRepository.GetAll<User>().First(x => x.Id == user.Id);
            _dBRepository.Delete(target);
            _dBRepository.Save();
        }
        public void GetUserById(int id)
        {
            using (var context = new System.Data.Entity.DbContext("data source=.;initial catalog=Hatsukoi;integrated security=True; TrustServerCertificate=true;MultipleActiveResultSets=true;"))
            {
                var a = context.Database.SqlQuery<User>(
                    $@"
                SELECT *
                FROM [User]
                WHERE Id = ${id}
                ").ToList();
                //return a;
            }
        }
    }
}
