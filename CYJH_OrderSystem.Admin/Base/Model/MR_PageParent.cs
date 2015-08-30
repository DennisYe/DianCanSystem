using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 页面
    /// </summary>
    [Serializable]
    public class MR_PageParent {
        #region Model

        /// <summary>
        /// 关系ID
        /// </summary>
        public int RID { get; set; }

        /// <summary>
        /// 父级页面ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 页面ID
        /// </summary>
        public int PID { get; set; }

        #endregion Model

    }
}
