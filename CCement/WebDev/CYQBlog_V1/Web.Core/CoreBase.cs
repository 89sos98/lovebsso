using System;
using CYQ.Data.Xml;
using CYQ.Data.Table;
using Web.Core;
using System.Web;
using CYQ.Entity.MySpace;
namespace Web.Core
{
    public abstract class CoreBase : ICore
    {
        ICore _ICore;
        public CoreBase(ICore custom)
        {
            _ICore = custom;
        }
        public CoreBase(ICore custom, bool validateLogin)
        {
            _ICore = custom;
            if (validateLogin)
            {
                if (!UserAction.IsOnline(true) || UserAction.UserInfo.Get<int>(Users.ID) != DomainID)
                {
                    GoTo(Config.HttpHost + "/" + UserAction.UserInfo.Get<string>(Users.UserName) + "/admin");
                }
            }
        }
        public bool HadWww
        {
            get
            {
                return _ICore.Request.Url.Host.IndexOf("www.") == 0 || _ICore.Request.Url.Host.IndexOf(Config.Www) == 0;
            }
        }
        /// <summary>
        /// Url前缀
        /// </summary>
        public string UrlPrefix
        {
            get
            {
                return HadWww ? "/" + Domain : "";
            }
        }

        #region IHttpCustom 成员

        public XmlHelper Document
        {
            get
            {
                return _ICore.Document;
            }
        }

        public MDataRow DomainUser
        {
            get
            {
                return _ICore.DomainUser;
            }
        }

        public UserLogin UserAction
        {
            get
            {
                return _ICore.UserAction;
            }
        }

        public MutilLanguage Language
        {
            get
            {
                return _ICore.Language;
            }
        }

        public string Domain
        {
            get
            {
                return _ICore.Domain;
            }
        }
        public int DomainID
        {
            get
            {
                return _ICore.DomainID;
            }
        }
        public int LoginUserID
        {
            get
            {
                return _ICore.LoginUserID;
            }
        }
        public HttpRequest Request
        {
            get
            {
                return _ICore.Request;
            }
        }

        public string UrlPara
        {
            get
            {
                return _ICore.UrlPara;
            }
        }
        public string UrlType
        {
            get
            {
                return _ICore.UrlType;
            }
        }
        public string UrlReferrer
        {
            get
            {
                return _ICore.UrlReferrer;
            }
        }
        public int GetParaInt(int num)
        {
            return _ICore.GetParaInt(num);
        }
        public string GetPara(int num)
        {
            return _ICore.GetPara(num);
        }
        public string GetPara(int num, string defaultValue)
        {
            return _ICore.GetPara(num, defaultValue);
        }
        public void GoTo(string url)
        {
            _ICore.GoTo(url);
        }
        public string Get(string name, params string[] defaultValue)
        {
            return _ICore.Get(name, defaultValue);
        }
        public string MapPath(string path)
        {
            return _ICore.MapPath(path);
        }
        public string SetCDATA(string text)
        {
            return _ICore.SetCDATA(text);
        }
        #endregion

        public string GetPagerUrl(int num)
        {
            int paraLength = UrlPara.Trim('/') == "" ? 0 : UrlPara.Trim('/').Split('/').Length;
            if (num - paraLength == 2)
            {
                if (UrlType == "index")//用户首页
                {
                    return "/index/{0}";
                }
                return UrlPara.TrimEnd('/') + "/all/{0}";
            }
            else if (num - paraLength == 1)//不够位，补数
            {
                return UrlPara.TrimEnd('/') + "/{0}";
            }
            string para = GetPara(num, "");
            int index = UrlPara.LastIndexOf(para);
            return UrlPara.Remove(index).TrimEnd('/') + "/{0}";

        }
    }
}
