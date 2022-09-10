using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.Services.Interface;

namespace UniProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CheckoutController : Controller
    {
        private readonly IProductService _productService;
        public CheckoutController(IProductService productService)
        {
            _productService = productService;
        }

        #region Show Checkouts
        [Route("admin/CheckoutWithSellers")]
        public IActionResult ShowAccounts(string filter = "")
        {
            
            return View(_productService.GetSoldProducts(User.Identity.Name));
        }
        #endregion

        #region Details of Checkouts
        [Route("admin/CheckoutWithSellersDetails/{sellerID}")]
        public IActionResult ShowAccountsDetails(int sellerID)
        {
            
            return View(_productService.GetProductsOfASeller(sellerID));
        }
        #endregion
    }
}
