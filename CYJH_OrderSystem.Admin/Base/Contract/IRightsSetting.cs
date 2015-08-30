using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface IRightsSetting : IMenusGetting {
        /// <summary>
        /// 将一个目录的访问权限赋给对象
        /// </summary>
        /// <param name="pageId">页面ID</param>
        /// <param name="forId">要复制到的对象ID</param>
        /// <param name="btnRights">按钮权限</param>
        /// <param name="updateWhenExists">对象已经有权限时，是否更新</param>
        bool AddRole(int pageId, IList<int> forId, string btnRights, bool updateWhenExists);

        /// <summary>
        /// 批量更新权限
        /// </summary>
        /// <param name="foridList">对象列表</param>
        /// <param name="rolsSettings">页面ID，以及对应的按钮访问权限</param>
        bool UpdateBtnExp(IList<int> foridList, Dictionary<int, string> rolsSettings);

        /// <summary>
        /// 移除对象对某个页面的访问权限
        /// </summary>
        /// <param name="foridList">对象列表</param>
        /// <param name="pid">页面ID</param>
        bool RemoveRole(System.Collections.Generic.IList<int> foridList, int pid);
    }
}
