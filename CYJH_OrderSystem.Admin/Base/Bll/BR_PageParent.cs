using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Dal;
using CYJH_OrderSystem.Admin.Base.Model;
using System.Web;

namespace CYJH_OrderSystem.Admin.Base.Bll {
    public class BR_PageParent {
        private DR_PageParent _dal = new DR_PageParent();

        /// <summary>
        /// 删除隐藏页和父页的关联
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public bool Delete(int pid, int parentID, out string exception) {
            exception = string.Empty;
            try {
                return _dal.Delete(pid, parentID) > 0;
            } catch (Exception ex) {
                exception = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// 是否存在该删除隐藏页和父页的关联
        /// <param name="pid">页面ID</param>
        /// <param name="parentid">父页面ID</param>
        /// </summary>
        public bool Exists(int PID, int ParentID) {
            return _dal.Exists(PID, ParentID);
        }

        /// <summary>
        /// 增加一条隐藏页和父页的关联， 返回新增加的标识列<para/>
        /// 增加失败返回0(已存在)
        /// <param name="pid">页面ID</param>
        /// <param name="parentid">父页面ID</param>
        /// </summary>
        public int Add(int pId, int parentId, out string exStr) {
            exStr = "";
            if (pId <= 0) {
                exStr = "页面不存在";
                return 0;
            }
            if (parentId <= 0) {
                exStr = "父级页面不存在";
                return 0;
            }
            try {
                if (!Exists(pId, parentId)) {
                    return _dal.Add(pId, parentId);
                }
            } catch (Exception ex) {
                exStr = ex.ToString();
            }
            return 0;
        }
    }
}