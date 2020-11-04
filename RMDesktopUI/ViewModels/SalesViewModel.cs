﻿using System;
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
        private BindingList<ProductModel> _cart;
        private string _itemQuantity;

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
        
        public string ItemQuantity
        {
            get { return _itemQuantity; }
            set { _itemQuantity = value; NotifyOfPropertyChange(() => ItemQuantity); }
        }
       
        public BindingList<ProductModel> Cart
        {
            get { return _cart; }
            set { _cart = value; NotifyOfPropertyChange(() => Cart); }
        }

        public string SubTotal
        {
            get
            {
                return "$0.00";
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
                return true;
            }
        }

        public void AddToCart()
        {

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

        }
    }
}
