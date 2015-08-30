using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    /// <summary>
    /// 组 编辑
    /// </summary>
    public interface IGroupManage {
        /// <summary>
        /// 取权限组列表
        /// </summary>
        IList<MR_Group> GetList();

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="groupId">要删除的组ID</param>
        bool DeleteGroup(int groupId);

        /// <summary>
        /// 添加或更新权限组基本信息，返回组ID
        /// </summary>
        /// <param name="groupName">名称</param>
        /// <param name="groupId">要修改的组ID，0表示添加。</param>
        bool AddUpdate(string groupName, int groupId);

        /// <summary>
        /// 取得本组的成员
        /// <param name="groupID">用户组ID</param>
        /// </summary>
        System.Collections.Generic.IList<MR_Admin> GetMemberList(int groupID);

        /// <summary>
        /// 取得一个权限组的信息
        /// </summary>
        /// <param name="groupId">组ID</param>
        MR_Group GetGroup(int groupId);
    }
}
