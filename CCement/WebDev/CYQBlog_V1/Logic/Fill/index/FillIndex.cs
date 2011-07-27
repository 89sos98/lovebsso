using System;
using CYQ.Entity.MySpace;
using CYQ.Data.Xml;
using System.Xml;
using Web.Core;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Entity;
namespace Logic
{
    public class FillIndex : CoreBase
    {
        public FillIndex(ICore custom) : base(custom) { }
        /// <summary>
        /// ��乲ͬ�Ĳ���
        /// </summary>
        public void FillCommon()
        {
            FillHead();
            FillFoot();
            FillBulletin();
            FillArticlClass();
            FillPhotoClass();
            FillArticlArchive();
            FillNewComment();
            FillLinks();
            FillVisitCount();
        }

        public void FillHead()//ͷ��
        {
            Document.LoadData(DomainUser);
            Document.SetFor(IDKey.labSpaceName);
            Document.SetFor(IDKey.labSpaceIntro);
            Document.SetFor(IDKey.labCustomCss);

            if (UserAction.IsOnline(false))//�ѵ�½
            {
                Document.LoadData(UserAction.UserInfo);
                Document.SetFor(IDKey.labUserName, SetType.A, ValueReplace.New, "/" + ValueReplace.New);
                Document.SetFor(IDKey.logStatus, SetType.A, Language.Get(IDLang.logout), "/sys/logout");
            }
            else//δ��½
            {
                Document.SetFor(IDKey.labUserName, SetType.Href,"/sys/reg");//����ע��
                Document.Set(IDKey.logStatus, SetType.Href, "/sys/login/" + Encode.Url(Convert.ToString(Request.UrlReferrer), true));
            }
            foreach (XmlNode node in Document.GetList("a", Document.GetByID(IDKey.headRightMenu)))
            {
                if (!HasPre(node,Config.HttpHost))
                {
                    Document.Set(node, SetType.Href, Config.HttpHost + ValueReplace.Source);
                }
            }
            if (HadWww)
            {

                foreach (XmlNode node in Document.GetList("a", Document.GetByID(IDKey.headMenu)))
                {
                    if (!HasPre(node, UrlPrefix))
                    {
                        Document.Set(node, SetType.Href, UrlPrefix + ValueReplace.Source);
                    }
                }
            }
        }
        public void FillFoot() //β��
        {
            Document.Remove(IDKey.footBody);
        }
        public void FillBulletin()//����
        {
            Document.LoadData(DomainUser);
            Document.SetFor(IDKey.labBulletin);
        }
        public void FillArticlClass()//���·���
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                table = action.Select(0, 0, string.Format("{0}={1} and {2}=0", Class.UserID, DomainID,Class.TypeID), out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.SetForeach(IDKey.labArticleClass, "<li><a href=\"" + UrlPrefix+ "/article/category/{0}\" >{1}</a> ({2})</li>", Class.ID, Class.Name, Class.Count);
            }
        }
        public void FillPhotoClass()//������
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                table = action.Select(0, 0, string.Format("{0}={1} and {2}=1", Class.UserID, DomainID, Class.TypeID), out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.SetForeach(IDKey.labPhotoClass, "<li><a href=\"" + UrlPrefix + "/photo/category/{0}\" >{1}</a> ({2})</li>", Class.ID, Class.Name, Class.Count);
            }
        }
        public void FillArticlArchive()//���µ���
        {
            MDataTable table;
            using (MAction action = new MAction(string.Format(CustomTable.ArticleArchive,DomainID)))
            {
                table = action.Select();
            }
            Document.LoadData(table);
            Document.SetForeach(IDKey.labArticleArchive, "<li><a href=\"" + UrlPrefix + "/article/list/{0}/{1}\" >{0}-{1}</a> ({2})</li>", CustomeKey.Year, CustomeKey.Month, CustomeKey.Count);
        }
        public void FillNewComment()//��������
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Comment))
            {
                table = action.Select(1, 10, string.Format("{0}=0 and {1}={2} order by {3} desc",Comment.TypeID,Comment.ContentUserID, DomainID,Comment.CreateTime), out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach);
                Document.SetForeach(IDKey.labNewComment, "<li><a href=\"" + UrlPrefix + "/article/detail/{0}\" >{1}</a></li>", Comment.ContentID, Comment.Body);
            }
        }

        string Document_OnForeach(string text, object[] values, int row)
        {
            string key =System.Web.HttpUtility.HtmlEncode(Convert.ToString(values[1]));
            if (!string.IsNullOrEmpty(key) && key.Length > 12)
            {
                values[1] =SetCDATA(key.Substring(0, 12));
            }
            return text;
        }
        public void FillLinks()//��������,����Ҫ���Զ����׺,���Լ�CData���ⱻ�Զ��Ӻ�׺
        {
            MDataTable table;
            int count;
            using (MAction action = new MAction(TableNames.Blog_Link))
            {
                table = action.Select(0, 0, string.Format("{0}={1}", Links.UserID, DomainID), out count);
            }
            if (count > 0)
            {
                Document.LoadData(table);
                Document.SetForeach(IDKey.labLinks,SetCDATA("<li><a href=\"{0}\" title=\"{2}\" >{1}</a></li>"), Links.LinkUrl,Links.Title,Links.Body);
            }
        }
        public void FillVisitCount()//ͳ��
        {
            Document.LoadData(DomainUser);
            Document.SetFor(IDKey.labVisitCount);
            Document.SetFor(IDKey.labAmount);
        }

        private bool HasPre(XmlNode node, string pre)
        {
            if (node.Attributes != null && node.Attributes["href"] != null && node.Attributes["href"].Value.IndexOf(pre) > -1)
            {
                return true;
            }
            return false;
        }

    }
}
