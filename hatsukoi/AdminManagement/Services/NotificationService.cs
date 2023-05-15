using AdminManagement.Models.ViewModels;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminManagement.Services
{
    public class NotificationService
    {
        private readonly IRepository _repository;

        public NotificationService(IRepository repository)
        {
            _repository = repository;
        }

        public List<CenterNotifyViewModel> GetAll()
        {
            var data = _repository.ListWhere<CenterNotify>(x=>true).OrderByDescending(c=> c.SendTime).Select(o=> new CenterNotifyViewModel
            {
                Id = o.Id,
                LinkTo = o.LinkTo,
                SendTime=o.SendTime.AddHours(8).ToString("G"),
                IsSend = (bool)o.IsSend ,
                Text =o.Text,
                Title =o.Title
            }).ToList();
            return data;
        }
        
        public bool Create(CenterNotifyCreateViewModel data)
        {
            bool result = true;
            try
            {
                var entity = new CenterNotify
                {
                    LinkTo = data.LinkTo,
                    SendTime = string.IsNullOrEmpty(data.SendTime)? DateTime.UtcNow : DateTime.Parse(data.SendTime + " " + data.TimePick).AddHours(-8),
                    Text = data.Text,
                    Title = data.Title,
                    SendImg = "https://res.cloudinary.com/dwummfedl/image/upload/v1679983604/hatsukoi_item2023/3/28%20%E4%B8%8A%E5%8D%88%2006:06:43.png",
                    IsSend = string.IsNullOrEmpty(data.SendTime) ? true: false,
                };
                _repository.Create(entity);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool Update(CenterNotifyEditViewModel data)
        {
            bool result = true;
            try
            {
                var entity =  _repository.FirstOrDefault<CenterNotify>(o => o.Id == data.Id);
                entity.Title = data.Title;
                entity.Text = data.Text;
                entity.LinkTo = data.LinkTo;
                _repository.Update<CenterNotify>(entity);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool Delete(int id)
        {
            bool result = true;
            try
            {
                var entity = _repository.FirstOrDefault<CenterNotify>(o => o.Id == id);
                _repository.Delete<CenterNotify>(entity);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
