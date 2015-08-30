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
using CYJH_OrderSystem.Admin.Base;
namespace CYJH_OrderSystem.Admin.Role {
    public partial class IfrPageInfoEditor : System.Web.UI.Page {

        /// <summary>
        /// 当前已分配的隐藏页ID
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
        /// 页面ID
        /// </summary>
        public int NodeId {
            get {
                return ViewState["NodeId"].GetInt(0, false);
            }
            set {
                ViewState["NodeId"] = value;
            }
        }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string NodeName {
            get {
                return ViewState["NodeName"].GetString("");
            }
            set {
                ViewState["NodeName"] = value;
            }
        }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string NodeURL {
            get {
                return ViewState["NodeURL"].GetString("");
            }
            set {
                ViewState["NodeURL"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                this.NowInListId = new List<int>();
                this.NodeId = Request.QueryString["id"].GetInt(0, false);
                if (this.NodeId > 0) {
                    InitPageInfo();
                }

            }
        }

        private void InitPageInfo() {
            IRoleManage irm = GetInterface.GetIRoleManage();

            MR_PageInfo pinfo = irm.GetNode(this.NodeId);

            if (pinfo.PID != this.NodeId || pinfo.PID <= 0) {
                return;
            }

            this.NodeName = pinfo.PName;
            this.NodeURL = pinfo.PUrl;

            IList<MR_PageInfo> pageinfos = irm.GetHidePage(this.NodeId);
            this.dl_InChild.DataSource = pageinfos;
            this.dl_InChild.DataBind();
            this.NowInListId = new List<int>();
            if (pageinfos != null) {
                foreach (MR_PageInfo apage in pageinfos) {
                    this.NowInListId.Add(apage.PID);
                }
            }
            IList<MR_PageInfo> waitpageinfos = irm.GetListToList(-1, false);

            this.dl_MenuList.DataSource = waitpageinfos;
            this.dl_MenuList.DataBind();
        }

        /// <summary>
        /// 绑定列表的时候，判断一个菜单项是否可添加
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Boolean GetEnable(int pid) {
            return !this.NowInListId.Contains(pid);
        }

        /// <summary>
        /// 点击添加新隐藏页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_save_Click(object sender, EventArgs e) {
            string pname = this.tb_add_pname.Text.Trim();
            string purl = this.tb_add_url.Text.Trim();
            int parentid = -1;
            IRoleManage irm = GetInterface.GetIRoleManage();
            irm.AddNode(pname, purl, parentid, 0);
            InitPageInfo();

            SiteRuleCheck.FlushPageAndRightCache();
        }

        protected void dl_MenuList_ItemCommand(object source, DataListCommandEventArgs e) {
            string cmdName = e.CommandName;
            if (cmdName == "Insert") {
                int childId = this.dl_MenuList.DataKeys[e.Item.ItemIndex].GetInt(0);
                IPageParent ipp = GetInterface.GetIPageParent();
                ipp.AddChild(this.NodeId, childId);
                InitPageInfo();

                SiteRuleCheck.FlushPageAndRightCache();
            }
        }

        protected void dl_InChild_ItemCommand(object source, DataListCommandEventArgs e) {
            string cmdName = e.CommandName;
            if (cmdName == "Remove") {
                int childId = this.dl_InChild.DataKeys[e.Item.ItemIndex].GetInt(0);
                IPageParent ipp = GetInterface.GetIPageParent();
                ipp.RemoveChild(this.NodeId, childId);
                InitPageInfo();

                SiteRuleCheck.FlushPageAndRightCache();
            }
        }
    }
}
