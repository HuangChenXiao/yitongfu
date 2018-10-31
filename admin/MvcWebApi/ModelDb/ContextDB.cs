using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.Data;

namespace ModelDb
{
    public class ContextDB
    {
        private static readonly string strConn = ConfigurationManager.AppSettings["ConnectionString"];
        public static Database Context()
        {
            var providerName = "System.Data.SqlClient";
            var connection = Database.OpenConnectionString(strConn, providerName);
            return connection;
        }
    }
}
