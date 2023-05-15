namespace Hatsukoi.Models.Dtos.Create
{
#nullable disable
    public class SpecificationDto
    {
        public string FirstSpecification { get; set; }
        public string SecondSpecification { get; set; }

        public string[] FirstSepcItem { get; set; }
        public string[] SecondSepcItem { get; set; }
        public decimal[] UnitPrice { get; set; }

    }
   
}
