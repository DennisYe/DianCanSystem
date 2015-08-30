using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CYJH_OrderSystem.Admin.Base.Bll;
using Shared;
using CYJH_OrderSystem.Admin.Base.Model;
using System.Web.UI.HtmlControls;
using CYJH_OrderSystem.Admin.Base.Contract;
using System.IO;
using System.Xml.Linq;
using System.Reflection;

namespace CYJH_OrderSystem.Admin.Role {
    public partial class PageList : System.Web.UI.Page {
        BR_PageInfo pageBLL = new BR_PageInfo();
        BR_PageParent pageParentBLL = new BR_PageParent();

        protected void Page_Load(object sender, EventArgs e) {

            if (ProcAjaxSwitch()) return;

            if (!IsPostBack) {
                BindShow();
            }
        }

        #region AJAX处理

        /// <summary>
        /// 处理AJAX选择
        /// </summary>
        /// <returns></returns>
        private bool ProcAjaxSwitch() {

            //100成功,要刷单个
            //200成功,要刷列表
            //101失败,不操作
            //202失败,刷页面

            var action = Request.GetF("Action");
            int code = 101;
            string errMsg = "";
            AjaxHTMLData ajaxHTMLData = new AjaxHTMLData();
            switch (action) {
                case "EditPage":
                    ProcAjaxEditPage(out code, out errMsg, out ajaxHTMLData);
                    break;
                case "AddChildPage":
                    ProcAjaxAddChildPage(out code, out errMsg, out ajaxHTMLData);
                    break;
                case "DeletePage":
                    ProcAjaxDeletePage(out code, out errMsg, out ajaxHTMLData);
                    break;
                case "AddHidePageRe":
                    ProcAjaxAddHidePageRe(out code, out errMsg, out ajaxHTMLData);
                    break;
                case "GetChkNewPages":
                    ProcAjaxGetChkNewPages(out code, out errMsg, out ajaxHTMLData);
                    break;
                case "ImportPageCfg":
                    ProcAjaxImportPageCfg(out code, out errMsg, out ajaxHTMLData);
                    break;
                default:
                    return false;
            }

            Response.Write(new {
                Code = code,
                ErrMsg = errMsg,
                PageHTML = ajaxHTMLData.PageTreeListHTML,
                SelHTML = ajaxHTMLData.PageTreeDrListHTML,
                HidePageList = ajaxHTMLData.HidePagesListHTML,
                ChkNewPagesListHTML = ajaxHTMLData.ChkNewPagesListHTML
            }.GetJSON());
            Response.End();
            return true;
        }

        /// <summary>
        /// 添加导入的页面配置-单个,如果存在相同url则，使用更新
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="child"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private bool DoImportPageCfgOne(int pId, MR_PageInfo child, out string errMsg) {
            errMsg = "";
            var isURL = false;

            if (child.PName.IsEmpty()) {
                errMsg = "请输入页面名称";
                return false;
            }
            if (child.PUrl.IsNotEmpty()) {
                isURL = true;
            }
            if (child.PUrl.IsNotEmpty()) {
                var existPage = pageBLL.GetModel(child.PUrl);
                if (existPage != null) {
                    //存在同URL的页面
                    if (existPage.ParentID != pId) {
                        errMsg = "其他位置存在同URL的页面";
                        return false;
                    }
                    //同位置下的，采用更新机制
                    child.PID = existPage.PID;
                }
            }

            child.IsUrl = isURL;

            if (child.HideParentID > 0) {
                child.ParentID = -1;
            } else {
                child.ParentID = pId;
            }

            if (child.PID <= 0) {
                if (!pageBLL.Add(child, out errMsg)) {
                    if (errMsg.IsEmpty()) {
                        errMsg = "添加失败,可能已添加";
                        return false;
                    } else {
                        errMsg = "添加失败,ex:" + errMsg;
                        return false;
                    }
                }
            }
            if (child.ParentID == -1 && child.PID > 0) {
                if (!pageParentBLL.Exists(child.PID, pId)) pageParentBLL.Add(child.PID, pId, out errMsg);
            }

            errMsg = "";
            return true;
        }
        /// <summary>
        /// 添加导入的页面配置-递归列表
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="childs"></param>
        /// <returns></returns>
        private string DoImportPageCfgList(int pId, List<MR_PageInfo> childs) {
            if (childs == null) return string.Empty;
            string errMsg = "";
            foreach (var child in childs) {
                string oneErrMsg;
                if (!DoImportPageCfgOne(pId, child, out oneErrMsg)) {
                    if (oneErrMsg.IsEmpty()) {
                        errMsg += child.PName + "添加失败\r\n";
                    } else {
                        errMsg += child.PName + "添加失败:" + oneErrMsg + "\r\n";
                    }
                    continue;
                }
                if (child.Childs != null) {
                    errMsg += DoImportPageCfgList(child.PID, child.Childs);
                }
            }
            return errMsg;
        }
        /// <summary>
        /// 处理AJAX-导入页面配置
        /// </summary>
        private void ProcAjaxImportPageCfg(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 200;
            errMsg = "";
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
                ChkNewPagesListHTML = "",
            };

