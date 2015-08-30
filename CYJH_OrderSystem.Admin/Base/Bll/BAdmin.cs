using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using CYJH_OrderSystem.Admin.Base.Dal;
namespace CYJH_OrderSystem.Admin.Base.BLL {
    internal class BAdmin : CYJH_OrderSystem.Admin.Base.Contract.IAdmin {

        #region IAdmin 成员

        public bool Login(string userName, string userPwd, string userIp, out Model.MR_Admin model) {
            model = new DAdmin().GetModel(userName, userPwd.MD5(), userIp);
            if (model == null || model.AID < 0)
                return false;
            return true;
        }

        public bool LoginByGateWay(string userName, string userIp, out Model.MR_Admin model) {
            model = new DAdmin().GetModel(userName, userIp);
            if (model == null || model.AID < 0)
                return false;
            return true;
        }

        #endregion
    }
}
