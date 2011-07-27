using System;
using Web.Core;
using CYQ.Data.Table;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Data.Xml;
using CYQ.Entity;

namespace Logic
{
    public class FillArticle : CoreBase
    {
        public FillArticle(ICore custom)
            : base(custom)
        {

        }
        /// <summary>
        /// 发表评论
        /// </summary>
        public void FillCommentPost()
        {
            if (UserAction.IsOnline(false))
            {
                Document.LoadData(UserAction.UserInfo);
                Document.SetFor(IDKey.txtNickName, SetType.Value);
                Document.Remove(IDKey.labReg);
            }
            else
            {
                Document.Set(IDKey.labReg,SetType.Href,Config.HttpHost+"/sys/reg"+Config.UrlAspx);
            }
        }
        /// <summary>
        /// 文章详细
        /// </summary>
        public void FillDetail()
        {
            MDataTable table = null;
            int count = 0;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                if (action.Fill(GetParaInt(3)))
                {
                    FillArticleContent(action.Data);//填充文章详细
                    int classID = action.Get<int>(Content.ClassID);
                    if (action.ResetTable(TableNames.Blog_Class))
                    {
                        if (action.Fill(classID))//文章分类归属
                        {
                            Document.LoadData(action.Data);
                            Document.SetFor(IDKey.labName, SetType.A, ValueReplace.New, UrlPrefix + "/article/category/" + classID);

                            if (action.ResetTable(CustomTable.ArticleComment))//评论查询
                            {
                                table = action.Select(GetParaInt(4), 50, string.Format("{0}={1}", Comment.ContentID, GetParaInt(3)), out count);
                            }  
                        }
                    }
                    
                }
                else
                {
                    Document.Remove(IDKey.Node_ArticleDetail);
                }
            }
            if (count == 0)
            {
                Document.Remove(IDKey.labCommentList);//移除评论
            }
            else
            {
                FillForeachComment(table);//评论填充
            }
            if (count > 50)//填充分页
            {
                new Pager(count, GetParaInt(4), 50, UrlPrefix + GetPagerUrl(4)).FormatPager(Document);
            }
            else
            {
                Document.Remove(IDKey.Node_Pager);//移除分页
            }
        }
        
        /// <summary>
        /// 填充文章列表
        /// </summary>
        /// <param name="paraSplitNum">分页是第几个参数</param>
        /// <param name="attachWhere">附加where条件</param>
        public void FillList(int paraSplitNum, string attachWhere)
        {
            if (DomainUser == null)
            {
                return;
            }
            int pageIDKey=GetParaInt(paraSplitNum);
            if (pageIDKey==0)
            {
                pageIDKey = 1;
            }
            int pageSize = DomainUser.Get<int>(Users.ArticleListSize);
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                string where = string.Format("{0}={1} and {2}=0 and {3}=[#TRUE] order by {4} [#DESC],{5} desc", Content.UserID, DomainID, Content.TypeID, Content.IsPub, Content.IsTop, Content.CreateTime);
                if (!string.IsNullOrEmpty(attachWhere))
                {
                    where = attachWhere + " and " + where;
                }
                table = action.Select(pageIDKey, pageSize, where, out count);
            }
            if (count > 0)
            {
                FillForeachArticle(table);
            }
            else
            {
                Document.RemoveChild(IDKey.labArticleList, 1);
            }
            if (count > pageSize)
            {
                new Pager(count, pageIDKey, pageSize, UrlPrefix + GetPagerUrl(paraSplitNum)).FormatPager(Document);
            }
            else
            {
                Document.Remove(IDKey.Node_Pager);
            }
        }

        #region 私有方法
        private void FillForeachComment(MDataTable table)//循环填充评论列表
        {
            Document.LoadData(table);
            Document.Set(IDKey.labUserID, SetType.Href, Config.HttpHost + "/{0}");
            Document.Set(IDKey.labNickName, SetType.A, "{1}", Config.HttpHost + "/{0}");
            Document.Set(IDKey.labHeadUrl, SetType.Src, Config.HttpHost + "/{2}");
            Document.Set(IDKey.divCreateTime, "{3}");
            Document.Set(IDKey.divBody, "{4}");
            Document.Set(IDKey.labRow, SetType.A, "#{5}", "#{6}");
            Document.OnForeach+=new XmlHelper.SetForeachEventHandler(Document_OnForeach_Comment);
            Document.SetForeach(IDKey.labCommentList,SetType.InnerXml,
                Users.UserName, Comment.NickName, Users.HeadUrl, Comment.CreateTime, Comment.Body, CustomeKey.Row, Comment.ID);
        }
        string Document_OnForeach_Comment(string text, object[] values,int row)
        {
            if (string.IsNullOrEmpty(Convert.ToString(values[2])))
            {
               values[2] =row%2==0? Config.DefaultHead:Config.DefaultHead2;
            }
            values[4] = System.Web.HttpUtility.HtmlEncode(Convert.ToString(values[4]));
            return text;
        }
        private void FillArticleContent(MDataRow row)//填充文章
        {
            Document.Set(IDKey.labSign, DomainUser.Get<string>(Users.Sign));
            Document.LoadData(row);
            Document.SetFor(IDKey.labTitle);
            Document.SetFor(IDKey.labBody);
            Document.SetFor(IDKey.labCreateTime);
            Document.SetFor(IDKey.labTag, SetType.InnerXml, ValueReplace.Source + ":" + ValueReplace.New);
            Document.SetFor(IDKey.labHits, SetType.InnerText, ValueReplace.Source + "(" + ValueReplace.New + ")");
            Document.SetFor(IDKey.labCommentCount, SetType.InnerText, ValueReplace.Source + "(" + ValueReplace.New + ")");
            if (UserAction.IsOnline(false) && UserAction.UserInfo.Get<int>(Users.ID) == DomainID)
            {
                Document.SetFor(IDKey.labID, SetType.Href, UrlPrefix + "/admin/article/edit/"+ValueReplace.New);
            }
            else
            {
                Document.Remove(IDKey.labEdit);
            }
        }
        private void FillForeachArticle(MDataTable table)//填充循环文章列表
        {
            Document.Set(IDKey.labTitle, SetType.A, "{0}", UrlPrefix + "/article/detail/{5}");
            Document.Set(IDKey.labCreateTime, "{1}");
            Document.Set(IDKey.labAbstract, "{2}");
            Document.Set(IDKey.labHits, "({3})");
            Document.Set(IDKey.labCommentCount,"({4})");
            if (UserAction.IsOnline(false) && UserAction.UserInfo.Get<int>(Users.ID) == DomainID)
            {
                Document.Set(IDKey.labID, SetType.Href, UrlPrefix + "/admin/article/edit/{5}");
            }
            else
            {
                Document.Remove(IDKey.labEdit);
            }

            Document.LoadData(table);
            Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach);
            Document.SetForeach(IDKey.labArticleList,SetType.InnerXml, Content.Title, Content.CreateTime, Content.Abstract, Content.Hits, Content.CommentCount, Content.ID,Content.IsTop);

        }

        string Document_OnForeach(string text, object[] values, int row)
        {
            bool isTop = false;
            bool.TryParse(Convert.ToString(values[6]), out isTop);
            if (isTop)
            {
                values[0] = "["+Language.Get(IDLang.top)+"]" + values[0];
            }
            return text;
        }
        #endregion
    }
}
