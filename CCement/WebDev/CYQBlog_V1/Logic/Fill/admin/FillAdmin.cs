using System;
using Web.Core;
using System.Xml;
using CYQ.Entity;
using CYQ.Data.Xml;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Data.Table;


namespace Logic
{
    public class FillAdmin:CoreBase
    {
        public FillAdmin(ICore custom): base(custom,true){ }

        public void FillMenu()//填充左侧菜单
        {
            if (HadWww)
            {
                foreach (XmlNode node in Document.GetList("a", Document.GetByID(IDKey.Node_AdminMenu)))
                {
                    Document.Set(node, SetType.Href, "/" + Domain + ValueReplace.Source);
                }
            }
        }
        /// <summary>
        /// 填充统计
        /// </summary>
        public void FillVisitCount()
        {
            Document.LoadData(UserAction.UserInfo);
            Document.SetFor(IDKey.labVisitCount);
            string where=string.Format("{0}={1}",Content.UserID,LoginUserID);
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                Document.Set(IDKey.labArticleCount, action.GetCount(where+string.Format(" and {0}=0",Content.TypeID)).ToString());
                Document.Set(IDKey.labPhotoCount, action.GetCount(where + string.Format(" and {0}=1", Content.TypeID)).ToString());
                if (action.ResetTable(TableNames.Blog_Comment))
                {
                    Document.Set(IDKey.labCommentCount, action.GetCount(where).ToString());
                }
            }
        }
        /// <summary>
        /// 填充配置
        /// </summary>
        public void FillSetting()
        {
            Document.LoadData(UserAction.UserInfo);
            Document.SetFor(IDKey.txtSpaceName);
            Document.SetFor(IDKey.txtSpaceIntro);
            Document.SetFor(IDKey.txtBulletin);
            Document.SetFor(IDKey.txtSign);
            Document.SetFor(IDKey.txtCustomCss);
            Document.SetFor(IDKey.txtEmail, SetType.Value);
            Document.SetFor(IDKey.txtNickName, SetType.Value);
            Document.SetFor(IDKey.txtArticleListSize, SetType.Select);
            Document.SetFor(IDKey.txtPhotoListSize, SetType.Select);
            Document.SetFor(IDKey.labHeadUrl, SetType.Src);
        }
        public void FillLinks()
        {
            int id=GetParaInt(4);
            using (MAction action = new MAction(TableNames.Blog_Link))
            {
                if (id > 0)//编辑链接
                {
                    switch (GetPara(3))
                    {
                        case "edit":
                            if (action.Fill(id))
                            {
                                Document.LoadData(action.Data);
                                Document.SetFor(IDKey.txtTitle, SetType.Value);
                                Document.SetFor(IDKey.txtLinkUrl, SetType.Value);
                                Document.SetFor(IDKey.txtBody);
                                Document.Set(IDKey.myAct, SetType.Value, "EditLink");
                            }
                            break;
                        case "del":
                            action.Delete(Links.ID + "=" + id + " and " + Links.UserID + "=" + LoginUserID);
                            break;
                    }
                }
                int count;
                MDataTable table = action.Select(0, 0, Links.UserID + "=" + LoginUserID, out count);
                if (count > 0)
                {
                    FillForeachLink(table);
                }
                else
                {
                    Document.Remove(IDKey.labLinks);
                }
            }

            //填充列表

        }

        public void FillTemplate()
        {
            MDataTable table;
            using (MAction action = new MAction(TableNames.Blog_Skin))
            {
                table = action.Select();
            }
            Document.Set(IDKey.labTemplateLogo, SetType.Src, Config.HttpHost + "{0}/template.gif");
            Document.Set(IDKey.labSetTemplate, SetType.Value, "{1}");
            Document.Set(IDKey.labTitle, "{2}");
            Document.LoadData(table);
            Document.OnForeach += new XmlHelper.SetForeachEventHandler(Document_OnForeach);
            Document.SetForeach(IDKey.labSkinList, SetType.InnerXml, Skin.SkinPath, Skin.ID, Skin.Name);
        }

        string Document_OnForeach(string text, object[] values, int row)
        {
            if (Convert.ToString(values[1]) == DomainUser.Get<string>(Users.SkinID))
            {
                text = text.Replace("input", "input checked=\"checked\"");
            }
            return text;
        }
        #region 其它私有方法
        private void FillForeachLink(MDataTable table)
        {
            Document.Set(IDKey.labEdit, SetType.Href, UrlPrefix + "/admin/link/edit/{0}");
            Document.Set(IDKey.labDelete, SetType.Href, UrlPrefix + "/admin/link/del/{0}");
            Document.Set(IDKey.labTitle, "{1}");
            Document.Set(IDKey.labLinkUrl,SetType.A,"{2}","{2}");
      
            Document.LoadData(table);
            Document.SetForeach(IDKey.labLinks,SetType.InnerXml,Links.ID, Links.Title, Links.LinkUrl);

        }
        #endregion
    }
}
