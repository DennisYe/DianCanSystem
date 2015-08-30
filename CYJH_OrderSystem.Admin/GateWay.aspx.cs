using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base;
using CYJH_OrderSystem.Admin.Base.Factorys;
using CYJH_OrderSystem.Admin.Base.Contract;
using Shared;
using CYJH_OrderSystem.Admin.Base.Model;
using System.Web.Security;

namespace CYJH_OrderSystem.Admin {
    public partial class GateWay : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                DoSign();
            } catch (Exception ex) {
                Response.Write(Server.HtmlEncode(ex.Message) + "<br/><a href='" + AdminPageStatic.GetLoginURL() + "'>点击这里重新登录</a>");
            }
        }
        private void DoSign() {
            string par_username = Server.UrlDecode(Request.QueryString["username"]);
            string par_rndcode = Server.UrlDecode(Request.QueryString["rnd"]);
            string par_sign = Server.UrlDecode(Request.QueryString["sign"]);
            if (string.IsNullOrEmpty(par_username) || string.IsNullOrEmpty(par_username) || string.IsNullOrEmpty(par_sign)) {
                //如果不包含其中的数据,那么直接跳转到集成登录页面
                Response.Redirect(AdminAppSetting.TcAdminLoginPage());
            }

            string md5sign = (par_username + par_rndcode + AdminAppSetting.JumpLoginKey()).MD5();
            if (md5sign == par_sign) {//验证通过
                IAdmin gatewayobj = GetInterface.GetIAdmin();
                CYJH_OrderSystem.Admin.Base.Model.MR_Admin model;
                bool islogin = gatewayobj.LoginByGateWay(par_username, Request.UserHostAddress, out model);
                if (model == null || model.AID <= 0) {
                    Response.Write("<br/><a href='" + AdminPageStatic.GetLoginURL() + "'>验证失败！请确定您有访问该系统的权限！</a>");
                    return;
                } else {
                    AdminPageStatic.SaveUserFormsCookie(model);
                    Response.Write("<script type='text/javascript'>window.setTimeout(function(){location.href='" + AdminPageStatic.GetDefaultURL() + "'}, 2000)</script>");
                    Response.Write("<a href='" + AdminPageStatic.GetDefaultURL() + "'>您已登录成功，如果2秒内没有自动进入系统，请点击这里</a>");

                    return;
                }
            }
            Response.Write(par_username + "<br/>" + par_sign + "<br/>" + par_rndcode + "<br/>" + md5sign);
        }
    }
}
