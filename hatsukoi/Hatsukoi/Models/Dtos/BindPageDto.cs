using Hatsukoi.Common;

namespace Hatsukoi.Models.Dtos
{
    public class BindPageDto
    {
        public string Email { get; set; }
        public HatsukoiEnum.ExternalLoginType LoginType { get; set; }
        public HatsukoiEnum.ExternalLoginType RegisterType { get; set; }
        public string Token { get; set; }
    }
}
