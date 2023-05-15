using static Hatsukoi.Common.HatsukoiEnum;

namespace Hatsukoi.Models.ViewModels
{
    public class UserSimpleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public StoreStatus StoreStatus { get; set; }
        public string Image { get; set; }
        public MemberLevel MemberLevel { get; set; }
        public bool IsEmailCertified { get; set; }
    }
}
