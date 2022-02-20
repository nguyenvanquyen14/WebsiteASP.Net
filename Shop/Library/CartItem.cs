using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public decimal PriceSale { get; set; }
        public int Qty { get; set; }
        public decimal Amount { get; set; }
        public CartItem() { }
        public CartItem( int proid, string name, string img, decimal pricesale, int qty)
        {
            this.ProductId = proid;
            this.Name = name;
            this.Img = img;
            this.PriceSale = pricesale;
            this.Qty = qty;
            this.Amount = pricesale * qty;

        }
    }
}