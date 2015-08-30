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
using System.Text;
using CYJH_OrderSystem.Admin.Base;

namespace CYJH_OrderSystem.Admin.Role {
    public partial class RoleManage : System.Web.UI.Page {
        /// <summary>
        /// 当前设置项目，是管理员，还是用户组
        /// </summary>
        private CYJH_OrderSystem.Admin.Base.Factorys.ERightType NowSettingType {
            get {
                return (CYJH_OrderSystem.Admin.Base.Factorys.ERightType)ViewState["nowType"];
            }
            set {
                ViewState["nowType"] = value;
            }
        }
        /// <summary>
        /// 通过URL传递进来的管理员ID列表，在第一次绑定组的时候，如果这里面有存在值，需要加入一个选项。
        /// </summary>
        public string AdminIdList {
            get {
                return ViewState["AdminIdList"].GetString(string.Empty);
            }
            set {
                ViewState["AdminIdList"] = value;
            }
        }

        /// <summary>
        /// 通过URL传递进来的用户组ID列表，在第一次绑定组的时候，如果这里面有存在值，需要加入一个选项。 
        /// </summary>
        public string GroupIdList {
            get {
                return ViewState["GroupIdList"].GetString(string.Empty);
            }
            set {
                ViewState["GroupIdList"] = value;
            }
        }

        /// <summary>
        /// 当前已分配的目录ID
        /// </summary>
        public IList<int> NowInListId {
            get {
                return (IList<int>)ViewState["NowInListId"];
            }
            set {
                ViewState["NowInListId"] = value;
            }
        }
        /// <summary>
        /// 批量设置权限时使用，保存当前已经添加的项目
        /// </summary>
        public IList<MR_PageInfo> NowAddToIdList {
            get {
                return (IList<MR_PageInfo>)ViewState["NowAddToGroup"];
            }
            set {
                ViewState["NowAddToGroup"] = value;
            }
        }
        /// <summary>
        /// 当前选择的根目录名称，仅用于显示
        /// </summary>
        public string NowSelectRootName {
            get {
                return ViewState["NowSelectRootName"].ToString();
            }
            set {
                ViewState["NowSelectRootName"] = value;
            }
        }
        /// <summary>
        /// 当前选择的根目录ID，仅用于快捷设置
        /// </summary>
        public int NowSelectRootId {
            get {
                return ViewState["NowSelectRootId"].GetInt(0);
            }
            set {
                ViewState["NowSelectRootId"] = value;
            }
        }

        private string Type_Group = "1";
        private string Type_Admin = "2";

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                AdminPageStatic.RoleChecked(this);

                this.NowSettingType = ERightType.Unknown;

                NowAddToIdList = new List<MR_PageInfo>();

                //绑定一级目录
                BindMenu();

