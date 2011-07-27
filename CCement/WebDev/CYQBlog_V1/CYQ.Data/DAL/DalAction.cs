namespace CYQ.Data.DAL
{
    using CYQ.Data;
    using System;
    using System.Configuration;
    using System.Windows.Forms;

    internal class DalAction
    {
        public const string OleDb = "System.Data.OleDb";
        public const string OracleClient = "System.Data.OracleClient";
        public const string SqlClient = "System.Data.SqlClient";

        public static DalType GetDalType(string providerName)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return DalType.Sql;

                case "System.Data.OleDb":
                    return DalType.Access;

                case "System.Data.OracleClient":
                    return DalType.Oracle;
            }
            return DalType.Sql;
        }

        public static DbBase GetHelper(string conn)
        {
            string providerName = "";
            if (conn.Length < 0x20)
            {
                conn = (conn.Length < 1) ? "Conn" : conn;
                if (ConfigurationManager.ConnectionStrings[conn] == null)
                {
                    throw new Exception(string.Format("从配置文件webconfig中找不到 {0} 的链接字符串配置项!", conn));
                }
                providerName = ConfigurationManager.ConnectionStrings[conn].ProviderName;
                conn = ConfigurationManager.ConnectionStrings[conn].ConnectionString;
            }
            if (string.IsNullOrEmpty(providerName))
            {
                providerName = GetpPovider(conn);
            }
            switch (providerName)
            {
                case "System.Data.SqlClient":
                    return new SQLHelper(conn, providerName);

                case "System.Data.OleDb":
                    if (!string.IsNullOrEmpty(AppConfig.AccessDbNameForWeb))
                    {
                        conn = string.Format(conn, AppDomain.CurrentDomain.BaseDirectory + AppConfig.AccessDbNameForWeb);
                    }
                    else if (!string.IsNullOrEmpty(AppConfig.AccessDbNameForApp))
                    {
                        conn = string.Format(conn, Application.StartupPath + AppConfig.AccessDbNameForWeb);
                    }
                    return new OleHelper(conn, providerName);

                case "System.Data.OracleClient":
                    return new OracleHelper(conn, providerName);
            }
            return new SQLHelper(conn, providerName);
        }

        public static string GetpPovider(string conn)
        {
            conn = conn.ToLower();
            if (conn.Contains("microsoft.jet.oledb.4.0"))
            {
                return "System.Data.OleDb";
            }
            if (!conn.Contains("provider=msdaora") && !conn.Contains("provider=oraoledb.oracle"))
            {
                return "System.Data.SqlClient";
            }
            return "System.Data.OracleClient";
        }
    }
}

