using Coravel.Invocable;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Scheduler
{
    public class CancelUnpaidOrders : IInvocable
    {
        private readonly IRepository _repository;

        public CancelUnpaidOrders(IRepository repository)
        {
            _repository = repository;
        }
        public  Task Invoke()
        {
            CancelOrders();
            return Task.CompletedTask;
        }
        public void CancelOrders()
        {
            var unpaidOrders = _repository.ListWhere<Order>
                (o => o.Status == (int)OrderStatus.NotPay && o.CreateTime.AddDays(1) < DateTime.UtcNow);


            foreach (var order in unpaidOrders)
            {
                order.Status = (int)OrderStatus.Cancelled;
                 _repository.Update(order);
            }
        }
    }
}
