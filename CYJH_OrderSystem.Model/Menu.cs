using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYJH_OrderSystem.Model {
    public class Menu {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
    }
}