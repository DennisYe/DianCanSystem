using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Safe.Base.DbHelper;
using System.Configuration;

namespace CYJH_OrderSystem.DAL
{
    public class SQLHelps
    {
        /// <summary>
        /// DNFData库的数据库连接对象
        /// </summary>
        /// <returns></returns>
        public static Safe.Base.Contract.IDbHelper OrderSystemData()
        {
            //return Safe.CFData.DbHelper.Factory.GetIDbHelper(EConnType.MSSQL_CFDATA, true);
            string connString = "data source=127.0.0.1;initial catalog=CYJH_OrderSystem;user id=sa;password=123456a;persist security info=True;Connection Timeout=5000;";//ConfigurationManager.ConnectionStrings["CYJHOrderSysConn"].ConnectionString;
            SQLHelper sqlHelp = new SQLHelper(connString, true);
            return sqlHelp;
            //Safe.Base.DbHelper.SQLHelper.FromFile(@"D:\Configs\DataBase\CYJH_OrderSystem.dbc", true);
        }
    }
}

