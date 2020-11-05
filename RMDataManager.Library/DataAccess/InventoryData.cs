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
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess();

            return sql.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "RMData");
        }

        public void SaveInventory(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.SaveData("dbo.spInventoryInsert", item, "RMData");
        }
    }
}
