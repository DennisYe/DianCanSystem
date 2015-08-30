using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Bll {
    public class BR_PageInfo {
        private DR_PageInfo _dal = new DR_PageInfo();

        /// <summary>
        /// 检测一批URL是否都存在库中,返回不存在的列表
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> CheckPageURLs(IEnumerable<string> urls) {
            if (urls == null || urls.Count() <= 0) return new List<string>();
            return _dal.CheckPageURLs(urls);
        }

        /// <summary>
        /// 删除一节点,必须没有子显示节点,否则将删除失败
        /// </summary>
        public bool Delete(int PID, out string exStr) {
            exStr = string.Empty;
            try {
                return _dal.Delete(PID) > 0;
            } catch (Exception ex) {
                exStr = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 添加节点，返回是否成功，成功时，PID值>0
        /// </summary>
        public bool Add(MR_PageInfo model, out string exStr) {
            exStr = string.Empty;
            try {
                model.PID = _dal.Add(model);
                return model.PID > 0;
            } catch (Exception ex) {
                exStr = ex.ToString();
                return false;
            }
        }

        /// <summary>
        /// 更新页面
        /// </summary>
        public bool Update(MR_PageInfo model) {
            return _dal.Update(model);
        }

        /// <summary>
        /// 根据url完整匹配得到model
        /// <param name="urlPath">页面URL完整匹配</param>
        /// </summary>
        public MR_PageInfo GetModel(string urlPath) {
            return _dal.GetModel(urlPath);
        }
        /// <summary>
        /// 得到
        /// <param name="urlPath">页面相对路径</param>
        /// </summary>
        public MR_PageInfo GetModel(int PID) {
            return _dal.GetModel(PID);
        }
        /// <summary>
        /// 获得所有,单纯的列表
        /// </summary>
        public IList<Model.MR_PageInfo> GetAllList() {
            return _dal.GetList();
        }
        /// <summary>
        /// 获得所有隐藏页面,单纯的列表
        /// </summary>
        public IList<Model.MR_PageInfo> GetAllHideList() {
            return _dal.GetList(null, true);
        }

        /// <summary>
        /// 获取所有子列表(包含隐藏页面),会移除已处理的页面对象
        /// </summary>
        /// <param name="allPages"></param>
        /// <param name="allHidePages"></param>
        /// <param name="parentPageId"></param>
        /// <returns></returns>
        protected List<MR_PageInfo> GetAllChilds(IList<MR_PageInfo> allPages, IList<MR_PageInfo> allHidePages, int parentPageId) {
            List<MR_PageInfo> list = new List<MR_PageInfo>();

            //先从所有中取到
            for (var i = 0; i < allPages.Count; i++) {
                var p = allPages[i];
                if (p.ParentID == parentPageId) {
                    list.Add(p);
                    allPages.RemoveAt(i);
                    i--;
                }
            }
            //处理每项
            foreach (var p in list) {
                //得到每项的子显示页面
                p.Childs = GetAllChilds(allPages, allHidePages, p.PID);
                //得到每项的子隐藏页面,一起并到子页面列表
                for (var i = 0; i < allHidePages.Count; i++) {
                    var h = allHidePages[i];
                    if (h.HideParentID == p.PID) {
                        p.Childs.Add(h);
                        allHidePages.RemoveAt(i);
                        i--;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有页面,带父子级结构,包含隐藏页面
        /// </summary>
        /// <returns></returns>
        public List<MR_PageInfo> GetAllPages() {
            //不包含隐藏页面的所有页面
            IList<MR_PageInfo> allPages = _dal.GetList(false, null);
            //所有隐藏页面
            IList<MR_PageInfo> allHidePages = new DR_PageParent().GetList();
            return GetAllChilds(allPages, allHidePages, 0);
        }


        /// <summary>
        /// 检测是否存在URL
        /// <param name="PID">页面ID</param>
        /// <param name="PID">如果是update,那么需要传入页面ID,是insert则传入0</param>
        /// </summary>
        public bool ExistURL(string url, int PID) {
            return _dal.ExistURL(url, PID);
        }
    }
}
