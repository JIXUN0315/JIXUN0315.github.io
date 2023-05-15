using Hatsukoi.Repository.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.QueryModels.Category
{
    public class HomeProductFilterQueryModel
    {
        public int CategoryId { get; set; }

        //副類別
        public int SubCategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
        public List<HomeProductCardQueryModel>? HomeProductCards { get; set; }
    }

    public class HomeProductCardQueryModel
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
        public string? Tag { get; set; }
    }
}
