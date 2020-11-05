using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndPoint _productEndPoint;
        private BindingList<ProductModel> _products;
        private ProductModel _selectedItem;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity = 1;

        public SalesViewModel(IProductEndPoint productEndPoint)
        {
            _productEndPoint = productEndPoint;            
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
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal = (item.Product.RetailPrice * item.QuantityInCart) + subTotal;
                }
                return subTotal.ToString("C");
            }
        }
        public string Tax
        {
            get
            {
                return "$0.00";
            }
        }
        public string Total
        {
            get
            {
                return "$0.00";
            }
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
            NotifyOfPropertyChange(() => existingItem.DisplayText);
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
        }
    }
}
