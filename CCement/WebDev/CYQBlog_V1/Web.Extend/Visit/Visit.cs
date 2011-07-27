using System;
using System.Web;
using Web.Core;
using System.Collections.Generic;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Data.Cache;
namespace Web.Extend
{
    internal class Visit:CoreBase
    {
        CacheManage Cache = CacheManage.Instance;
        public Visit(ICore custom) : base(custom) { }
        public List<string> UserVisitList
        {
            get
            {
                string key="UserVisitList";
                if (!Cache.Contains(key))
                {
                    Cache.Add(key, new List<string>(), null, 60 * 24);
                }
                return Cache.Get(key) as List<string>;
            }
        }
        public List<string> ContentVisitList
        {
            get
            {
                string key = "ContentVisitList";
                if (!Cache.Contains(key))
                {
                    Cache.Add(key, new List<string>(), null, 60 * 24);
                }
                return Cache.Get(key) as List<string>;
            }
        }
        public void SetUserVisit()
        {
            //判断是否能加1
            if (IsCanAddUserVisit())
            {
                Add(0);
            }
        }
        public void SetContentVisit()
        {
            //判断是否能加1
            if (IsCanAddContentVisit())
            {
                Add(1);
            }
        }
        private bool IsCanAddUserVisit()
        {
            List<string> dic = UserVisitList;
            string item = DomainID + "_" + GetSesstionID();
            if (dic.Contains(item))
            {
                if (dic.Count > 300)
                {
                    dic.RemoveRange(1, 200);
                }
                return false;
            }
            else
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["id"] = 0;
                }
                dic.Add(item);
                return true;
            }
            
        }
        private bool IsCanAddContentVisit()
        {
            List<string> dic = ContentVisitList;
            string item = GetParaInt(3) + "_" + GetSesstionID();
            if (dic.Contains(item))
            {
                if (dic.Count > 500)
                {
                    dic.RemoveRange(1, 300);
                }
                return false;
            }
            else
            {
                dic.Add(item);
                return true;
            }
        }

        private void Add(int key)
        {
            string sql = string.Empty;
            switch (key)
            {
                case 0:
                    sql = string.Format(CustomSQL.UserVisitAdd, DomainID);
                    break;
                case 1:
                    sql = string.Format(CustomSQL.ContentVisitAdd, GetParaInt(3));
                    break;
            }
            if (sql == string.Empty)
            { return; }
            //当前用户计数器+1;
            using (MProc proc = new MProc(sql))
            {
                proc.ExeNonQuery();
            }

        }
        public string GetSesstionID()
        {
            if (HttpContext.Current.Session == null)
            {
                
                string sesstionID = string.Empty;
                string cookieName = "VisitSesstionID";
                HttpCookie cookie = Request.Cookies[cookieName];
                if (cookie == null)
                {
                    sesstionID = Guid.NewGuid().ToString();
                    cookie = new HttpCookie(cookieName, sesstionID);
                    cookie.Domain = Config.Domain;
                    cookie.Expires = DateTime.Now.AddMinutes(10);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    sesstionID = cookie.Value;
                }
                return sesstionID;
            }
            else
            {
                return HttpContext.Current.Session.SessionID;
            }
        }
    }
}
