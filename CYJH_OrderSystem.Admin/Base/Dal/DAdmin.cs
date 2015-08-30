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
    internal class DAdmin {

        /// <summary>
        /// 根据用户名和密码获取一个管理员信息实体
        /// <param name="aname">用户名</param>
        /// <param name="apwd">密码（MD5加密）</param>
        /// </summary>
        public MR_Admin GetModel(string aname, string apwd, string ip) {
            Safe.Base.Contract.IDbHelper dbHelper = SQLHelpers.TcAdmin();
            try {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select  top 1 AID,AName,ANickName,IP,Email,R_Admin.GID ,R_Admin.ALastTime from R_Admin ");
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

        /// <summary>
        /// 根据用户名获取一个管理员信息实体
        /// <param name="aname">用户名</param>
        /// </summary>
        public MR_Admin GetModel(string aname, string ip) {
            Safe.Base.Contract.IDbHelper dbHelper = SQLHelpers.TcAdmin();
            try {
                StringBuilder strSql = new StringBuilder();

                strSql.Append("select  top 1 AID,AName,ANickName,IP,Email,R_Admin.GID ,R_Admin.ALastTime from R_Admin ");
                strSql.Append(" where AName=@AName ");
                SqlParameter[] parameters = {
					new SqlParameter("@AName",aname)
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
    }
}
