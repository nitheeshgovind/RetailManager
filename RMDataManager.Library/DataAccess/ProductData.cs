using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var products = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "RMData");

            return products;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var product = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "RMData").FirstOrDefault();

            return product;
        }
    }
}
