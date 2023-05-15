using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.QueryModels.Category;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository
{
    public class HomeProductFilterRepository : EFRepository, IHomeProductFilterRepository
    {
        public HomeProductFilterRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public IEnumerable<IGrouping<int, HomeProductFilterQueryModel>> GetAllCategories()
        {
            var queryString = $@"
                    SELECT
                 c.Id AS CategoryId,c.CategoryName,
                 sc.Id AS SubCategoryId, sc.SubCategoryName
                    FROM [SubCategory] sc
                    INNER JOIN [Category] c ON c.Id = sc.CategoryId
                ";
            var allCategories = GetSQLQuery<HomeProductFilterQueryModel>(queryString);
            var categoryList = allCategories.GroupBy(x => x.CategoryId);
            return categoryList;
        }
    }
}
