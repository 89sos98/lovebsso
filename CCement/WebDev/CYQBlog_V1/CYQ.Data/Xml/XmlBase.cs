namespace CYQ.Data.Xml
{
    using CYQ.Data;
    using CYQ.Data.Cache;
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    public abstract class XmlBase : IDisposable
    {
        private bool _ReadOnly;
        protected string htmlNameSpace = "http://www.w3.org/1999/xhtml";
        internal string PreXml = "preXml";
        protected CacheManage theCache = CacheManage.Instance;
        public string xmlCacheKey = string.Empty;
        public XmlDocument xmlDoc = new XmlDocument();
        public string xmlFilePath = string.Empty;
        protected XmlNamespaceManager xnm;

        public string ClearCDATA(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("#!!#", @"\").Replace("#!0!#", @"\0");
                text = text.Replace(AppConfig.CDataLeft, string.Empty).Replace(AppConfig.CDataRight, string.Empty);
            }
            return text;
        }

        protected XmlElement Create(string tag)
        {
            if (this.xnm == null)
            {
                return this.xmlDoc.CreateElement(tag);
            }
            return this.xmlDoc.CreateElement(tag, this.xnm.LookupNamespace(this.PreXml));
        }

        public virtual void Dispose()
        {
            if (this.xmlDoc != null)
            {
                if (!this.ReadOnly)
                {
                    this.xmlDoc.RemoveAll();
                }
                this.xmlDoc = null;
            }
        }

        protected XmlNode Fill(string xPath, XmlNode parent)
        {
            if (parent != null)
            {
                return parent.SelectSingleNode(xPath.Replace("//", "descendant::"), this.xnm);
            }
            return this.xmlDoc.SelectSingleNode(xPath, this.xnm);
        }

        private string GenerateKey(string absFilePath)
        {
            this.xmlFilePath = absFilePath;
            absFilePath = absFilePath.Replace(":", "").Replace("/", "").Replace(@"\", "").Replace(".", "");
            return absFilePath;
        }

        private XmlDocument GetCloneFrom(XmlDocument xDoc)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xDoc.InnerXml);
            return document;
        }

        protected string GetXPath(string tag, string attr, string value)
        {
            string str = "//" + ((this.xnm != null) ? (this.PreXml + ":") : "") + tag;
            if (attr == null)
            {
                return str;
            }
            if (value != null)
            {
                string str2 = str;
                return (str2 + "[@" + attr + "='" + value + "']");
            }
            return (str + "[@" + attr + "]");
        }

        public bool Load(string absFilePath)
        {
            bool flag = false;
            this.xmlCacheKey = this.GenerateKey(absFilePath);
            flag = this.LoadFromCache(this.xmlCacheKey);
            if (!flag)
            {
                flag = this.LoadFromFile(absFilePath);
            }
            return flag;
        }

        public bool LoadFromCache(string key)
        {
            if (!this.theCache.Contains(key))
            {
                return false;
            }
            if (this._ReadOnly)
            {
                this.xmlDoc = this.theCache.Get(key) as XmlDocument;
            }
            else
            {
                this.xmlDoc = this.GetCloneFrom(this.theCache.Get(key) as XmlDocument);
            }
            return true;
        }

        private bool LoadFromFile(string absFilePath)
        {
            bool flag;
            if (!File.Exists(absFilePath))
            {
                return false;
            }
            try
            {
                string xml = string.Empty;
                if (this.xnm != null)
                {
                    if (AppConfig.UseFileLoadXml)
                    {
                        xml = File.ReadAllText(absFilePath, Encoding.UTF8).Replace("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", AppConfig.DtdUri);
                    }
                    else
                    {
                        ResolverDtd.Resolver(ref this.xmlDoc);
                    }
                }
                if (xml != string.Empty)
                {
                    this.xmlDoc.LoadXml(xml);
                }
                else
                {
                    this.xmlDoc.Load(absFilePath);
                }
                this.xmlCacheKey = this.GenerateKey(absFilePath);
                if (!this.theCache.Contains(this.xmlCacheKey))
                {
                    this.SaveToCache(this.xmlCacheKey, this.ReadOnly);
                }
                flag = true;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message + "path:" + absFilePath);
            }
            return flag;
        }

        protected void LoadNameSpace(string nameSpace)
        {
            this.xnm = new XmlNamespaceManager(this.xmlDoc.NameTable);
            this.xnm.AddNamespace(this.PreXml, nameSpace);
        }

        public void Save(string fileName)
        {
            if (this.xmlDoc != null)
            {
                this.xmlDoc.Save(fileName);
            }
        }

        public void SaveToCache(string key, bool readOnly)
        {
            this.SaveToCache(key, readOnly, 0);
        }

        public void SaveToCache(string key, bool readOnly, int cacheTimeMinutes)
        {
            if (this.xmlDoc != null)
            {
                if (readOnly)
                {
                    this.theCache.Add(key, this.xmlDoc, this.xmlFilePath, (double) cacheTimeMinutes);
                }
                else
                {
                    this.theCache.Add(key, this.GetCloneFrom(this.xmlDoc), this.xmlFilePath, (double) cacheTimeMinutes);
                }
            }
        }

        protected XmlNodeList Select(string xPath, XmlNode parent)
        {
            if (parent != null)
            {
                return parent.SelectNodes(xPath.Replace("//", "descendant::"), this.xnm);
            }
            return this.xmlDoc.SelectNodes(xPath, this.xnm);
        }

        public string SetCDATA(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            text = text.Replace(AppConfig.CDataLeft, string.Empty).Replace(AppConfig.CDataRight, string.Empty);
            text = text.Replace(@"\", "#!!#").Replace("\0", "#!0!#");
            return (AppConfig.CDataLeft + text + AppConfig.CDataRight);
        }

        public bool CacheIsChanged
        {
            get
            {
                return this.theCache.GetHasChanged(this.xmlCacheKey);
            }
            set
            {
                this.theCache.SetChange(this.xmlCacheKey, value);
            }
        }

        public bool DocIsCache
        {
            get
            {
                return this._ReadOnly;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this._ReadOnly;
            }
            set
            {
                this._ReadOnly = value;
            }
        }
    }
}

