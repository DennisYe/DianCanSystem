using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface ICheckUserServer {
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsExistsUser(string userName);
    }
}