using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class SaleController : ControllerBase
    {
        private Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public SaleController(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData saleData = new SaleData(_configuration);            
            saleData.SaveSale(sale, User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [Route("report")]
        [Authorize(Roles = "Manager,Admin")]
        public IEnumerable<SaleReportModel> GetSalesReport()
        {
            SaleData saleData = new SaleData(_configuration);
            return saleData.GetSaleReport();
        }
    }
}