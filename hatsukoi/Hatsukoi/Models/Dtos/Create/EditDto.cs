namespace Hatsukoi.Models.Dtos.Create
{
    public class EditDto
    {
#nullable disable
        public int Id { get; set; }
        public string FirstSpecification { get; set; }
        public string SecondSpecification { get; set; }

        public string[] FirstSepcItem { get; set; }
        public string[] SecondSepcItem { get; set; }
        public decimal[] UnitPrice { get; set; }

        public List<string> Img { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public string MadeCountry { get; set; }
        public string Description { get; set; }

        public string SubCategoryName { get; set; }

        public string Tag { get; set; }
        public string Editor { get; set; }

        public int SubCategoryId { get; set; }

        public string[] TagName { get; set; }
    }
}
