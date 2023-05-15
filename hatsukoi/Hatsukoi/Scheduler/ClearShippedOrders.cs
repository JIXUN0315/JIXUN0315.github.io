using Coravel.Invocable;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Scheduler
{
    public class ClearShippedOrders : IInvocable
    {
        private readonly IRepository _repository;

        public ClearShippedOrders(IRepository repository)
        {
            _repository = repository;
        }
        public  Task Invoke()
        {
            CompleteShippedOrders();
            return Task.CompletedTask;
        }
        public void CompleteShippedOrders()
        {
            var shippedOrders =_repository.ListWhere<Order>(o => o.Status == (int)OrderStatus.ToBeReceived && o.StatusSendTime != null && o.StatusSendTime.Value.AddMinutes(1) < DateTime.UtcNow);

            foreach (var order in shippedOrders)
            {
                order.Status = (int)OrderStatus.Completed;
                 _repository.Update(order);
            }
        }
    }
}
