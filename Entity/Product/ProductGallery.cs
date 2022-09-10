using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Product
{
    public class ProductGallery
    {
        public ProductGallery()
        {

        }

        #region properties
        [Key]
        public int GalleryId { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ImageName { get; set; }

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
