using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.Convertor;
using UniProject.Core.DTOs.UserPanel;
using UniProject.Core.Senders;
using UniProject.Core.Services.Interface;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IViewRenderService _viewRender;

        public UserController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender = viewRender;
        }

        [Route("UserPanel")]
        public IActionResult Index()
        {
            return View(_userService.GetUserInfoByUsername(User.Identity.Name));
        }

        #region EditProfile

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetUserDataByUsername(User.Identity.Name));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditUserProfileViewModel editUser)
        {
            if (!ModelState.IsValid)
                return View(editUser);

            string username = User.Identity.Name;
            var user = _userService.GetUserByUsername(username);

            //if (editUser.UserName != user.UserName)
            //{
            //    if (_userService.IsExistUserName(editUser.UserName))
            //    {
            //        ModelState.AddModelError("UserName", "نام کاربری معتبر نمیباشد");
            //        return View(editUser);
            //    }
            //}

            if (editUser.Email != user.Email)
            {
                if (_userService.IsExistEmail(editUser.Email))
                {
                    ModelState.AddModelError("Email", "کاربری قبلا با ایمیل وارد شده ثبت نام شده است");
                    return View(editUser);
                }

                user.IsActive = false;

                #region SendEmail
                string body = _viewRender.RenderToStringAsync("../Views/Shared/ActiveEmail", user);
                SendingEmail.SendEmail(editUser.Email, "فعال سازی", body);
                //return View("SuccessEditUser.cshtml");
                #endregion
            }

            //string username = User.Identity.Name;
            _userService.EditProfile(username, editUser);

            //Logout User
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login");
            //return View("~/Views/Account/SuccessRegister.cshtml");
        }

        #endregion

        #region ChangePassword

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (!ModelState.IsValid)
                return View(changePassword);

            string username = User.Identity.Name;
            //var checkUserPass = _userService.CheckOldPassword(username, changePassword.OldPassword);
            if (!_userService.CheckOldPassword(username, changePassword.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "رمز عبور فعلی شما صحیح نمیباشد");
                return View(changePassword);
            }

            _userService.ChangePassword(username, changePassword);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion
    }
}
