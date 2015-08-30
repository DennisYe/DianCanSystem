using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 权限组基本信息
    /// </summary>
    [Serializable]
    public class MR_Group {
        /// <summary>
        /// 组ID
        /// </summary>
        public int GID {
            get;
            set;
        }
        /// <summary>
        /// 组名称
        /// </summary>
        public string GName {
            get;
            set;
        }
    }
}
