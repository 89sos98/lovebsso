
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
    public class FillAdminPhoto : CoreBase
    {
        public FillAdminPhoto(ICore custom)
            : base(custom,true)
        {

        }
        public void FillPost(bool withEdit)
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                table = action.Select(0, 0, Class.TypeID + "=1 and " + Class.UserID + "=" + LoginUserID, out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.SetForeach(IDKey.txtClassID, "<option value=\"{0}\" >{1}</option>", Class.ID, Class.Name);
            }
            if (withEdit)
            {
                FillEdit();
            }
        }
       
        public void FillClass()
        {
            //分类地地址如：admin/article/class/edit/2
            int id = GetParaInt(5);

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
                            action.Delete(Class.Count + "=0 and " + Class.ID + "=" + id + " and " + Class.UserID + "=" + LoginUserID);
                            break;
                    }
                }
                int count;
                MDataTable table = action.Select(0, 0, Class.TypeID + "=1 and " + Class.UserID + "=" + LoginUserID, out count);
                if (count > 0)
                {
                    FillForeachClass(table);
                }
                else
                {
                    Document.Remove(IDKey.labPhotoClass);
                }
            }
        }

        public void Delete()
        {
            //路径为: admin/photo/del/{id}
            int id = GetParaInt(4);
            if (id > 0)
            {
                using (MAction action = new MAction(TableNames.Blog_Content))
                {
                    if (action.Fill(Content.ID + "=" + id + " and " + Content.UserID + "=" + LoginUserID))
                    {
                        string filePath = action.Get<string>(Content.Body);
                        string icon = action.Get<string>(Content.Icon);
                        int classID=action.Get<int>(Content.ClassID);
                        if (action.Delete())
                        {
                            //更新分类统计
                            int count = action.GetCount(Content.ClassID + "=" + classID);
                            if (action.ResetTable(TableNames.Blog_Class))
                            {
                                action.Set(Class.Count, count);
                                action.Update(classID);
                            }
                            if (action.ResetTable(TableNames.Blog_Comment))//删除相关评论
                            {
                                action.Delete(Comment.ContentID + "=" + id);
                            }
                            try
                            {
                                System.IO.File.Delete(MapPath(filePath));
                                System.IO.File.Delete(MapPath(icon));
                            }
                            catch { }
                        }
                    }
                }
            }
            GoTo(UrlPrefix + "/admin/photo");
        }

        /// <summary>
        /// 填充相片列表
        /// </summary>
        /// <param name="paraSplitNum">分页是第几个参数</param>
        /// <param name="attachWhere">附加where条件</param>
        public void FillList(int paraSplitNum, string attachWhere)
        {
            int pageIDKey = GetParaInt(paraSplitNum);
            if (pageIDKey==0)
            {
                pageIDKey = 1;
            }
            int pageSize = DomainUser.Get<int>(Users.PhotoListSize);
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                string where = string.Format("{0}={1} and {2}=1  order by {3} desc", Content.UserID, DomainID, Content.TypeID, Content.CreateTime);
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

        #region 其它私有方法
        private void FillForeachClass(MDataTable table)
        {
            Document.Set(IDKey.labEdit, SetType.Href, UrlPrefix + "/admin/photo/class/edit/{0}");
            Document.Set(IDKey.labDelete, SetType.Href, UrlPrefix + "/admin/photo/class/del/{0}");
            Document.Set(IDKey.labName, SetType.A, "{1} ({2})", UrlPrefix + "/admin/photo/category/{0}");
            Document.LoadData(table);
            Document.SetForeach(IDKey.labPhotoClass, SetType.InnerXml, Class.ID, Class.Name,Class.Count);
        }
        private void FillForeachPhoto(MDataTable table)//填充循环相片列表
        {
            Document.Set(IDKey.labEdit,SetType.Href,UrlPrefix + "/admin/photo/edit/{0}");
            Document.Set(IDKey.labDelete, SetType.Href, UrlPrefix + "/admin/photo/del/{0}");
            Document.Set(IDKey.labTitle, SetType.A, "{1}", UrlPrefix + "/photo/detail/{0}");
            Document.Set(IDKey.labCreateTime, "{2}");
            Document.Set(IDKey.labIsPub, "{3}");
            Document.Set(IDKey.labHits, ValueReplace.Source + "({4})");
            Document.Set(IDKey.labCommentCount, ValueReplace.Source + "({5})");
            Document.Set(IDKey.labIcon, SetType.Src,Config.HttpHost + "/{6}");
            Document.LoadData(table);
            Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach);
            Document.SetForeach(IDKey.labPhotoList, SetType.InnerXml,
            Content.ID, Content.Title, Content.CreateTime, Content.IsPub, Content.Hits, Content.CommentCount,Content.Icon);

        }

        string Document_OnForeach(string text, object[] values, int row)
        {
            bool isPub=false;
            bool.TryParse(Convert.ToString(values[3]), out isPub);
            values[3] = isPub ? Language.Get(IDLang.Publish) : Language.Get(IDLang.NoPublish);
            return text;
        }
        private void FillEdit()
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
                    Document.SetFor(IDKey.txtIsTop, SetType.Checked);
                    Document.SetFor(IDKey.txtIsMain, SetType.Checked);
                    Document.SetFor(IDKey.txtAbstract);
                    Document.Set(IDKey.labUpLoad, SetType.Disabled, "Disabled");
                    Document.Set(IDKey.myAct, SetType.Value, "EditArticle");
                }
            }
        }
        #endregion
    }
}
