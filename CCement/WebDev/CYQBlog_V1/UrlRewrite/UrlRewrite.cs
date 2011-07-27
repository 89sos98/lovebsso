using System;
using System.Web;

namespace CYQ.Data
{
    public class UrlConfig
    {
        /// <summary>
        /// 获取子目录[如果将网站放于虚拟主机的子目录中，获取子目录名称]
        /// </summary>
        public static string VirtualPath
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["VirtualPath"];
            }
        }
        /// <summary>
        /// 获取域名前缀名称[如blog.cyqdata.com中的blog]
        /// </summary>
        public static string Www
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["Www"];
            }
        }
        /// <summary>
        /// 自定义网站后缀[如www.cyqdata.com/index.shtml中的.shtml]
        /// </summary>
        public static string UrlAspx
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["UrlAspx"];
            }
        }
    }
    public class UrlRewrite : IHttpModule
    {

        public void HttpUrlRewrite(HttpContext context)
        {
            string host = "www.ajeelee.com";// context.Request.Url.Host.ToLower();
            string url = context.Request.Url.PathAndQuery;

            if (UrlConfig.VirtualPath != null)//有虚拟子目录时,路径先替换掉
            { url = url.Replace(UrlConfig.VirtualPath, ""); }

            //xp下带后缀Url转向测试
            string urlAspx = UrlConfig.UrlAspx;//检测是否启用自定义后缀
            bool withAspx = false;
            if (!string.IsNullOrEmpty(urlAspx))
            {
                withAspx = url.ToLower().IndexOf(urlAspx) > -1;
            }
            else
            {
                withAspx = url.ToLower().IndexOf(".mdb") > -1 || url.ToLower().IndexOf(".ashx") > -1;
            }
            if (!withAspx && url.IndexOf('.') > -1)//文件夹方式访问网址 
            {
                return;//允许直接访问原有路径
            }

            if (urlAspx != null && urlAspx.Length > 0)
            {
                url = url.Replace(urlAspx, "");
            }

            string firstKey = null, classKey = null, para = null, homeKey = null;
            string www = UrlConfig.Www;//检测是否启用二级域名或三级域名
            if (string.IsNullOrEmpty(www))
            {
                www = "www";
            }
            bool hasWww = host.IndexOf(www + ".") > -1;//是否系统网站
            firstKey = hasWww ? GetPara(url, 1, "") : host.Substring(0, host.IndexOf('.'));
            homeKey = IsHome(firstKey);//非用户,系统使用域名
            classKey = (homeKey == string.Empty) ? GetPara(url, (hasWww ? 2 : 1), "index") : homeKey;
            if (homeKey != string.Empty || firstKey.Length == 0)
            {
                para = url;
            }
            else
            {
                para = url.TrimStart('/').Remove(0, firstKey.Length);
            }
            string userName;
            context.RewritePath(GetASHX(classKey, firstKey, out userName), null, string.Format("u={0}&type={1}&para={2}&urlref={3}", userName, classKey, para, context.Request.UrlReferrer));

        }




        #region IHttpModule 成员

        public void Dispose()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpUrlRewrite(app.Context);
        }

        #endregion

        #region 其它方法
        /// <summary>
        /// Url分隔截取
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        public string GetPara(string sourceUrl, int num, string defaultValue)
        {
            if (string.IsNullOrEmpty(sourceUrl) || sourceUrl == "/")
            {
                return defaultValue;
            }
            if (sourceUrl.Substring(0, 1) != "/")
            {
                sourceUrl = "/" + sourceUrl;
            }
            string[] para = sourceUrl.TrimEnd('/').Split('/');
            if (para.Length > num && num > 0)
            {
                return para[num].ToLower();
            }
            return defaultValue;
        }

        private string GetASHX(string key, string firstKey, out string userName)
        {
            userName = string.Empty;
            string ashx = "~/home.ashx";
            switch (key)
            {
                //用户
                case "index":
                case "admin":
                case "article":
                case "photo":
                    if (firstKey == "index" || firstKey == "default" || firstKey == key || firstKey.Length < 5 || firstKey.IndexOf('.') > -1)
                    {
                        key = "home";
                        break;
                    }

                    userName = firstKey;
                    break;
                //系统

                case "home":
                case "error":
                    break;
                case "lang":
                    key = "language";
                    break;
                case "sys":
                    key = "reglogin";
                    break;
                default:
                    key = "error";
                    break;
            }
            return ashx.Replace("home", key);
        }

        /// <summary>
        /// 是否系统Home的目录路径
        /// </summary>
        private string IsHome(string key)
        {
            switch (key.ToLower())
            {
                case "":
                    return "home";
                case "lang":
                case "index":
                case "error":
                case "sys":
                    return key;
                default:
                    if (key.Length < 5)
                    {
                        return key;
                    }
                    break;
            }
            return string.Empty;
        }
        #endregion
    }
}
