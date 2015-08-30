using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 管理员权限
    /// </summary>
    [Serializable]
    public class MR_AdminRight {
        public MR_AdminRight() { }
        #region Model
        private int _aid;
        private int _pid;
        private string _btnrightexp;
        private int _clicktimes;
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AID {
            set { _aid = value; }
            get { return _aid; }
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
            set { _btnrightexp = value.PadRight(7, '0'); }
            get { return _btnrightexp; }
        }
        /// <summary>
        /// 该菜单项被点击的次数
        /// </summary>
        public int ClickTimes {
            set { _clicktimes = value; }
            get { return _clicktimes; }
        }
        #endregion Model


        #region "扩展内容"

        public bool RoleAdd {
            get {
                return this._btnrightexp[0] == '1';
            }
        }
        public bool RoleDelete {
            get {
                return this._btnrightexp[1] == '1';
            }
        }
        public bool RoleUpdate {
            get {
                return this._btnrightexp[2] == '1';
            }
        }
        public bool RoleSelect {
            get {
                return this._btnrightexp[3] == '1';
            }
        }
        public bool RoleSpec1 {
            get {
                return this._btnrightexp[4] == '1';
            }

        }
        public bool RoleSpec2 {
            get {
                return this._btnrightexp[5] == '1';
            }
        }
        public bool RoleSpec3 {
            get {
                return this._btnrightexp[6] == '1';
            }
        }

        #endregion
    }
}
