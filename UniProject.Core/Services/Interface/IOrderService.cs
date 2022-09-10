using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniProject.DataLayer.Entity.Order;

namespace UniProject.Core.Services.Interface
{
    public interface IOrderService
    {
        int AddOrder(string username, int productId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
        List<Order> GetOrdersOfUser(string userName);
        bool FinalyOrder(string userName, int orderId);
        Order GetCartForShow(string username);
        int AddCountToCart(int dedtailID);
        int LowOffCountToCart(int dedtailID);
        void RemoveItemFromCart(int productId, int orderId);
        bool IsInCartOrNot(int productID, string username);
    }
}
