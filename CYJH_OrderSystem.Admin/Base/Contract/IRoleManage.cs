using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    /// <remarks>目录权限设置</remarks>
    public interface IRoleManage {
        /// <summary>
        /// 添加一个菜单节点，返回节点ID
        /// </summary>
        /// <param name="text">节点文字</param>
        /// <param name="url">节点链接</param>
        /// <param name="parentId">父节点ID，根目录ID为0</param>
        bool AddNode(string text, string url, int parentId, int queue);

        /// <summary>
        /// 删除一个节点，及其子节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        bool Delete(int nodeId);

        /// <summary>
        /// 取目录列表
        /// </summary>
        System.Collections.Generic.IList<Model.MR_PageInfo> GetList();

        /// <summary>
        /// 更新一个目录项
        /// </summary>
        /// <param name="text">说明文字</param>
        /// <param name="url">链接地址</param>
        /// <param name="nodeId">节点ID</param>
        bool UpdateNode(string text, string url, int nodeId, int queue, bool defShowChild);


        /// <summary>
        /// 取一个目录下的隐藏目录
        /// </summary>
        /// <param name="nodeID">页面ID</param>
        IList<Model.MR_PageInfo> GetHidePage(int nodeID);

        /// <summary>
        /// 取目录列表
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <param name="includeHide">是否从同时取出该页面下的隐藏目录（parentID=-1 时，此选项无效）</param>
        /// <returns></returns>
        System.Collections.Generic.IList<Model.MR_PageInfo> GetListToList(int parentId, bool includeHide);
        /// <summary>
        /// 取目录列表
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <param name="includeHide">是否从同时取出该页面下的隐藏目录（parentID=-1 时，此选项无效）</param>
        /// <returns></returns>
        System.Collections.Generic.IList<Model.MR_PageInfo> GetList(int parentId, bool includeHide, bool includeChild);


        /// <summary>
        /// 根据ID获取节点
        /// </summary>
        /// <param name="nodeID">节点ID</param>
        /// <returns></returns>
        Model.MR_PageInfo GetNode(int nodeID);
        /// <summary>
        /// 批量更新权限
        /// </summary>
        /// <param name="aidsStr">管理员ID字符串，以“,”隔开</param>
        /// <param name="info">权限集合</param>
        bool UpdateRights(string aidsStr, Dictionary<int, string> info);


    }
}
