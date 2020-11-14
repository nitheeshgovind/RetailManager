using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData saleData;

        public SaleController(ISaleData saleData)
        {
            this.saleData = saleData;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            saleData.SaveSale(sale, User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [Route("report")]
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IEnumerable<SaleReportModel> GetSalesReport()
        {
            return saleData.GetSaleReport();
        }
    }
}