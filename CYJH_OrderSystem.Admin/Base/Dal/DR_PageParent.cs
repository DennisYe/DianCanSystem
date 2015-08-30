using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Dal {
    /// <summary>
    /// 隐藏页对应类
    /// </summary>
    internal class DR_PageParent {
        /// <summary>
        /// 是否存在该删除隐藏页和父页的关联
        /// <param name="pid">页面ID</param>
        /// <param name="parentid">父页面ID</param>
        /// </summary>
        public bool Exists(int PID, int ParentID) {
            string cmdText = "select 1 from R_PageParent where PID=@PID and ParentID=@ParentID ";
            SqlParameter[] parameters = {
					new SqlParameter("@PID", PID),
					new SqlParameter("@ParentID", ParentID)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 增加一条删除隐藏页和父页的关联， 返回新增加的标识列
        /// 增加失败返回0
        /// <param name="pid">页面ID</param>
        /// <param name="parentid">父页面ID</param>
        /// </summary>
        public int Add(int pid, int parentid) {
            string cmdText = "insert into R_PageParent(PID,ParentID) values(@PID,@ParentID)";
            SqlParameter[] parameters = {
					new SqlParameter("@PID", pid),
					new SqlParameter("@ParentID", parentid)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 删除隐藏页和父页的关联
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public int Delete(int pid, int parentID) {
            string cmdText = "DELETE FROM R_PageParent WHERE PID=@PID AND PARENTID=@ParentID";
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@PID", pid),
				new SqlParameter("@ParentID", parentID)
            };
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, sqlparams);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获得所有节点
        /// </summary>
        public IList<MR_PageInfo> GetList() {
            string cmdText = "select r.ParentID as 'HideParentID',r.PID," + DR_PageInfo.SELECT_ALL_COL_NOPK + "  FROM R_PageParent r left join R_PageInfo p on p.PID=r.PID";
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText);
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl);
        }
    }
}
