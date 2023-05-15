using Hatsukoi.Repository.EntityModel;

namespace Hatsukoi.Models.ViewModels
{
    public class HomeProductFilterViewModel
    {
        //主類別
        public int CategoryId { get; set; }
        //副類別
        public int SubCategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
        public List<HomeProductCard>? HomeProductCards { get; set; }

        public int TotalCount { get; set; } 
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string? SelectedSortOrder { get; set; }
        public string? SelectedPriceOrder { get; set; }
        public string? SelectedDateOrder { get; set; }
        public int SelectedTagOrder { get; set; }
    }

    public class HomeProductCard
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string? ProductFirstImg { get; set; }
        public decimal Price { get; set; }
        public int ProductStatus { get; set; }
        public int SellerId { get; set; }
        public int ViewTimes { get; set; }
        public DateTime? CreateDate { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }

        public List<int>? ProductTagList { get; set; }
        
    }

    public class ProductFilterInput
    {
        public int Id { get; set; }
        public int SubCatId { get; set; }
        public string? Keyword { get; set; }
        public string? SortOrder { get; set; }
        public string? PriceOrder { get; set; }
        public string? DateOrder { get; set; }
        public int TagOrder { get; set; }
    }
}
