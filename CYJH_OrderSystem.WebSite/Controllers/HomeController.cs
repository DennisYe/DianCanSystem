using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CYJH_OrderSystem.BLL;
using CYJH_OrderSystem.Model;
using System.Web.Services;
using CYJH_OrderSystem.BLL.ThirdApp;

namespace CYJH_OrderSystem.website.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private MenuManager _manuManager = new MenuManager();
        public ActionResult Index(int id=1)
        {
            //ViewBag.MenuId = id;
            List<Menu> menuList = _manuManager.GetAllMenu();
            ViewBag.RestaurantList = menuList;
            Menu currentMenu = menuList.Find(o => o.Id == id);
            if (currentMenu == null)
                throw new Exception("不存在menu");
            ViewBag.CurrentMenu = currentMenu;
            ViewBag.OrderIn4Hours = _manuManager.GetOrderIn4Hours(id);
            //ViewBag.MenuDetailList = _manuManager.GetMenuDetailByMenuId(id);
            //ViewBag.CategoryList = _manuManager.GetMenuCategory(id);
            return View();
        }



        public ActionResult Order(int id = 1)
        {
            ViewBag.OrderId = id;
            Order order = _manuManager.GetOrderByOrderId(id);
            if (order == null)
                throw new Exception("不存在Order:" + id);
            int menuId = order.MenuId;
            Menu menu = _manuManager.GetMenuByMenuId(menuId);
            if(menu==null)
                throw new Exception("不存在Menu:" + menuId);
            ViewBag.CurrentMenu = menu;
            ViewBag.OrderInfo = order;
            ViewBag.MenuDetailList = _manuManager.GetMenuDetailByMenuId(menuId);
            ViewBag.CategoryList = _manuManager.GetMenuCategory(menuId);
            ViewBag.OrderItemList = _manuManager.GetOrderItemsByOrderId(id);
            return View();
        }

        [WebMethod]
        public int CreateOrder(int menuId, string createName)
        {
            return _manuManager.CreateNewOrder(menuId, createName);
        }

        [WebMethod]
        public void AddOrderItems(List<OrderItem> itemList)
        {
            _manuManager.CreateOrderItem(itemList);
            //return _manuManager.GetOrderItemsByOrderId(1);

        }

        [WebMethod]
        public void DeleteFoodItem(int orderItemId) 
        {
            _manuManager.DeleteFoodByOrderItemId(orderItemId);
        }

        [WebMethod]
        public object GetOrderItems(int orderId) {
             List<OrderItem> orderItems = _manuManager.GetOrderItemsByOrderId(orderId);
             return Json(orderItems);;
        }
        [WebMethod]
        public void test() {
            //ElemeThirdApp thirdApp = new ElemeThirdApp();
            //var result = thirdApp.GetThirdAppMenuData("");
            //Response.Write(result);
            ElemeThirdApp.GetMethod("http://api.np.mobilem.360.cn/redirect/down/?from=lm_227852&appid=3193808");
        }

    }
}