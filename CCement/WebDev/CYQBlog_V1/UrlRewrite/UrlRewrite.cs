using System;
using System.Web;

namespace CYQ.Data
{
    public class UrlConfig
    {
        /// <summary>
        /// ��ȡ��Ŀ¼[�������վ����������������Ŀ¼�У���ȡ��Ŀ¼����]
        /// </summary>
        public static string VirtualPath
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["VirtualPath"];
            }
        }
        /// <summary>
        /// ��ȡ����ǰ׺����[��blog.cyqdata.com�е�blog]
        /// </summary>
        public static string Www
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["Www"];
            }
        }
        /// <summary>
        /// �Զ�����վ��׺[��www.cyqdata.com/index.shtml�е�.shtml]
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

            if (UrlConfig.VirtualPath != null)//��������Ŀ¼ʱ,·�����滻��
            { url = url.Replace(UrlConfig.VirtualPath, ""); }

            //xp�´���׺Urlת�����
            string urlAspx = UrlConfig.UrlAspx;//����Ƿ������Զ����׺
            bool withAspx = false;
            if (!string.IsNullOrEmpty(urlAspx))
            {
                withAspx = url.ToLower().IndexOf(urlAspx) > -1;
            }
            else
            {
                withAspx = url.ToLower().IndexOf(".mdb") > -1 || url.ToLower().IndexOf(".ashx") > -1;
            }
            if (!withAspx && url.IndexOf('.') > -1)//�ļ��з�ʽ������ַ 
            {
                return;//����ֱ�ӷ���ԭ��·��
            }

            if (urlAspx != null && urlAspx.Length > 0)
            {
                url = url.Replace(urlAspx, "");
            }

            string firstKey = null, classKey = null, para = null, homeKey = null;
            string www = UrlConfig.Www;//����Ƿ����ö�����������������
            if (string.IsNullOrEmpty(www))
            {
                www = "www";
            }
            bool hasWww = host.IndexOf(www + ".") > -1;//�Ƿ�ϵͳ��վ
            firstKey = hasWww ? GetPara(url, 1, "") : host.Substring(0, host.IndexOf('.'));
            homeKey = IsHome(firstKey);//���û�,ϵͳʹ������
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




        #region IHttpModule ��Ա

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

        #region ��������
        /// <summary>
        /// Url�ָ���ȡ
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
                //�û�
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
                //ϵͳ

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
        /// �Ƿ�ϵͳHome��Ŀ¼·��
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
