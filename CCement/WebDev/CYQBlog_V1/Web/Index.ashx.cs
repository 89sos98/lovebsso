using System;
using CYQ.Data.Xml;
using Logic;
using CYQ.Entity.MySpace;

namespace Web
{
    public class Index : Module.HttpCustom
    {
        public override bool AllowCache
        {
            get
            {
                return true;
            }
        }
        protected override void Page_Load()
        {
            new FillIndex(this).FillCommon();
            new FillArticle(this).FillList(2, null);
            new FillPhoto(this).FillNewTopList();
        }
        protected override void Page_OnCache()
        {
            new FillIndex(this).FillCommon();
        }
    }
}
