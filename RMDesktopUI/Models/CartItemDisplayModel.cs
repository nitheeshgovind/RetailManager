using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Models
{
    public class CartItemDisplayModel : NotifyPropertyChanged
    {
        private ProductDisplayModel _product;

        public ProductDisplayModel Product
        {
            get { return _product; }
            set
            {
                _product = value;
                RaisePropertyChange();
                RaisePropertyChange(nameof(DisplayText));
            }
        }

        private int _quantity;
        public int QuantityInCart
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChange();
                RaisePropertyChange(nameof(DisplayText));
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName} ({QuantityInCart})";
            }
        }
    }
}
