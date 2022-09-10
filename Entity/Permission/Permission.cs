using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Permission
{
    public class Permission
    {
        public Permission()
        {
                
        }

        #region properties
        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان دسترسی")]
        [Required(ErrorMessage = "*لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نباید از {1} بیشتر باشد")]
        public string PermissionTitle { get; set; }

        public int? ParentId { get; set; }
        #endregion

        #region Relations
        [ForeignKey("ParentId")]
        public virtual List<Permission> Permissions { get; set; }
        public virtual List<RolePermission> RolePermission { get; set; }
        #endregion
    }
}
