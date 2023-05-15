using AdminManagement.Models.ViewModels;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;

namespace AdminManagement.Services
{
    public class LineService
    {
        private readonly IRepository _repository;

        public LineService(IRepository repository)
        {
            _repository = repository;
        }

        public List<LineViewModel> GetLineMessage()
        {
            var messageData = _repository.ListWhere<LineMessage>(x => true).OrderByDescending(c => c.SendTime)
                .Select(m => new LineViewModel
                {
                    Id = m.Id,
                    SendTime = m.SendTime.AddHours(8).ToString("G"),
                    Text = m.Text,
                    IsSend = (bool)m.IsSend
                }).ToList();

            return messageData;
        }

       
        public bool Create(LineCreateViewModel createData)
        {
            bool result = true;
            try
            {
                var lineEntity = new LineMessage
                {
                    SendTime = string.IsNullOrEmpty(createData.SendTime) ? DateTime.UtcNow : DateTime.Parse(createData.SendTime + " " + createData.TimePick).AddHours(-8),
                    Text = createData.Text,
                    IsSend = string.IsNullOrEmpty(createData.SendTime) ? true : false,
                };
                _repository.Create(lineEntity);
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
                var entity = _repository.FirstOrDefault<LineMessage>(m => m.Id == id);
                _repository.Delete<LineMessage>(entity);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool Update(LineEditViewModel data)
        {
            bool result = true;
            try
            {
                var entity = _repository.FirstOrDefault<LineMessage>(m => m.Id == data.Id);
                entity.Text = data.Text;
                _repository.Update<LineMessage>(entity);
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
