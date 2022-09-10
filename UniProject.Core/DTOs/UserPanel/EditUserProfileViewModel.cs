using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.UserPanel
{
    public class EditUserProfileViewModel
    {
        public string UserName { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
