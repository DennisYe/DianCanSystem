using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using CYJH_OrderSystem.Admin.Base.Factorys;
using CYJH_OrderSystem.Admin.Base.LangPack;
using System.Data;
using CYJH_OrderSystem.Admin.Base;

namespace CYJH_OrderSystem.Admin.Role {
    public partial class FastAddAdmin : System.Web.UI.Page {
        private IAdminManage iamobj = GetInterface.GetIAdminManage();

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                BindData();
                BindGroupList();
            }
        }

        public IList<MR_Group> GroupList {
            get {
                if (ViewState["GroupList"] == null) {
                    IGroupManage rgmobj = GetInterface.GetIGroupManage();
                    IList<MR_Group> tmp = rgmobj.GetList();
                    if (tmp == null)
                        tmp = new List<MR_Group>();
                    ViewState["GroupList"] = tmp;
                }
                return (IList<MR_Group>)ViewState["GroupList"];
            }
        }
        private void BindGroupList() {
            this.ddl_GroupList.DataSource = this.GroupList;
            this.ddl_GroupList.DataTextField = "GName";
            this.ddl_GroupList.DataValueField = "GID";
            this.ddl_GroupList.DataBind();
            this.ddl_GroupList.Items.Insert(0, new ListItem("请选择", "0"));
        }

        private void BindData() {
            ConServer.TcAdminChkol tcAdminChkol = new ConServer.TcAdminChkol();
            DataTable adminListTable = tcAdminChkol.GetAdminList();
            IList<MR_Admin> oldAdminList = iamobj.GetList();
            FilterOldAdmin(adminListTable, oldAdminList);
            adminList.DataSource = adminListTable;
            adminList.DataBind();
            AdminPageStatic.RoleChecked(this);
        }

        private static void FilterOldAdmin(DataTable adminListTable, IList<MR_Admin> oldAdminList) {

            //过滤已有的账户
            for (int i = 0; i < adminListTable.Rows.Count; i++) {
                DataRow row = adminListTable.Rows[i];
                object aname = row["AName"];
                if (aname != null && oldAdminList.Count(a => a.AName == aname.ToString()) > 0) {
                    adminListTable.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e) {
            int selectedGID = ddl_GroupList.SelectedValue.GetInt(0, false);
            if (selectedGID <= 0) {
                this.Alert("必须选择一个预设组,否则将无法登录!");
                return;
            }

            foreach (GridViewRow dvr in adminList.Rows) {
                CheckBox cb = (CheckBox)dvr.FindControl("cb_AName");
                Label lab_ANickName = (Label)dvr.FindControl("lab_ANickName");
                Label lab_Email = (Label)dvr.FindControl("lab_Email");
                if (cb.Checked) {
                    iamobj.Add(cb.Text, "", lab_ANickName.Text, lab_Email.Text, selectedGID);
                }
            }
            this.Alert("添加成功!!");
            SiteRuleCheck.FlushPageAndRightCache();
            BindData();
        }
    }
}
