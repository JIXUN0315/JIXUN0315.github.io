using Hatsukoi.Repository.EntityModel;
using static Hatsukoi.Common.HatsukoiEnum;

namespace AdminManagement.Models.Dtos
{
#nullable disable
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool IsEmailCertified { get; set; }
    }
}
