namespace CYQ.Data
{
    using System;
    using System.Configuration;

    public class AppConfig
    {
        private static string _CDataLeft;
        private static string _CDataRight;

        public static string GetApp(string key)
        {
            return Convert.ToString(ConfigurationManager.AppSettings[key]);
        }

        public static string GetConn(string key)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[key];
            if (settings != null)
            {
                return settings.ConnectionString;
            }
            return "";
        }

        public static string AccessDbNameForApp
        {
            get
            {
                return GetApp("AccessDbNameForApp");
            }
        }

        public static string AccessDbNameForWeb
        {
            get
            {
                return GetApp("AccessDbNameForWeb");
            }
        }

        public static string Aop
        {
            get
            {
                return GetApp("Aop");
            }
        }

        public static string AutoID
        {
            get
            {
                return GetApp("AutoID");
            }
        }

        public static int CacheTime
        {
            get
            {
                int result = 0;
                int.TryParse(GetApp("CacheTime"), out result);
                return result;
            }
        }

        public static string CDataLeft
        {
            get
            {
                if (_CDataLeft == null)
                {
                    _CDataLeft = GetApp("CDataLeft");
                    if (_CDataLeft == null)
                    {
                        _CDataLeft = "<![CDATA[MMS::";
                    }
                }
                return _CDataLeft;
            }
        }

        public static string CDataRight
        {
            get
            {
                if (_CDataRight == null)
                {
                    _CDataRight = GetApp("CDataRight");
                    if (_CDataRight == null)
                    {
                        _CDataRight = "::MMS]]>";
                    }
                }
                return _CDataRight;
            }
        }

        public static string Domain
        {
            get
            {
                return GetApp("Domain");
            }
        }

        public static string DtdUri
        {
            get
            {
                string app = GetApp("DtdUri");
                if ((app != null) && (app.IndexOf("http://") == -1))
                {
                    app = AppDomain.CurrentDomain.BaseDirectory + app.TrimStart(new char[] { '/' });
                }
                return app;
            }
        }

        public static bool IsWriteLog
        {
            get
            {
                bool flag;
                bool.TryParse(GetApp("IsWriteLog"), out flag);
                return flag;
            }
        }

        public static string LogConn
        {
            get
            {
                return GetConn("LogConn");
            }
        }

        public static string LogPath
        {
            get
            {
                return GetApp("LogPath");
            }
        }

        public static bool UseFileLoadXml
        {
            get
            {
                bool flag;
                bool.TryParse(GetApp("UseFileLoadXml"), out flag);
                return flag;
            }
        }
    }
}

