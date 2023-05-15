using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.DBRepository.Interface;
using Hatsukoi.Repository.EntityModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository
{
    public class ProductRepository : EFRepository, IProductRepository
    {
        public ProductRepository(HatsukoiContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public void GetProductByProductId(Product id)
        {
            //GetProductByProductId(id);
        }
        //private void GetProductByProductId(Product id)
        //{
        //    return IProductRepository<Product>()?.FirstOrDefault(x => x.Id == id);
        //}
    }
}
