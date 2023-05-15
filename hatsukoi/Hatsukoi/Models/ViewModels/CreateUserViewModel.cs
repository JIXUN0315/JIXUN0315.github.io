namespace Hatsukoi.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool Gender { get; set; }
    }
}
