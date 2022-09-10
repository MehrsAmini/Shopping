using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity
{
    public class Wallet
    {
        public Wallet()
        {

        }

        #region properties
        [Key]
        public int WalletId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        public int TypeId { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        public int UserId { get; set; }

        [Display(Name = "مبلغ تراکنش")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تایید پرداخت")]
        public bool IsPay { get; set; }

        [Display(Name = "شرح")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string Description { get; set; }
        #endregion

        #region Relation
        public virtual WalletType WalletType { get; set; }
        public virtual User User { get; set; }

        #endregion
    }
}
