using System;
using CYQ.Entity;
using Tool;
using Web.Core;

namespace Module
{
    internal class PageLoadUrl
    {
        public static string GetHtmlUrl(string urlPara,string type,string skinPath)
        {
            string htmlPath = IDPage.Error;
            switch (type)//分支到不同的处理程序ashx中
            {
                case "home":
                    htmlPath = LoadHome(urlPara);
                    break;
                case "sys":
                    htmlPath = LoadSys(urlPara);
                    break;
                case "lang":
                    return string.Empty;
                case "index":
                    htmlPath = LoadIndex(urlPara);
                    break;
                case "article":
                    htmlPath = LoadArticle(urlPara);
                    break;
                case "photo":
                    htmlPath = LoadPhoto(urlPara);
                    break;
                case "admin":
                    htmlPath = LoadAdmin(urlPara);
                    break;
                case "error":
                    htmlPath = LoadError(urlPara);
                    break;
                
            }
            if (htmlPath == IDPage.Error)
            {
                //if (type != "error")
                //{
                //    System.Web.HttpContext.Current.Response.Redirect(Config.HttpHost + "/error/"+type);
                //}
                return Config.SystemSkinPath+htmlPath;
            }
            else if (htmlPath == string.Empty)
            {
                return string.Empty;
            }
            return skinPath+htmlPath;
        }

        #region 加载不同的html
        /// <summary>
        /// 加载系统首页
        /// </summary>
        private static string LoadHome(string url)
        {
            string key = Common.GetPara(url, 1,"all");
            switch (key)
            {
                case "all":
                case "index":
                    return IDPage.Index;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载登陆/注册/退出/访问等页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string LoadSys(string url)
        {
            string key = Common.GetPara(url, 2, "");
            switch (key)
            {
                case "reg":
                    return IDPage.Reg;
                case "login":
                    return IDPage.Login;
                case "visit":
                case "logout":
                    return string.Empty;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载用户首页
        /// </summary>
        private static string LoadIndex(string url)
        {
            string key = Common.GetPara(url, 1,"index");
            switch (key)
            {
                case "index":
                    return IDPage.Index;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载用户-文章
        /// </summary>
        private static string LoadArticle(string url)
        {
            string key = Common.GetPara(url,2,"all");
            switch (key)
            {
                case "all":
                case "category":
                case "list":
                    return IDPage.ArticleList;
                case "detail":
                    return IDPage.ArticleDetail;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载用户-图片
        /// </summary>
        private static string LoadPhoto(string url)
        {
            string key = Common.GetPara(url, 2, "all");
            switch (key)
            {
                case "all":
                case "category":
                case "list":
                    return IDPage.PhotoList;
                case "detail":
                    return IDPage.PhotoDetail;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载用户-后台
        /// </summary>
        private static string LoadAdmin(string url)
        {
            string key = Common.GetPara(url, 2,"index");
            switch (key)
            {
                case "index":
                    return IDPage.Index;
                case "setting":
                    return IDPage.Setting;
                case "link":
                    return IDPage.Link;
                case "template":
                    return IDPage.Template;
                case "password":
                    return IDPage.Password;
                case "article":
                    switch (Common.GetPara(url, 3,"index"))
                    {
                        case "edit":
                        case "post":
                            return IDPage.ArticlePost;
                        case "index":
                        case "category":
                        case "all":
                        case "del":
                            return IDPage.ArticleList;
                        case "class":
                            return IDPage.ArticleClass;
                    }
                    break;
                case "photo":
                    switch (Common.GetPara(url, 3, "index"))
                    {
                        case "edit":
                        case "post":
                            return IDPage.PhotoPost;
                        case "index":
                        case "category":
                        case "all":
                        case "del":
                            return IDPage.PhotoList;
                        case "class":
                            return IDPage.PhotoClass;
                    }
                    break;
            }
            return IDPage.Error;
        }
        /// <summary>
        /// 加载错误提示界面
        /// </summary>
        private static string LoadError(string url)
        {
            //string key = Common.GetPara(url, 2);
            //switch (key)
            //{
            //    case "":
            //    case "index":
            //        return PageHtml;
            //}
            return IDPage.Error;
        }
        #endregion
    }
}
