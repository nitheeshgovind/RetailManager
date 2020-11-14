using System.Collections.Generic;
using System.Linq;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess sql;

        public ProductData(ISqlDataAccess sql)
        {            
            this.sql = sql;
        }

        public List<ProductModel> GetProducts()
        {
            var products = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", new { }, "RMData");

            return products;
        }

        public ProductModel GetProductById(int productId)
        {
            var product = sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "RMData").FirstOrDefault();

            return product;
        }
    }
}
