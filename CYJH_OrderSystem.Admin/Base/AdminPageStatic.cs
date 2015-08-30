using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;

namespace CYJH_OrderSystem.Admin.Base {
    public static class AdminPageStatic {

        /// <summary>
        /// 将层级的页面list转化为一个list并修改名称,用"|-"来分级
        /// </summary>
        /// <param name="childList"></param>
        /// <param name="level"></param>
        /// <param name="toList"></param>
        public static void ParseMenuChild(IList<MR_PageInfo> childList, int level, ref IList<MR_PageInfo> toList) {
            foreach (var child in childList) {
                child.PName = "　".Repeat(level) + "|-" + child.PName;
                toList.Add(child);
                if (child.Childs != null) {
                    ParseMenuChild(child.Childs, level + 1, ref toList);
                }
            }
        }


        /// <summary>
        /// 获得当前登录后台的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrUserName() {
            var admin = AdminPageStatic.GetLoginUserInfo();
            return admin == null ? "" : admin.AName;
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsSAdmin() {
            var admin = AdminPageStatic.GetLoginUserInfo();
            if (admin != null && admin.GID == -1) return true;
            return false;
        }

        /// <summary>
        /// 检查校验码是否正确
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static bool CheckCheckCode(string checkCode) {
            if (HttpContext.Current.Session[AdminAppSetting.SESSION_NAME_CHECKCODE] != null) {
                return (checkCode.ToLower() == HttpContext.Current.Session[AdminAppSetting.SESSION_NAME_CHECKCODE].ToString().ToLower());
            } else {
                return false;
            }
        }




        /// <summary>
        /// 检查页面权限
        /// </summary>
        /// <param name="page"></param>
        public static void RoleChecked(Page page) {
            //Safe.Base.Role.Utility.RoleChecked(page, GetLoginUserInfo().AID.ToString(), new ImplCtrlRule(), new ImplSiteCache());
        }



        #region 凭据管理

        /// <summary>
        /// 获取登录页
        /// </summary>
        /// <returns></returns>
        public static string GetLoginURL() {
            return ZAuthentication.AuthHelper.LoginURL;
        }
        /// <summary>
        /// 获取登录后的默认页
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultURL() {
            return ZAuthentication.AuthHelper.DefaultURL;
        }

        /// <summary>
        /// 是否已登录
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin() {
            var u = ZAuthentication.AuthHelper.CurrentUser;
            if (u != null && u.Identity != null && u.Identity.IsAuthenticated) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 跳转到登录页
        /// </summary>
        /// <param name="appReturnURL"></param>
        public static void RedirectToLoginPage(bool appReturnURL) {
            ZAuthentication.AuthHelper.RedirectToLoginPage(appReturnURL);
        }

        /// <summary>
        /// 登录成功后，设置COOKIE
        /// </summary>
        /// <param name="model"></param>
        public static void SaveUserFormsCookie(MR_Admin model) {
            ZAuthentication.AuthHelper.SetUser(new ZAuthentication.UserTickModel(
                model.AName, model.GetJSON(), DateTime.Now, DateTime.Now.AddHours(3), StaticFunctions.GetUserIp()));
        }

        /// <summary>
        /// 取得登录用户的COOKIE,获取失败会自动跳到登录页
        /// </summary>
        /// <returns></returns>
        public static MR_Admin GetLoginUserInfo() {
            return GetLoginUserInfo(true);
        }
        /// <summary>
        /// 取得登录用户的COOKIE,可设置获取失败是否自动跳到登录页
        /// </summary>
        /// <returns></returns>
        public static MR_Admin GetLoginUserInfo(bool jumpToLoginPage) {
            if (IsLogin()) {
                try {
                    return ZAuthentication.AuthHelper.CurrentUser.Identity.Model.PassportString.JSONDeserialize<MR_Admin>();
                } catch { }
            }
            if (jumpToLoginPage)
                HttpContext.Current.Response.Redirect(GetLoginURL());
            return null;
        }


        /// <summary>
        /// 登出
        /// </summary>
        public static void LogOut() {
            ZAuthentication.AuthHelper.SignOut();
        }

        #endregion


    }
}
