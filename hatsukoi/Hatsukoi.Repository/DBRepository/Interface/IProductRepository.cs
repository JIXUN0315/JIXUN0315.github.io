using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatsukoi.Repository.DBRepository.Interface
{
    public interface IProductRepository : IRepository
    {
        void GetProductByProductId(Product id);
        
    }
}
