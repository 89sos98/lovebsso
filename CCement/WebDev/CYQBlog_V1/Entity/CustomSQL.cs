using System;
using System.Collections.Generic;
using System.Text;

namespace CYQ.Entity.MySpace
{
    public class CustomTable
    {
        /// <summary>
        /// 文章档案 to_char(CreateTime,'yyyy')
        /// </summary>
        public const string ArticleArchive = "(SELECT count(*) as CountA,[#YEAR](CreateTime) as YearA,[#MONTH](CreateTime) as MonthA FROM Blog_Content where TypeID=0 and  UserID={0} group by [#YEAR](CreateTime),[#MONTH](CreateTime) )v";
       /// <summary>
       /// 文章评论
       /// </summary>
        public const string ArticleComment = "(SELECT c.*,u.HeadUrl,u.UserName FROM Blog_Comment c LEFT JOIN Blog_User u ON c.UserID=u.ID) v";
       
    }
    public class CustomSQL
    {
        /// <summary>
        /// 用户访问计数+1 参数：1个
        /// </summary>
        public const string UserVisitAdd = "Update Blog_User Set VisitCount=VisitCount+1 where ID={0}";
        /// <summary>
        ///文章/图片访问计数+1 参数：1个
        /// </summary>
        public const string ContentVisitAdd = "Update Blog_Content Set Hits=Hits+1 where ID={0}";
    }
}
