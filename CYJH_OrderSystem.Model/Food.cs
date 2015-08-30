using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CYJH_OrderSystem.Model {
    public class Food {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int MenuId { get; set; }

        public int CategoryId { get; set; }

        public string Comment { get; set; }
    }
}