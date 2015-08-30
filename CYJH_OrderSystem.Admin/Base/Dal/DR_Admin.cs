using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using Safe.Base.Utility;

namespace CYJH_OrderSystem.Admin.Base.Dal {
    /// <summary>
    /// 管理员数据库操作类
    /// </summary>
    internal class DR_Admin {
        public DR_Admin() {
        }

        #region  成员方法
        /// <summary>
        /// 是否管理员信息
        /// </summary>
        /// <param name="AID">管理员ID</param>
        public bool Exists(int AID) {
            string cmdText = "select 1 from R_Admin where AID=@AID";
            SqlParameter[] parameters = {
					new SqlParameter("@AID", AID)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 是否管理员信息
        /// </summary>
        /// <param name="AID">管理员ID</param>
        public bool Exists(string AName) {
            string cmdText = "select 1 from R_Admin where AName=@AName";
            SqlParameter[] parameters = {
					new SqlParameter("@AName", AName)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 增加管理员信息
        /// 返回新增加的标识列,如果增加失败返回0
        /// </summary>
        public int Add(MR_Admin model) {
            string pname = "p_AddAdmin";
            SqlParameter[] parameters = new SqlParameter[]{
					new SqlParameter("@AName", model.AName),
                    new SqlParameter("@APwd", model.APwd),
                    new SqlParameter("@ANickName", model.ANickName),
                    new SqlParameter("@Email", model.Email),
                    new SqlParameter("@GID",model.GID)
            };

            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(pname, CommandType.StoredProcedure, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新管理员信息，返回受影响的行数
        /// 更新失败，返回0
        /// </summary>
        public int UpdateBaseInfo(string nickName, string email, int aid) {
            StringBuilder cmdText = new StringBuilder("update R_Admin set ANickName=@ANickName,Email=@Email where AID=@AID;");
            SqlParameter[] parameters = {
					new SqlParameter("@AID",aid),
					new SqlParameter("@ANickName",nickName ),
					new SqlParameter("@Email", email )
            };
            return SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText.ToString(), parameters);
        }

        /// <summary>
        /// 重置管理员密码
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="newPwd">MD5加密过的新密码</param>
        /// <returns></returns>
        public int ResetPwd(int adminId, string newPwd) {
            string cmdText = "update R_Admin set APwd=@APwd where AID=@AID";
            SqlParameter[] parameters = {
					new SqlParameter("@AID", adminId),
					new SqlParameter("@APwd",newPwd)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 修改密码 
        /// </summary>
        /// <param name="adminID">管理员ID</param>
        /// <param name="oldPwdMD5">旧密码（MD5加密）</param>
        /// <param name="newPwdMD5">新密码（MD5加密）</param>
        /// <returns>
        /// -1：旧密码不正确
        ///  1：修改成功
        /// </returns>
        public int UpdatePwd(int adminID, string oldPwdMD5, string newPwdMD5) {
            IDataParameter[] ipas =  {
                new SqlParameter("@adminID", adminID ),
                new SqlParameter("@oldPwdMD5",oldPwdMD5 ),
                new SqlParameter("@newPwdMD5", newPwdMD5 )
                                    };
            return SQLHelpers.TcAdmin().ExecuteProc("P_UPDATE_AdminPWD", false, ipas).ReturnValue;
        }

        /// <summary>
        /// 重新设置预设组
        /// </summary>
        /// <param name="adminID">管理员ID</param>
        /// <param name="newGroupid">新的组</param>
        /// <param name="updateRights">是否保留预设的权限, 
        /// 0 不调整页面访问权限 
        /// 1 更新页面访问权限为新组的 
        /// 2 保留旧的权限，同时追加新用户组的权限
        /// </param> 
        public bool UpdateGroup(int adminID, int newGroupid, int updateRights) {
            IDataParameter[] ipas =  {
                new SqlParameter("@adminID", adminID ),
                new SqlParameter("@newGroupID",newGroupid ),
                new SqlParameter("@updateRights", updateRights )
                                    };
            SQLHelpers.TcAdmin().ExecuteProc("P_UPDATE_AdminGroup", false, ipas);
            return true;
        }

        /// <summary>
        /// 删除用户
        /// 只有超级管理员才可以删除用户
        /// <param name="AID">管理员ID</param>
        /// <para param name="delID">要删除的用户ID</para>
        /// </summary>
        public int Delete(int AID, int delID) {
            string pname = "p_DeleteAdmin";// -1 只有管理员可以删除  0 删除失败 1 删除成功
            SqlParameter[] parameters = {
					new SqlParameter("@aid", AID),
                    new SqlParameter("@delId",delID)};
            return SQLHelpers.TcAdmin().ExecuteProc(pname, false, parameters).ReturnValue;
        }


        /// <summary>
        /// 得到一个管理员信息实体
        /// <param name="AID">管理员ID</param>
        /// </summary>
        public MR_Admin GetModel(int AID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 AID,AName,ANickName,IP,Email,R_Admin.GID,R_Admin.ALastTime ,case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
            strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
            strSql.Append(" where AID=@AID ");
            SqlParameter[] parameters = {
					new SqlParameter("@AID",AID)};
            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(strSql.ToString(), parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }


        /// <summary>
        /// 得到一个管理员信息实体
        /// <param name="AName">管理员登录名</param>
        /// </summary>
        public MR_Admin GetModelForSign(string AName) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 AID,AName,ANickName,IP,Email,R_Admin.GID,R_Admin.ALastTime ,case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
            strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
            strSql.Append(" where R_Admin.AName=@AName ");
            SqlParameter[] parameters = {
					new SqlParameter("@AName",AName)};
            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(strSql.ToString(), parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名和密码获取一个管理员信息实体
        /// <param name="aname">用户名</param>
        /// <param name="apwd">密码（MD5加密）</param>
        /// </summary>
        public MR_Admin GetModel(string aname, string apwd, string ip) {
            Safe.Base.Contract.IDbHelper dbHelper = SQLHelpers.TcAdmin();
            try {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select  top 1 AID,AName,ANickName,IP,Email,R_Admin.GID ,R_Admin.ALastTime ,case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
                strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
                strSql.Append(" where AName=@AName and APwd=@APwd ");
                SqlParameter[] parameters = {
					new SqlParameter("@AName",aname),
                    new SqlParameter("@APwd",apwd)
            };
                DataTable dtbl = dbHelper.ExecuteFillDataTable(strSql.ToString(), parameters);
                if (dtbl.Rows.Count > 0) {
                    MR_Admin result = Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModel(dtbl.Rows[0]);

                    dbHelper.ExecuteNonQuery("UPDATE R_Admin SET ALastTime=getdate() , IP=@ip Where AID=@aid", new SqlParameter("@aid", result.AID), new SqlParameter("@ip", ip));

                    return result;
                } else {
                    return null;
                }
            } finally {
                dbHelper.EndConnection();
            }
        }

        ///// <summary>
        ///// 获得管理员信息列表
        ///// </summary>
        //public IList<MR_Admin> GetList() {
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select AID,AName,ANickName,IP,Email ,R_Admin.ALastTime ,R_Admin.GID,  case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
        //    strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
        //    strSql.Append(" order by R_Admin.GID asc");
        //    DataTable tbl = SQLHelpers.TcFAQ().ExecuteFillDataTable(strSql.ToString());
        //    return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModels(tbl);
        //}
        /// <summary>
        /// 获得管理员信息列表
        /// </summary>
        public IList<MR_Admin> GetList() {
            return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModels(GetDTList());
        }

        public DataTable GetDTList() {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AID,AName,ANickName,IP,Email ,R_Admin.ALastTime ,R_Admin.GID,  case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
            strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
            strSql.Append(" order by R_Admin.GID asc");
            return SQLHelpers.TcAdmin().ExecuteFillDataTable(strSql.ToString());
        }

        /// <summary>
        /// 获得管理员信息列表
        /// </summary>
        public IList<MR_Admin> GetListByNickName(string nickName) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AID,AName,ANickName,IP,Email ,R_Admin.ALastTime ,R_Admin.GID,  case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
            strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
            strSql.Append(" Where ANickName like '%" + nickName + "%'");
            strSql.Append(" order by R_Admin.GID asc");
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(strSql.ToString());
            return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModels(tbl);
        }

        /// <summary>
        /// 获得同组管理员信息列表
        /// </summary>
        /// <param name="groupID">用户组ID</param>
        public IList<MR_Admin> GetList(int groupID) {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AID,AName,ANickName,IP,Email,R_Admin.GID,R_Admin.ALastTime, case when R_Admin.GID=-1 then '超级管理员' else R_Group.GName end as GName from R_Admin ");
            strSql.Append("left join R_Group on R_Admin.GID = R_Group.GID");
            strSql.Append(" where R_Admin.GID=@GID");
            strSql.Append(" order by R_Admin.GID asc");
            SqlParameter[] sqlparams = new SqlParameter[] { new SqlParameter("@GID", groupID) };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(strSql.ToString(), sqlparams);
            return Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModels(tbl);
        }

        /// <summary>
        /// 修改管理员IP
        /// </summary>
        /// <param name="aid">管理员ID</param>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public int ModifyIp(int aid, string ip) {
            string cmdText = "UPDATE [R_Admin] SET [IP] = @IP WHERE AID=@AID";
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@IP",ip),
                new SqlParameter("@AID",aid)
            };
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, sqlparams);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 修改管理员最后登录时间
        /// </summary>
        /// <param name="aid">管理员ID</param>
        /// <param name="ip">最后登录时间</param>
        /// <returns></returns>
        public int ModifyLastTime(int aid, DateTime time) {
            string cmdText = "UPDATE [R_Admin] SET [ALastTime] = @ALastTime WHERE AID=@AID";
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@ALastTime",time),
                new SqlParameter("@AID",aid)
            };
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, sqlparams);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 判断是否存在此密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool IsExists_PageSecPwd(int id, string pwd) {
            string sql = string.Empty;
            sql = "select * from SecondPwd where id=@id and pwd=@pwd";
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@id",id),
                new SqlParameter("@pwd",pwd)
            };

            return SQLHelpers.TcAdmin().Exists(sql, sqlparams);
        }

        /// <summary>
        /// 根据管理员名字获取其ID
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        public int GetIdByAdminName(string adminName) {
            string sql = "select aid from R_Admin where AName=@AName";
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@AName",adminName)
            };

            IDataReader reader = null;
            try {
                reader = SQLHelpers.TcAdmin().ExecuteReader(sql, parameters);
                if (reader.Read()) {
                    MR_Admin mr_admin = Safe.Base.Utility.ModelConvertHelper<MR_Admin>.ToModel(reader);
                    return mr_admin.AID;
                } else {
                    return 0;
                }
            } finally {
                if (reader != null) {
                    reader.Close();
                }
            }
        }

        /// <summary>
        /// 根据管理员登录名取得昵称
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns></returns>
        public string GetNickNameByName(string adminName) {
            string sql = "select top 1 ANickName from R_Admin where AName=@AName";
            return SQLHelpers.TcAdmin().ExecuteScalar(sql, new SqlParameter("@AName", adminName)).GetString(adminName);
        }
        #endregion  成员方法
    }
}