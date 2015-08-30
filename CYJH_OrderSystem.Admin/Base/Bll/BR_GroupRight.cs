using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Bll {
    public class BR_GroupRight {
        private DR_GroupRight _dal = new DR_GroupRight();

        public IList<MR_PageInfo> GetAllList() {
            return _dal.GetAllList();
        }
    }
}
