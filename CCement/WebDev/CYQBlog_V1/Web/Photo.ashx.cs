using System;
using CYQ.Entity.MySpace;
using CYQ.Data;
using CYQ.Entity;
using Logic;
namespace Web
{
    public class Photo : Module.HttpCustom
    {
        protected override void Page_Load()
        {
            new FillIndex(this).FillCommon();

            FillPhoto photo = new FillPhoto(this);
            string key = GetPara(2);
            int num = 3;
            string attachWhere = string.Empty;
            switch (key)
            {
                case "category":
                    attachWhere = string.Format("{0}={1}", Content.ClassID, GetParaInt(3));
                    num = 4;
                    break;
            }
            switch (key)
            {
                case "1":
                case "all":
                case "category":
                    photo.FillList(num, attachWhere);
                    break;
                case "detail":
                    photo.FillDetail();
                    photo.FillCommentPost();
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
                    result = article.PostComment(1);
                    break;
            }
            if (result)
            {
                Reflesh(true);
            }
        }
    }
}
