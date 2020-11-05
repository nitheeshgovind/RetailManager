using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Controllers
{
    [System.Web.Mvc.Authorize]
    public class SaleController : ApiController
    {
        public HttpStatusCodeResult Post(SaleModel sale)
        {
            SaleData saleData = new SaleData();
            saleData.SaveSale(sale, RequestContext.Principal.Identity.GetUserId());

            return new HttpStatusCodeResult(200);
        }
    }
}