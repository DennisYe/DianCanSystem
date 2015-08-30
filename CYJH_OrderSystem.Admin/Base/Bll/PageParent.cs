using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.BLL {
    internal class PageParent : IPageParent {
        #region IPageParent 成员

        public bool AddChild(int parentId, int childId) {
            return new DR_PageParent().Add(childId, parentId) > 0;
        }

        public bool RemoveChild(int parentId, int childId) {
            return new DR_PageParent().Delete(childId, parentId) > 0;
        }

        #endregion
    }
}
