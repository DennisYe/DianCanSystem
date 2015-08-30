using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base;

namespace CYJH_OrderSystem.Admin {
    public partial class Logout : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            AdminPageStatic.LogOut();
            Session.Clear();
            Response.Redirect(AdminAppSetting.TcAdminLoginPage());
        }
    }
}
