using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity.Product;

namespace UniProject.Core.DTOs.Admin
{
    public class ShowProductCategories
    {
        public List<DataLayer.Entity.Product.ProductCategory> ProductCategories { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
