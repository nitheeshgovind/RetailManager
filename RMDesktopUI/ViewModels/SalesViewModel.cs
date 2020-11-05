using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;
        private IConfigHelper _configHelper;
        private BindingList<ProductModel> _products;
        private ProductModel _selectedItem;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity = 1;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper)
        {
            _productEndPoint = productEndPoint;
            _configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        public async Task LoadProducts()
        {
            Products = new BindingList<ProductModel>(await _productEndPoint.GetAll());
        }
        

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set { _products = value; NotifyOfPropertyChange(() => Products); }
        }

        public ProductModel SelectedProduct
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get { return _cart; }
            set { _cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                return (CalculateSubTotal() + CalculateTax()).ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal tax = 0;
            decimal taxRate = _configHelper.GetTaxRate();

            foreach (var item in Cart)
            {
                if (item.Product.IsTaxable)
                {
                    tax += (item.Product.RetailPrice * item.QuantityInCart * taxRate / 100);
                }

            }
            return tax;
        }

        public bool CanAddToCart
        {
            get
            {
                return ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel();
                item.Product = SelectedProduct;
                item.QuantityInCart = ItemQuantity;

                Cart.Add(item);
            }
            
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                return true;
            }
        }

        public void RemoveFromCart()
        {

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }
    }
}
