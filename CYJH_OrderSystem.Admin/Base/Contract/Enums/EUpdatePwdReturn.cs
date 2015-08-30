using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract.Enums {
    /// <summary>
    /// 修改密码后的返回值
    /// </summary>
    public enum EUpdatePwdReturn {
        /// <summary>
        /// 修改成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 旧密码不正确
        /// </summary>
        OldPwdDeny = -1,

        /// <summary>
        /// 修改失败
        /// </summary>
        Error = 0
    }
}