            var pId = Request.GetF("PID").GetInt(0, false);
            var cfgJSON = Request.GetF("CfgJSON");
            List<MR_PageInfo> childs = null;
            try {
                childs = cfgJSON.JSONDeserialize<List<MR_PageInfo>>();
            } catch (Exception ex) {
                code = 101;
                errMsg = "导入配置解析错误：" + ex.Message;
                return;
            }

            errMsg = DoImportPageCfgList(pId, childs);
            if (errMsg.IsEmpty()) code = 200;
            else code = 201;

            List<MR_PageInfo> allPagesTree;
            ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
            ajaxHTMLData.PageTreeDrListHTML = BindPageTreeDrList(allPagesTree).RenderControl();
            ajaxHTMLData.HidePagesListHTML = BindHidePages().RenderControl();
            ajaxHTMLData.ChkNewPagesListHTML = this.LoadControl("~/Role/UserControl/ChkNewPages.ascx").RenderControl();
        }

        /// <summary>
        /// 处理AJAX-获取检测新页面的内容
        /// </summary>
        private void ProcAjaxGetChkNewPages(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 200;
            errMsg = "";
            var uCtrl = this.LoadControl("~/Role/UserControl/ChkNewPages.ascx");
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
                ChkNewPagesListHTML = uCtrl.RenderControl(),
            };

        }

        /// <summary>
        /// 处理AJAX-编辑页面
        /// </summary>
        private void ProcAjaxEditPage(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 101;
            errMsg = "";
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
            };
            List<MR_PageInfo> allPagesTree;

            var editPageInfo = Request.Form.GetModel<MR_PageInfo>();
            if (editPageInfo == null || editPageInfo.PID <= 0) {
                errMsg = "参数不全";
                return;
            }

            var oldPageInfo = pageBLL.GetModel(editPageInfo.PID);
            if (oldPageInfo == null) {
                code = 200;
                errMsg = "页面已经不存在,请重新操作";
                ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
                ajaxHTMLData.PageTreeDrListHTML = BindPageTreeDrList(allPagesTree).RenderControl();
                return;
            }

            bool isHidePage = false;//是否隐藏页面
            //关系变更
            if (oldPageInfo.ParentID > -1 && oldPageInfo.ParentID != editPageInfo.ParentID) {
                if (CheckPIDInChilds(oldPageInfo.PID, editPageInfo.ParentID)) {
                    errMsg = "所属的父级不能是自己或自己的子集";
                    return;
                }
            }
            if (oldPageInfo.ParentID == -1) {
                isHidePage = true;
            }

            oldPageInfo.PName = editPageInfo.PName;
            oldPageInfo.PUrl = editPageInfo.PUrl;
            oldPageInfo.Queue = editPageInfo.Queue;
            oldPageInfo.DefShowChild = editPageInfo.DefShowChild;
            if (oldPageInfo.ParentID > -1) oldPageInfo.ParentID = editPageInfo.ParentID;

            if (oldPageInfo.PUrl.IsNotEmpty()) {
                if (pageBLL.ExistURL(oldPageInfo.PUrl, oldPageInfo.PID)) {
                    errMsg = "已存在相同URL的页面";
                    return;
                }
            }
            if (!pageBLL.Update(oldPageInfo)) {
                errMsg = "更新失败";
                return;
            }

            code = 200;
            ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
            ajaxHTMLData.PageTreeDrListHTML = BindPageTreeDrList(allPagesTree).RenderControl();
            if (isHidePage) {
                ajaxHTMLData.HidePagesListHTML = BindHidePages().RenderControl();
            }
            ajaxHTMLData.ChkNewPagesListHTML = this.LoadControl("~/Role/UserControl/ChkNewPages.ascx").RenderControl();
        }

        /// <summary>
        /// 处理AJAX-删除页面节点
        /// </summary>
        private void ProcAjaxDeletePage(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 101;
            errMsg = "";
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
            };
            string exStr = "";

            var PID = Request.GetF("PID").GetInt(0, false);
            var HideParentID = Request.GetF("HideParentID").GetInt(0, false);
            var DelPage = Request.GetF("DelPage").GetInt(0, false);

            if (HideParentID > 0) {
                //是隐藏页面,先删除关联
                if (!pageParentBLL.Delete(PID, HideParentID, out exStr)) {
                    if (exStr.IsEmpty()) {
                        code = 100;
                        errMsg = "";
                    } else {
                        errMsg = "关联删除失败:" + exStr;
                    }
                    return;
                } else {
                    code = 100;
                }
            }

            if (DelPage == 1) {
                if (pageBLL.Delete(PID, out exStr)) {
                    code = 100;
                    errMsg = "";
                } else {
                    code = 101;
                    errMsg = "删除失败,请先删除该页面的所有非隐藏子页面" + (exStr.IsEmpty() ? "" : "\n异常:" + exStr);
                }
            }
            if (code == 100) {
                List<MR_PageInfo> allPagesTree;
                ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
                ajaxHTMLData.PageTreeDrListHTML = BindPageTreeDrList(allPagesTree).RenderControl();
                ajaxHTMLData.HidePagesListHTML = BindHidePages().RenderControl();
                ajaxHTMLData.ChkNewPagesListHTML = this.LoadControl("~/Role/UserControl/ChkNewPages.ascx").RenderControl();
            }

        }

        /// <summary>
        /// 处理AJAX-隐藏页面关联
        /// </summary>
        private void ProcAjaxAddHidePageRe(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 101;
            errMsg = "";
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
            };
            string exStr = "";

            var PID = Request.GetF("PID").GetInt(0, false);
            var ParentID = Request.GetF("ParentID").GetInt(0, false);

            if (pageParentBLL.Add(PID, ParentID, out exStr) <= 0) {
                if (exStr.IsEmpty()) {
                    code = 100;
                    errMsg = "关联失败,可能已添加";
                } else {
                    code = 101;
                    errMsg = "关联失败,ex:" + exStr;
                }
            } else {
                code = 100;
                errMsg = "";
            }
            if (code == 100) {
                List<MR_PageInfo> allPagesTree;
                ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
                ajaxHTMLData.HidePagesListHTML = BindHidePages().RenderControl();
            }
        }

        /// <summary>
        /// 处理AJAX-添加子节点
        /// </summary>
        private void ProcAjaxAddChildPage(out int code, out string errMsg, out AjaxHTMLData ajaxHTMLData) {
            code = 101;
            errMsg = "";
            ajaxHTMLData = new AjaxHTMLData {
                PageTreeListHTML = "",
                PageTreeDrListHTML = "",
                HidePagesListHTML = "",
            };
            string exStr = "";

            var parentID = Request.GetF("ParentID").GetInt(0, false);
            var pageType = Request.GetF("PageType");
            var pName = Request.GetF("PName");
            var pUrl = Request.GetF("PUrl");
            var queue = Request.GetF("Queue").GetInt(0, false);
            var defShowChild = Request.GetF("DefShowChild").GetBoolean(false);

            var isURL = false;
            var isHide = (pageType == "HidePage");

            if (pName.IsEmpty()) {
                code = 201;
                errMsg = "请输入页面名称";
                return;
            }
            if (pUrl.IsNotEmpty()) {
                isURL = true;
            }
            if (pUrl.IsNotEmpty()) {
                if (pageBLL.ExistURL(pUrl, 0)) {
                    code = 201;
                    errMsg = "已存在相同URL的页面";
                    return;
                }
            }

            var model = new MR_PageInfo {
                ParentID = parentID,
                PName = pName,
                PUrl = pUrl,
                Queue = queue,
                DefShowChild = defShowChild,
                IsUrl = isURL,
            };

            if (isHide) {
                model.ParentID = -1;
            }

            if (pageBLL.Add(model, out exStr)) {
                code = 200;
                errMsg = "";
            } else {
                if (exStr.IsEmpty()) {
                    code = 202;
                    errMsg = "添加失败,可能已添加";
                    return;
                } else {
                    code = 202;
                    errMsg = "添加失败,ex:" + exStr;
                    return;
                }
            }

            if (code == 200) {
                if (isHide && model.PID > 0) {
                    pageParentBLL.Add(model.PID, parentID, out exStr);
                }

                List<MR_PageInfo> allPagesTree;
                ajaxHTMLData.PageTreeListHTML = BindPageTree(out allPagesTree).RenderControl();
                ajaxHTMLData.ChkNewPagesListHTML = this.LoadControl("~/Role/UserControl/ChkNewPages.ascx").RenderControl();
                switch (pageType) {
                    case "Dir":
                    case "ShowPage":
                        ajaxHTMLData.PageTreeDrListHTML = BindPageTreeDrList(allPagesTree).RenderControl();
                        break;
                    case "HidePage":
                        ajaxHTMLData.HidePagesListHTML = BindHidePages().RenderControl();
                        break;
                }
            }

        }


        #endregion

        #region 页面绑定
        /// <summary>
        /// 页面绑定相关初始化
        /// </summary>
        private List<MR_PageInfo> BindShow() {
            List<MR_PageInfo> allPagesTree;
            BindPageTree(out allPagesTree);
            BindPageTreeDrList(allPagesTree);
            BindHidePages();
            return allPagesTree;
        }
        /// <summary>
        /// 绑定页面树
        /// </summary>
        /// <returns></returns>
        private Control BindPageTree(out List<MR_PageInfo> allPagesTree) {
            allPagesTree = pageBLL.GetAllPages();
            rp_PageItem.DataSource = allPagesTree;
            rp_PageItem.DataBind();
            return rp_PageItem;
        }
        /// <summary>
        /// 绑定页面树的下拉菜单
        /// </summary>
        /// <param name="allPages"></param>
        /// <returns></returns>
        private Control BindPageTreeDrList(List<MR_PageInfo> allPages) {
            if (allPages == null) allPages = pageBLL.GetAllPages();
            var tree = allPages.GetTree(p => p.ParentID > -1, p => p.PName, p => p.PID.ToString(), p => p.Childs);
            List_AllPages.DataSource = tree.ToListItems(true, " > ", "");
            List_AllPages.DataTextField = "Text";
            List_AllPages.DataValueField = "Value";
            List_AllPages.DataBind();

            return List_AllPages;
        }
        /// <summary>
        /// 绑定隐藏页面列表
        /// </summary>
        private Control BindHidePages() {
            rep_HidePages.DataSource = pageBLL.GetAllHideList();
            rep_HidePages.DataBind();
            return rep_HidePages;
        }

        /// <summary>
        /// 判断要改变的父级ID是否在自己的子集里
        /// </summary>
        /// <param name="oldPID"></param>
        /// <param name="changePID"></param>
        /// <returns></returns>
        private bool CheckPIDInChilds(int oldPID, int changePID) {
            //自己也算
            if (oldPID == changePID) return true;

            var allPages = pageBLL.GetAllPages();
            MR_PageInfo oldPInfo = null;
            bool isEnd;
            return CheckPIDInChildsByPages(oldPID, ref oldPInfo, changePID, allPages, out isEnd);
        }
        /// <summary>
        /// 判断要改变的父级ID是否在自己的子集里---递归处理方法
        /// </summary>
        /// <param name="oldPID"></param>
        /// <param name="oldPInfo"></param>
        /// <param name="changePID"></param>
        /// <param name="pages"></param>
        /// <returns></returns>
        private bool CheckPIDInChildsByPages(int oldPID, ref MR_PageInfo oldPInfo, int changePID, List<MR_PageInfo> pages, out bool isEnd) {
            foreach (var page in pages) {
                if (page.PID == oldPID) {
                    //找到自己,那么只从自己的子集找起,结果直接返回最外层
                    isEnd = true;
                    oldPInfo = page;
                    if (page.Childs == null && page.Childs.Count <= 0) {
                        return false;
                    }
                    return CheckPIDInChildsByPages(oldPID, ref oldPInfo, changePID, page.Childs, out isEnd);
                }
                if (page.PID == changePID) {
                    //找到目标,如果自己还没找到算通过,否则
                    isEnd = true;
                    if (oldPInfo == null) return false;
                    else return true;
                }
                //递归,先序遍历,如果里面找到,那么就有,没找到,继续遍历其他的
                if (page.Childs != null && page.Childs.Count <= 0) {
                    var re = CheckPIDInChildsByPages(oldPID, ref oldPInfo, changePID, page.Childs, out isEnd);
                    //里面说不用找了,直接返回结果
                    if (isEnd) return re;
                    //里面找到了,直接返回结果
                    if (re) { isEnd = true; return true; }
                }
            }
            //如果都没找到,则返回不在里面
            isEnd = false;
            return false;
        }

        /// <summary>
        /// 递归绑定树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rp_PageItem_ItemDataBound(object sender, RepeaterItemEventArgs e) {
            if (e.Item == null || e.Item.DataItem == null) return;
            var page = e.Item.DataItem as MR_PageInfo;
            var DivChilds = e.Item.FindControl("DivChilds") as HtmlGenericControl;
            if (page == null || DivChilds == null) return;

            Repeater rp = new Repeater();
            DivChilds.Controls.Add(rp);
            rp.ID = "rp_PageItem";
            rp.ItemDataBound += new RepeaterItemEventHandler(rp_PageItem_ItemDataBound);
            rp.ItemTemplate = rp_PageItem.ItemTemplate;
            rp.DataSource = page.Childs;
            rp.DataBind();
        }

        /// <summary>
        /// 按格式化获取页面URL 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formatStr"></param>
        /// <returns></returns>
        protected string GetPagURL(object url, string formatStr) {
            string urlStr = url.GetString(string.Empty);
            if (urlStr.IsEmpty()) return string.Empty;
            return string.Format(formatStr, urlStr);
        }
        #endregion
    }


    public class CheckNewMgrPageItem {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICheckNewMgrPages Impl { get; set; }
    }

    public class AjaxHTMLData {
        public string PageTreeListHTML { get; set; }
        public string PageTreeDrListHTML { get; set; }
        public string HidePagesListHTML { get; set; }
        public string ChkNewPagesListHTML { get; set; }
    }

}