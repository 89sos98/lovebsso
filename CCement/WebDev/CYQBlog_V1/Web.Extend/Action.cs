using CYQ.Data.Table;
using CYQ.Data.Xml;
using CYQ.Entity;
using Web.Core;
namespace Web.Extend
{
    public class Action:CoreBase
    {
        public Action(ICore custom) : base(custom)  {  }
        public TitleInfo GetTitle()
        {
            TitleInfo info = new TitleInfo("秋色园", "专业提供网站建站程序", "www.cyqdata.com");
            string key = (UrlType == Domain) ? "sys" : UrlType;
            switch (key)
            {
                case "error":
                case "home":
                case "sys":
                    info = new  HomeTitle(this).Get();
                    break;
                case "index":
                case "article":
                case "photo":
                case "admin":
                    info = new UserTitle(this).Get();
                    break;
            }
            info.Title += info.Split + Language.Get(IDLang.sitename);
            info.ClearHtml();
            return info;
        }
        public void SetVisit()
        {
            Visit visit=null;
            int visitCount = 0;
            switch (UrlType)
            {
                case "index":
                case "article":
                case "photo":
                    visitCount++;
                    break;
            }
            switch (UrlType)
            {
                case "article":
                case "photo":
                    visitCount++;
                    break;
            }
            if (visitCount > 0)
            {
                visit = new Visit(this);
                visit.SetUserVisit();
            }
            if (visitCount > 1)
            {
                visit.SetContentVisit();
            }
        }
    }
}
