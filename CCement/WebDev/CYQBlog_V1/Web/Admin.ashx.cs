using System;
using Module;
using Logic;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Entity;
using Web.Core;
namespace Web
{
    public class Admin : HttpCustom
    {
        public override string SkinPath
        {
            get
            {
                return Config.AdminSkinPath;
            }
        }
        protected override void Page_Load()
        {
            new FillIndex(this).FillHead();
            FillAdmin admin = new FillAdmin(this);
            admin.FillMenu();
            string key = GetPara(2, "index");
            switch (key)
            {
                case "index"://默认首页
                    admin.FillVisitCount();
                    break;
                case "setting":
                    admin.FillSetting();
                    break;
                case "link":
                    admin.FillLinks();
                    break;
                case "template":
                    admin.FillTemplate();
                    break;
                case "article":
                    FillAdminArticle article = new FillAdminArticle(this);
                    switch (GetPara(3, "all"))
                    {
                        case "all":
                            article.FillList(4, null);
                            break;
                        case "category":
                            article.FillList(5, Content.ClassID + "=" + GetParaInt(4));
                            break;
                        case "class":
                            article.FillClass();
                            break;
                        case "edit":
                            article.FillPost(true);
                            break;
                        case "post":
                            article.FillPost(false);
                            break;
                        case "del":
                            article.Delete();
                            break;
                    }
                    break;
                case "photo":
                    FillAdminPhoto photo = new FillAdminPhoto(this);
                    switch (GetPara(3, "all"))
                    {
                        case "all":
                            photo.FillList(4, null);
                            break;
                        case "category":
                            photo.FillList(5, Content.ClassID + "=" + GetParaInt(4));
                            break;
                        case "class":
                            photo.FillClass();
                            break;
                        case "edit":
                            photo.FillPost(true);
                            break;
                        case "post":
                            photo.FillPost(false);
                            break;
                        case "del":
                            photo.Delete();
                            break;
                    }
                    break;

            }

        }
        protected override void OnPost()
        {
            bool result = false;
            PostAdmin admin = new PostAdmin(this);
            PostAdminArticle article = new PostAdminArticle(this);
            PostAdminPhoto photo = new PostAdminPhoto(this);
            switch (ThisAction)
            {
                case "Setting":
                    result=admin.PostSetting();
                    break;
                case "PostLink":
                    result = admin.PostLink();
                    break;
                case "EditLink":
                    result = admin.PostEditLink();
                    break;
                case "PostArticle":
                    result = article.PostArticle();
                    break;
                case "EditArticle":
                    result = article.PostEditArticle();
                    break;
                case "PostPhoto":
                    result = photo.PostPhoto();
                    break;
                case "EditPhoto":
                    result = photo.PostEditPhoto();
                    break;
                case "PostTemplate":
                    result = admin.PostTemplate();
                    if (result)
                    {
                        //清除缓存
                        Context.Application.Remove(Domain + "_Skin");
                    }
                    break;
                case "PostPassword":
                    result = admin.PostEditPassword();
                    break;
                case "PostClass":
                    if (GetPara(2) == "photo")
                    {
                        result = photo.PostClass();
                    }
                    else
                    {
                        result = article.PostClass();
                    }
                    break;
                case "EditClass":
                    if (GetPara(2) == "photo")
                    {
                        result = photo.PostEditClass();
                    }
                    else
                    {
                        result = article.PostEditClass();
                    }
                    
                    break;
            }
            if (result)
            {
                Reflesh(false);
            }
            else
            {
                Document.Set(IDKey.postMessage, Language.Get(IDLang.posterror));
            }
        }
    }
}
