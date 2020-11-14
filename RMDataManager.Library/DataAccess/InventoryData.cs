using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess sql;

        public InventoryData(ISqlDataAccess sqlDataAccess)
        {
            sql = sqlDataAccess;
        }

        public List<InventoryModel> GetInventory()
        {
            return sql.LoadData<InventoryModel, dynamic>("dbo.spInventoryGetAll", new { }, "RMData");
        }

        public void SaveInventory(InventoryModel item)
        {
            sql.SaveData("dbo.spInventoryInsert", item, "RMData");
        }
    }
}
