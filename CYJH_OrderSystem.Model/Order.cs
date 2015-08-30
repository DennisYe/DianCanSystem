using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYJH_OrderSystem.Model {
    public class Order {
        public int MenuId { get; set; }

        public string CreateTime { get; set; }

        public int Id { get; set; }

        public string CreateName { set; get; }
    }
}