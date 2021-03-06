﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        
        [Authorize(Roles = "Admin,Manager")]
        public IEnumerable<InventoryModel> Get()
        {
            InventoryData inventory = new InventoryData();
            return inventory.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel model)
        {
            InventoryData inventory = new InventoryData();
            inventory.SaveInventory(model);
        }
    }
}