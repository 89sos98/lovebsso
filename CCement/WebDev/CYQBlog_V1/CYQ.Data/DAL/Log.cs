namespace CYQ.Data.DAL
{
    using CYQ.Data;
    using CYQ.Data.SQL;
    using System;
    using System.IO;
    using System.Web;

    internal class Log
    {
        private static void InsertLogToData(string message)
        {
            string logConn = AppConfig.LogConn;
            if (string.IsNullOrEmpty(logConn))
            {
                WriteLogText(message);
            }
            else
            {
                string str2 = HttpContext.Current.Request.Url.ToString();
                DbBase helper = DalAction.GetHelper(logConn);
                helper.WriteLog = false;
                try
                {
                    helper.AddParameters("@PageUrl", str2);
                    helper.AddParameters("@ErrorMessage", message);
                    if (helper.dalType == DalType.Oracle)
                    {
                        helper.ExeNonQuery(string.Format("insert into ErrorLogs(ID,PageUrl,ErrorMessage) values({0}.nextval,@PageUrl,@ErrorMessage))", SQLString.AutoID), false);
                    }
                    else
                    {
                        helper.ExeNonQuery("insert into ErrorLogs(PageUrl,ErrorMessage) values(@PageUrl,@ErrorMessage)", false);
                    }
                }
                catch
                {
                    WriteLogText(message);
                }
                finally
                {
                    helper.Dispose();
                }
            }
        }

        public static void WriteLog(string message)
        {
            if (!AppConfig.IsWriteLog)
            {
                throw new Exception("Error on DataOperator:" + message);
            }
            InsertLogToData(message);
        }

        private static void WriteLogText(string message)
        {
            if (!string.IsNullOrEmpty(AppConfig.LogPath))
            {
                try
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + AppConfig.LogPath;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string str2 = DateTime.Today.ToString("yyyyMMdd") + ".txt";
                    string str3 = path + str2;
                    File.AppendAllText(str3, "\r\n------------------------\r\nlog:" + HttpContext.Current.Request.Url.ToString() + "\r\n" + message);
                }
                catch
                {
                    throw new Exception("Error on WriteLog :" + message);
                }
            }
        }
    }
}

