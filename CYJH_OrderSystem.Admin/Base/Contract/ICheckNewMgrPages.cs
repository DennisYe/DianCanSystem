using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CYJH_OrderSystem.Admin.Base.Model;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface ICheckNewMgrPages {
        /// <summary>
        /// 检测新的页面,返回页面列表,可带层级结构
        /// </summary>
        /// <returns></returns>
        List<MR_PageInfo> CheckNewMgrPages();
    }
}