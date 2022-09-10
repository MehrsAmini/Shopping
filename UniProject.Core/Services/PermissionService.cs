using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Context;
using UniProject.DataLayer.Entity;
using UniProject.DataLayer.Entity.Permission;

namespace UniProject.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly UniProjectContext _context;
        public PermissionService(UniProjectContext context)
        {
            _context = context;
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }
            _context.SaveChanges();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.Where(r => r.RoleId != 1).ToList();
        }

        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //Delete All Roles User
            _context.UserRoles
                .Where(r => r.UserId == userId).ToList()
                .ForEach(r => _context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(rolesId, userId);
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            role.IsDeleted = false;
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleByRoleId(int roleId)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleId == roleId);
        }

        public void UpdateRole(Role role)
        {
            _context.Entry(role).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRoleByRoleId(int roleId)
        {
            Role role = _context.Roles.SingleOrDefault(r => r.RoleId == roleId);
            role.IsDeleted = true;
            UpdateRole(role);
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermissionsToRole(int roleId, List<int> permission)
        {
            foreach (var item in permission)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = item,
                });
            }
            _context.SaveChanges();
        }

        public List<int> GetPermissionRoleByRoleId(int roleId)
        {
            return _context.RolePermissions
                .Where(p => p.RoleId == roleId)
                .Select(p => p.PermissionId).ToList();
        }

        public void UpdatePermissionsToRole(int roleId, List<int> permission)
        {
            _context.RolePermissions.Where(p => p.RoleId == roleId)
                .ToList().ForEach(p => _context.RolePermissions.Remove(p));

            AddPermissionsToRole(roleId, permission);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;

            List<int> UserRoles = _context.UserRoles
                .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public void AddRoleToUser(int roleId, int userId)
        {
            _context.UserRoles.Add(new UserRole()
            {
                RoleId = roleId,
                UserId = userId
            });
            _context.SaveChanges();
        }
    }
}
