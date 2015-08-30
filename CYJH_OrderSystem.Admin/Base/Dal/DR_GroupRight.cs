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
    /// 用户组权限数据库操作
    /// </summary>
    internal class DR_GroupRight {
        public DR_GroupRight() {
        }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录

        /// </summary>
        public bool Exists(int GID, int PID) {
            string cmdText = "select 1 from R_GroupRight where GID=@GID and PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@GID",GID),
					new SqlParameter("@PID", PID)
            };

            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 更新组权限
        /// </summary>
        /// <param name="gids"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateRights(int[] gids, Dictionary<int, string> info) {
            int result = 0;
            Safe.Base.Contract.IDbHelper qmnobj = SQLHelpers.TcAdmin();
            qmnobj.SetHandClose(false);
            try {
                foreach (int gid in gids) {
                    foreach (KeyValuePair<int, string> item in info) {
                        string pname = "p_AddUpdateGroupRight";
                        SqlParameter[] parameters = {
					    new SqlParameter("@gid", gid),
					    new SqlParameter("@pid", item.Key),
					    new SqlParameter("@btnRightExp", item.Value),
                        new SqlParameter("@updateWhenExists", true)};
                        result = result + qmnobj.ExecuteProc(pname, false, parameters).ReturnValue;
                    }
                }

            } finally {
                qmnobj.EndConnection();
            }
            return result;
        }


        /// <summary>
        ///  增加或更新数据， 如果存在则更新
        /// </summary>
        /// <param name="pid">页面ID</param>
        /// <param name="btnRightExp">访问权限</param>
        /// <param name="gids">组ID</param>
        /// <returns></returns>
        public int AddOrUpdate(int pid, string btnRightExp, bool updateWhenExists, params int[] gids) {
            int result = 0;
            Safe.Base.Contract.IDbHelper qmnobj = SQLHelpers.TcAdmin();
            qmnobj.SetHandClose(false);
            try {
                if (gids != null) {
                    foreach (int gid in gids) {
                        string pname = "p_AddUpdateGroupRight";
                        SqlParameter[] parameters = {
					    new SqlParameter("@gid", gid),
					    new SqlParameter("@pid", pid),
					    new SqlParameter("@btnRightExp", btnRightExp),
                        new SqlParameter("@updateWhenExists",updateWhenExists )};
                        result = result + qmnobj.ExecuteProc(pname, false, parameters).ReturnValue;
                    }
                }
            } finally {
                qmnobj.EndConnection();
            }
            return result;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MR_GroupRight GetModel(int GID, int PID) {
            string cmdText = "select  top 1 GID,PID,BtnRightExp from R_GroupRight where GID=@GID and PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@GID", GID),
					new SqlParameter("@PID", PID)};

            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText, parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_GroupRight>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }

        /// <summary>
        /// 取得组的预设菜单， 不含隐藏目录
        /// </summary>
        /// <param name="groupID">组ID</param>
        /// <param name="parentID">用户ID</param>
        /// <param name="includeChild">是否同时取子节点</param>
        /// <returns></returns>
        public IList<MR_PageInfo> GetList(int groupID, int parentID, bool includeChild) {
            IList<MR_PageInfo> result = new List<MR_PageInfo>();
            Safe.Base.Contract.IDbHelper tqmn = SQLHelpers.TcAdmin();
            tqmn.SetHandClose(false);
            try {
                GetChilds(groupID, parentID, includeChild, ref result, ref tqmn);
                return result;
            } finally {
                tqmn.EndConnection();
            }

        }


        #region "根据组ID和父结点ID取一个目录的子项"

        private void GetChilds(int groupId, int parentID, bool incluedChild, ref  IList<MR_PageInfo> result, ref Safe.Base.Contract.IDbHelper dbhelper) {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.*,b.btnrightexp from r_pageinfo as a left join  R_GroupRight as b on a.pid=b.pid where b.gid=@gid and a.parentID=@parentID");
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@gid",groupId ),
                new SqlParameter("@parentID",parentID)
            };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(sb.ToString(), sqlparams);
            IList<MR_PageInfo> tmp = Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl); //该结点的子节点

            if (tmp != null) {
                foreach (MR_PageInfo tmpc in tmp) {
                    if (!result.Contains(tmpc)) {
                        result.Add(tmpc);
                        if (incluedChild)
                            GetChilds(groupId, tmpc.PID, incluedChild, ref result, ref dbhelper);
                    }
                }
            }
        }

        #endregion

        #region "删除组的某个页面权限"

        /// <summary>
        /// 删除管理员对某个页面的访问权限（会同时删除子页面，不含隐藏页）返回1
        /// </summary>
        /// <param name="PID">页面ID</param>
        /// <param name="GID">组ID</param>
        public int Delete(int PID, IList<int> GID) {
            Safe.Base.Contract.IDbHelper dbhelper = SQLHelpers.TcAdmin();
            dbhelper.SetHandClose(true);
            try {
                foreach (int tmpgid in GID)
                    Delete(PID, tmpgid, dbhelper);
            } finally {
                dbhelper.EndConnection();
            }
            return 1;
        }


        private void Delete(int PID, int GID, Safe.Base.Contract.IDbHelper dbhelper) {
            IList<MR_PageInfo> childs = GetChild(PID, GID, ref dbhelper);
            if (childs != null) {
                foreach (MR_PageInfo tmp in childs) {
                    Delete(tmp.PID, GID, dbhelper);
                }
            }
            string cmdText = "delete from R_GroupRight where GID=@GID and PID=@PID";
            SqlParameter[] parameters = { new SqlParameter("@GID", GID), new SqlParameter("@PID", PID) };
            dbhelper.ExecuteNonQuery(cmdText, parameters);
        }

        /// <summary>
        /// 取得组在某个页面下是否有子项
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        private IList<MR_PageInfo> GetChild(int pid, int gid, ref Safe.Base.Contract.IDbHelper dbhelper) {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT R_PageInfo.* FROM R_PageInfo LEFT JOIN R_GroupRight on R_GroupRight.PID = R_PageInfo.PID WHERE R_GroupRight.GID=@gid and  R_PageInfo.ParentID=@pid ");
            DataTable dt = dbhelper.ExecuteFillDataTable(sb.ToString(), new SqlParameter("@gid", gid), new SqlParameter("@pid", pid));
            if (dt == null)
                return null;
            if (dt.Rows.Count == 0)
                return null;
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(dt);
        }

        #endregion



        public IList<MR_PageInfo> GetAllList() {
            var dt = SQLHelpers.TcAdmin().ExecuteFillDataTable(@"
SELECT r.GID,r.BtnRightExp,p.* FROM R_GroupRight r
left join R_PageInfo p on p.PID=r.PID
union all
SELECT r.GID,r.BtnRightExp,p.* FROM R_GroupRight r
left join R_PageParent pp on r.PID=pp.ParentID
left join R_PageInfo p on pp.PID=p.PID
where pp.ParentID is not null
");
            return ModelConvertHelper<MR_PageInfo>.ToModels(dt);
        }
        #endregion  成员方法
    }
}
