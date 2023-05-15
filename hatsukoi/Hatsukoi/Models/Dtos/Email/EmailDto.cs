using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Models.Dtos.Email
{
    public class EmailDto
    {
        public string Email { get; set; }
        public string Account { get; set; }
        public string Name { get; set; } = "Hatsukoi 新會員";

        public EmailType EmailType { get; set; }

        public int Id { get; set; }
    }
}
