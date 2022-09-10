using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product
{
    public class ShowProductListItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Price { get; set; }
        public string ImageName { get; set; }
    }
}
