using CYQ.Data.Table;
using CYQ.Data.Xml;
using CYQ.Entity;
using Web.Core;
namespace Web.Extend
{
    internal class HomeTitle:CoreBase
    {
        public HomeTitle(ICore custom) : base(custom)
        {
        }
        public TitleInfo Get()
        {
            TitleInfo info = new TitleInfo("index", null, null);
            switch (UrlType)
            {
                case "index":
                    info.Title = Language.Get(IDLang.homeindex);
                    break;
                case "sys":
                    switch (GetPara(2))
                    {
                        case "reg":
                            info.Title = Language.Get(IDLang.reg);
                            break;
                        case "login":
                            info.Title = Language.Get(IDLang.login);
                            break;
                    }
                    break;
                
            }
            return info;
        }
    }
}
