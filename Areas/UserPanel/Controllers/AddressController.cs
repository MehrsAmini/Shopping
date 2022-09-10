using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.DTOs.UserPanel;
using UniProject.Core.Services.Interface;

namespace UniProject.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IUserService _userService;
        public AddressController(IUserService userService)
        {
            _userService = userService;
        }

        #region Show Address
        [Route("UserPanel/ShowAddress")]
        public IActionResult ShowAddress()
        {
            return View(_userService.GetAllAdresses(User.Identity.Name));
        }
        #endregion

        #region Create Address
        [Route("UserPanel/CreateAddress")]
        public IActionResult CreateAddress()
        {
            return View();
        }

        [Route("UserPanel/CreateAddress")]
        [HttpPost]
        public IActionResult CreateAddress(AddressViewModel address)
        {
            if (!ModelState.IsValid)
                return View(address);

            _userService.AddAdress(User.Identity.Name, address);

            return Redirect("/UserPanel/ShowAddress");
        }
        #endregion

        #region Edit Address
        [Route("UserPanel/EditAddress/{addressId}")]
        public IActionResult EditAddress(int addressId)
        {
            return View(_userService.GetAddressByAddressId(addressId));
        }

        [Route("UserPanel/EditAddress/{addressId}")]
        [HttpPost]
        public IActionResult EditAddress(EditAddressViewModel address)
        {
            if (!ModelState.IsValid)
                return View(address);

            _userService.UpdateAddress(address);
            return Redirect("/UserPanel/ShowAddress");
        }
        #endregion
    }
}
