using CYQ.Data.Table;
using CYQ.Entity.MySpace;
using CYQ.Data;
using Web.Core;
namespace Web.Extend
{
    internal class UserTitle:CoreBase
    {
        public UserTitle(ICore custom)
            : base(custom)
        {
        }
        public TitleInfo Get()
        {
            TitleInfo info = new TitleInfo("index",null,null);
            if (DomainUser != null)
            {
                info.Title = DomainUser.Get<string>(Users.SpaceName);
                if (string.IsNullOrEmpty(info.Title))
                {
                    info.Title = DomainUser.Get<string>(Users.UserName);
                }
                info.Description = DomainUser.Get<string>(Users.SpaceIntro);  
                switch (UrlType)
                {
                    case "photo":
                    case "admin":
                    case "guestbook":
                    case "index":
                        info.Keywords = info.Title + info.Split + DomainUser.Get<string>(Users.UserName);
                        break;
                    case "article":
                        #region 文章标题处理
                        
                        string key = GetPara(2, "");
                        switch (key)
                        {
                            case "detail":
                                using (MAction action = new MAction(TableNames.Blog_Content))
                                {
                                    if (action.Fill(GetParaInt(3)))
                                    {
                                        info.Title = action.Get<string>(Content.Title) + info.Split + info.Title;
                                        info.Keywords = action.Get<string>(Content.Tag);
                                        info.Description = action.Get<string>(Content.Abstract);
                                    }
                                }
                                break;
                            case "category":
                                using (MAction action = new MAction(TableNames.Blog_Class))
                                {
                                    if (action.Fill(GetParaInt(3)))
                                    {
                                        info.Title = action.Get<string>(Class.Name) + info.Split + info.Title;
                                        info.Keywords = action.Get<string>(Class.Name);
                                    }
                                }
                                break;
                            case "list":
                                info.Title = GetParaInt(3) + info.Split + GetParaInt(4) + info.Split + info.Title;
                                info.Keywords = GetParaInt(3) + info.Split + GetParaInt(4);
                                break;
                        }
                        break;

                        #endregion
                }
            }
            return info;
        }

       
    }
}
