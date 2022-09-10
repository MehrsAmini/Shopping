using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin.Product
{
    public class RequestAddProductDto
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ProductName { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Brand { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "موجودی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Inventory { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int SellerId { get; set; }

        public List<IFormFile> Images { get; set; }

        public List<AddNewFeaturesDto> Features { get; set; }
    }
}
