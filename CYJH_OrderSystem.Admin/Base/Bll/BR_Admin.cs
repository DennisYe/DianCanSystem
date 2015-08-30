using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using Shared;
namespace CYJH_OrderSystem.Admin.Base.BLL {
    internal class BR_Admin : ILogin {

        #region ILogin 成员
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="userIp"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Login(string userName, string userPwd, string userIp, out CYJH_OrderSystem.Admin.Base.Model.MR_Admin model) {
            model = new CYJH_OrderSystem.Admin.Base.Dal.DR_Admin().GetModel(userName, userPwd.MD5(), userIp);
            if (model == null || model.AID <= 0)
                return false;
            return true;
        }

        #endregion
    }
}
