using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CYJH_OrderSystem.Model;
using Safe.Base.DbHelper;
using System.Configuration;
using System.Data;
using Shared;

namespace CYJH_OrderSystem.DAL {
    public class MenuData {

        private SQLHelper _sqlHelp = SQLHelps.OrderSystemData() as SQLHelper;
        public List<Menu> GetAllMenu() {
            //string connString = ConfigurationManager.ConnectionStrings["CYJHOrderSysConn"].ConnectionString;
            //SQLHelper sqlHelp = new SQLHelper(connString, true);
            
            string sqlCmd = "select * from Menu";
            List<Menu> menuList=new List<Menu>();

            if (_sqlHelp.TryConnection()) {
                DataTable menuResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                for (int i=0;i<menuResult.Rows.Count;i++) {
                    DataRow dr=menuResult.Rows[i];
                    Menu menu=new Menu();
                    menu.Id=Int32.Parse(dr["Id"].ToString());
                    menu.RestaurantName=dr["RestaurantName"].ToString();
                    menu.Phone=dr["Phone"].ToString();
                    menu.Address=dr["Address"].ToString();
                    menu.Comment=dr["Comment"].ToString();
                    menuList.Add(menu);
                }
            } else {
                throw new Exception("数据库连接失败");
            }
            return menuList;
        }

        public Menu GetMenuByMenuId(int menuId) {
            string sqlCmd = "select * from [Menu] where Id=" + menuId;
            if (_sqlHelp.TryConnection()) {
                DataTable orderResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                if (orderResult.Rows.Count != 0) {
                    DataRow dr = orderResult.Rows[0];
                    Menu menu = new Menu();
                    menu.Id = Int32.Parse(dr["Id"].ToString());
                    menu.RestaurantName = dr["RestaurantName"].ToString();
                    menu.Phone = dr["Phone"].ToString();
                    menu.Address = dr["Address"].ToString();
                    menu.Comment = dr["Comment"].ToString();
                    return menu;
                }

            } else {
                throw new Exception("数据库连接失败");
            }
            return null;
        }

        public List<Food> GetFoodsByMenuId(int menuId) {
            string sqlCmd = "select * from Food where MenuId=" + menuId;
            List<Food> foodList = new List<Food>();

            if (_sqlHelp.TryConnection()) {
                DataTable foodResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                for (int i = 0; i < foodResult.Rows.Count; i++) {
                    DataRow dr = foodResult.Rows[i];
                    Food food = new Food();
                    food.Id = Int32.Parse(dr["Id"].ToString());
                    food.Name = dr["Name"].ToString();
                    food.Price = Double.Parse(dr["Price"].ToString());
                    food.CategoryId = Int32.Parse(dr["CategoryId"].ToString());
                    food.MenuId =Int32.Parse(dr["MenuId"].ToString());
                    food.Comment = dr["Comment"].ToString();
                    foodList.Add(food);
                }
            } else {
                throw new Exception("数据库连接失败");
            }
            return foodList;
        }

        public List<Category> GetMenuCategory(int menuId) {
            string sqlCmd = "select distinct c.Id,c.[Name] from Category c with(NOLOCK)" +
                            " inner join Food f with(NOLOCK) on c.Id=f.CategoryId" +
                            " where f.MenuId=" + menuId;
            List<Category> categoryList = new List<Category>();

            if (_sqlHelp.TryConnection()) {
                DataTable categoryResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                for (int i = 0; i < categoryResult.Rows.Count; i++) {
                    DataRow dr = categoryResult.Rows[i];
                    Category category = new Category();
                    category.Id = Int32.Parse(dr["Id"].ToString());
                    category.Name = dr["Name"].ToString();
                    categoryList.Add(category);
                }
            } else {
                throw new Exception("数据库连接失败");
            }

            return categoryList;

        }
        public int CreateNewOrder(int menuId,string createName) {
            string sqlCmd = "insert into dbo.[Order](MenuId,CreateName,CreateTime) values(" + menuId + ",'" + createName + "','" + DateTime.Now + "')"//这样有可能会有SQL注入的情况，需要改为使用参数的方式传入
                            + " SELECT CAST(scope_identity() AS int)";
            int newOrderId = 0;
            if (_sqlHelp.TryConnection()) {
                newOrderId = Int32.Parse(_sqlHelp.ExecuteScalar(sqlCmd).ToString());

            } else {
                throw new Exception("数据库连接失败");
            }
            return newOrderId;
        }

