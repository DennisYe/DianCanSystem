/*
 * 检查指定用户是否存在。
 * 如果不存在返回0 
 * 存在返回1
 * 
 */
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
     * 检查用户名是否存在
     */
    public partial class CheckUser : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                string username = Request["username"];
                string md5sign = Request["sign"];

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(md5sign)) {
                    Response.Write("0");
                    return;
                }

                string sysmd5sign = (username + AdminAppSetting.JumpLoginKey()).MD5();
                if (md5sign.ToLower() == sysmd5sign.ToLower()) {//签名正确
                    if (CYJH_OrderSystem.Admin.Base.Factorys.GetInterface.GetICheckUserServer().IsExistsUser(username))
                        Response.Write("1");
                    else
                        Response.Write("0");
                } else {
                    Response.Write("0");
                }

            }
        }
    }
}
