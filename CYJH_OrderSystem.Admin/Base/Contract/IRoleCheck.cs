using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    /// <remarks>页面访问权限检查接口</remarks>
    public interface IRoleCheck : IMenusGetting {
        /// <summary>
        /// 检查管理员是否有目录的访问权限，同时返回对该页面按钮的访问权限表达式
        /// </summary>
        /// <remarks>当有权限访问页面时，点击次数+1</remarks>
        /// <param name="adminId">当前登录的作者</param>
        /// <param name="url">当前访问页面的URL</param>
        /// <param name="btnRights">当前页面的按钮访问权限表达式</param>
        /// <returns>
        /// true:可以访问该目录
        /// false:不可访问该目录
        /// </returns>
        bool IsInRole(int adminId, string url, string superAdminRole, bool updateClickTime, out string btnRights);

        /// <summary>
        /// 登录,返回成功与否
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userPwdMD5">登录密码密文</param>
        /// <param name="userIp">登录IP</param>
        /// <param name="model">登录成功后，会输出当前登录管理员的基本信息</param>
        /// <param name="text">管理员信息</param>
        bool Login(string userName, string userPwdMD5, string userIp, out Model.MR_Admin model);

        /// <summary>
        /// 从缓存中取页面ID，如果缓存不存在，会先填充
        /// </summary>
        /// <param name="pageurl"></param>
        /// <returns></returns>
        int GetPageId(string pageurl);


        /// <summary>
        /// 取子菜单,可选是否包含下面所有的子集,并且转换为一个list
        /// </summary>
        /// <param name="forId">管理员ID</param>
        /// <param name="parentID">父菜单ID</param>
        /// <param name="includeChild">是否同时取子菜单的子菜单</param>
        /// <param name="upClickTime">是否同时更新点击量</param>
        /// <returns></returns>
        IList<Model.MR_PageInfo> GetChildMenuToList(int forId, int parentID, bool includeChild, bool upClickTime);
        /// <summary>
        /// 取子菜单,可选是否包含下面所有的子集,子集将包含在各自model的Childs成员中
        /// </summary>
        /// <param name="forId">管理员ID</param>
        /// <param name="parentID">父菜单ID</param>
        /// <param name="includeChild">是否同时取子菜单的子菜单</param>
        /// <param name="upClickTime">是否同时更新点击量</param>
        /// <returns></returns>
        IList<Model.MR_PageInfo> GetChildMenu(int forId, int parentID, bool includeChild, bool upClickTime);
    }
}
