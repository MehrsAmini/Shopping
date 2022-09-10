using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs
{
    public class CheckoutWithSellersViewModel
    {
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public int ProductsValue { get; set; }
        public int SiteProfit { get; set; }
        public int PayToSeller { get; set; }
    }
}
