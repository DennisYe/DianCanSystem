using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using System.Data;
using CYJH_OrderSystem.Admin.Base.Factorys;
using CYJH_OrderSystem.Admin.Base.LangPack;

namespace CYJH_OrderSystem.Admin.Role {
    public partial class GroupList : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                DvBind();
            }
        }

        protected void lb_save_Click(object sender, EventArgs e) {
            IGroupManage gmobj = GetInterface.GetIGroupManage();
            string gname = this.tb_GroupName.Text.Trim();
            if (string.IsNullOrEmpty(gname)) {
                this.Alert(AdminCollect.ERR_GROUPNAME_NULL);
                return;
            }
            gmobj.AddUpdate(gname, 0);
            DvBind();
            SiteRuleCheck.FlushPageAndRightCache();
        }

        private void DvBind() {
            IGroupManage gmobj = GetInterface.GetIGroupManage();
            IList<MR_Group> groupList = gmobj.GetList();
            this.gvList.DataSource = groupList;
            this.gvList.DataBind();
        }



        /// <summary>
        /// 点击批量设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e) {
            string aidList = "";
            string span = "";
            foreach (GridViewRow dvr in this.gvList.Rows) {
                CheckBox cb = (CheckBox)dvr.FindControl("cb_GID");
                if (cb.Checked) {
                    aidList += span + gvList.DataKeys[dvr.RowIndex].Value;
                    span = ",";
                }
            }
            if (span == ",") {
                Response.Redirect("RoleManage.aspx?groupid=" + aidList);
            } else {
                Page.Alert(AdminCollect.COMM_ERR_PLEASE_CHESED_MORE_THEN_ONE);
            }
        }

        /// <summary>
        /// 点击编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowEditing(object sender, GridViewEditEventArgs e) {
            this.gvList.EditIndex = e.NewEditIndex;
            DvBind();
        }

        /// <summary>
        /// 在编辑状态下点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            this.gvList.EditIndex = -1;
            DvBind();
        }


        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            TextBox tb = (TextBox)this.gvList.Rows[e.RowIndex].FindControl("tb_Gname_Editor");
            string newName = tb.Text.Trim();
            int gid = this.gvList.DataKeys[e.RowIndex].Value.GetInt(0);
            if (string.IsNullOrEmpty(newName)) {
                this.Alert(AdminCollect.ERR_GROUPNAME_NULL);
                return;
            }
            IGroupManage igmobj = GetInterface.GetIGroupManage();
            igmobj.AddUpdate(newName, gid);
            this.gvList.EditIndex = -1;
            DvBind();
            SiteRuleCheck.FlushPageAndRightCache();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            IGroupManage igm = GetInterface.GetIGroupManage();
            int gid = this.gvList.DataKeys[e.RowIndex].Value.GetInt(0);
            bool result = igm.DeleteGroup(gid);
            this.gvList.EditIndex = -1;
            DvBind();
            if (result)
                this.Alert(AdminCollect.DELETE_SUCCESSFUL);
            else
                this.Alert(AdminCollect.DELETE_FAILED);
            SiteRuleCheck.FlushPageAndRightCache();
        }
    }
}
