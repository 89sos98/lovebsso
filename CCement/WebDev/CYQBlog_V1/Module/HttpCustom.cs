using System;
using System.Web;
using System.Web.SessionState;
using CYQ.Data.Xml;
using CYQ.Data.Table;
using CYQ.Data;
using CYQ.Data.Cache;
using Web.Core;
using CYQ.Entity;
using CYQ.Entity.MySpace;
namespace Module
{
    public abstract class HttpCustom: IHttpHandler, IRequiresSessionState, ICore
    {
        #region 常用属性
        private HttpContext _Context;
        /// <summary>
        /// 当前上下文
        /// </summary>
        public HttpContext Context
        {
            get
            {
                return _Context;
            }
        }
        private HttpRequest _Request;
        /// <summary>
        /// 当前上下文-请求对象
        /// </summary>
        public HttpRequest Request
        {
            get
            {
                return _Request;
            }
        }
        private XmlHelper _Document;
        /// <summary>
        /// 当前Xml文档对象
        /// </summary>
        public XmlHelper Document
        {
            get
            {
                return _Document;
            }
        }
        public int LoginUserID
        {
            get
            {
                if (_Login.IsOnline(false))
                {
                    return _Login.UserInfo.Get<int>(Users.ID);
                }
                return 0;
            }
        }
        private MutilLanguage _Language;
        /// <summary>
        /// 当前语言文件对象
        /// </summary>
        public MutilLanguage Language
        {
            get
            {
                return _Language;
            }
        }
        private UserLogin _Login;
        /// <summary>
        /// 当前系统登陆帮助对象
        /// </summary>
        public UserLogin UserAction
        {
            get
            {
                return _Login;
            }
        }
        protected string ThisAction;
        private string _UrlPara;
        /// <summary>
        /// 请求的Url参数，即： context.Request.Url.PathAndQuery;
        /// </summary>
        public string UrlPara
        {
            get
            {
                return _UrlPara;
            }
        }
        private string _UrlType;
        /// <summary>
        /// 请求的url中首分类，即如index、article、photo、admin等
        /// </summary>
        public string UrlType
        {
            get
            {
                return _UrlType;
            }
        }
        private string _UrlReferrer;
        /// <summary>
        /// 外部来源请求的Url
        /// </summary>
        public string UrlReferrer
        {
            get
            {
                return _UrlReferrer;
            }
        }
        private string _Domain;
        /// <summary>
        /// 当前用户域名
        /// </summary>
        public string Domain
        {
            get
            {
                return _Domain;
            }
        }
        private int _DomainID;
        public int DomainID
        {
            get
            {
                if (_DomainID == 0)
                {
                    SetDomainID();
                }
                return _DomainID;
            }
        }
        private string _SkinPath;
        public virtual string SkinPath
        {
            get
            {
                if (string.IsNullOrEmpty(_SkinPath))
                {
                    SetSkinPath();
                }
                return _SkinPath;
            }
        }
        public virtual string LangSkinPath
        {
            get
            {
                if (string.IsNullOrEmpty(_SkinPath))
                {
                    SetSkinPath();
                }
                return _SkinPath;
            }
        }
        private MDataRow _DomainUser;
        public MDataRow DomainUser
        {
            get
            {
                if (_DomainUser == null)
                {
                    _DomainUser = _Login.Get(_Domain);
                }
                return _DomainUser;
            }
        }
        public virtual bool AllowCache
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region 页页处理流程 IHttpHandler 成员

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Page_Init(context);
            Page_PreLoad();
            if (_Document.DocIsCache)//从页面读取缓存
            {
                Page_OnCache();//引发缓存用户操作区
            }
            else
            {
                Page_Load();//此用户操作区
            }
            Page_Render();
            Page_PreEnd();
            Page_End();
        }

        #endregion

        #region Page_Init 初始化页面参数
        private void Page_Init(HttpContext context)
        {
           
            context.Response.ContentType = "text/html";
            _Context = context;
            _Request = context.Request;
            if (_Request.QueryString.Count > 0)
            {
                _UrlPara = _Request.QueryString["para"];
                _Domain = _Request.QueryString["u"];
                _UrlType = _Request.QueryString["type"];
                _UrlReferrer = _Request.QueryString["urlref"];
                InitObject(context);
            }
        }
        private void InitObject(HttpContext context)
        {
           
            _Document = new XmlHelper(true);
            _Login = new UserLogin();
            _Language = new MutilLanguage(MapPath(LangSkinPath + IDPage.Language));
        }
        protected virtual void OnPost(){ }
     
