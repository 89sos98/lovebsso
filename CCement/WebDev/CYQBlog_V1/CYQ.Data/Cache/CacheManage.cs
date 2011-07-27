namespace CYQ.Data.Cache
{
    using CYQ.Data;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;

    public class CacheManage
    {
        private Dictionary<string, CacheDependencyInfo> cacheState = new Dictionary<string, CacheDependencyInfo>();
        private readonly HttpContext H = new HttpContext(new HttpRequest("Null.File", "http://cyq1162.cnblogs.com", string.Empty), new HttpResponse(null));
        private Cache theCache;

        private CacheManage()
        {
            this.theCache = this.H.Cache;
        }

        public void Add(string key, object value)
        {
            this.Add(key, value, null);
        }

        public void Add(string key, object value, string filePath)
        {
            if (!this.Contains(key))
            {
                this.Add(key, value, filePath, 0.0);
            }
        }

        public void Add(string key, object value, string filePath, double cacheTimeMinutes)
        {
            if (!this.Contains(key))
            {
                this.Insert(key, value, filePath, cacheTimeMinutes);
            }
        }

        public bool Contains(string key)
        {
            return ((this.theCache != null) && (this.theCache[key] != null));
        }

        public object Get(string key)
        {
            if (this.Contains(key))
            {
                return this.theCache[key];
            }
            return null;
        }

        public bool GetHasChanged(string key)
        {
            if (this.cacheState.ContainsKey(key) && this.Contains(key))
            {
                CacheDependencyInfo info = this.cacheState[key];
                if (info != null)
                {
                    return (!info.IsChanged && info.UserChange);
                }
            }
            return false;
        }

        private void Insert(string key, object value, string filePath, double cacheTimeMinutes)
        {
            CacheDependency dependencies = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                dependencies = new CacheDependency(filePath);
            }
            double cacheTime = cacheTimeMinutes;
            if (cacheTimeMinutes == 0.0)
            {
                cacheTime = AppConfig.CacheTime;
            }
            this.theCache.Insert(key, value, dependencies, DateTime.Now.AddMinutes((cacheTime == 0.0) ? 20.0 : cacheTime), TimeSpan.Zero, CacheItemPriority.Normal, null);
            if (this.cacheState.ContainsKey(key))
            {
                this.cacheState.Remove(key);
            }
            CacheDependencyInfo info = new CacheDependencyInfo(dependencies);
            this.cacheState.Add(key, info);
        }

        public void Remove(string key)
        {
            if (this.Contains(key))
            {
                this.theCache.Remove(key);
                this.cacheState.Remove(key);
            }
        }

        public void SetChange(string key, bool change)
        {
            if (this.cacheState.ContainsKey(key) && this.Contains(key))
            {
                CacheDependencyInfo info = this.cacheState[key];
                if (info != null)
                {
                    info.UserSetChange(change);
                }
            }
        }

        public int Count
        {
            get
            {
                if (this.theCache != null)
                {
                    return this.theCache.Count;
                }
                return 0;
            }
        }

        public static CacheManage Instance
        {
            get
            {
                return Shell.instance;
            }
        }

        private class Shell
        {
            internal static readonly CacheManage instance = new CacheManage();
        }
    }
}

