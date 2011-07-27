namespace CYQ.Data.Xml
{
    using CYQ.Data;
    using System;
    using System.Web;
    using System.Xml;

    public class MutilLanguage : IDisposable
    {
        private XmlHelper helper = new XmlHelper(true);
        public LanguageKey lanKey;

        public MutilLanguage(string filePath)
        {
            if (!this.helper.Load(filePath))
            {
                throw new Exception("加载语言文件失败:" + filePath);
            }
            this.GetFromCookie();
        }

        public void Dispose()
        {
            this.helper.Dispose();
        }

        public string Get(object lanID)
        {
            XmlNode byID = this.helper.GetByID(Convert.ToString(lanID));
            if (byID == null)
            {
                return "no find";
            }
            if (this.lanKey != LanguageKey.China)
            {
                string str = this.lanKey.ToString().ToLower().Substring(0, 3);
                if (byID.Attributes[str] != null)
                {
                    return byID.Attributes[str].Value;
                }
            }
            return byID.InnerXml;
        }

        private void GetFromCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[AppConfig.Domain + "_LanKey"];
            if (cookie != null)
            {
                try
                {
                    this.lanKey = (LanguageKey) Enum.Parse(typeof(LanguageKey), cookie.Value);
                }
                catch
                {
                    this.lanKey = LanguageKey.China;
                }
            }
        }

        public void SetToCookie(LanguageKey lanKey)
        {
            this.SetToCookie(lanKey.ToString());
        }

        public void SetToCookie(string lanKey)
        {
            HttpCookie cookie = new HttpCookie(AppConfig.Domain + "_LanKey", lanKey);
            cookie.Domain = AppConfig.Domain;
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}

