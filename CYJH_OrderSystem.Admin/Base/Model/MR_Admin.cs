using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 简单游管理员
    /// </summary>
    [Serializable]
    public class MR_Admin {
        public MR_Admin() {
        }
        #region Model
        private int _aid;
        private string _aname;
        private string _apwd;
        private string _anickname;
        private string _ip;
        private string _email;
        private int _gid;
        private DateTime _alasttime;
        private string _gname;
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AID {
            set {
                _aid = value;
            }
            get {
                return _aid;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string AName {
            set {
                _aname = value;
            }
            get {
                return _aname;
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string APwd {
            set {
                _apwd = value;
            }
            get {
                return _apwd;
            }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string ANickName {
            set {
                _anickname = value;
            }
            get {
                return _anickname;
            }
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP {
            set {
                _ip = value;
            }
            get {
                return _ip;
            }
        }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email {
            set {
                _email = value;
            }
            get {
                return _email;
            }
        }
        /// <summary>
        /// -1表示超级管理员，超级管理员访问页面将不受任何限制。
        /// </summary>
        public int GID {
            set {
                _gid = value;
            }
            get {
                return _gid;
            }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime ALastTime {
            set {
                _alasttime = value;
            }
            get {
                return _alasttime;
            }
        }
        /// <summary>
        /// 用户组名称（附加属性，不属于该表）
        /// </summary>
        public string GName {
            set {
                _gname = value;
            }
            get {
                return _gname;
            }
        }
        #endregion Model
    }
}
