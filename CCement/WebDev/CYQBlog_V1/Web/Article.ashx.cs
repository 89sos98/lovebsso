using System;
using CYQ.Entity.MySpace;
using CYQ.Data;
using CYQ.Entity;
using Logic;
namespace Web
{
    public class Article : Module.HttpCustom
    {
        public override bool AllowCache
        {
            get
            {
                return true;
            }
        }
        //开启缓存时Page_Load不被调用，只调用本函数
        protected override void Page_OnCache()
        {
            new FillIndex(this).FillHead();//头部不缓存
            switch (GetPara(2))
            {
                case "detail"://发表评论框不缓存
                    new FillArticle(this).FillCommentPost();
                    break;

            }
        }
        protected override void Page_Load()
        {
            new FillIndex(this).FillCommon();

            FillArticle article = new FillArticle(this);
            string key=GetPara(2);
            int num = 3;
            string attachWhere = string.Empty;
            switch (key)
            {
                case "category":
                    attachWhere = string.Format("{0}={1}",Content.ClassID,GetParaInt(3));
                    num = 4;
                    break;
                case "list":
                    attachWhere = string.Format("Year({0})={1} and Month({0})={2}", Content.CreateTime, GetParaInt(3),GetParaInt(4));
                    num = 5;
                    break;
            }
            switch(key)
            {
                case "1":
                case "all":
                case "category":
                case "list":
                    article.FillList(num, attachWhere);
                    break;
                case "detail":
                    article.FillDetail();
                    article.FillCommentPost();
                    break;

            }

        }
        protected override void OnPost()
        {
            bool result = false;
            PostIndex article = new PostIndex(this);
            switch (ThisAction)
            {
                case "PostComment":
                    result=article.PostComment(0);
                    break;
            }
            if (result)
            {
                Reflesh(true);
            }
        }
    }
}
