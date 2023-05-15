namespace Hatsukoi.Models.ViewModels.Create
{
    public class CreateViewModel
    {
#nullable disable
        public int Id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public List<string> CategoryName { get; set; }

        public List<string> SubCategoryName { get; set; }

        public string MadeCountry { get; set; }

        public string FirstSpecification { get; set; }
        public string SecondSpecification { get; set; }
        //public List<string> FirstSepcItem { get; set; }
        public string[] FirstSepcItem { get; set; }
        //public List<string> SecondSepcItem { get; set; }
        public string[] SecondSepcItem { get; set; }
        //public List<string> SpecificationMix { get; set; }
        public string[] SpecificationMix { get; set; }

        //public List<decimal?> UnitPrice { get; set; }
        public decimal?[] UnitPrice { get; set; }
        public List<string> Tag { get; set; }
        public string Description { get; set; }
        public string Editor { get; set; }

        public List<int> CategoryId { get; set; }
        public List<int> SubCategoryId { get; set; }

        public List<string> Img { get; set; }

        public List<string> TagStatus { get; set; }
        public string CategoryStatus { get; set; }
        public string SubcategoryStatus { get; set; }
        
    }
}
