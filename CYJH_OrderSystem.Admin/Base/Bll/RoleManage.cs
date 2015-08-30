using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;

namespace CYJH_OrderSystem.Admin.Base.BLL {
    internal class RoleManage : IRoleManage {
        #region IRoleManage 成员
        /// <summary>
        /// 添加一个菜单节点，返回节点ID
        /// </summary>
        /// <param name="text">节点文字</param>
        /// <param name="url">节点链接</param>
        /// <param name="parentId">父节点ID，根目录ID为0</param>
        public bool AddNode(string text, string url, int parentId, int queue) {
            if (text == string.Empty)
                return false;
            MR_PageInfo model = new MR_PageInfo();
            model.PName = text;
            model.ParentID = parentId;
            model.Queue = queue;
            if (url.Length > 0) {
                model.PUrl = url;
                model.IsUrl = true;
            } else {
                model.PUrl = string.Empty;
                model.IsUrl = false;
            }
            int result = new DR_PageInfo().Add(model);
            if (result > 0) {
                StaticFunctions.ClearServerCache();
                StaticFunctions.ClearClientPageCache();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除一个节点，及其子节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        public bool Delete(int nodeId) {
            int result = new DR_PageInfo().Delete(nodeId);

            return result >= 1;
        }

        /// <summary>
        /// 取得所有节点
        /// </summary>
        public IList<Model.MR_PageInfo> GetList() {
            return new DR_PageInfo().GetList();
        }

        /// <summary>
        /// 更新一个目录项
        /// </summary>
        /// <param name="text">说明文字</param>
        /// <param name="url">链接地址</param>
        /// <param name="nodeId">节点ID</param>
        public bool UpdateNode(string text, string url, int nodeId, int queue, bool defShowChild) {
            int result = new DR_PageInfo().Update(text, url, queue, nodeId, defShowChild);
            if (result == 0)
                return false;
            return true;
        }


        /// <summary>
        /// 取一个目录下的隐藏目录
        /// </summary>
        public IList<Model.MR_PageInfo> GetHidePage(int nodeID) {
            return new DR_PageInfo().GetHidePage(nodeID);
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="parentId">父节点ID</param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetListToList(int parentId, bool includeHide) {
            return new DR_PageInfo().GetChildToList(parentId, includeHide);
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="parentId">父节点ID</param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetList(int parentId, bool includeHide, bool includeChild) {
            return new DR_PageInfo().GetChild(parentId, includeHide, includeChild);
        }

        /// <summary>
        /// 根据ID，获取节点
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        /// <returns></returns>
        public MR_PageInfo GetNode(int nodeID) {
            if (nodeID <= 0)
                return null;
            return new DR_PageInfo().GetModel(nodeID);
        }

        /// <summary>
        /// 批量更新权限
        /// </summary>
        /// <param name="aids">管理员ID数组</param>
        /// <param name="info">权限集合</param>
        public bool UpdateRights(string aidsStr, Dictionary<int, string> info) {
            string[] aids = aidsStr.Split(',');
            int[] adminIDsInt = new int[aids.Length];
            if (aids.Length > 0) {
                for (int i = 0; i < aids.Length; i++) {
                    adminIDsInt[i] = Convert.ToInt32(aids[i]);
                }
                int result = new DR_AdminRight().UpdateRights(adminIDsInt, info);
                if (result == 0)
                    return false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
