using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs
{
    public class ResetPasswordViewModel
    {
        public string ActiveCode { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "تعداد کاراکترهای رمزعبور باید بین {1} و {2} باشد")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار آن مطابقت ندارد")]
        public string RePassword { get; set; }
    }
}
