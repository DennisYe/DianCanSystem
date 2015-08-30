using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface IPageParent {

        /// <summary>
        /// 将一个页面设为另外一个页面的子页面
        /// </summary>
        bool AddChild(int parentId, int childId);

        /// <summary>
        /// 从子页面列表中移除某页
        /// </summary>
        bool RemoveChild(int parentId, int childId);
    }
}
