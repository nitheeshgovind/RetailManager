using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale, RequestContext.Principal.Identity.GetUserId());            
        }

        [Route("report")]
        [Authorize(Roles = "Manager,Admin")]
        public IEnumerable<SaleReportModel> GetSalesReport()
        {
            SaleData saleData = new SaleData();
            return saleData.GetSaleReport();
        }
    }
}