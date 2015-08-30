using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 页面
    /// </summary>
    [Serializable]
    public class MR_PageInfo {
        public MR_PageInfo() {
            _queue = 10000;
        }
        #region Model
        private int _pid;
        private string _pname;
        private string _purl;
        private bool _isurl;
        private int _queue;
        private int _parentid;
        private string _btnrightexp; //附加属性，不是这张表

        /// <summary>
        /// 页面ID
        /// </summary>
        public int PID {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PName {
            set { _pname = value; }
            get { return _pname; }
        }
        /// <summary>
        /// 超链接

        /// </summary>
        public string PUrl {
            set { _purl = value; }
            get { return _purl; }
        }
        /// <summary>
        /// 是否为超链接
        /// </summary>
        public bool IsUrl {
            set { _isurl = value; }
            get { return _isurl; }
        }
        /// <summary>
        /// 母菜单的排列顺序
        /// </summary>
        public int Queue {
            set { _queue = value; }
            get { return _queue; }
        }
        /// <summary>
        /// 父菜单ID(-1表示是隐藏页,0 表示是根目录下面的)
        /// </summary>
        public int ParentID {
            set { _parentid = value; }
            get { return _parentid; }
        }

        /// <summary>
        /// 页面按钮权限（附加属性，不是这张表）
        /// </summary>
        public string BtnRightExp {
            set {
                _btnrightexp = value;
            }
            get {
                return _btnrightexp;
            }
        }
        /// <summary>
        /// 在导航中是否默认展开子项
        /// </summary>
        public bool DefShowChild { get; set; }
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

        /// <summary>
        /// 子页面列表
        /// </summary>
        public List<MR_PageInfo> Childs { get; set; }


        /// <summary>
        /// [权限]管理员ID
        /// </summary>
        public int AID { get; set; }
        /// <summary>
        /// [权限]管理员组ID
        /// </summary>
        public int GID { get; set; }

        /// <summary>
        /// 隐藏页面的父级ID
        /// </summary>
        public int HideParentID { get; set; }

        #endregion

    }
}
