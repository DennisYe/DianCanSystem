using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.BLL;

namespace CYJH_OrderSystem.Admin.Base.Factorys {

    [Serializable]
    public enum ERightType {
        Admin = 0,
        Group = 1,
        Unknown = -1
    }

    public static class GetInterface {

        public static IAdmin GetIAdmin() {
            return new BAdmin();
        }
        /// <summary>
        /// 取得IAdminManage 实例 
        /// </summary>
        /// <returns></returns>
        public static IAdminManage GetIAdminManage() {
            return new AdminManage();
        }


        /// <summary>
        /// 取得IGroupManage 实例 
        /// </summary>
        /// <returns></returns>
        public static IGroupManage GetIGroupManage() {
            return new GroupManage();
        }

        /// <summary>
        /// 取得IMenusGetting实例
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public static IMenusGetting GetIMenusGetting(ERightType er) {
            switch (er) {
                case ERightType.Admin:
                    return new AdminManage();
                case ERightType.Group:
                    return new GroupManage();
            }
            return null;
        }

        /// <summary>
        /// 取得IRightsSetting实例
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public static IRightsSetting GetIRightsSetting(ERightType er) {
            switch (er) {
                case ERightType.Admin:
                    return new AdminManage();
                case ERightType.Group:
                    return new GroupManage();
            }
            return null;
        }

        /// <summary>
        /// 取得IRoleCheck 实例
        /// </summary>
        /// <returns></returns>
        public static IRoleCheck GetIRoleCheck() {
            return new AdminManage();
        }

        /// <summary>
        /// 取得IRoleManage 实例 
        /// </summary>
        /// <returns></returns>
        public static IRoleManage GetIRoleManage() {
            return new RoleManage();
        }

        /// <summary>
        /// 取得IPageParent 实例
        /// </summary>
        /// <returns></returns>
        public static IPageParent GetIPageParent() {
            return new PageParent();
        }

        public static ICheckUserServer GetICheckUserServer() {
            return new AdminManage();
        }
    }
}
