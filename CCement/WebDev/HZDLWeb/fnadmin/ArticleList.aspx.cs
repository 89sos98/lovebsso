using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class fnadmin_ArticleList : System.Web.UI.Page
{
    string strsql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    PagerInfo pinfo = new PagerInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/ArticleList.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            int? cateid = null;
            KeyMode? md = null;
            if (!string.IsNullOrEmpty(Request.Params.Get("cateid")))
                cateid = int.Parse(Request.Params.Get("cateid"));
            if (!string.IsNullOrEmpty(Request.Params.Get("md")))
                md = (KeyMode)sbyte.Parse(Request.Params.Get("md"));

            BindData(Request.QueryString["key"], md, cateid);
        }
    }

    // 绑定页面数据
    private void BindData(string _key, KeyMode? _keymode, int? _categoryid)
    {
        #region 绑定分类
        CFunc.BindCategorys(selCategory, new ListItem("全部", string.Empty), false, CategoryArea.Article);

        #endregion

        #region 初始化
        txtKey.Value = _key;
        if (null != _keymode)
            selKeymode.Value = _keymode.ToString();
        if (null != _categoryid)
            selCategory.Value = _categoryid.ToString();   
        #endregion

        #region 获取文章记录数
        strsql = "select count(*) from [Products] where 1=1";
        string strwhere = null;
        if (null != _categoryid)
            strwhere += " and [CategoryId]=" + _categoryid;
        strwhere += GetKeyWhere(_key, _keymode);
        if (!string.IsNullOrEmpty(strwhere))
            strsql += strwhere;
        object objcount = sqlM.GetFistColumn(CommandType.Text, strsql);
        #endregion

        #region 绑定分页控件
        int count = 0;
        if (DBNull.Value != objcount && null != objcount)
            count = int.Parse(objcount.ToString());

        pinfo.Recordcount = count;
        pinfo.PageSize = 15;
        pinfo.TotalPage = (count % pinfo.PageSize == 0) ? count / pinfo.PageSize : count / pinfo.PageSize + 1;
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            pinfo.CurrentPageIndex = int.Parse(Request.QueryString["page"]);
        else
            pinfo.CurrentPageIndex = 1;

        Pager1.PInfo = pinfo;
        #endregion

        #region 获取文章列表
        if (count > 0)
        {
            if (pinfo.CurrentPageIndex > 1)
                strsql = "select top " + pinfo.PageSize + " * from [Products] where [ID] not in(select top " + (pinfo.CurrentPageIndex - 1) * pinfo.PageSize + " [ID] from [Products] where 1=1 and [IsCorP]=1";
            else
                strsql = "select top " + pinfo.PageSize + " * from [Products] where 1=1 and [IsCorP]=1";

            if (pinfo.CurrentPageIndex > 1)
                strsql += strwhere + " order by [UpTime] desc)" + strwhere + " order by [UpTime] desc";
            else
                strsql += strwhere + " order by [UpTime] desc";

            DataSet ds = sqlM.GetDataSet(CommandType.Text, strsql);

            rptArts.DataSource = ds;
            rptArts.DataBind();                
        }
        #endregion
    }

    //搜索
    protected void BtnSearch_Click(object sender, EventArgs e) {
        string p = null;
        if (!string.IsNullOrEmpty(selCategory.Value))
            p += "&cateid=" + HttpUtility.UrlEncode(selCategory.Value);      
        if (!string.IsNullOrEmpty(txtKey.Value.Trim()))
            p += "&key=" + HttpUtility.UrlEncode(txtKey.Value.Trim());
        if (!string.IsNullOrEmpty(selKeymode.Value))
            p += "&md=" + HttpUtility.UrlEncode(selKeymode.Value);

        Response.Redirect("ArticleList.aspx?page=1" + p);
    }

    //批量删除
    protected void BtnDelete_Click(object sender, EventArgs e) {
        if (string.IsNullOrEmpty(Request.Form["arts"]))
            return;
        else
        {
            strsql = "delete from [Products] where [ID] in (" + Request.Form["arts"] + ")";
            sqlM.ExecuteSql(CommandType.Text, strsql);
        }

        //绑定数据
        int? cateid = null;
        if (!string.IsNullOrEmpty(selCategory.Value))
            cateid = int.Parse(selCategory.Value);
        BindData(txtKey.Value.Trim(), (KeyMode)sbyte.Parse(selKeymode.Value), cateid);
    }


    //组合搜索条件sql
    private string GetKeyWhere(string _key, KeyMode? _keymode)
    {
        if (null != _keymode)
        {
            switch (_keymode)
            {
                case KeyMode.Key: return " and [Key] like '%" + _key + "%'"; break;
                case KeyMode.Title: return " and [PName] like '%" + _key + "%'"; break;
                //case KeyMode.Content: return " and [Content] like '%" + _key + "%'"; break;
                //case KeyMode.TitleAndContent: return " and ([Title] like '%" + _key + "%' or [Content] like '%" + _key + "%')"; break;
                default: return null; break;
            }
        }
        else return null;
    }

}
