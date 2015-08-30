using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CYJH_OrderSystem.DAL;
using CYJH_OrderSystem.Model;

namespace CYJH_OrderSystem.BLL {
    public class MenuManager {
        private MenuData _menudata = new MenuData();
        public List<Menu> GetAllMenu() {
            return _menudata.GetAllMenu();
        }

        public Menu GetMenuByMenuId(int menuId) {
            return _menudata.GetMenuByMenuId(menuId);
        }
        public List<Food> GetMenuDetailByMenuId(int menuId) {
            return _menudata.GetFoodsByMenuId(menuId);
        }

        public List<Category> GetMenuCategory(int menuId) {
            return _menudata.GetMenuCategory(menuId);
        }
        public int CreateNewOrder(int menuId,string createName) {
            return _menudata.CreateNewOrder(menuId,createName);
        }

        public void CreateOrderItem(List<OrderItem> itemList)
        {
            _menudata.CreateOrderItem(itemList);
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId) {
            return _menudata.GetOrderItemsByOrderId(orderId);
        }

        public Order GetOrderByOrderId(int orderId) {
            return _menudata.GetOrderByOrderId(orderId);
        }

        public void DeleteFoodByOrderItemId(int orderItemId) {
            _menudata.DeleteFoodByOrderItemId(orderItemId);
        }

        public List<Order> GetOrderIn4Hours(int menuId) {
            return _menudata.GetOrderIn4Hours(menuId);
        }
    }
}