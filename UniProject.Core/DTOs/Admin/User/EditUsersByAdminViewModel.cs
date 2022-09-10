using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.Admin
{
    public class EditUsersByAdminViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        [EmailAddress(ErrorMessage = "لطفا ایمیل خود را به درستی وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string Password { get; set; }

        [Display(Name = "وضعیت کاربر")]
        public bool IsActive { get; set; }
        public List<int> UserRoles { get; set; }
    }
}
