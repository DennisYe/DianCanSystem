using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CYJH_OrderSystem.Admin.Base.Contract;
using CYJH_OrderSystem.Admin.Base.Model;
using CYJH_OrderSystem.Admin.Base.Dal;
using System.Data;

namespace CYJH_OrderSystem.Admin.Base.BLL {
    internal class GroupManage : IGroupManage, IRightsSetting {

        #region IGroupManage 成员

        public IList<MR_Group> GetList() {
            return new DR_Group().GetList();
        }

        public bool DeleteGroup(int groupId) {
            int result = new DR_Group().Delete(groupId);
            if (result == 0)
                return false;
            return true;
        }

        public bool AddUpdate(string groupName, int groupId) {
            if (groupId <= 0) {
                int result = new DR_Group().Add(groupName);
                if (result == 0)
                    return false;
                return true;
            } else {
                int result = new DR_Group().Update(groupId, groupName);
                if (result == 0)
                    return false;
                return true;
            }
        }

        public IList<MR_Admin> GetMemberList(int groupID) {
            return new DR_Admin().GetList(groupID);
        }

        public MR_Group GetGroup(int groupId) {
            return new DR_Group().GetModel(groupId);
        }

        #endregion

        #region IRightsSetting 成员

        public bool AddRole(int pageId, IList<int> forId, string btnRights, bool updateWhenExists) {
            new DR_GroupRight().AddOrUpdate(pageId, btnRights, updateWhenExists, forId.ToArray());
            return true;
        }

        public bool UpdateBtnExp(IList<int> foridList, Dictionary<int, string> rolsSettings) {
            new DR_GroupRight().UpdateRights(foridList.ToArray(), rolsSettings);
            return true;
        }

        public bool RemoveRole(IList<int> foridList, int pid) {
            new DR_GroupRight().Delete(pid, foridList);
            return true;
        }

        #endregion

        #region IMenusGetting 成员

        public IList<MR_PageInfo> GetChildMenuToList(int forId, int parentID, bool includeChild) {
            return new DR_GroupRight().GetList(forId, parentID, includeChild);
        }

        #endregion
    }
}
