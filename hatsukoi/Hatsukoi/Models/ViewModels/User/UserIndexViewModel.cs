namespace Hatsukoi.Models.ViewModels.User
{
    public class UserIndexViewModel
    {
        public string UserName { get; set; }
        public string UserImg { get; set; }
        public string UserGender { get; set; }
        public DateTime UserCreateTime { get; set; }
        public string UserLevel { get; set; }
        public string ThisYearMissionName { get; set; }
        public string NextYearMissionName { get; set; }
        public decimal ThisYearMissionPrice { get; set; }
        public decimal NextYearMissionPrice { get; set; }
    }
}
