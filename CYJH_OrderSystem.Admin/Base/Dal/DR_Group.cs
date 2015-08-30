using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Shared;
using CYJH_OrderSystem.Admin.Base.Model;
using Safe.Base.Utility;

namespace CYJH_OrderSystem.Admin.Base.Dal {
    /// <summary>
    /// 用户组数据库操作
    /// </summary>
    internal class DR_Group {
        #region  成员方法
        /// <summary>
        /// 是否存在该权限组
        /// </summary>
        public bool Exists(int GID) {
            string cmdText = "select 1 from R_Group where GID=@GID";
            SqlParameter[] parameters = {
					new SqlParameter("@GID", GID)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }


        /// <summary>
        /// 增加权限组， 返回新增加的标识列

        /// 增加失败返回0
        /// <param name="gname">用户组名</param>
        /// </summary>
        public int Add(string gname) {
            string cmdText = "insert into R_Group(GName) values (@GName);select @@IDENTITY";
            SqlParameter[] parameters = {
					new SqlParameter("@GName", gname)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新用户组名称

        /// 返回受影响的行数，更新失败返回0
        /// <param name="gname">用户组名</param>
        /// </summary>
        public int Update(int gid, string gname) {
            string cmdText = "update R_Group set GName=@GName where GID=@GID";
            SqlParameter[] parameters = {
					new SqlParameter("@GID", gid),
					new SqlParameter("@GName", gname)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 删除权限组

        /// 返回受影响的行数，删除失败返回0
        /// <param name="GID">用户组ID</param>
        /// </summary>
        public int Delete(int GID) {
            string cmdText = "delete from R_Group where GID=@GID ";
            SqlParameter[] parameters = {
					new SqlParameter("@GID",GID)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 取得组信息
        /// </summary>
        /// <param name="groupID">组ID</param>
        /// <returns></returns>
        public MR_Group GetModel(int groupID) {
            string cmdText = "select GID,GName FROM R_Group where gid=@GID";
            SqlParameter[] sqlparams = new SqlParameter[] { new SqlParameter("@GID", groupID) };

            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText, sqlparams);
            return Safe.Base.Utility.ModelConvertHelper<MR_Group>.ToModel(tbl.Rows[0]);
        }

        /// <summary>
        /// 获得权限组列表
        /// </summary>
        public IList<MR_Group> GetList() {
            string cmdText = "select GID,GName FROM R_Group ";
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText);
            return Safe.Base.Utility.ModelConvertHelper<MR_Group>.ToModels(tbl);
        }

        #endregion  成员方法
    }
}
