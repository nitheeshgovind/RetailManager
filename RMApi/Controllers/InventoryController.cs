using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public InventoryController(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin,Manager")]
        public IEnumerable<InventoryModel> Get()
        {
            InventoryData inventory = new InventoryData(_configuration);
            return inventory.GetInventory();
        }

        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel model)
        {
            InventoryData inventory = new InventoryData(_configuration);
            inventory.SaveInventory(model);
        }
    }
}