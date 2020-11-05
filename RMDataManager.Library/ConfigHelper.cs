using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            decimal taxRate = 0;

            string taxRateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTaxRate = decimal.TryParse(taxRateText, out taxRate);
            if (!isValidTaxRate)
            {
                throw new ConfigurationErrorsException("Tax Rate is not set up correctly!");
            }
            return taxRate;
        }
    }
}
