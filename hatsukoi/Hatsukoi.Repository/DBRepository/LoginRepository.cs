using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository
{
    public class LoginRepository : EFRepository, ILoginRepository
    {
        public LoginRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }
    }
}
