using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shared;
using CYJH_OrderSystem.Admin.Base;

namespace CYJH_OrderSystem.Admin.ConServer {
    /*
     * 检查用户是否在线  0 不在线   1 在线
     */
    public partial class IsOnline : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            if (AdminPageStatic.IsLogin()) {//没登录
                Response.Write("0");
                return;
            }


            string username = Request["username"];
            string md5sign = Request["sign"];

            if (string.IsNullOrEmpty(username) || username != User.Identity.Name || string.IsNullOrEmpty(md5sign)) {
                Response.Write("0");
                return;
            }

            string sysmd5sign = (username + AdminAppSetting.JumpLoginKey()).MD5();
            if (md5sign.ToLower() == sysmd5sign.ToLower()) {//签名正确
                Response.Write("1");
            } else {
                Response.Write("0");
            }
        }
    }
}
