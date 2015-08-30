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
    public partial class IfrAdminEditor : System.Web.UI.Page {

        public MR_Admin AdminInfo {
            get {
                if (ViewState["AdminInfo"] == null) {
                    ViewState["AdminInfo"] = AdminPageStatic.GetLoginUserInfo();
                }
                return (MR_Admin)ViewState["AdminInfo"];
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                BindUserInfo();
            }
        }
        /// <summary>
        /// 绑定用户信息
        /// </summary>
        private void BindUserInfo() {
            this.tb_Email.Text = this.AdminInfo.Email;
            this.tb_NickName.Text = this.AdminInfo.ANickName;
        }


        protected void btn_save_Click(object sender, EventArgs e) {
            IAdminManage iamobj = GetInterface.GetIAdminManage();
            string anickname = this.tb_NickName.Text.Trim();
            string pwd1 = this.tb_LoginPwd1.Text.Trim();
            string pwd2 = this.tb_LoginPwd2.Text.Trim();
            string oldpwd = this.tb_OldPwd.Text.Trim();
            string email = this.tb_Email.Text.Trim();

            if (!string.IsNullOrEmpty(oldpwd)) {
                if (!pwd1.IsPassWord(false)) {
                    this.Alert(CYJH_OrderSystem.Admin.Base.LangPack.AdminCollect.EDITOR_INFO_ERR_PWD_ERR);
                    return;
                }
                if (pwd1 != pwd2) {
                    this.Alert(AdminCollect.EDITOR_INFO_ERR_PWD_NOT_THE_SAME);
                    return;
                }
                CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn result1 = iamobj.UpdatePwd(this.AdminInfo.AID, oldpwd, pwd1);
                if (result1 == CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn.OldPwdDeny) {
                    this.Alert(AdminCollect.EDITOR_INFO_ERR_OLD_PWD_ERR);
                    return;
                } else if (result1 == CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn.Error) {
                    this.Alert(AdminCollect.EDITOR_INFO_ERR_SAVE_ERR);
                    return;
                }
            }

            if (anickname.Length == 0 || anickname.Length > 10) {
                this.Alert(CYJH_OrderSystem.Admin.Base.LangPack.AdminCollect.EDITOR_INFO_ERR_NICKNAME_ERR);
                return;
            }

            if (!email.IsEmail()) {
                this.Alert(CYJH_OrderSystem.Admin.Base.LangPack.AdminCollect.EDITOR_INFO_ERR_EMAIL_ERR);
                return;
            }

            bool result = iamobj.UpdateBaseInfo(anickname, email, this.AdminInfo.AID);
            if (result) {
                this.AdminInfo.Email = email;
                this.AdminInfo.ANickName = anickname;
                AdminPageStatic.SaveUserFormsCookie(this.AdminInfo);

                this.Alert(CYJH_OrderSystem.Admin.Base.LangPack.AdminCollect.EDITOR_INFO_SUCCESS);
            } else {
                this.Alert(CYJH_OrderSystem.Admin.Base.LangPack.AdminCollect.EDITOR_INFO_SUCCESS_PWD_BUT_BASEINFO);
            }


        }


    }
}
