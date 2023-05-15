using System.Linq.Expressions;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hatsukoi.Repository.DBRepository
{
    public class EFRepository : IRepository
    {
        //注入! readonly 是只能在類別裡面做指派
        private readonly HatsukoiContext _dbContext;
        private readonly IConfiguration _config;

        public EFRepository(HatsukoiContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _config = configuration;
        }

        //transaction 的時候是用同一個 DBcontext 所以才會公開出去
        //DbContext 是為了讓繼承這個repository 能讀取，以此來作客制化的CRUD(因此只有get)
        //改成protect
        public HatsukoiContext DbContext { get { return _dbContext; } }


        public void Create<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            Save();
        }
        public async Task CreateAsync<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.SaveChangesAsync();
        }
        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            Save();
        }
        //要修改成protect
        public IQueryable<T> GetAll<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return  _dbContext.Set<T>().AnyAsync(expression);
        }
        public bool Any<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().Any(expression);
        }
        //要修改成protect
        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public List<T> ListWhere<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().Where(expression).ToList() ?? new List<T>();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }
        public void Update<T>(T entity) where T : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            Save();
        }
        //我自己寫的
        public List<T> GetSQLQuery<T>(string queryString) where T : class
        {
            var conn = _config.GetConnectionString("HatsukoiContext");
            List<T> couponList;
            using (var context = new System.Data.Entity.DbContext(conn))
            {
                couponList = context.Database.SqlQuery<T>(queryString).ToList();
                return couponList;
            }
        }
    }
}
