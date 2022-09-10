using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.Security;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Entity;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IPermissionService _permission;
        public RolesController(IPermissionService permission)
        {
            _permission = permission;
        }

        #region ShowRoles
        [Route("admin/roles")]
        [PermissionChecker(6)]
        public IActionResult ShowRoles()
        {
            List<Role> RoleList = _permission.GetRoles();

            return View(RoleList);
        }
        #endregion

        #region CreateRoles
        [PermissionChecker(7)]
        [Route("admin/CreateRole")]
        public IActionResult CreateRole()
        {
            ViewData["Permissions"] = _permission.GetAllPermissions();

            return View();
        }

        [Route("admin/CreateRole")]
        [HttpPost]
        [PermissionChecker(7)]
        public IActionResult CreateRole(Role role,List<int> SelectedPermissions)
        {
            if (!ModelState.IsValid)
                return View(role);

            int roleId = _permission.AddRole(role);

            _permission.AddPermissionsToRole(roleId, SelectedPermissions);
            
            return Redirect("/admin/roles");
        }
        #endregion

        #region EditRole
        [Route("admin/EditRole/{roleId}")]
        [PermissionChecker(8)]
        public IActionResult EditRole(int roleId)
        {
            Role role = _permission.GetRoleByRoleId(roleId);

            ViewData["Permissions"] = _permission.GetAllPermissions();

            ViewData["SelectedPermissions"] = _permission.GetPermissionRoleByRoleId(roleId);

            return View(role);
        }

        [Route("admin/EditRole/{roleId}")]
        [HttpPost]
        [PermissionChecker(8)]
        public IActionResult EditRole(Role role, List<int> SelectedPermissions)
        {
            if (!ModelState.IsValid)
                return View(role);

            _permission.UpdateRole(role);

            _permission.UpdatePermissionsToRole(role.RoleId, SelectedPermissions);

            return Redirect("/admin/roles");
        }
        #endregion

        #region DeleteRole
        [Route("admin/DeleteRole/{roleId}")]
        [PermissionChecker(9)]
        public IActionResult DeleteRole(int roleId, Role role)
        {
            role = _permission.GetRoleByRoleId(roleId);

            ViewData["Permissions"] = _permission.GetAllPermissions();

            ViewData["SelectedPermissions"] = _permission.GetPermissionRoleByRoleId(roleId);

            return View(role);
        }

        [Route("admin/DeleteRole/{roleId}")]
        [HttpPost]
        [PermissionChecker(9)]
        public IActionResult DeleteRole(int roleId)
        {
            _permission.DeleteRoleByRoleId(roleId);

            return Redirect("/admin/roles");
        }
        #endregion
    }
}
