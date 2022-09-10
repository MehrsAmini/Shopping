using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product
{
    public class ShowProductsViewModel
    {
        public List<DataLayer.Entity.Product.Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
