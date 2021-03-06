﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly ISqlDataAccess sql;
        private readonly IProductData products;

        public SaleData(ISqlDataAccess sql, IProductData products)
        {
            this.sql = sql;
            this.products = products;
        }

        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"The product id of { item.ProductId} could not be found in the database.");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;
                if (productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * ConfigHelper.GetTaxRate() / 100;
                }

                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel()
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
            };
            sale.CashierId = cashierId;
            sale.Total = sale.SubTotal + sale.Tax;

            try
            {
                sql.BeginTransaction("RMData");

                sql.SaveDataInTransaction("dbo.spSaleInsert", sale);

                int id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                foreach (var item in details)
                {
                    item.SaleId = id;
                    sql.SaveDataInTransaction("dbo.spSaleDetailInsert", item);
                }

                sql.CommitTransaction();
            }
            catch
            {
                sql.RollbackTransaction();
                throw;
            }

        }

        /// <summary>
        /// Generates the Sales Report data
        /// </summary>
        /// <returns></returns>
        public List<SaleReportModel> GetSaleReport()
        {
            return sql.LoadData<SaleReportModel, dynamic>("dbo.spSaleReport", new { }, "RMData");
        }
    }
}
