using Hatsukoi.Models.ViewModels;

namespace Hatsukoi.Service.Interface
{
    public interface IHomeProductFilterService
    {
        public IEnumerable<IGrouping<int, HomeProductFilterViewModel>> GetAllCategories();
    }
}