        private bool LoadFrame(string htmlPath)
        {
            if (!string.IsNullOrEmpty(htmlPath))
            {
                if (!_Document.Load(MapPath(htmlPath)))
                {
                    throw new Exception("加html出错:" + htmlPath);
                }
                return true;
            }
            return false;
        }
        private bool LoadFrameCache(string httpUrl)
        {
            if (!string.IsNullOrEmpty(httpUrl))
            {
                _Document.xmlCacheKey=httpUrl;
                if (!_Document.CacheIsChanged)
                {
                    _Document.ReadOnly = true;
                    if (!_Document.LoadFromCache(httpUrl))
                    {
                        _Document.ReadOnly = false;
                    }
                    return _Document.ReadOnly;
                }
            }
            return false;
        }
        #endregion

        #region Page_PreLoad 页面预加载Html/中英文翻译


        private void Page_PreLoad()
        {
            //尝试读取三级缓存，页面缓存
            if (Config.PageCacheTime > 0 && AllowCache)
            {
               LoadFrameCache(Request.Url.PathAndQuery);
               
            }
            if (!_Document.DocIsCache)//读取缓存失败
            {
                if (LoadFrame(PageLoadUrl.GetHtmlUrl(UrlPara, UrlType, SkinPath)))//正常加载html
                {
                    if (!_Document.CacheIsChanged)
                    {
                        CssAndImg(_Document);//处理Css/图片路径
                    }
                    ReplaceCacheNode();//替换Cache节点，二级缓存
                    ReplaceTitle();//替换标题                   
                }
            }
            Translate(_Document);//统一进行翻译
            if (_Request.Form.Count > 0)
            {
                if (!string.IsNullOrEmpty(_Request.Form["myAct"]))
                {
                    ThisAction = _Request.Form["myAct"];
                    OnPost();
                }
            }
        }
        #region 内部逻辑方法
        private void Translate(XmlHelper doc)
        {
            System.Xml.XmlNodeList list = doc.GetList("*", "key");
            if (list != null && list.Count > 0)
            {
                string key = null;
                for (int i = 0; i < list.Count; i++)
                {
                    key = list[i].Attributes["key"].Value;
                    list[i].InnerXml = _Language.Get(key);
                }
            }
            list = doc.GetList("*", "keyvalue");
            if (list != null && list.Count > 0)
            {
                string key = null;
                for (int i = 0; i < list.Count; i++)
                {
                    key = list[i].Attributes["keyvalue"].Value;
                    list[i].Attributes["value"].InnerXml = _Language.Get(key);
                }
            }
        }
        private void CssAndImg(XmlHelper doc)
        {
            System.Xml.XmlNodeList list = doc.GetList("link", "href");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Attributes["href"].Value = SkinPath + list[i].Attributes["href"].Value;
                }
            }
            list = doc.GetList("img", "src");
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Attributes["src"].Value = SkinPath + list[i].Attributes["src"].Value;
                }
            }

        }
        private void ReplaceCacheNode()
        {
            XmlHelper commonCache = new XmlHelper(true);
            commonCache.ReadOnly = true;
            if (commonCache.Load(MapPath(SkinPath + IDPage.CommonCache)))
            {
                if (!commonCache.CacheIsChanged)
                {
                    CssAndImg(commonCache);//处理Css/图片路径
                    commonCache.CacheIsChanged = true;
                }
                //节点替换
                System.Xml.XmlNodeList list = _Document.GetList("*", "cachefrom");
                if (list != null && list.Count > 0)
                {
                    string id = string.Empty;
                    for (int i = 0; i < list.Count; i++)
                    {
                        id = list[i].Attributes["cachefrom"].Value;
                        _Document.ReplaceNode(commonCache.GetByID(id), list[i]);
                    }
                }
            }
            commonCache.Dispose();
        }
        private void ReplaceTitle()
        {
            Web.Extend.TitleInfo info = new Web.Extend.Action(this).GetTitle();
            if (info != null)
            {
                System.Xml.XmlNodeList nodeList = Document.GetList("title");
                if (nodeList != null && nodeList.Count > 0)
                {
                    System.Xml.XmlNode titleNode = nodeList[0];
                    Document.Set(titleNode, SetType.InnerText, info.Title);
                    if (!string.IsNullOrEmpty(info.Description))
                    {
                        Document.InsertAfter(Document.CreateNode("meta", "", "name", "description", "content", info.Description), titleNode);
                    }
                    if (!string.IsNullOrEmpty(info.Keywords))
                    {
                        Document.InsertAfter(Document.CreateNode("meta", "", "name", "keywords", "content", info.Keywords), titleNode);
                    }
                }
            }
        }
        private void AddAspxToLinkUrl()
        {

            System.Xml.XmlNodeList list = _Document.GetList("a", "href");
            if (list != null && list.Count > 0)
            {
                string urlAspx = Config.UrlAspx;
                string href = string.Empty;
                foreach (System.Xml.XmlNode node in list)
                {
                    href = node.Attributes["href"].Value;
                    if (href.IndexOf(urlAspx) == -1)
                    {
                        node.Attributes["href"].Value = href.TrimEnd('/') + Config.UrlAspx;
                    }
                }
            }
        }
        
        #endregion
       
        #endregion

        #region Page_Load | Page_OnCache 加载页面/引发缓存 各页面实现
        protected abstract void Page_Load();

        protected virtual void Page_OnCache() 
        {
            //开启三级缓存，页面缓存时处理
        }
        
        protected virtual void Page_Render()
        { }
        #endregion

        #region Page_PreEnd 页面输出之前,处理是否需要缓存/是否自定后缀转向
        private void Page_PreEnd()
        {
            new Web.Extend.Action(this).SetVisit();
            if (!_Document.DocIsCache)//非缓存文档
            {
                if (!string.IsNullOrEmpty(Config.UrlAspx))
                {
                    AddAspxToLinkUrl();
                }
               
            }
            //开启三级缓存，页面缓存
            if (Config.PageCacheTime > 0 && AllowCache && !_Document.DocIsCache)
            {
                _Document.SaveToCache(Request.Url.PathAndQuery, true, Config.PageCacheTime);
                _Document.ReadOnly = true;//设置为只读,免去Disponse时清空缓存内容
            }
            
        }
        #endregion

        #region Page_End 输出页面到客户端
        private void Page_End()
        {
            if (_Document != null && _Document.xmlDoc.InnerXml.Length > 0)
            {
                string outXml = _Document.xmlDoc.InnerXml.Replace("<![CDATA[MMS::", string.Empty).Replace("::MMS]]>", string.Empty).Replace("xmlns=\"\"", string.Empty);
                int index = outXml.IndexOf('>') + 1;
                if (index > 10)
                {
                    outXml = outXml.Replace(outXml.Substring(0, index), "<!doctype html>");
                }
                _Context.Response.Write(outXml);
            }
            _Document.Dispose();
            _Login.Dispose();
            _Language.Dispose();
        }
        #endregion

        #region 内部私有方法
        private void SetDomainID()
        {
            if (!string.IsNullOrEmpty(_Domain))
            {
                int.TryParse(Convert.ToString(_Context.Application.Get(_Domain)), out _DomainID);
                if (_DomainID == 0 && DomainUser != null)
                {
                    _DomainID = DomainUser.Get<int>(Users.ID);
                    SetCache(_Domain,_DomainID);
                }
            }
        }
        private void SetSkinPath()
        {
            if (!string.IsNullOrEmpty(_Domain) && DomainID>0)
            {
                _SkinPath = Convert.ToString(_Context.Application.Get(_Domain + "_Skin"));
                if (string.IsNullOrEmpty(_SkinPath) && DomainUser != null)
                {
                    int skinID = DomainUser.Get<int>(Users.SkinID);
                    using (MAction action = new MAction(TableNames.Blog_Skin))
                    {
                        if (action.Fill(skinID))
                        {
                            _SkinPath = action.Get<string>(Skin.SkinPath);
                            SetCache(_Domain + "_Skin", _SkinPath);
                        }
                    }
                }
            }
            else
            {
                _SkinPath=Config.SystemSkinPath;
            }
        }
        private void SetCache(string name, object value)
        {
            if (_Context.Application.Get(name) == null)
            {
                _Context.Application.Lock();
                _Context.Application.Add(name,value);
                _Context.Application.UnLock();
            }
            else
            {
                _Context.Application.Lock();
                _Context.Application.Set(name,value);
                _Context.Application.UnLock();

            }
        }
        #endregion

        #region 公用方法[供页面调用]
        public bool Click(string buttonName)
        {
            return Request.Form[buttonName] != null;
        }
        public string Get(string name,params string[] defaultValue)
        {
            string value=Request.Form[name];
            if (!string.IsNullOrEmpty(value))
            {
                return value.Trim();
            }
            else if(defaultValue.Length>0)
            {
                return defaultValue[0];
            }
            return value;
        }
        public int GetParaInt(int num)
        {
            int result = 0;
            int.TryParse(GetPara(num), out result);
            return result;
        }
        public string GetPara(int num)
        {
            return Tool.Common.GetPara(UrlPara, num);
        }
        public string GetPara(int num,string defaultValue)
        {
            return Tool.Common.GetPara(UrlPara, num, defaultValue);
        }
        public void GoTo(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }
        public void Reflesh(bool clearCache)
        {
            if (clearCache)
            {
                _Document.xmlCacheKey = Request.Url.PathAndQuery;
                _Document.CacheIsChanged = true;
            }
            GoTo(Convert.ToString(Request.UrlReferrer));
        }
        public string MapPath(string path)
        {
            return HttpContext.Current.Server.MapPath(path.TrimStart('/'));
        }
        public string SetCDATA(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            return AppConfig.CDataLeft + text + AppConfig.CDataRight;
        }
        #endregion
    }

}
