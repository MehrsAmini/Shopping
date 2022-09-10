using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniProject.Core.Services.Interface;

namespace UniProject.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #region ShowOrders
        [Route("UserPanel/ShowOrders")]
        public IActionResult ShowOrders()
        {
            //Sure about this order for this user
            var order = _orderService.GetOrdersOfUser(User.Identity.Name);

            return View(order);
        }
        #endregion

        #region ShowOrderDetails
        [Route("UserPanel/ShowOrderDetails/{orderId}")]
        public IActionResult ShowOrderDetails(int orderId, bool finaly = false)
        {
            //Sure about this order for this user
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.finaly = finaly;

            return View(order);
        }
        #endregion

        #region ShowCart
        [Route("UserPanel/ShowCart/{orderId}")]
        public IActionResult ShowCart(int orderId)
        {
            //Sure about this order for this user
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        #region + or - Cart Items
        public IActionResult AddCountToCart(int dedtailID)
        {
            int add = _orderService.AddCountToCart(dedtailID);
            return Redirect("/UserPanel/ShowCart/" + add);
        }

        public IActionResult LowOffCountToCart(int dedtailID)
        {
            int low = _orderService.LowOffCountToCart(dedtailID);
            return Redirect("/UserPanel/ShowCart/" + low);
        }
        #endregion

        #region Delete Cart Items
        public IActionResult RemoveCartItems(int productId,int orderId)
        {
            _orderService.RemoveItemFromCart(productId, orderId);
            return Redirect("/UserPanel/ShowCart/" + orderId);

        }
        #endregion

        #endregion

        #region Checkout
        [Route("Checkout/{orderId}")]
        public IActionResult Checkout(int orderId)
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);

            if (order.IsFinaly == true)
            {
                return Redirect("/notfound");
            }

            return View(order);
        }
        #endregion

        #region FinalyOrder
        public IActionResult FinalyOrder(int id)
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);

            if (order.IsFinaly == true)
            {
                return Redirect("/notfound");
            }

            _orderService.FinalyOrder(User.Identity.Name, id);

            return View(order);
        }
        #endregion
    }
}