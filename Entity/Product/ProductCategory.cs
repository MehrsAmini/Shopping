using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Product
{
    public class ProductCategory
    {
        public ProductCategory()
        {

        }

        #region Property
        [Key]
        public int CategoryId { get; set; }

        [Display(Name ="عنوان گروه دوره")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string CategoryTitle { get; set; }

        [Display(Name = "آدرس  گروه دوره")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string CategoryUrl { get; set; }

        public string? CategoryImageName { get; set; }

        [Display(Name = "تاریخ افزودن")]
        public DateTime RegisterDate { get; set; }

        public bool IsDelete { get; set; }

        public int? ParentId { get; set; }
        #endregion

        #region Relations
        [InverseProperty("ProductCategory")]
        public virtual List<Product> Category { get; set; }

        [ForeignKey("ParentId")]
        public virtual List<ProductCategory> ProductCategories { get; set; }
        #endregion
    }
}
