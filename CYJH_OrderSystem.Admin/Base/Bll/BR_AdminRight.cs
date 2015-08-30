using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Bll {
    public class BR_AdminRight {
        private DR_AdminRight _dal = new DR_AdminRight();


        /// <summary>
        /// 是否存在该权限记录
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <param name="PID">页面ID</param>
        public bool Exists(int AID, int PID) {
            return _dal.Exists(AID, PID);
        }

        /// <summary>
        /// 所有权限配置同时带页面对象(包含隐藏页面)<para/>
        /// 隐藏页面的权限为父级页面的权限
        /// </summary>
        /// <returns></returns>
        public IList<MR_PageInfo> GetAllList() {
            return _dal.GetAllList();
        }
    }
}
