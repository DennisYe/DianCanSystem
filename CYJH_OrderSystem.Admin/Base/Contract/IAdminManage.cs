using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    /// <summary>
    /// 管理员 编辑
    /// </summary>
    public interface IAdminManage {

        /// <summary>
        /// 添加用户信息，返回用户ID
        /// </summary>
        /// <param name="text">用户信息，其中最后登录时间和最后登录IP不需要填写</param>
        /// <returns>adminId</returns>
        int Add(string aname, string apwd, string anickname, string email, int gid);

        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="adminID"></param>
        /// <returns></returns>
        bool UpdateBaseInfo(string nickName, string email, int adminID);

        /// <summary>
        /// 更新用户所属组
        /// </summary>
        /// <param name="newGroupId"></param>
        /// <param name="adminID"></param>
        /// <returns></returns>
        bool UpdateGroup(int newGroupId, int adminID, Enums.EUpdateGroupArg arg);

        /// <summary>
        /// 删除用户，返回受影响的行数
        /// </summary>
        /// <remarks>只有超级管理员可以删除用户</remarks>
        /// <param name="adminId">管理员ID</param>
        /// <param name="deleteId">要删除的用户ID</param>
        bool Delete(int adminId, int deleteId);

        /// <summary>
        /// 重置密码（超级管理员为其他管理员重重置密码）,返回新密码
        /// </summary>
        /// <param name="adminId">管理员ID</param> 
        string ResetPwd(int adminId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="adminID">管理员ID</param>
        /// <param name="oldPwd">旧密码明文</param>
        /// <param name="newPwd">新密码明文</param>
        /// <returns></returns>
        Enums.EUpdatePwdReturn UpdatePwd(int adminID, string oldPwd, string newPwd);

        /// <summary>
        /// 取得管理员列表
        /// </summary>
        System.Collections.Generic.IList<Model.MR_Admin> GetList();
        /// <summary>
        /// 取得管理员列表
        /// </summary>
        System.Data.DataTable GetDTList();

        /// <summary>
        /// 根据昵称取管理员列表
        /// </summary>
        /// <returns></returns>
        System.Collections.Generic.IList<Model.MR_Admin> GetListByNickName(string nickName);

        /// <summary>
        /// 取得一个管理员的基本信息
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        Model.MR_Admin GetAdmin(int adminId);

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool IsExists_PageSecPwd(int id, string pwd);

        /// <summary>
        /// 根据管理员名字获取其ID
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        int GetIdByAdminName(string adminName);

        /// <summary>
        /// 根据管理员的名字来获取管理者的昵称
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string GetNickNameByName(string aName);

    }
}
