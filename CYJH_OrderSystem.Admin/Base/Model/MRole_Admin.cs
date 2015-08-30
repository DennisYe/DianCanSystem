using System;
namespace CYJH_OrderSystem.Admin.Base.Model {
    /// <summary>
    /// 实体类Role_Admin 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class MRole_Admin {
        public MRole_Admin() {
        }
        #region Model
        private int _aid;
        private string _aname;
        private string _apwd;
        private string _anickname;
        private string _mid;
        private string _ip;
        private string _email;
        /// <summary>
        /// ID
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
        /// 登录名
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
        /// 目录ID
        /// </summary>
        public string MID {
            set {
                _mid = value;
            }
            get {
                return _mid;
            }
        }
        /// <summary>
        /// 最后登录IP
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
        /// EMAIL
        /// </summary>
        public string Email {
            set {
                _email = value;
            }
            get {
                return _email;
            }
        }
        #endregion Model

    }
}
