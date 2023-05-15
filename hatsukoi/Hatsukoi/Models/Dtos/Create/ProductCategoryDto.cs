using Hatsukoi.Repository;
using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.Dtos.Create
{
#nullable disable
    public class ProductSubCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

        public string[] TagName { get; set; }
    }
}
