using Hatsukoi.Models.Dtos.Email;

namespace Hatsukoi.Service.Interface
{
    public interface IEmailService
    {
        public void Send(EmailDto emailDto);
    }
}
