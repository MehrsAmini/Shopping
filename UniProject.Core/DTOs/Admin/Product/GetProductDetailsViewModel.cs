using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product
{
    public class GetProductDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Price { get; set; }
        public int Inventory { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
        public List<AddNewFeaturesDto> Features { get; set; }
    }
}
