

namespace Hatsukoi.Helpers
{
    public static class FilterTagHelpers
    {
        public static string GetPriceFilterTagText(string priceOrder)
        {
            switch (priceOrder)
            {
                case "price300":
                    return "NT$300 以下";
                case "price300_500":
                    return "NT$300 - 500";
                case "price500_1000":
                    return "NT$500 - 1,000";
                case "price1000_2000":
                    return "NT$1,000 - 2,000";
                case "price2000_2500":
                    return "NT$2,000 - 2,500";
                case "price2500_5000":
                    return "NT$2,500 - 5,000";
                case "price5000":
                    return "NT$5,000 以上";
                default:
                    return "";
            }
        }

        public static string GetDateFilterTagText(string dateOrder)
        {
            switch (dateOrder)
            {
                case "one_week":
                    return "一週內";
                case "one_month":
                    return "一個月內";
                case "three_months":
                    return "三個月內";
                case "one_year":
                    return "一年內";
                default:
                    return "";
            }
        }

        public static string GetTagFilterTagText(int tagOrder)
        {
            switch (tagOrder)
            {
                case 2:
                    return "夏日補水";
                case 4:
                    return "送禮推薦";
                case 6:
                    return "節慶";
                case 7:
                    return "少女系";
                case 8:
                    return "男子系";
                case 9:
                    return "日韓系";
                case 10:
                    return "歐美系";
                case 11:
                    return "愛台灣系";
                case 12:
                    return "萌系";
                case 13:
                    return "實用系";
                default:
                    return "";
            }
        }

    }
}
