using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using CYJH_OrderSystem.Admin.Base.Dal;
using System.Data;
using System.Web;

namespace CYJH_OrderSystem.Admin.Base.BLL {
    /// <summary>
    /// 管理员操作类，实现了IAdminManage接口
    /// </summary>
    internal class AdminManage : IAdminManage, IRoleCheck, IRightsSetting, ICheckUserServer {

        #region IRightsSetting 成员
        /// <summary>
        /// 将一个目录的访问权限赋给对象
        /// </summary>
        /// <param name="pageId">页面ID</param>
        /// <param name="forId">要复制到的对象ID</param>
        /// <param name="btnRights">按钮权限</param>
        /// <param name="updateWhenExists">对象已经有权限时，是否更新</param>
        public bool AddRole(int pageId, IList<int> forId, string btnRights, bool updateWhenExists) {
            int[] adminIDsInt = forId.ToArray();
            if (adminIDsInt.Length > 0) {
                int result = new DR_AdminRight().AddOrUpdate(pageId, btnRights, updateWhenExists, adminIDsInt);
                if (result == 0)
                    return false;
                return true;
            }
            return false;
        }

        public bool UpdateBtnExp(IList<int> foridList, Dictionary<int, string> rolsSettings) {
            new DR_AdminRight().UpdateRights(foridList.ToArray(), rolsSettings);
            return true;
        }

        /// <summary>
        /// 移除对管理
        /// </summary>
        /// <param name="foridList"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public bool RemoveRole(IList<int> foridList, int pid) {
            new DR_AdminRight().Delete(pid, foridList);
            return true;
        }

        #endregion

        #region IMenusGetting 成员

