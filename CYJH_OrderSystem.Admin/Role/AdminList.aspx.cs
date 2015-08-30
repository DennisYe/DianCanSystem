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
using System.Data;
using CYJH_OrderSystem.Admin.Base.LangPack;
using CYJH_OrderSystem.Admin.Base;
namespace CYJH_OrderSystem.Admin.Role {
    public partial class AdminList : System.Web.UI.Page {

        public enum GetListAction {
            All = 0,
            ByNickName = 1
        }



        public GetListAction NowAction {
            get {
                return (GetListAction)ViewState["NowAction"].GetInt(0, false);
            }
            set {
                ViewState["NowAction"] = value;
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

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                this.NowAction = GetListAction.All;
                BindGroupList();
                DgBind();
            }
            AdminPageStatic.RoleChecked(this);
        }

        private void BindGroupList() {
            this.ddl_GroupList.DataSource = this.GroupList;
            this.ddl_GroupList.DataTextField = "GName";
            this.ddl_GroupList.DataValueField = "GID";
            this.ddl_GroupList.DataBind();
            this.ddl_GroupList.Items.Insert(0, new ListItem("请选择", "0"));

        }

        private void DgBind() {
            IAdminManage iamobj = GetInterface.GetIAdminManage();
            string keyword = this.tb_NickName.Text;
            IList<MR_Admin> adminList;// = iamobj.GetList();

            if (NowAction == GetListAction.All) {
                adminList = iamobj.GetList();
            } else if (keyword.ExistsSQLKeyChar()) {
                this.Alert("请填写中英文或数字，不要包含空格等特殊字符！");
                return;
            } else {
                adminList = iamobj.GetListByNickName(keyword);
            }
            this.gvList.DataSource = adminList;
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
                CheckBox cb = (CheckBox)dvr.FindControl("cb_NickName");
                if (cb.Checked) {
                    aidList += span + gvList.DataKeys[dvr.RowIndex].Value;
                    span = ",";
                }
            }
            if (span == ",") {
                Response.Redirect("RoleManage.aspx?adminid=" + aidList);
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
            DgBind();
        }

        /// <summary>
        /// 在编辑状态下点击取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            this.gvList.EditIndex = -1;
            DgBind();
        }


        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowIndex >= 0 && e.Row.RowIndex == this.gvList.EditIndex) {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddl_GroupList");
                ddl.DataSource = this.GroupList;
                ddl.DataTextField = "GName";
                ddl.DataValueField = "GID";
                ddl.DataBind();
                MR_Admin drv = (MR_Admin)e.Row.DataItem;
                int gid = drv.GID;
                if (gid == -1) {
                    ddl.Items.Add(new ListItem("超级管理员", "-1"));
                    if (AdminPageStatic.GetLoginUserInfo().GID != -1)
                        ddl.Enabled = false;
                    else
                        ddl.Enabled = true;
                }
                ddl.SelectedValue = gid.ToString();

            }
        }

        /// <summary>
        /// 添加新管理员时，点击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_save_Click(object sender, EventArgs e) {

            string aname = this.tb_LoginName.Text.Trim();
            string anickname = this.tb_ANickName.Text.Trim();
            string pwd1 = this.tb_LoginPwd1.Text.Trim();
            string pwd2 = this.tb_LoginPwd2.Text.Trim();
            string email = this.tb_Email.Text.Trim();
            int gid = this.ddl_GroupList.SelectedValue.GetInt(0);

            if (gid == 0) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_GROUP_ERR);
                return;
            }

            if (!aname.IsUserName()) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_USERNAME_ERR);
                return;
            }

            if (anickname.Length == 0 || anickname.Length > 10) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_NICKNAME_ERR);
                return;
            }

            if (pwd1 != pwd2) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_PWD_NOT_THE_SAME);
                return;
            }

            if (!email.IsEmail()) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_EMAIL_ERR);
                return;
            }

            IAdminManage iamobj = GetInterface.GetIAdminManage();

            int aid = iamobj.Add(aname, pwd1, anickname, email, gid);

            SiteRuleCheck.FlushPageAndRightCache();
            DgBind();

        }

        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvList_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            IAdminManage iam = GetInterface.GetIAdminManage();
            GridViewRow gvr = this.gvList.Rows[e.RowIndex];

            int oldGid = ((LinkButton)gvr.FindControl("btnupdate_Update")).CommandArgument.ToString().GetInt(0);
            string nickName = ((TextBox)gvr.FindControl("tb_NickName")).Text.Trim();
            int gid = ((DropDownList)gvr.FindControl("ddl_GroupList")).SelectedValue.GetInt(0);
            string email = ((TextBox)gvr.FindControl("tb_Email")).Text.Trim();
            int AID = this.gvList.DataKeys[e.RowIndex].Value.GetInt(0);

            if (nickName.Length == 0 || nickName.Length > 10) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_NICKNAME_ERR);
                return;
            }
            if (!email.IsEmail()) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_EMAIL_ERR);
                return;
            }
            if (!iam.UpdateBaseInfo(nickName, email, AID)) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_SAVE_ERR);
                return;
            }


            if (gid == 0) {
                this.Alert(AdminCollect.EDITOR_INFO_ERR_GROUP_ERR);
                return;
            }
            int arg = ((DropDownList)gvr.FindControl("ddl_rolearg")).SelectedValue.GetInt(0);
            if (!(arg == 0 && gid == oldGid)) {
                if (!iam.UpdateGroup(gid, AID, (CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdateGroupArg)arg)) {
                    this.Alert(AdminCollect.EDITOR_INFO_ERR_SAVE_GROUP_ERR);
                }
            }
            SiteRuleCheck.FlushPageAndRightCache();
            this.gvList.EditIndex = -1;
            DgBind();
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            int AID = this.gvList.DataKeys[e.RowIndex].Value.GetInt(0);
            IAdminManage iam = GetInterface.GetIAdminManage();
            if (iam.Delete(AdminPageStatic.GetLoginUserInfo().AID, AID)) {
                this.gvList.EditIndex = -1;
                DgBind();
            } else {
                this.Alert(AdminCollect.DELETE_ADMIN_ERR);
                DgBind();
            }
            SiteRuleCheck.FlushPageAndRightCache();

        }

        private void ReSetPwd(int adminid) {
            string newpwd = CYJH_OrderSystem.Admin.Base.Factorys.GetInterface.GetIAdminManage().ResetPwd(adminid);
            if (string.IsNullOrEmpty(newpwd)) {
                this.Alert("修改失败！");
            } else {
                this.Alert("修改成功，新密码为 ：" + newpwd);
            }
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e) {
            string cmdname = e.CommandName;
            switch (cmdname) {
                case "resetpwd":
                    ReSetPwd(e.CommandArgument.GetInt(0));
                    break;
            }
        }

        protected void btn_search_Click(object sender, EventArgs e) {
            this.NowAction = GetListAction.ByNickName;
            DgBind();
        }

        protected void btn_all_Click(object sender, EventArgs e) {
            this.NowAction = GetListAction.All;
            DgBind();
        }

    }
}