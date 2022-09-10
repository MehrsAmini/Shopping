using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product.Delete
{
    public class ProductInfoViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public bool IsActive { get; set; }
        public string SellerName { get; set; }

        public string CategoryTitleLevel1 { get; set; }
        public string CategoryTitleLevel2 { get; set; }
        public string CategoryTitleLevel3 { get; set; }
    }
}
