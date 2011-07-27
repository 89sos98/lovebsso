namespace CYQ.Data.Cache
{
    using System;
    using System.Web.Caching;

    internal class CacheDependencyInfo
    {
        private DateTime CacheChangeTime = DateTime.MinValue;
        private CacheDependency FileDependency;
        public bool UserChange;

        public CacheDependencyInfo(CacheDependency dependency)
        {
            if (dependency != null)
            {
                this.FileDependency = dependency;
                this.CacheChangeTime = this.FileDependency.UtcLastModified;
            }
        }

        public void UserSetChange(bool change)
        {
            this.UserChange = !this.IsChanged && change;
        }

        public bool IsChanged
        {
            get
            {
                if ((this.FileDependency == null) || (!this.FileDependency.HasChanged && !(this.CacheChangeTime != this.FileDependency.UtcLastModified)))
                {
                    return false;
                }
                this.CacheChangeTime = this.FileDependency.UtcLastModified;
                return true;
            }
        }
    }
}

