using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Product
{
    public class ProductFeature
    {
        public ProductFeature()
        {

        }

        #region properties
        [Key]
        public int FeatureId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatuerTitle { get; set; }

        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FeatureValue { get; set; }

        public int ProductId { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreateDate { get; set; }
        #endregion

        #region relations
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        #endregion
    }
}
