using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CYJH_OrderSystem.Admin.Base.Model;
using Shared;
using Safe.Base.Utility;
using Safe.Base.Contract;

namespace CYJH_OrderSystem.Admin.Base.Dal {
    /// <summary>
    /// 节点数据库操作
    /// </summary>
    internal class DR_PageInfo {

        /// <summary>
        /// SQL语句SELECT除了主键的所有列，带有表别名p
        /// </summary>
        public const string SELECT_ALL_COL_NOPK = "p.PName,p.PUrl,p.IsUrl,p.Queue,p.ParentID,p.DefShowChild";

        public DR_PageInfo() {
        }
        #region  成员方法
        /// <summary>
        /// 是否存在该节点
        /// </summary>
        public bool Exists(int PID) {
            string cmdText = "select 1 from R_PageInfo where PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@PID",PID)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 检测一批URL是否都存在库中,返回不存在的列表
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public List<string> CheckPageURLs(IEnumerable<string> urls) {
            if (urls == null) return new List<string>();
            string sql = @"select u as 'PUrl' from(";
            string sp = "";
            foreach (var f in urls) {
                var path = f.TrimEx();
                if (path.IsEmpty()) continue;
                sql += sp;
                sql += " select '" + path.Replace("'", "''") + "' u ";
                sp = " union ";
            }
            sql += @")pu left join R_PageInfo p on p.PUrl=pu.u where p.PId is NULL";

            return SQLHelpers.TcAdmin().ExecuteFillDataTable(sql).ToModels<MR_PageInfo>().Select(p => p.PUrl).ToList();
        }


        /// <summary>
        /// 增加节点， 返回新增加的标识列
        /// 增加失败返回0
        /// </summary>
        public int Add(MR_PageInfo model) {

            string cmdText = @"
if not exists (select top 1 1 from R_PageInfo where PUrl=@Purl and PName=@PName and ParentID=@ParentID) begin
    insert into R_PageInfo(PName,PUrl,IsUrl,Queue,ParentID,DefShowChild) values (@PName,@PUrl,@IsUrl,@Queue,@ParentID,@DefShowChild);
    select SCOPE_IDENTITY();
end else begin
    select 0
end
";

            SqlParameter[] parameters = {
					new SqlParameter("@PName",  model.PName),
					new SqlParameter("@PUrl",model.PUrl),
					new SqlParameter("@IsUrl", model.IsUrl),
					new SqlParameter("@Queue", model.Queue),
					new SqlParameter("@ParentID", model.ParentID),
					new SqlParameter("@DefShowChild", model.DefShowChild)};
            object obj = SQLHelpers.TcAdmin().ExecuteScalar(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新节点，返回受影响的行数
        /// 更新失败返回0
        /// </summary>
        public int Update(string text, string url, int queue, int nodeId, bool defShowChild) {
            string cmdtext = "update R_PageInfo set PName=@PName,Queue=@Queue,PUrl=@PUrl,DefShowChild=@DefShowChild where PID=@PID ";
            SqlParameter[] parameters = {
					new SqlParameter("@PID", nodeId),
					new SqlParameter("@PName", text),
                    new SqlParameter("@Queue" , queue),
					new SqlParameter("@PUrl", url),
					new SqlParameter("@DefShowChild", defShowChild)};
            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdtext, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新页面
        /// </summary>
        public bool Update(MR_PageInfo model) {
            string cmdtext = "update R_PageInfo set PName=@PName,Queue=@Queue,PUrl=@PUrl,DefShowChild=@DefShowChild,ParentID=@ParentID where PID=@PID ";
            SqlParameter[] parameters = {
					new SqlParameter("@PID", model.PID),
					new SqlParameter("@PName", model.PName),
                    new SqlParameter("@Queue" , model.Queue),
					new SqlParameter("@PUrl", model.PUrl),
					new SqlParameter("@DefShowChild", model.DefShowChild),
					new SqlParameter("@ParentID", model.ParentID)};
            return SQLHelpers.TcAdmin().ExecuteNonQuery(cmdtext, parameters) > 0;
        }

        /// <summary>
        /// 删除一节点，返回受影响的行数
        /// 删除失败返回0
        /// </summary>
        public int Delete(int PID) {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append("      IF NOT EXISTS(SELECT TOP 1 1 FROM R_PageInfo WHERE ParentID=@PID) BEGIN ");
            sqlstr.Append("             DELETE FROM R_PageInfo WHERE PID=@PID ");
            sqlstr.Append("      END ");

            SqlParameter[] parameters = {
					new SqlParameter("@PID", PID)};
            return SQLHelpers.TcAdmin().ExecuteNonQuery(sqlstr.ToString(), parameters);

        }

        /// <summary>
        /// 检测是否存在URL
        /// <param name="PID">页面ID</param>
        /// <param name="PID">如果是update,那么需要传入页面ID,是insert则传入0</param>
        /// </summary>
        public bool ExistURL(string url, int pID) {
            string cmdText = "select  top 1 1 from R_PageInfo where PUrl=@PUrl" + (pID > 0 ? " and PID<>@PID" : "");
            SqlParameter[] parameters = {
					new SqlParameter("@PUrl", url)
					,new SqlParameter("@PID", pID)};

            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 得到节点
        /// <param name="PID">页面ID</param>
        /// </summary>
        public MR_PageInfo GetModel(int PID) {
            string cmdText = "select  top 1 p.PID," + SELECT_ALL_COL_NOPK + " from R_PageInfo p where PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@PID", PID)};

            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText, parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }
        /// <summary>
        /// 根据url完整匹配得到model
        /// <param name="urlPath">页面URL完整匹配</param>
        /// </summary>
        public MR_PageInfo GetModel(string urlPath) {
            string cmdText = "select  top 1 p.PID," + SELECT_ALL_COL_NOPK + " from R_PageInfo p where PUrl=@url";
            SqlParameter[] parameters = {
					new SqlParameter("@url", urlPath)};

            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText, parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }

        /// <summary>
        /// 获得所有节点
        /// </summary>
        public IList<Model.MR_PageInfo> GetList() {
            return GetList(null, null);
        }
        /// <summary>
        /// 获得所有节点
        /// </summary>
        public IList<Model.MR_PageInfo> GetList(bool? hasHidePage, bool? isHidePage) {
            string whereSQL = "";
            bool hasWhere = false;
            if (hasHidePage.HasValue && hasHidePage.Value) {
                if (!hasWhere) {
                    hasWhere = true;
                    whereSQL += " where ";
                }
                whereSQL += " ParentID > -1";
            }
            if (isHidePage.HasValue) {
                if (!hasWhere) {
                    hasWhere = true;
                    whereSQL += " where ";
                }
                if (isHidePage.Value) {
                    whereSQL += " ParentID = -1";
                } else {
                    whereSQL += " ParentID > -1";
                }
            }
            string cmdText = "select p.PID," + SELECT_ALL_COL_NOPK + "  FROM R_PageInfo p " + whereSQL + " order by Queue asc";
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText);
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl);
        }


