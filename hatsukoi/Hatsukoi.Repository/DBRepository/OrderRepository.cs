using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository
{
    public class OrderRepository : EFRepository, IOrderRepository
    {
        public OrderRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }
        public List<Order> GetOrders(int sellerId)
        {
            return DbContext.Set<Order>().Include(o => o.User).Where(o => o.SellerId == sellerId).ToList();
        }
        public Order GetOrder(string orderNum)
        {
            var order = DbContext.Set<Order>()
               .Include(o => o.User)
               .Include(o => o.OrderDetails)
               .FirstOrDefault(o => o.OrderNumber == orderNum);
            return order;
        }
    }
}
