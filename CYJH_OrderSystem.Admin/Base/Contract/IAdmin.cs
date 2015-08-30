using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface IAdmin {
        /// <summary>
        /// 登录,返回成功与否
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userPwd">登录密码明文</param>
        /// <param name="userIp">登录IP</param>
        /// <param name="model">登录成功后，会输出当前登录管理员的基本信息</param>
        /// <param name="text">管理员信息</param>
        bool Login(string userName, string userPwd, string userIp, out Model.MR_Admin model);

        /// <summary>
        /// 通过接口登录时
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userIp">IP</param>
        /// <param name="model">管理员信息</param>
        /// <returns></returns>
        bool LoginByGateWay(string userName, string userIp, out Model.MR_Admin model);
    }
}
