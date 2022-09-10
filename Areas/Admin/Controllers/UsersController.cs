using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.DTOs.Admin;
using UniProject.Core.DTOs.UserPanel;
using UniProject.Core.Security;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Entity;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        public UsersController(IUserService userServise, IPermissionService permissionService)
        {
            _userService = userServise;
            _permissionService = permissionService;
        }

        [Route("admin")]
        [PermissionChecker(1)]
        public IActionResult Index()
        {
            return View();
        }

        #region ShowUsers
        [Route("admin/users")]
        [PermissionChecker(2)]
        public IActionResult ShowUsers(int pageId = 1, string Username = "", string Email = "")
        {
            ShowUsersForAdminViewModel showUsers = _userService.GetUsers(pageId, Username, Email);
            return View(showUsers);
        }
        #endregion

        #region ShowDeletedUsers
        [Route("admin/deletedUsers")]
        [PermissionChecker(2)]
        public IActionResult ShowDeletedUsers(int pageId = 1, string Username = "", string Email = "")
        {
            ShowUsersForAdminViewModel showUsers = _userService.GetDeletedUsers(pageId, Username, Email);
            return View(showUsers);
        }
        #endregion

        #region CreateUser
        [Route("admin/CreateUser")]
        [PermissionChecker(3)]
        public IActionResult CreateUser()
        {
            ViewData["Roles"] = _permissionService.GetRoles();

            return View();
        }

        [Route("admin/CreateUser")]
        [HttpPost]
        [PermissionChecker(3)]
        public IActionResult CreateUser(CreateUserViewModel createUser, List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return View(createUser);

            if (_userService.IsExistUserName(createUser.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمیباشد");
                return View(createUser);
            }
            if (_userService.IsExistEmail(createUser.Email))
            {
                ModelState.AddModelError("Email", "کاربری قبلا با ایمیل وارد شده ثبت نام شده است");
                return View(createUser);
            }

            int userId = _userService.AddUserFromAdmin(createUser);
            _permissionService.AddRolesToUser(SelectedRoles, userId);

            return Redirect("/Admin/Users");
        }
        #endregion

        #region EditUser
        [Route("admin/edituser/{userId}")]
        [PermissionChecker(4)]
        public IActionResult EditUser(int userId)
        {
            var editUsers = _userService.ShowUserForEditMode(userId);
            ViewData["Role"] = _permissionService.GetRoles();

            return View(editUsers);
        }

        [Route("admin/edituser/{userId}")]
        [HttpPost]
        [PermissionChecker(4)]
        public IActionResult EditUser(EditUsersByAdminViewModel editUsers, List<int> SelectedRoles)
        {
            ViewData["Role"] = _permissionService.GetRoles();

            if (!ModelState.IsValid)
                return View(editUsers);

            var user = _userService.GetUserById(editUsers.UserId);
            if (editUsers.Email != user.Email)
            {
                if (_userService.IsExistEmail(editUsers.Email))
                {
                    ModelState.AddModelError("Email", "کاربری قبلا با ایمیل وارد شده ثبت نام شده است");
                    return View(editUsers);
                }
            }

            _userService.EditUsersByAdmin(editUsers);
            _permissionService.EditRolesUser(editUsers.UserId, SelectedRoles);

            return Redirect("/Admin/Users");
        }
        #endregion

        #region DeleteUser
        [Route("admin/DeleteUser/{userId}")]
        [PermissionChecker(5)]
        public IActionResult DeleteUser(int userId, DeleteUserByAdminViewModel infoUsers)
        {
            infoUsers = _userService.GetUserInfoById(userId);
            ViewData["Role"] = _permissionService.GetRoles();

            return View(infoUsers);
        }

        [HttpPost]
        [Route("admin/DeleteUser/{userId}")]
        [PermissionChecker(5)]
        public IActionResult DeleteUser(int userId)
        {
            _userService.DeleteUserById(userId);
            return Redirect("/Admin/Users");
        }
        #endregion
    }
}
