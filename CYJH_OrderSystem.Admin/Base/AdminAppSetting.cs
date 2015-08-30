using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shared;

namespace CYJH_OrderSystem.Admin.Base {
    public static class AdminAppSetting {
        /// <summary>
        /// 后台版本号
        /// </summary>
        public const string Version = "2.0.3";

        public const string SESSION_NAME_CHECKCODE = "NumCode";

        /// <summary>
        /// 登录接口的KEY
        /// </summary>
        /// <returns></returns>
        public static string JumpLoginKey() {
            return System.Configuration.ConfigurationManager.AppSettings["loginkey"];
        }
        /// <summary>
        /// 集成登录页面
        /// </summary>
        /// <returns></returns>
        public static string TcAdminLoginPage() {
            return System.Configuration.ConfigurationManager.AppSettings["tcAdminLoginPage"];
        }
    }
}
