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
        public void Post(SaleModel sale)
        {
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale, RequestContext.Principal.Identity.GetUserId());            
        }

        [Route("api/sale/report")]
        [AllowAnonymous]
        public IEnumerable<SaleReportModel> GetSalesReport()
        {
            SaleData saleData = new SaleData();
            return saleData.GetSaleReport();
        }
    }
}