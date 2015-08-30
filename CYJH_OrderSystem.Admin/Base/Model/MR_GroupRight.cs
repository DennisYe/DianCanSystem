using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 用户组权限

    /// </summary>
    [Serializable]
    public class MR_GroupRight {
        public MR_GroupRight() { }
        #region Model
        private int _gid;
        private int _pid;
        private string _btnrightexp;
        /// <summary>
        /// 用户组ID
        /// </summary>
        public int GID {
            set { _gid = value; }
            get { return _gid; }
        }
        /// <summary>
        /// 页面ID
        /// </summary>
        public int PID {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 按钮权限表达式

        /// </summary>
        public string BtnRightExp {
            set { _btnrightexp = value; }
            get { return _btnrightexp; }
        }
        #endregion Model
    }
}
