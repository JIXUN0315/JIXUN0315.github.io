using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Hatsukoi.Repository.DBRepository
{
    public class UserRepository : EFRepository, IUserRepository
    {

        //public List<T> GetSQLQuery<T>(string queryString) where T : class
        //{
        //    var conn = _config.GetConnectionString("HatsukoiContext");
        //    List<T> couponList;
        //    using (var context = new System.Data.Entity.DbContext(conn))
        //    {
        //        couponList = context.Database.SqlQuery<T>(queryString).ToList();
        //        return couponList;
        //    }
        //}
        public UserRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }
        public void UpdateList(List<Chatroom> msgs)
        {
            foreach (var msg in msgs)
            {
                msg.IsRead = 1;
                DbContext.Entry(msg).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
        }
        public void UpdateNotiList(List<Notification> notis)
        {
            foreach (var msg in notis)
            {
                msg.IsRead = 1;
                DbContext.Entry(msg).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
        }
    }
}
