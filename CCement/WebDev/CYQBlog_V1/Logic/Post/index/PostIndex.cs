
using Web.Core;
using CYQ.Entity.MySpace;
using CYQ.Data;
using CYQ.Entity;

namespace Logic
{
    public class PostIndex : CoreBase
    {
        public PostIndex(ICore custom)
            : base(custom)
        {

        }
        /// <summary>
        /// 发表评论
        /// </summary>
        public bool PostComment(int typeID)
        {
            string body=Get(IDKey.txtBody);
            bool result = false;
            if (body.Length > 2)
            {
                using (MAction action = new MAction(TableNames.Blog_Comment))
                {
                    int contentID = GetParaInt(3);
                    if (contentID > 0)
                    {
                        action.Set(Comment.ContentUserID, DomainID);
                        action.Set(Comment.TypeID, typeID);
                        action.Set(Comment.Body, body);
                        action.Set(Comment.ContentID, contentID);
                        action.Set(Comment.NickName, Get(IDKey.txtNickName, Language.Get(IDLang.anonymous)));
                        action.Set(Comment.UserID, LoginUserID);
                        if (action.Insert())
                        {
                            int commentCount = action.GetCount(Comment.ContentID + "=" + contentID);//文章评论统计
                            if (action.ResetTable(TableNames.Blog_Content))
                            {
                                action.Set(Content.CommentCount, commentCount);
                                result = action.Update(contentID);
                            }
                        }
                    }
                    if (!result)
                    {
                        Document.Set(IDKey.postMessage, Language.Get(IDLang.posterror));
                    }
                }
            }
            return result;
        }
    }
}
