using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base;
using CYJH_OrderSystem.Admin.Base.Model;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Factorys;
using Shared;
using System.Web.UI.HtmlControls;

namespace CYJH_OrderSystem.Admin {
    public partial class Left : System.Web.UI.Page {
        public string MenuTitle {
            get {
                object obj = ViewState["MenuTitle"];

                return obj == null ? "Header" : obj.ToString();
            }
            set {
                ViewState["MenuTitle"] = value;
            }
        }

        public int PageId {
            get {
                return Convert.ToInt32(ViewState["PageId"]);
            }
            set {
                ViewState["PageId"] = value;
            }
        }

        public IList<MR_PageInfo> Childs;

        public string AppQueryStr = "";

        protected void Page_Load(object sender, EventArgs e) {
            StaticFunctions.ClearClientPageCache();
            if (!Page.IsPostBack) {
                this.Childs = new List<MR_PageInfo>();
                if (Request.QueryString["id"] != null) {
                    string idstr = Request.QueryString["id"];
                    int id = 0;
                    int.TryParse(idstr, out id);
                    this.PageId = id;

                    LoadChilds();
                }

                string proId = Request.GetQ("ProjectId");
                if (!string.IsNullOrEmpty(proId)) {
                    AppQueryStr += "ProjectId=" + proId;
                }
            }
        }

        public string AppCurrQueryString(object urlObj) {
            string url = urlObj.GetString(string.Empty);
            if (!string.IsNullOrEmpty(AppQueryStr)) {
                if (url.IndexOf("?") < 0) {
                    url += "?";
                } else if (!url.EndsWith("?")) {
                    url += "&";
                }
            }
            return url + AppQueryStr;
        }

        private void LoadChilds() {
            MR_Admin adminInfo = AdminPageStatic.GetLoginUserInfo();
            int aid = adminInfo.AID;
            int gid = adminInfo.GID;

            IRoleManage irm = GetInterface.GetIRoleManage();
            this.MenuTitle = irm.GetNode(this.PageId).PName;
            if (gid > 0) {
                this.Childs = SiteRuleCheck.GetAdminPages(aid, gid, PageId, true);
            } else if (gid == -1) {
                this.Childs = irm.GetList(PageId, false, true);
            } else {
                this.Childs = new List<MR_PageInfo>();
            }
            if (this.Childs == null) this.Childs = new List<MR_PageInfo>();
            rep_Nav.DataSource = this.Childs;
            rep_Nav.DataBind();
        }

        protected void rep_Nav_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            Repeater repNav = (Repeater)sender;
            MR_PageInfo pageInfo = (MR_PageInfo)e.Item.DataItem;
            if (pageInfo == null) return;
            HtmlGenericControl childPanel = (HtmlGenericControl)e.Item.FindControl("childItem");
            if (pageInfo != null && childPanel != null) {
                if (pageInfo.Childs != null && pageInfo.Childs.Count > 0) {
                    Repeater childNav = new Repeater();
                    childNav.HeaderTemplate = repNav.HeaderTemplate;
                    childNav.ItemTemplate = repNav.ItemTemplate;
                    childNav.FooterTemplate = repNav.FooterTemplate;
                    childNav.ItemDataBound += new RepeaterItemEventHandler(rep_Nav_ItemDataBound);
                    childPanel.Controls.Add(childNav);
                    childNav.DataSource = pageInfo.Childs;
                    childNav.DataBind();
                    if (pageInfo.DefShowChild) {
                        childPanel.Style.Add("display", "block");
                    } else {
                        childPanel.Style.Add("display", "none");
                    }
                } else {
                    childPanel.Visible = false;
                }
            }
        }

        protected bool HasChilds(object childsObj) {
            if (childsObj == null) return false;
            IList<MR_PageInfo> childs = (IList<MR_PageInfo>)childsObj;
            if (childs == null || childs.Count <= 0) return false;
            return true;
        }
    }
}
