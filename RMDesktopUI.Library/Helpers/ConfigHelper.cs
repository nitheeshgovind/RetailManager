using System.Configuration;

namespace RMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
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