        public void CreateOrderItem(List<OrderItem> itemList)
        {
            //string sqlCmd = "insert into OrderDetail(OrderId,UserName,FoodId,Comment,CreateTime) values(" + orderItem.OrderId + ","
            //    + orderItem.UserName + "," + orderItem.FoodId + "," + orderItem.Comment + "," + DateTime.Now + ")";
            string insertContent = string.Empty;
            int first=0;
            foreach (var orderItem in itemList)
            {
                if (first++ == 0)
                    insertContent += " select " + orderItem.OrderId + ",'" + orderItem.UserName + "'," + orderItem.FoodId + ",'" + orderItem.Comment + "','" + DateTime.Now + "',"+orderItem.Amount;
                else
                    insertContent += " union select " + orderItem.OrderId + ",'" + orderItem.UserName + "'," + orderItem.FoodId + ",'" + orderItem.Comment + "','" + DateTime.Now + "'," + orderItem.Amount;
            }
            string sqlCmd = "insert into OrderDetail(OrderId,UserName,FoodId,Comment,CreateTime,Amount) " + insertContent;
            if (_sqlHelp.TryConnection()) {
                _sqlHelp.ExecuteNonQuery(sqlCmd);
            } else {
                throw new Exception("数据库连接失败");
            }
        }

        public List<OrderItem> GetOrderItemsByOrderId(int orderId) {
            string sqlCmd = "select od.Id,f.Name,f.Price,od.UserName,od.Comment,od.CreateTime,cat.BelongCate,od.Amount" +
                            " from OrderDetail od with (NOLOCK)" +
                            " inner join Food f with (nolock) on od.FoodId=f.Id"+
                            " inner join Category cat with(NOLOCK) on cat.Id=f.CategoryId" +
                            " where od.OrderId = " + orderId +
                            " order by od.UserName,od.Id";
            List<OrderItem> orderItemList = new List<OrderItem>();
            if (_sqlHelp.TryConnection()) {
                DataTable orderItemResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                for (int i = 0; i < orderItemResult.Rows.Count; i++) {
                    DataRow dr = orderItemResult.Rows[i];
                    OrderItem orderItem = new OrderItem();
                    orderItem.Id = Int32.Parse(dr["Id"].ToString());
                    orderItem.OrderId = orderId;
                    orderItem.UserName = dr["UserName"].ToString();
                    orderItem.FoodPrice = Double.Parse(dr["Price"].ToString());
                    orderItem.FoodName = dr["Name"].ToString();
                    orderItem.CreateTime = DateTime.Parse(dr["CreateTime"].ToString()).ToString("HH:mm");
                    orderItem.Comment = dr["Comment"].ToString();
                    orderItem.BelongCate = dr["BelongCate"].ToString();
                    orderItem.Amount = Int32.Parse(dr["Amount"].ToString());
                    orderItemList.Add(orderItem);
                }
            } else {
                throw new Exception("数据库连接失败");
            }
            return orderItemList;
        }

        public Order GetOrderByOrderId(int orderId) {
            string sqlCmd = "select * from [order] where Id=" + orderId;
            if (_sqlHelp.TryConnection()) {
                DataTable orderResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                if(orderResult.Rows.Count!=0 ) {
                    DataRow dr = orderResult.Rows[0];
                    Order order= new Order();
                    order.Id = orderId;
                    order.MenuId = Int32.Parse(dr["MenuId"].ToString());
                    order.CreateName = dr["CreateName"].ToString();
                    order.CreateTime = DateTime.Parse(dr["CreateTime"].ToString()).ToString("HH:mm");
                    return order;
                }

            } else {
                throw new Exception("数据库连接失败");
            }
            return null;
        }

        public void DeleteFoodByOrderItemId(int orderItemId) {
            string sqlCmd = "delete from [OrderDetail] where Id = " + orderItemId;
            if (_sqlHelp.TryConnection()) {
                _sqlHelp.ExecuteNonQuery(sqlCmd);
            } else {
                throw new Exception("数据库连接失败");
            }
        }

        public List<Order> GetOrderIn4Hours(int menuId) {
            string sqlCmd =string.Format("select * from [order] where  datediff (hh,CreateTime,getdate())<5 order by CreateTime desc",menuId);
            List<Order> orderList = new List<Order>();
            if (_sqlHelp.TryConnection()) {
                DataTable orderResult = _sqlHelp.ExecuteFillDataTable(sqlCmd);
                for (var i = 0; i < orderResult.Rows.Count;i++ ) {
                    DataRow dr = orderResult.Rows[i];
                    Order order = new Order();
                    order.Id = Int32.Parse(dr["Id"].ToString());
                    order.MenuId = menuId;
                    order.CreateName = dr["CreateName"].ToString();
                    order.CreateTime = DateTime.Parse(dr["CreateTime"].ToString()).ToString("HH:mm");
                    orderList.Add(order);
                }

            } else {
                throw new Exception("数据库连接失败");
            }
            return orderList;
        }

    }
}