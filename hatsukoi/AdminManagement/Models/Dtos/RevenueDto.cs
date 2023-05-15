namespace AdminManagement.Models.Dtos
{
    public class RevenueDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal MonthTotalRevenue { get; set; }
        public decimal YearTotalRevenue { get; set; }
    }
}
