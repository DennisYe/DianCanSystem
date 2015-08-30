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
    /// 管理员权限数据库操作
    /// </summary>
    internal class DR_AdminRight {
        #region  成员方法
        /// <summary>
        /// 是否存在该权限记录
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <param name="PID">页面ID</param>
        public bool Exists(int AID, int PID) {
            string cmdText = "select 1 from R_AdminRight where AID=@AID and PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@AID", AID),
					new SqlParameter("@PID", PID)};
            return SQLHelpers.TcAdmin().Exists(cmdText, parameters);
        }

        /// <summary>
        /// 增加权限记录， 如果存在则更新
        /// 返回影响的行数，失败返回0
        /// </summary>
        /// <param name="pid">页面ID</param>
        /// <param name="btnRightExp">按钮权限表达式</param>
        /// <param name="aid">管理员ID</param>
        public int AddOrUpdate(int pid, string btnRightExp, bool updateWhenExists, params int[] aid) {
            int result = 0;
            Safe.Base.Contract.IDbHelper qmnobj = SQLHelpers.TcAdmin();
            qmnobj.SetHandClose(false);
            try {
                if (aid != null) {
                    foreach (int item in aid) {
                        string pname = "p_AddUpdateAdminRight";
                        SqlParameter[] parameters = {
					        new SqlParameter("@aid", SqlDbType.Int,4),
					        new SqlParameter("@pid", SqlDbType.Int,4),
					        new SqlParameter("@btnRightExp", SqlDbType.NVarChar,20),
                            new SqlParameter("@updateWhenExists", true )};
                        parameters[0].Value = item;
                        parameters[1].Value = pid;
                        parameters[2].Value = btnRightExp;
                        result = result + qmnobj.ExecuteProc(pname, false, parameters).ReturnValue;
                    }
                }
            } finally {
                qmnobj.EndConnection();
            }
            return result;
        }

        /// <summary>
        /// 批量更新权限
        /// 返回受影响的行数
        /// </summary>
        /// <param name="aids">管理员ID数组</param>
        /// <param name="info">权限集合</param>
        public int UpdateRights(int[] aids, Dictionary<int, string> info) {
            int result = 0;
            Safe.Base.Contract.IDbHelper qmnobj = SQLHelpers.TcAdmin();
            qmnobj.SetHandClose(false);
            try {
                foreach (int aid in aids) {
                    foreach (KeyValuePair<int, string> item in info) {
                        string pname = "p_AddUpdateAdminRight";
                        SqlParameter[] parameters = {
					        new SqlParameter("@aid", aid),
					        new SqlParameter("@pid", item.Key),
					        new SqlParameter("@btnRightExp", item.Value),
                            new SqlParameter("@updateWhenExists", true )};
                        result = result + qmnobj.ExecuteProc(pname, false, parameters).ReturnValue;
                    }
                }

            } finally {
                qmnobj.EndConnection();
            }
            return result;
        }

        /// <summary>
        /// 更新点击数，返回受影响的行数
        /// 更新失败返回0
        /// <param name="aid">管理员ID</param>
        /// <param name="pid">页面ID</param>
        /// </summary>
        public int UpdateClick(int aid, int pid) {
            string cmdText = "update R_AdminRight set ClickTimes=ClickTimes+1 where AID=@AID and PID=@PID";
            SqlParameter[] parameters = {
					new SqlParameter("@AID", aid),
					new SqlParameter("@PID", pid)};

            object obj = SQLHelpers.TcAdmin().ExecuteNonQuery(cmdText, parameters);
            if (obj == null) {
                return 0;
            } else {
                return Convert.ToInt32(obj);
            }
        }



        /// <summary>
        /// 得到一个权限记录实体
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <param name="PID">页面ID</param>
        public MR_AdminRight GetModel(int AID, int PID) {
            string cmdText = "select  top 1 AID,PID,BtnRightExp,ClickTimes from R_AdminRight where AID=@AID and PID=@PID ";
            SqlParameter[] parameters = {
					new SqlParameter("@AID", AID),
					new SqlParameter("@PID", PID)};

            DataTable dtbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(cmdText, parameters);
            if (dtbl.Rows.Count > 0) {
                return Safe.Base.Utility.ModelConvertHelper<MR_AdminRight>.ToModel(dtbl.Rows[0]);
            } else {
                return null;
            }
        }

        ///// <summary>
        ///// 获得权限记录列表
        ///// </summary>
        //public DataTable GetList() {
        //    string cmdText = "select AID,PID,BtnRightExp,ClickTimes FROM R_AdminRight order by ClickTimes desc";
        //    return SQLHelpers.TcFAQ().ExecuteFillDataTable(cmdText);
        //}

        /// <summary>
        /// 检查管理员是否有目录的访问权限，同时返回对该页面按钮的访问权限表达式
        /// </summary>
        /// <remarks>当有权限访问页面时，点击次数+1</remarks>
        /// <param name="adminId">当前登录的作者</param>
        /// <param name="url">当前访问页面的URL</param>
        /// <param name="btnRights">当前页面的按钮访问权限表达式</param>
        /// <returns>
        /// true:可以访问该目录
        /// false:不可访问该目录
        /// </returns>
        public bool IsInRoles(int adminId, int pageId, string superAdminRole, bool updateClickTime, out string btnRights) {
            string pname = "p_IsInRoles";
            btnRights = string.Empty;
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@aid", adminId),
                new SqlParameter("@pageId",pageId ),
                new SqlParameter("@btnRights",SqlDbType.NVarChar,20),
                new SqlParameter("@updateClickTime",  updateClickTime),
                new SqlParameter("@superAdminRole",superAdminRole)
            };
            sqlparams[2].Direction = ParameterDirection.Output;

            Safe.Base.Contract.CommandResult ComResult = SQLHelpers.TcAdmin().ExecuteProc(pname, false, sqlparams);
            int result = ComResult.ReturnValue;
            if (result == 0)
                return false;

            btnRights = ComResult.OutPutValue["btnRights"].ToString();
            return true;
        }

        /// <summary>
        /// 登录后取得对应权限，不含隐藏菜单
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="parentID">父节点ID</param>
        /// <param name="includeChild">是否同时取子节点</param>
        public IList<MR_PageInfo> GetMenusToList(int adminId, int parentID, bool includeChild, bool updateClickTime) {
            IList<MR_PageInfo> result = new List<MR_PageInfo>();
            Safe.Base.Contract.IDbHelper tqmn = SQLHelpers.TcAdmin();
            tqmn.SetHandClose(false);
            try {
                GetChildsToList(adminId, parentID, includeChild, ref result, ref tqmn);
                if (result != null && result.Count > 0 && updateClickTime) {
                    string sql = "UPDATE R_AdminRight SET ClickTimes=ClickTimes+1 WHERE AID=@AID AND PID=@PID";
                    tqmn.ExecuteNonQuery(sql, new SqlParameter("@AID", adminId), new SqlParameter("@PID", parentID));
                }
                return result;
            } finally {
                tqmn.EndConnection();
            }

        }
        /// <summary>
        /// 登录后取得对应权限，不含隐藏菜单
        /// </summary>
        /// <param name="adminId">管理员ID</param>
        /// <param name="parentID">父节点ID</param>
        /// <param name="includeChild">是否同时取子节点</param>
        public IList<MR_PageInfo> GetMenus(int adminId, int parentID, bool includeChild, bool updateClickTime) {
            IList<MR_PageInfo> result = new List<MR_PageInfo>();
            Safe.Base.Contract.IDbHelper tqmn = SQLHelpers.TcAdmin();
            tqmn.SetHandClose(false);
            try {
                GetChilds(adminId, parentID, includeChild, ref result, ref tqmn);
                if (result != null && result.Count > 0 && updateClickTime) {
                    string sql = "UPDATE R_AdminRight SET ClickTimes=ClickTimes+1 WHERE AID=@AID AND PID=@PID";
                    tqmn.ExecuteNonQuery(sql, new SqlParameter("@AID", adminId), new SqlParameter("@PID", parentID));
                }
                return result;
            } finally {
                tqmn.EndConnection();
            }

        }

        #region "根据管理员ID和父结点ID取一个目录的子项"

        /// <summary>
        /// 获取权限内,某个页面下的所有层级子页面,并转换为List
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="parentID"></param>
        /// <param name="incluedChild"></param>
        /// <param name="result"></param>
        /// <param name="dbhelper"></param>
        private void GetChildsToList(int adminId, int parentID, bool incluedChild, ref  IList<MR_PageInfo> result, ref Safe.Base.Contract.IDbHelper dbhelper) {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.*,b.btnrightexp from r_pageinfo as a left join  r_adminright as b on a.pid=b.pid where b.aid=@aid and a.parentID=@parentID order by a.Queue asc, b.ClickTimes desc");
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@aid",adminId),
                new SqlParameter("@parentID",parentID)
            };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(sb.ToString(), sqlparams);
            IList<MR_PageInfo> tmp = Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl); //该结点的子节点

            if (tmp != null) {
                foreach (MR_PageInfo tmpc in tmp) {
                    if (!result.Contains(tmpc)) {
                        result.Add(tmpc);
                        if (incluedChild)
                            GetChildsToList(adminId, tmpc.PID, incluedChild, ref result, ref dbhelper);
                    }
                }
            }
        }
        private void GetChilds(int adminId, int parentID, bool incluedChild, ref  IList<MR_PageInfo> result, ref Safe.Base.Contract.IDbHelper dbhelper) {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.*,b.btnrightexp from r_pageinfo as a left join  r_adminright as b on a.pid=b.pid where b.aid=@aid and a.parentID=@parentID order by a.Queue asc, b.ClickTimes desc");
            SqlParameter[] sqlparams = new SqlParameter[]{
                new SqlParameter("@aid",adminId),
                new SqlParameter("@parentID",parentID)
            };
            DataTable tbl = SQLHelpers.TcAdmin().ExecuteFillDataTable(sb.ToString(), sqlparams);
            result = Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(tbl); //该结点的子节点

            if (incluedChild && result != null) {
                foreach (MR_PageInfo tmpc in result) {
                    IList<MR_PageInfo> childs = null;
                    GetChilds(adminId, tmpc.PID, incluedChild, ref childs, ref dbhelper);
                    tmpc.Childs = childs.ToList();
                }
            }
        }

        #endregion

        #endregion  成员方法


        #region "删除管理员的某个页面权限"

        /// <summary>
        /// 删除管理员对某个页面的访问权限（会同时删除子页面，不含隐藏页）返回1
        /// </summary>
        /// <param name="PID">页面ID</param>
        /// <param name="AID">管理员ID</param>
        public int Delete(int PID, IList<int> AID) {
            Safe.Base.Contract.IDbHelper dbhelper = SQLHelpers.TcAdmin();
            dbhelper.SetHandClose(true);
            try {
                foreach (int taid in AID) {
                    Delete(PID, taid, dbhelper);
                }
            } finally {
                dbhelper.EndConnection();
            }
            return 1;
        }


        private void Delete(int PID, int AID, Safe.Base.Contract.IDbHelper dbhelper) {
            IList<MR_PageInfo> childs = GetChild(PID, AID, ref dbhelper);
            if (childs != null) {
                foreach (MR_PageInfo tmp in childs) {
                    Delete(tmp.PID, AID, dbhelper);
                }
            }
            string cmdText = "delete from R_AdminRight where AID=@AID and PID=@PID";
            SqlParameter[] parameters = { new SqlParameter("@AID", AID), new SqlParameter("@PID", PID) };
            dbhelper.ExecuteNonQuery(cmdText, parameters);
        }

        /// <summary>
        /// 取得管理员在某个页面下是否有子项
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        private IList<MR_PageInfo> GetChild(int pid, int aid, ref Safe.Base.Contract.IDbHelper dbhelper) {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT R_PageInfo.* FROM R_PageInfo LEFT JOIN R_AdminRight on R_AdminRight.PID = R_PageInfo.PID WHERE R_AdminRight.AID=@aid and  R_PageInfo.ParentID=@pid ");
            DataTable dt = dbhelper.ExecuteFillDataTable(sb.ToString(), new SqlParameter("@aid", aid), new SqlParameter("@pid", pid));
            if (dt == null)
                return null;
            if (dt.Rows.Count == 0)
                return null;
            return Safe.Base.Utility.ModelConvertHelper<MR_PageInfo>.ToModels(dt);
        }

        #endregion

        /// <summary>
        /// 所有权限配置同时带页面对象(包含隐藏页面)<para/>
        /// 隐藏页面的权限为父级页面的权限
        /// </summary>
        /// <returns></returns>
        public IList<MR_PageInfo> GetAllList() {
            var dt = SQLHelpers.TcAdmin().ExecuteFillDataTable(@"
SELECT r.AID,r.BtnRightExp,p.* FROM R_AdminRight r
left join R_PageInfo p on p.PID=r.PID
union all
SELECT r.AID,r.BtnRightExp,p.* FROM R_AdminRight r
left join R_PageParent pp on r.PID=pp.ParentID
left join R_PageInfo p on pp.PID=p.PID
where pp.ParentID is not null
        ");
            return ModelConvertHelper<MR_PageInfo>.ToModels(dt);
        }
    }
}
