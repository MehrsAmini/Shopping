using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity
{
    public class Address
    {
        public Address()
        {

        }

        #region property
        public int AddressID { get; set; }
        
        public int UserId { get; set; }

        [Display(Name = "نام و نام خانواذگی")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string MobileNumber { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string Province { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string City { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        public string AddressDetails { get; set; }

        [Display(Name = "کدپستی")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string PostCode { get; set; }

        #endregion

        #region relations
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        #endregion
    }
}
