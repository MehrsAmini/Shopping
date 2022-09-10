using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniProject.DataLayer.Entity.Permission
{
    public class RolePermission
    {
        public RolePermission()
        {

        }

        #region properties
        [Key]
        public int RPID { get; set; }

        public int RoleId { get; set; }

        public int PermissionId { get; set; }
        #endregion

        #region Relations
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
        #endregion
    }
}
