using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYJH_OrderSystem.Model {
    public class OrderItem {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public string CreateTime { get; set; }

        public int FoodId { get; set; }

        public string FoodName { get; set; }

        public double FoodPrice { get; set; }

        //当前项的归属类，比如菜，饭，汤
        public string BelongCate { get; set; }

        public int Amount { get; set; }
    }
}