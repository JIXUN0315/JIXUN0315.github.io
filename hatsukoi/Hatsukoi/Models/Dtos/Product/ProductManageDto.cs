namespace Hatsukoi.Models.Dtos.Product
{
#nullable disable
    public class ProductManageDto
    {
        public int ProductId { get; set; }
        public string ProductFirstImg { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
        public int ProductStatus { get; set; }

    }
    public class ProductManageIdListDto
    {
        public List<ProductManageDto> productManageDtos { get; set; }
    }
}