        /// <summary>
        /// 取管理在某个菜单下的子菜单
        /// </summary>
        /// <param name="forId"></param>
        /// <param name="parentID"></param>
        /// <param name="includeChild"></param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetChildMenuToList(int forId, int parentID, bool includeChild) {
            if (forId <= 0)
                return null;
            return new DR_AdminRight().GetMenusToList(forId, parentID, includeChild, false);
        }

        #endregion

        #region IAdminManage 成员

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="aname"></param>
        /// <param name="apwd"></param>
        /// <param name="anickname"></param>
        /// <param name="email"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public int Add(string aname, string apwd, string anickname, string email, int gid) {
            MR_Admin model = new MR_Admin();
            model.AName = aname;
            model.APwd = apwd.MD5();
            model.ANickName = anickname;
            model.Email = email;
            model.GID = gid;
            return new DR_Admin().Add(model);
        }

        /// <summary>
        /// 更新管理员基本信息
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="email"></param>
        /// <param name="adminID"></param>
        /// <returns></returns>
        public bool UpdateBaseInfo(string nickName, string email, int adminID) {
            return new DR_Admin().UpdateBaseInfo(nickName, email, adminID) != 0;
        }

        /// <summary>
        /// 更新预设组信息
        /// </summary>
        /// <param name="newGroupId"></param>
        /// <param name="adminID"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public bool UpdateGroup(int newGroupId, int adminID, CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdateGroupArg arg) {
            return new DR_Admin().UpdateGroup(adminID, newGroupId, (int)arg);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="deleteId"></param>
        /// <returns></returns>
        public bool Delete(int adminId, int deleteId) {
            if (adminId <= 0 || deleteId <= 0)
                return false;
            int result = new DR_Admin().Delete(adminId, deleteId);
            return result == 1;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public string ResetPwd(int adminId) {
            if (adminId <= 0)
                return "";
            string newPwd = "ABC123456";
            int result = new DR_Admin().ResetPwd(adminId, newPwd.MD5());
            if (result == 0)
                return "";
            else
                return newPwd;
        }

        /// <summary>
        /// 修改二级密码
        /// </summary>
        /// <param name="adminID"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn UpdatePwd(int adminID, string oldPwd, string newPwd) {
            if (adminID <= 0)
                return CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn.Error;
            int result = new DR_Admin().UpdatePwd(adminID, oldPwd.MD5(), newPwd.MD5());
            return (CYJH_OrderSystem.Admin.Base.Contract.Enums.EUpdatePwdReturn)result;
        }

        /// <summary>
        /// 取管理员列表
        /// </summary>
        /// <returns></returns>
        public IList<MR_Admin> GetList() {
            return new DR_Admin().GetList();
        }
        public DataTable GetDTList() {
            return new DR_Admin().GetDTList();
        }

        /// <summary>
        /// 根据ID取信息
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public MR_Admin GetAdmin(int adminId) {
            if (adminId <= 0)
                return null;
            return new DR_Admin().GetModel(adminId);
        }

        /// <summary>
        /// 判断密码是否正确（确认结算）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool IsExists_PageSecPwd(int id, string pwd) {
            return new DR_Admin().IsExists_PageSecPwd(id, pwd.MD5());
        }

        /// <summary>
        /// 根据管理员名字获取其ID
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        public int GetIdByAdminName(string adminName) {
            return new DR_Admin().GetIdByAdminName(adminName);
        }


        #endregion

        #region IRoleCheck 成员

        /// <summary>
        /// 检查管理员是否有访问某个页面的权限 
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="url"></param>
        /// <param name="btnRights"></param>
        /// <returns></returns>
        public bool IsInRole(int adminId, string url, string superAdminRole, bool updateClickTime, out string btnRights) {
            if (adminId <= 0) {
                btnRights = string.Empty;
                return false;
            }
            if (url == string.Empty) {
                btnRights = string.Empty;
                return false;
            }
            int pageid = GetPageId(url);
            return new DR_AdminRight().IsInRoles(adminId, pageid, superAdminRole, updateClickTime, out btnRights);
        }

        /// <summary>
        /// 管理员登录后台
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <param name="userPwdMD5">密码密文</param>
        /// <param name="userIp">用户IP</param>
        /// <param name="model">输出值，当前登录用户的基本信息</param>
        /// <returns></returns>
        public bool Login(string userName, string userPwdMD5, string userIp, out MR_Admin model) {
            model = new DR_Admin().GetModel(userName, userPwdMD5, userIp);
            if (model == null || model.AID < 0)
                return false;
            return true;
        }

        /// <summary>
        /// 从缓存中取页面ID，如果缓存不存在，会先填充
        /// </summary>
        /// <param name="pageurl"></param>
        /// <returns></returns>
        public int GetPageId(string pageurl) {
            pageurl = pageurl.ToLower();
            if (pageurl.IndexOf("rnd=") > 0) {
                pageurl = pageurl.Substring(0, pageurl.IndexOf("rnd=") - 1);
            }
            string key = "TMPPAGEINFO";

            string linkkey = key + pageurl;
            if (HttpContext.Current.Cache[linkkey] != null) {
                return HttpContext.Current.Cache[linkkey].GetInt(0, false);
            }

            Dictionary<string, int> dic;
            if (HttpContext.Current.Cache[key] == null) {
                dic = new Dictionary<string, int>();
                IList<MR_PageInfo> pinfo = new DR_PageInfo().GetList();
                foreach (MR_PageInfo info in pinfo) {
                    if (!string.IsNullOrEmpty(info.PUrl)) {
                        if (!dic.ContainsKey(info.PUrl.ToLower())) {
                            dic.Add(info.PUrl.ToLower(), info.PID);
                        }
                    }
                }
                HttpContext.Current.Cache.Insert(key, dic, null, DateTime.UtcNow.AddHours(1), TimeSpan.Zero);
            } else {
                dic = (Dictionary<string, int>)HttpContext.Current.Cache[key];
            }
            //找到库中匹配的
            IEnumerable<string> res = dic.Keys.Where(u => pageurl.StartsWith(u));
            string dbPageUrl = string.Empty;
            //匹配中url最长的
            foreach (string tmp in res) {
                if (tmp.Length > dbPageUrl.Length)
                    dbPageUrl = tmp;
            }
            if (!string.IsNullOrEmpty(dbPageUrl)) {
                int pageid = dic[dbPageUrl];
                HttpContext.Current.Cache.Insert(linkkey, pageid);
                return pageid;
            };
            return 0;
        }


        /// <summary>
        /// 取子菜单
        /// </summary>
        /// <param name="forId">管理员ID</param>
        /// <param name="parentID">父菜单ID</param>
        /// <param name="includeChild">是否同时取子菜单的子菜单</param>
        /// <param name="upClickTime">是否同时更新点击量</param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetChildMenuToList(int forId, int parentID, bool includeChild, bool upClickTime) {
            if (forId <= 0)
                return null;
            return new DR_AdminRight().GetMenusToList(forId, parentID, includeChild, upClickTime);
        }
        /// <summary>
        /// 取子菜单
        /// </summary>
        /// <param name="forId">管理员ID</param>
        /// <param name="parentID">父菜单ID</param>
        /// <param name="includeChild">是否同时取子菜单的子菜单</param>
        /// <param name="upClickTime">是否同时更新点击量</param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetChildMenu(int forId, int parentID, bool includeChild, bool upClickTime) {
            if (forId <= 0)
                return null;
            return new DR_AdminRight().GetMenus(forId, parentID, includeChild, upClickTime);
        }

        #endregion



        #region IAdminManage 成员


        public string GetNickNameByName(string aName) {
            return new DR_Admin().GetNickNameByName(aName);
        }

        #endregion

        #region ICheckUserServer 成员

        public bool IsExistsUser(string userName) {
            return new DR_Admin().Exists(userName);
        }

        #endregion

        #region IAdminManage 成员


        public IList<MR_Admin> GetListByNickName(string nickName) {
            return new DR_Admin().GetListByNickName(nickName);
        }

        #endregion
    }
}