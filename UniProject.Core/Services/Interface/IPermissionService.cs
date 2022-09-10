using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity;
using UniProject.DataLayer.Entity.Permission;

namespace UniProject.Core.Services.Interface
{
    public interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        void AddRolesToUser(List<int> roleIds, int userId);
        void AddRoleToUser(int roleId, int userId);
        void EditRolesUser(int userId, List<int> rolesId);
        int AddRole(Role role);
        Role GetRoleByRoleId(int roleId);
        void UpdateRole(Role role);
        void DeleteRoleByRoleId(int roleId);
        #endregion

        #region Permissions
        List<Permission> GetAllPermissions();
        void AddPermissionsToRole(int roleId, List<int> permission);
        List<int> GetPermissionRoleByRoleId(int roleId);
        void UpdatePermissionsToRole(int roleId, List<int> permission);
        bool CheckPermission(int permissionId, string userName);
        #endregion
    }
}
