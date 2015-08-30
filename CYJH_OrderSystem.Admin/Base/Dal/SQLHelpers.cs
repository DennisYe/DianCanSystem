using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CYJH_OrderSystem.Admin.Base.Dal {
    public static class SQLHelpers {
        /// <summary>
        /// 后台的数据库
        /// </summary>
        /// <returns></returns>
        public static Safe.Base.Contract.IDbHelper TcAdmin() {
            //return null;// Safe.TcUserReport.DbHelper.Factory.GetIDbHelper(EConnType.MSSQL_DEFAULT, true);
            return Safe.Base.DbHelper.SQLHelper.FromFile(@"D:\Configs\DataBase\TLData.dbc", true);
        }
    }
}
