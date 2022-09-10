using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.Core.Services.Interface;
using UniProject.DataLayer.Context;
using UniProject.DataLayer.Entity;
using UniProject.DataLayer.Entity.Order;

namespace UniProject.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly UniProjectContext _context;
        private readonly IUserService _userService;
        public OrderService(UniProjectContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public int AddOrder(string username, int productId)
        {
            int userId = _userService.GetUserIdByUsername(username);

            Order order = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            var product = _context.Products.Find(productId);

            //if user hasn't a Cart
            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = product.Price,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            ProductId = productId,
                            Count = 1,
                            Price = product.Price
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }

            //if user has a Cart
            else
            {
                OrderDetail detail = _context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.ProductId == productId);

                if (detail != null)
                {
                    detail.Count++;

                    _context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        ProductId = productId,
                        Price = product.Price,
                    };
                    _context.OrderDetails.Add(detail);
                }

                _context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }

            return order.OrderId;
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.SingleOrDefault(o => o.OrderId == orderId);
            order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == orderId).Sum(d => d.Price * d.Count);

            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userId = _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;

            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductGallery)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public List<Order> GetOrdersOfUser(string userName)
        {
            int userId = _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;
            return _context.Orders.Where(o => o.UserId == userId).ToList();
        }

        public bool FinalyOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUsername(userName);
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            if (_userService.RemainderWallet(userName) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    IsPay = true,
                    Description = "فاکتور شما #" + order.OrderId,
                    UserId = userId,
                    TypeId = 2
                });
                _context.Orders.Update(order);

                #region low inventory
                List<OrderDetail> orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
                foreach (var item in orderDetails)
                {
                    var product = _context.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                    product.Inventory = (product.Inventory) - (item.Count);
                    _context.Products.Update(product);
                }
                #endregion

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public Order GetCartForShow(string username)
        {
            int userId = _userService.GetUserIdByUsername(username);
            Order order = _context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
            if (order != null)
            {
                return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductGallery)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == order.OrderId);
            }
            else
            {
                return null;
            }
        }

        public int AddCountToCart(int dedtailID)
        {
            var cartItem = _context.OrderDetails.Find(dedtailID);

            cartItem.Count++;
            int inventory = _context.Products.SingleOrDefault(p => p.ProductId == cartItem.ProductId).Inventory;
            if (cartItem.Count >= inventory)
            {
                cartItem.Count = inventory;
            }
            //int productId = cartItem.ProductId;

            UpdatePriceOrder(cartItem.OrderId);
            _context.SaveChanges();

            return cartItem.OrderId;
        }

        public int LowOffCountToCart(int dedtailID)
        {
            var cartItem = _context.OrderDetails.Find(dedtailID);

            cartItem.Count--;
            if (cartItem.Count <= 1)
            {
                cartItem.Count = 1;
            }

            UpdatePriceOrder(cartItem.OrderId);
            _context.SaveChanges();

            return cartItem.OrderId;
        }
        public void RemoveItemFromCart(int productId, int orderId)
        {
            var cartitem = _context.OrderDetails.Where(p => p.ProductId == productId).FirstOrDefault();

            cartitem.IsRemoved = true;

            _context.SaveChanges();
        }

        public bool IsInCartOrNot(int productID, string username)
        {
            int userId = _userService.GetUserIdByUsername(username);
            Order order = _context.Orders.SingleOrDefault(o => o.IsFinaly == false && o.UserId == userId);

            if (order != null)
            {
                List<int> productIds = _context.OrderDetails.Where(od => od.OrderId == order.OrderId)
                .Select(od => od.ProductId).ToList();
                if (productIds.Contains(productID))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }

        }
    }
}
