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
    public class WalletController : Controller
    {
        private readonly IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.ListWallet = _userService.GetAllTransaction(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [Route("UserPanel/Wallet")]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetAllTransaction(User.Identity.Name);
                return View(charge);
            }
            int walletId = _userService.ChargeWallet(User.Identity.Name, charge.Price, "شارژ حساب");

            #region Online Payment

            //send amount of price
            var payment = new ZarinpalSandbox.Payment(charge.Price);

            //send request to dargah & give redirect of darkhast
            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44354/OnlinePayment/" + walletId, "winshop2022@gmail.com", "09019162495");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }

            #endregion

            return null;
        }
    }
}
