using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity.Permission;

namespace UniProject.DataLayer.Entity
{
    public class Role
    {
        public Role()
        {

        }

        #region properties
        [Key]
        public int RoleId { get; set; }

        [Display(Name ="نقش کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }
        public bool IsDeleted { get; set; }
        #endregion

        #region Relations
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<RolePermission> RolePermission { get; set; }
        #endregion
    }
}
