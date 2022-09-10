using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity.Order;

namespace UniProject.DataLayer.Entity.Product
{
    public class Product
    {
        #region properties
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ProductName { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }

        [Display(Name = "برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Brand { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "موجودی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Inventory { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SellerId { get; set; }
        #endregion

        #region relations
        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        [ForeignKey("SellerId")]
        public virtual User User { get; set; }

        public virtual ICollection<ProductGallery> ProductGallery { get; set; }

        public virtual ICollection<ProductFeature> ProductFeature { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }
        #endregion
    }
}
