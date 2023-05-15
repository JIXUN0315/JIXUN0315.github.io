using Hatsukoi.Repository.EntityModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using Hatsukoi.Repository.Interface;
using AdminManagement.Models.ViewModels;

namespace AdminManagement.Services
{
    public class SalesDataService
    {

        private readonly IRepository _repository;

        public SalesDataService(IRepository repository)
        {
            _repository = repository;
        }

        public List<CategorySalesDataVM> GetSalesDataByCategory(int month)
        {
            var sqlQuery = $@"
                SELECT
                    c.CategoryName AS CategoryName, 
                    SUM(od.quantity * od.UnitPrice) AS SalesData
                FROM
                    category c
                    INNER JOIN subcategory sc ON c.id = sc.CategoryId
                    INNER JOIN product p ON sc.id = p.SubCategory
                    INNER JOIN productspecification ps ON p.id = ps.productid
                    INNER JOIN orderdetail od ON ps.id = od.productspecificationid
                    INNER JOIN[Order] o ON od.OrderId = o.id
                WHERE
                    MONTH(o.StatusFinishTime) = ${month}
                GROUP BY
                    c.CategoryName";

            var salesData = _repository.GetSQLQuery<CategorySalesDataVM>(sqlQuery).ToList();

            return salesData;
        }


    }


}
