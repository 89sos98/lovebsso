using System;
using Web.Core;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Data.Table;
using CYQ.Entity;
using CYQ.Data.Xml;


namespace Logic
{
    public class FillPhoto:CoreBase
    {
        public FillPhoto(ICore custom) : base(custom) { }
        public void FillNewTopList()
        {
            int count;
            MDataTable table;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                string where = string.Format("{0}={1} and {2}=1 and {3}=[#TRUE] order by {4} [#DESC],{5} desc", Content.UserID, DomainID, Content.TypeID, Content.IsPub, Content.IsTop, Content.CreateTime);
                table = action.Select(1, 6, where, out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.Set(IDKey.labPhoto, SetType.Src, "{0}");
                Document.Set(IDKey.labPhotoLink, SetType.Href,UrlPrefix+"/photo/detail/{1}");
                Document.SetForeach(IDKey.labNewPhoto, SetType.InnerXml, Content.Icon, Content.ID);
            }
            else
            {
                Document.Remove(IDKey.Node_Body_Right_Photo);
            }
        }
        /// <summary>
        /// 照片详细
        /// </summary>
        public void FillDetail()
        {
            MDataTable table = null;
            int count = 0;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                if (action.Fill(GetParaInt(3)))
                {
                    FillPhotoContent(action.Data);//填充相片详细
                    int classID = action.Get<int>(Content.ClassID);
                    if (action.ResetTable(TableNames.Blog_Class))
                    {
                        if (action.Fill(classID))//文章分类归属
                        {
                            Document.LoadData(action.Data);
                            Document.SetFor(IDKey.labName, SetType.A, ValueReplace.New, UrlPrefix + "/photo/category/" + classID);

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
        }
        /// <summary>
        /// 填充相片列表
        /// </summary>
        /// <param name="paraSplitNum">分页是第几个参数</param>
        /// <param name="attachWhere">附加where条件</param>
        public void FillList(int paraSplitNum, string attachWhere)
        {
            int pageIDKey=GetParaInt(paraSplitNum);
            if (pageIDKey==0)
            {
                pageIDKey = 1;
            }
            int pageSize = DomainUser.Get<int>(Users.PhotoListSize);
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                string where = string.Format("{0}={1} and {2}=1 and {3}=[#TRUE] order by {4} [#DESC],{5} desc", Content.UserID, DomainID, Content.TypeID, Content.IsPub, Content.IsTop, Content.CreateTime);
                if (!string.IsNullOrEmpty(attachWhere))
                {
                    where = attachWhere + " and " + where;
                }
                table = action.Select(pageIDKey, pageSize, where, out count);
            }
            if (count > 0)
            {
                FillForeachPhoto(table);
            }
            else
            {
                Document.RemoveChild(IDKey.labPhotoList, 1);
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
            Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach_Comment);
            Document.SetForeach(IDKey.labCommentList, SetType.InnerXml,
                Users.UserName, Comment.NickName, Users.HeadUrl, Comment.CreateTime, Comment.Body, CustomeKey.Row, Comment.ID);
        }
        string Document_OnForeach_Comment(string text, object[] values, int row)
        {
            if (string.IsNullOrEmpty(Convert.ToString(values[2])))
            {
                values[2] = row % 2 == 0 ? Config.DefaultHead : Config.DefaultHead2;
            }
            return text;
        }
        private void FillPhotoContent(MDataRow row)//填充照片内容
        {
            Document.LoadData(row);
            Document.SetFor(IDKey.labTitle);
            Document.SetFor(IDKey.labBody,SetType.Src);
            Document.SetFor(IDKey.labAbstract);
            Document.SetFor(IDKey.labCreateTime);
            Document.SetFor(IDKey.labTag, SetType.InnerXml, ValueReplace.Source + ":" + ValueReplace.New);
            Document.SetFor(IDKey.labHits, SetType.InnerText, ValueReplace.Source + "(" + ValueReplace.New + ")");
            Document.SetFor(IDKey.labCommentCount, SetType.InnerText, ValueReplace.Source + "(" + ValueReplace.New + ")");
            if (UserAction.IsOnline(false) && UserAction.UserInfo.Get<int>(Users.ID) == DomainID)
            {
                Document.SetFor(IDKey.labID, SetType.Href, UrlPrefix + "/admin/photo/edit/" + ValueReplace.New);
            }
            else
            {
                Document.Remove(IDKey.labEdit);
            }
        }
        private void FillForeachPhoto(MDataTable table)//填充循环相片列表
        {  
            Document.Set(IDKey.labPhoto, SetType.Src, "{0}");
            Document.Set(IDKey.labPhotoLink, SetType.Href, UrlPrefix + "/photo/detail/{1}");
            Document.LoadData(table);
            Document.SetForeach(IDKey.labPhotoList, SetType.InnerXml, Content.Icon, Content.ID);

        }
       
        #endregion
    }
}
