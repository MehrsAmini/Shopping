using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.UserPanel
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "رمز عبور فعلی")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string OldPassword { get; set; }

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار رمز عبور جدید")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [Compare("NewPassword", ErrorMessage = "کلمه عبور با تکرار آن مطابقت ندارد")]
        public string ReNewPassword { get; set; }
    }
}
