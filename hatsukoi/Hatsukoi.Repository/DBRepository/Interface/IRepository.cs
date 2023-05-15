
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.EntityModel;
using System.Linq.Expressions;

namespace Hatsukoi.Repository.Interface
{
    public interface IRepository
    {
        //定義這個頁面要有的屬性
        //要拔掉
        public HatsukoiContext DbContext { get; }
        public Task CreateAsync<T>(T entity) where T : class;
        public void Create<T>(T entity) where T : class;
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        //要拔掉
        public IQueryable<T> GetAll<T>() where T : class;
        List<T> ListWhere<T>(Expression<Func<T, bool>> expression) where T : class;

        T FirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class;

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        bool Any<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        //要拔掉
        public void Save();
        public List<T> GetSQLQuery<T>(string queryString) where T : class;
    }
}
