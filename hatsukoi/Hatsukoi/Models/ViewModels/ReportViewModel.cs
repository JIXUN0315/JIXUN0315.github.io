using static Hatsukoi.Models.ViewModels.ProductViewModel;

namespace Hatsukoi.Models.ViewModels
{

    public class ReportViewModel
    {
#nullable disable

        public decimal? NotShipPrice { get; set; }
        public decimal? ShipPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public List<int?> Count { get; set; }

        public List<string> Date { get; set; }
        public List<Decimal?> Price { get; set; }
        public List<string> NowMonthDate { get; set; }
        public List<string> LastYearDate { get; set; }
        public List<Decimal?> NowMonthPrice { get; set; }
        public List<Decimal?> LastYearPrice { get; set; }




    }
}
