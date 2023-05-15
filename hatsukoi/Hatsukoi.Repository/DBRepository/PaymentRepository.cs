using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hatsukoi.Repository.DBRepository
{
    public class PaymentRepository : EFRepository
    {
        public PaymentRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public int CreateOrderAndOrderDetail(Order order, List<OrderDetail> orderDetailProduct , List<Cart> cartProduct)
        {
            using(var tran = DbContext.Database.BeginTransaction())
            {
                try
                {
                    DbContext.Entry(order).State = EntityState.Added;
                    DbContext.SaveChanges();
                    var orderId = order.Id;
                    foreach (var item in orderDetailProduct)
                    {
                        item.OrderId = orderId;
                        DbContext.Entry(item).State = EntityState.Added;
                    }
                    if (order.CouponId > 0)
                    {
                        //已被使用 Used = 3
                        var coupon = DbContext.Set<CouponList>().FirstOrDefault(cl => cl.CouponId == order.CouponId && cl.UserId == order.UserId);
                        coupon.Status = 3;
                        DbContext.Update(coupon);
                    }
                    foreach (var item in cartProduct)
                    {
                        DbContext.Remove(item);
                        
                    }

                    DbContext.SaveChanges();
                    tran.Commit();

                    return orderId;

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return 0 ;
                }
            }
        }


    }

}
