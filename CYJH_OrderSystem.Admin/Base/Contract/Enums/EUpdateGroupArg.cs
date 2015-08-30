using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract.Enums {
    /// <summary>
    /// 
    /// </summary>
    public enum EUpdateGroupArg {
        /// <summary>
        /// 不调整权限
        /// </summary>
        Leave = 0,
        /// <summary>
        /// 调整为新组的权限
        /// </summary>
        Update = 1,
        /// <summary>
        /// 将新组的权限追加到现有权限中。
        /// </summary>
        Append = 2
    }
}
