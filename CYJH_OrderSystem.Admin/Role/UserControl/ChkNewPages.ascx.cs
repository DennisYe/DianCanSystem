using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Shared;
using CYJH_OrderSystem.Admin.Base.Bll;
using CYJH_OrderSystem.Admin.Base.Contract;

namespace CYJH_OrderSystem.Admin.Role.UserControl {
    public partial class ChkNewPages : System.Web.UI.UserControl {
        protected BR_PageInfo pageBLL = new BR_PageInfo();
        protected BR_PageParent pageParentBLL = new BR_PageParent();

        protected List<CheckNewMgrPageItem> CheckNewPagesItems { get; set; }
        protected void Page_Load(object sender, EventArgs e) {
        }
        protected override void Render(HtmlTextWriter writer) {
            BindShow();
            base.Render(writer);
        }

        public void BindShow() {
            CheckNewPagesItems = GetCheckNewPagesItems();
        }

        public List<CheckNewMgrPageItem> GetCheckNewPagesItems() {
            List<CheckNewMgrPageItem> list = new List<CheckNewMgrPageItem>();
            var cfgFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/ConfigFile/CheckNewMgrPages/"), "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var f in cfgFiles) {
                ParseCheckNewMgrPageItem(f, ref list);
            }
            return list;
        }
        protected void ParseCheckNewMgrPageItem(string cfgPath, ref List<CheckNewMgrPageItem> list) {
            try {
                var doc = XDocument.Load(cfgPath);
                foreach (var itemEl in doc.Root.Elements("Item")) {
                    var item = new CheckNewMgrPageItem();
                    item.Type = itemEl.Attribute("Type").Value;
                    item.Name = itemEl.Element("Name").Value;
                    item.Id = item.Name.MD5();
                    switch (item.Type) {
                        case "Impl":
                            item.Impl = (ICheckNewMgrPages)pageBLL.GetType().Assembly.CreateInstance(itemEl.Element("Impl").Value);
                            break;
                    }
                    if (item.Impl != null) {
                        list.Add(item);
                    }
                }
            } catch {
            }
        }
    }
}