                this.AdminIdList = Request.QueryString["adminid"].GetString(string.Empty);
                this.GroupIdList = Request.QueryString["groupid"].GetString(string.Empty);
                if (!string.IsNullOrEmpty(this.AdminIdList)) {//如果有传入管理员列表，批量分配权限
                    TypeChanged(Type_Admin, true);
                } else if (!string.IsNullOrEmpty(this.GroupIdList)) {
                    TypeChanged(Type_Group, true);
                }
            }
        }

        /// <summary>
        /// 当类型变化时触发 
        /// </summary>
        /// <param name="nowType"></param>
        /// <param name="bindIdList"></param>
        private void TypeChanged(string nowType, bool bindIdList) {
            this.ddl_Type.SelectedValue = nowType;
            if (nowType == Type_Group) {//选择的是预设组
                this.NowSettingType = ERightType.Group;
                BindGroupList(bindIdList);
            } else if (nowType == Type_Admin) {//选择的是管理员
                this.NowSettingType = ERightType.Admin;
                BindAdminList(bindIdList);
            }
            ReloadRoles();
        }

        private void ReloadRoles() {

            BindChildRight();
        }

        /// <summary>
        /// 绑定一级目录
        /// </summary>
        /// <param name="parendId"></param>
        private void BindMenu() {
            this.ddl_Root.Items.Clear();
            IRoleManage irmobj = GetInterface.GetIRoleManage();
            IList<MR_PageInfo> pinfos = irmobj.GetList(0, false, true);
            IList<MR_PageInfo> toList = new List<MR_PageInfo>();
            AdminPageStatic.ParseMenuChild(pinfos, 0, ref toList);
            this.ddl_Root.DataSource = toList;
            this.ddl_Root.DataTextField = "PName";
            this.ddl_Root.DataValueField = "PID";
            this.ddl_Root.DataBind();
            this.ddl_Root.Items.Insert(0, new ListItem("根目录", "0"));
        }

        /// <summary>
        /// 初始化管理员列表
        /// </summary>
        /// <param name="bindIdList"></param>
        private void BindAdminList(bool bindIdList) {
            IAdminManage iamobj = GetInterface.GetIAdminManage();
            IList<MR_Admin> adminList = iamobj.GetList();
            this.ddl_For.Items.Clear();
            this.ddl_For.DataSource = adminList;
            this.ddl_For.DataTextField = "ANickName";
            this.ddl_For.DataValueField = "AID";
            this.ddl_For.DataBind();

            //如果有传入页面ID
            string nicknames = "";
            string showids = "";
            string span = "";
            int searchCount = 0;//找到几个管理员ID会匹配
            if (this.AdminIdList != string.Empty) {
                string[] idarr = this.AdminIdList.Split(',');
                foreach (MR_Admin tmp in adminList) {
                    if (Array.IndexOf(idarr, tmp.AID.ToString()) > -1) {
                        nicknames += span + tmp.ANickName;
                        showids += span + tmp.AID.ToString();
                        span = ",";
                        searchCount++;
                    }
                }
            }

            //如果传入的ID能查找到管理员，添加一项，
            if (span == ",") {
                if (searchCount > 1)
                    this.ddl_For.Items.Add(new ListItem(nicknames, showids));
                if (bindIdList)  //如果默需要默认选择这一项
                    this.ddl_For.SelectedValue = showids;
            }

        }

        /// <summary>
        /// 初始化权限组列表
        /// </summary>
        /// <param name="bindIdList"></param>
        private void BindGroupList(bool bindIdList) {
            IGroupManage rgmobj = GetInterface.GetIGroupManage();
            IList<MR_Group> groupList = rgmobj.GetList();
            this.ddl_For.DataSource = groupList;
            this.ddl_For.DataTextField = "GName";
            this.ddl_For.DataValueField = "GID";
            this.ddl_For.DataBind();


            //如果有传入页面ID
            string names = "";
            string showids = "";
            string span = "";
            int searchCount = 0;//找到几个管理员ID会匹配
            if (this.GroupIdList != string.Empty) {
                string[] idarr = this.GroupIdList.Split(',');
                foreach (MR_Group tmp in groupList) {
                    if (Array.IndexOf(idarr, tmp.GID.ToString()) > -1) {
                        names += span + tmp.GName;
                        showids += span + tmp.GID.ToString();
                        span = ",";
                        searchCount++;
                    }
                }
            }

            //如果传入的ID能查找到管理员，添加一项，
            if (span == ",") {
                if (searchCount > 1)
                    this.ddl_For.Items.Add(new ListItem(names, showids));
                if (bindIdList)  //如果默需要默认选择这一项
                    this.ddl_For.SelectedValue = showids;
            }
        }


        /// <summary>
        /// 取得对象在选择的根目录下的权限，同时绑定待选页面
        /// </summary>
        private void BindChildRight() {
            //取得正确的接口用于取子项
            IRightsSetting irs = GetInterface.GetIRightsSetting(this.NowSettingType);
            if (irs == null)
                return;

            // 取得对象列表
            string foridlist = this.ddl_For.SelectedValue;

            // 取根目录
            int parentid = this.ddl_Root.SelectedValue.GetInt(0, false);
            this.NowSelectRootName = this.ddl_Root.SelectedItem.Text.ToString();
            this.NowSelectRootId = parentid;

            IRoleManage rmobj = GetInterface.GetIRoleManage();
            IList<MR_PageInfo> NowChild = rmobj.GetListToList(parentid, false);//取得目录下的子节点


            IList<MR_PageInfo> nowRight;
            if (foridlist.IsNumber(false)) {//如果是单个管理员或用户组
                int forid = foridlist.GetInt(0, false);
                nowRight = irs.GetChildMenuToList(forid, parentid, false);
            } else {//如果是多个管理员或用户组
                nowRight = this.NowAddToIdList;
            }


            this.NowInListId = new List<int>();
            foreach (MR_PageInfo aright in nowRight) {
                if (!this.NowInListId.Contains(aright.PID))
                    this.NowInListId.Add(aright.PID);
            }

            this.gvInRole.DataSource = nowRight;
            this.gvInRole.DataBind();

            this.dl_MenuList.DataSource = NowChild;
            this.dl_MenuList.DataBind();
        }


        /// <summary>
        /// 在绑定目录时取得一个菜单是否可添加
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Boolean GetEnable(int pid) {
            return !this.NowInListId.Contains(pid);
        }

        /// <summary>
        /// 添加菜单项到当前选中的对象中
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="defautExp"></param>
        private void AddPageRight(int pid, string defautExp) {
            int rootid = this.ddl_Root.SelectedValue.GetInt(0);
            string idlist = this.ddl_For.SelectedValue;
            bool isidlist = !idlist.IsNumber(false);

            IRightsSetting irs = GetInterface.GetIRightsSetting(this.NowSettingType);
            IRoleManage irm = GetInterface.GetIRoleManage();
            //如果权限没有设置，手动设为000000
            if (string.IsNullOrEmpty(defautExp)) {
                defautExp = "0000000";
            }

            //将这个权限赋给对象
            irs.AddRole(pid, idlist.ToIntList(','), defautExp, false);

            if (isidlist) {//如果对象是一组ID列表，需要将当前的表单缓存起来
                if (this.NowAddToIdList.Count(t => t.PID == pid) == 0) {
                    MR_PageInfo info = irm.GetNode(pid);
                    info.BtnRightExp = defautExp;
                    this.NowAddToIdList.Add(info);
                } else {
                    MR_PageInfo info = this.NowAddToIdList.First(t => t.PID == pid);
                    info.BtnRightExp = defautExp;
                }
            }

            BindChildRight();

            SiteRuleCheck.FlushPageAndRightCache();

        }

        /// <summary>
        /// 保存选择的权限
        /// </summary>
        private void SavePageBtnRule() {
            string idlist = this.ddl_For.SelectedValue;
            Dictionary<int, string> roles = new Dictionary<int, string>();
            foreach (GridViewRow dgitem in this.gvInRole.Rows) {
                if (dgitem.RowType == DataControlRowType.DataRow) {
                    int pid = this.gvInRole.DataKeys[dgitem.RowIndex].Value.GetInt(0);
                    StringBuilder rolestr = new StringBuilder();
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_1"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_2"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_3"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_4"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_5"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_6"));
                    rolestr.Append(GetBtnRightsFromGridViewRow(dgitem, "r_7"));
                    roles.Add(pid, rolestr.ToString());
                }
            }

            IRightsSetting irs = GetInterface.GetIRightsSetting(this.NowSettingType);
            irs.UpdateBtnExp(idlist.ToIntList(','), roles);

            SiteRuleCheck.FlushPageAndRightCache();

            ReloadRoles();
        }

        private string GetBtnRightsFromGridViewRow(GridViewRow dgitem, string id) {
            if (((CheckBox)dgitem.FindControl(id)).Checked)
                return "1";
            return "0";

        }

        /// <summary>
        /// 点击保存权限按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_save_Click(object sender, EventArgs e) {
            SavePageBtnRule();
        }

        /// <summary>
        /// 点击刷新对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_reload_Click(object sender, EventArgs e) {
            TypeChanged(this.ddl_Type.SelectedValue, true);
        }

        /// <summary>
        /// 根目录发生变化时，需要绑定子菜单列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Root_SelectedIndexChanged(object sender, EventArgs e) {
            ReloadRoles();
        }

        /// <summary>
        /// 用户发生变化时，需要重新加载现有权限
        /// 需要注意的是：如果是批量操作的用户。不需要重新加载权限，只把权限清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_For_SelectedIndexChanged(object sender, EventArgs e) {
            ReloadRoles();
        }

        /// <summary>
        /// 类型发生变化时，需要重新加载列表（或用户，或用户组）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e) {
            TypeChanged(this.ddl_Type.SelectedValue, true);
        }

        protected void dl_MenuList_ItemCommand(object source, DataListCommandEventArgs e) {
            string cmdName = e.CommandName;
            int pid = dl_MenuList.DataKeys[e.Item.ItemIndex].GetInt(0);
            if (pid == 0) {
                return;
            }

            if (cmdName == "Insert") { //将一个选项添加到当前选中的用户或用户组中
                AddPageRight(pid, e.CommandArgument.ToString());

                SiteRuleCheck.FlushPageAndRightCache();

            }
        }

        /// <summary>
        /// 移除对象的某个权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvInRole_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            int pageid = this.gvInRole.DataKeys[e.RowIndex].Value.GetInt(0);
            string idlist = this.ddl_For.SelectedValue;
            IRightsSetting irs = GetInterface.GetIRightsSetting(this.NowSettingType);
            irs.RemoveRole(idlist.ToIntList(','), pageid);

            SiteRuleCheck.FlushPageAndRightCache();

            ReloadRoles();
        }

        protected void gvInRole_RowCommand(object sender, GridViewCommandEventArgs e) {
            string cmdName = e.CommandName;
            switch (cmdName) {
                case "ChangeRoot":

                    SiteRuleCheck.FlushPageAndRightCache();

                    ListItem li = this.ddl_Root.Items.FindByValue(e.CommandArgument.ToString());
                    if (li != null && li != this.ddl_Root.SelectedItem) {
                        this.ddl_Root.SelectedValue = li.Value;
                        ReloadRoles();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
