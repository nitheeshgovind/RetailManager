using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class InventoryData
    {

        Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public InventoryData(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            return sql.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "RMData");
        }

        public void SaveInventory(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            sql.SaveData("dbo.spInventoryInsert", item, "RMData");
        }
    }
}
