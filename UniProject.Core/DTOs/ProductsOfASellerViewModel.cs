using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs
{
    public class ProductsOfASellerViewModel
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
        public int Sum { get; set; }
        public int OrderID { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
