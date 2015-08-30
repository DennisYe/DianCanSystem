using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Contract {
    public interface IMenusGetting {
        /// <summary>
        /// 取对象中父节点下的目录
        /// </summary>
        /// <param name="forId">对象ID</param>
        /// <param name="parentID">父节点ID</param>
        /// <param name="rootId">父节点</param>
        /// <param name="includeChild">是否包含子项</param>
        IList<CYJH_OrderSystem.Admin.Base.Model.MR_PageInfo> GetChildMenuToList(int forId, int parentID, bool includeChild);
    }
}
