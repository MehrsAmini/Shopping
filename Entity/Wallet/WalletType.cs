using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity
{
    public class WalletType
    {
        public WalletType()
        {

        }

        #region properties
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TypeId { get; set; }

        [Display(Name ="نوع تراکنش")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        public string TypeTitle { get; set; }
        #endregion

        #region Relation

        public virtual List<Wallet> Wallets { get; set; }
        
        #endregion
    }
}
