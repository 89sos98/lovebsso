
using Web.Core;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Data.Table;
using CYQ.Entity;
using CYQ.Data.Xml;
using System;
using CYQ.Editor;
namespace Logic
{
    public class FillAdminArticle : CoreBase
    {
        public FillAdminArticle(ICore custom)
            : base(custom,true)
        {

        }
        public void FillPost(bool withEdit)
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                table = action.Select(0, 0, Class.TypeID + "=0 and " + Class.UserID + "=" + LoginUserID, out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.SetForeach(IDKey.txtClassID, "<option value=\"{0}\" >{1}</option>", Class.ID, Class.Name);
            }
            //填充编辑器
            Editor editor = new Editor(Document, Language);
            editor.EditorID = "txtBody";
            editor.Width =700;
            editor.Height = 400;
            editor.BasePath = Config.HttpHost;
            if (withEdit)
            {
                FillEdit(editor);
            }
            Document.Set(IDKey.labEditor, editor.DEditorHtml(UrlPrefix + UrlPara));
        }
       
        /// <summary>
        /// 附带编辑删除
        /// </summary>
        public void FillClass()
        {
            //分类地地址如：admin/article/class/edit/2
            int id=GetParaInt(5);

            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                if (id > 0)//编辑链接
                {
                    switch (GetPara(4))
                    {
                        case "edit":
                            if (action.Fill(id))
                            {
                                Document.LoadData(action.Data);
                                Document.SetFor(IDKey.txtName, SetType.Value);
                                Document.Set(IDKey.myAct, SetType.Value, "EditClass");
                            }
                            break;
                        case "del":
                            action.Delete(Class.Count+"=0 and "+Class.ID + "=" + id + " and " + Class.UserID + "=" + LoginUserID);
                            break;
                    }
                }
                int count;
                MDataTable table = action.Select(0, 0,Class.TypeID+"=0 and "+Class.UserID + "=" + LoginUserID, out count);
                if (count > 0)
                {
                    FillForeachClass(table);
                }
                else
                {
                    Document.Remove(IDKey.labArticleClass);
                }
            }
        }

        public void Delete()
        {
            //路径为: admin/article/del/{id}
            int id =GetParaInt(4);
            if (id > 0)
            {
                using (MAction action = new MAction(TableNames.Blog_Content))
                {
                    if (action.Fill(Content.ID + "=" + id + " and " + Content.UserID + "=" + LoginUserID))
                    {
                        int classID = action.Get<int>(Content.ClassID);
                        if (action.Delete())
                        {
                            //更新分类统计
                            int count = action.GetCount(Content.ClassID + "=" + classID);
                            if (action.ResetTable(TableNames.Blog_Class))
                            {
                                action.Set(Class.Count, count);
                                action.Update(classID);
                            }
                            if (action.ResetTable(TableNames.Blog_Comment))
                            {
                                action.Delete(Comment.ContentID + "=" + id);
                            }
                        }
                    }
                }
            }
            GoTo(UrlPrefix + "/admin/article");
        }

        /// <summary>
        /// 填充文章列表
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
            int pageSize = DomainUser.Get<int>(Users.ArticleListSize);
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                string where = string.Format("{0}={1} and {2}=0  order by {3} desc", Content.UserID, DomainID, Content.TypeID, Content.CreateTime);
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

        #region 其它私有方法
        private void FillForeachClass(MDataTable table)
        {
            Document.Set(IDKey.labEdit, SetType.Href, UrlPrefix + "/admin/article/class/edit/{0}");
            Document.Set(IDKey.labDelete, SetType.Href, UrlPrefix + "/admin/article/class/del/{0}");
            Document.Set(IDKey.labName, SetType.A, "{1} ({2})", UrlPrefix + "/admin/article/category/{0}");
            Document.LoadData(table);
            Document.SetForeach(IDKey.labArticleClass,SetType.InnerXml, Class.ID, Class.Name,Class.Count);
        }
        private void FillForeachArticle(MDataTable table)//填充循环文章列表
        {
            Document.Set(IDKey.labEdit,SetType.Href,UrlPrefix + "/admin/article/edit/{0}");
            Document.Set(IDKey.labDelete, SetType.Href, UrlPrefix + "/admin/article/del/{0}");
            Document.Set(IDKey.labTitle, SetType.A, "{1}", UrlPrefix + "/article/detail/{0}");
            Document.Set(IDKey.labCreateTime, "{2}");
            Document.Set(IDKey.labIsPub, "{3}");
            Document.Set(IDKey.labHits, ValueReplace.Source + "({4})");
            Document.Set(IDKey.labCommentCount, ValueReplace.Source + "({5})");

            Document.LoadData(table);
            Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach);
            Document.SetForeach(IDKey.labArticleList,SetType.InnerXml,
            Content.ID, Content.Title, Content.CreateTime, Content.IsPub, Content.Hits, Content.CommentCount);

        }

        string Document_OnForeach(string text, object[] values, int row)
        {
            values[3] = Convert.ToString(values[3]) == "1" ? "已发布" : "未发布";
            return text;
        }
        private void FillEdit(Editor editor)
        {
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                if (action.Fill(Content.UserID + "=" + LoginUserID + " and " + Content.ID + "=" + GetParaInt(4)))
                {
                    Document.LoadData(action.Data);
                    Document.SetFor(IDKey.txtTitle, SetType.Value);
                    Document.SetFor(IDKey.txtClassID, SetType.Select);
                    Document.SetFor(IDKey.txtTag, SetType.Value);
                    Document.SetFor(IDKey.txtIsPub, SetType.Checked);
                    Document.SetFor(IDKey.txtIsRss, SetType.Checked);
                    Document.SetFor(IDKey.txtIsTop, SetType.Checked);
                    Document.SetFor(IDKey.txtIsMain, SetType.Checked);
                    editor.Text = action.Get<string>(Content.Body);
                    Document.Set(IDKey.myAct, SetType.Value, "EditArticle");
                }
            }
        }
        #endregion
    }
}
