using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity
{
    public class User
    {
        public User()
        {

        }

        #region properties
        [Key]
        public int UserId { get; set; }

        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string UserName { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "باقی مانده کیف پول")]
        public int RemainderWallet { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای رمزعبور باید بین {1} و {2} باشد")]
        public string Password { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(50, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string ActiveCode { get; set; }

        [Display(Name = "تصویر کاربر")]
        public string Avatar { get; set; }

        [Display(Name = "وضعیت کاربر")]
        public bool IsActive { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        public bool IsDelete { get; set; }
        #endregion

        #region Relations
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Address> Addresses { get; set; }

        public virtual List<Wallet> Wallets { get; set; }

        public virtual List<Product.Product> Products { get; set; }

        public virtual List<Order.Order> Orders { get; set; }
        #endregion
    }
}
