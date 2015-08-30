using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Shared;
using System.Text;
using CYJH_OrderSystem.Admin.Base;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Model;
using CYJH_OrderSystem.Admin.Base.Factorys;
namespace CYJH_OrderSystem.Admin {
    public partial class Main : System.Web.UI.Page {

        public string AdminUser = "";
        public string Menus = "";
        public int OnLoadId = 0;

        protected void Page_Load(object sender, EventArgs e) {
            this.AdminUser = AdminPageStatic.GetLoginUserInfo().ANickName;
            LoadMenus();
        }

        private void LoadMenus() {
            MR_Admin adminobj = AdminPageStatic.GetLoginUserInfo();
            int adminid = adminobj.AID;
            int groupid = adminobj.GID;

            IList<MR_PageInfo> info = new List<MR_PageInfo>();
            if (groupid > 0) {
                info = SiteRuleCheck.GetAdminPages(adminid, groupid, 0, false);
            } else if (groupid == -1) {
                IRoleManage irm = GetInterface.GetIRoleManage();
                info = irm.GetListToList(0, false);
            } else {
                Session["ErrInfo"] = "找不到用户组！";
                AdminPageStatic.RedirectToLoginPage(false);
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(" [");
            string span = "";
            if (info.Count > 0)
                OnLoadId = info[0].PID;
            foreach (MR_PageInfo tmp in info) {
                sb.Append(span);
                sb.Append("{");
                sb.AppendFormat(" 'Text': '{0}', 'Id': '{1}', 'URL': '{2}'", tmp.PName, tmp.PID, tmp.PUrl);
                sb.Append("}");
                span = ",";
            }
            sb.Append(" ]");
            this.Menus = sb.ToString();
        }

    }
}