        /// <summary>
        /// 取一个目录下的隐藏目录
        /// </summary>
        public IList<MR_PageInfo> GetHidePage(int adminId, int nodeID) {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.*,b.BtnRightExp from");
            sb.Append("(select p.PID," + SELECT_ALL_COL_NOPK + " from r_pageinfo p where pid in (select pid from r_pageparent where parentID=@parentID)) as a");
            sb.Append("left join r_adminright as b on a.pid=b.pid where b.aid=@aid");
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@parentID",nodeID),
                new SqlParameter("@aid",adminId)
            };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(sb.ToString(), sqlparams);
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl);
        }

        /// <summary>
        /// 取一个页面下的隐藏页
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetHidePage(int nodeID) {
            StringBuilder sb = new StringBuilder();
            sb.Append("select p.PID," + SELECT_ALL_COL_NOPK + " from r_pageinfo p WHERE parentID=-1 and pid in (select pid from r_pageparent where parentID=@parentID)  order by Queue asc");
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@parentID",nodeID)
            };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(sb.ToString(), sqlparams);
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl);
        }


        /// <summary>
        /// 取父节点下的所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetChildToList(int parentId, bool includeHide) {
            StringBuilder sql = new StringBuilder("SELECT p.PID," + SELECT_ALL_COL_NOPK + " FROM R_PageInfo p WHERE ParentID=@ParentID  order by Queue asc");
            if (includeHide) {
                sql.Append(" OR PID IN (SELECT * FROM r_pageparent WHERE parentID=@parentID)");
            }
            DataTable dt = SQLHelpers.TcAdmin().ExecuteFillDataTable(sql.ToString(), new SqlParameter("@ParentID", parentId));
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(dt);
        }

        /// <summary>
        /// 取父节点下的所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetChild(int parentId, bool includeHide, bool includeChild) {
            IDbHelper db = SQLHelpers.TcAdmin();
            db.SetHandClose(false);
            try {
                return GetChildRec(parentId, includeHide, includeChild, ref db);
            } catch (Exception ex) {
                throw ex;
            } finally {
                db.EndConnection();
            }
        }

        private List<MR_PageInfo> GetChildRec(int parentId, bool includeHide, bool includeChild, ref IDbHelper dbHelper) {
            StringBuilder sql = new StringBuilder("SELECT p.PID," + SELECT_ALL_COL_NOPK + " FROM R_PageInfo p WHERE ParentID=@ParentID  order by Queue asc");
            if (includeHide) {
                sql.Append(" OR PID IN (SELECT * FROM r_pageparent WHERE parentID=@parentID)");
            }
            DataTable dt = dbHelper.ExecuteFillDataTable(sql.ToString(), new SqlParameter("@ParentID", parentId));
            List<MR_PageInfo> childs = Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(dt).ToList();
            if (includeChild && childs != null) {
                foreach (var c in childs) {
                    c.Childs = GetChildRec(c.PID, includeHide, includeChild, ref dbHelper);
                }
            }
            return childs;
        }

        #endregion  成员方法
    }
}
