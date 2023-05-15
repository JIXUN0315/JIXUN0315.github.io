using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository.Interface
{
    public interface IOrderRepository : IRepository
    {
        public List<Order> GetOrders(int sellerId);
        public Order GetOrder(string orderId);
    }
}
