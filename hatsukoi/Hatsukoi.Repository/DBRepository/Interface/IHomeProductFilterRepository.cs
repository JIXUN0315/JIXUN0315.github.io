using Hatsukoi.Repository.Interface;
using Hatsukoi.Repository.QueryModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository.Interface
{
    public interface IHomeProductFilterRepository : IRepository
    {
        public IEnumerable<IGrouping<int, HomeProductFilterQueryModel>> GetAllCategories();
    }
}
