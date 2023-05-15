using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository.Interface
{
    public interface IUserRepository : IRepository
    {
        //public List<T> GetSQLQuery<T>(string queryString) where T : class;
        public void UpdateList(List<Chatroom> msgs);
        public void UpdateNotiList(List<Notification> notis);
    }
}
