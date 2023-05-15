namespace AdminManagement.Models.Dtos
{
    public class IncreasedUserDto
    {
        public List<MemberDto>? MemberData { get; set; }
        public List<SellerDto>? SellerData { get; set; }

        public class MemberDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public int MonthMemberCount { get; set; } = 0;
        }
        public class SellerDto
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public int MonthSellerCount { get; set; } = 0;
        }

    }
}
