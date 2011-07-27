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
        /// ��Ƭ��ϸ
        /// </summary>
        public void FillDetail()
        {
            MDataTable table = null;
            int count = 0;
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                if (action.Fill(GetParaInt(3)))
                {
                    FillPhotoContent(action.Data);//�����Ƭ��ϸ
                    int classID = action.Get<int>(Content.ClassID);
                    if (action.ResetTable(TableNames.Blog_Class))
                    {
                        if (action.Fill(classID))//���·������
                        {
                            Document.LoadData(action.Data);
                            Document.SetFor(IDKey.labName, SetType.A, ValueReplace.New, UrlPrefix + "/photo/category/" + classID);

                            if (action.ResetTable(CustomTable.ArticleComment))//���۲�ѯ
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
                Document.Remove(IDKey.labCommentList);//�Ƴ�����
            }
            else
            {
                FillForeachComment(table);//�������
            }
            if (count > 50)//����ҳ
            {
                new Pager(count, GetParaInt(4), 50, UrlPrefix + GetPagerUrl(4)).FormatPager(Document);
            }
            else
            {
                Document.Remove(IDKey.Node_Pager);//�Ƴ���ҳ
            }
        }

        /// <summary>
        /// ��������
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
        /// �����Ƭ�б�
        /// </summary>
        /// <param name="paraSplitNum">��ҳ�ǵڼ�������</param>
        /// <param name="attachWhere">����where����</param>
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
        #region ˽�з���
  
        private void FillForeachComment(MDataTable table)//ѭ����������б�
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
        private void FillPhotoContent(MDataRow row)//�����Ƭ����
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
        private void FillForeachPhoto(MDataTable table)//���ѭ����Ƭ�б�
        {  
            Document.Set(IDKey.labPhoto, SetType.Src, "{0}");
            Document.Set(IDKey.labPhotoLink, SetType.Href, UrlPrefix + "/photo/detail/{1}");
            Document.LoadData(table);
            Document.SetForeach(IDKey.labPhotoList, SetType.InnerXml, Content.Icon, Content.ID);

        }
       
        #endregion
    }
}
