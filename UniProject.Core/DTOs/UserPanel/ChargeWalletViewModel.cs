using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.Core.DTOs.UserPanel
{
    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [Range(5000, 1000000000, ErrorMessage = "مبلغ شارژ حساب باید بین {1} ریال و {2} ریال باشد")]
        public int Price { get; set; }
    }
}
