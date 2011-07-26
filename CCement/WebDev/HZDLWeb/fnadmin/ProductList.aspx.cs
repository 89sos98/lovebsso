using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_ProductList : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    PagerInfo pinfo = new PagerInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/ProductList.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");
            BindData(string.Empty, null);
        }
    }

    // 绑定页面数据
    private void BindData(string _pname, int? _categoryid)
    {
        #region 绑定产品系列
        CFunc.BindCategory(selCategory, new ListItem("全部", string.Empty), false, CategoryArea.Product);

        #endregion

        #region 初始化
        txtPname.Value = _pname;
        if (null != _categoryid)
            selCategory.Value = _categoryid.ToString();
        #endregion

        #region 获取产品记录数
        strSql = "select count([productid]) from [Product] where 1=1";
        string strWhere = null;
        if (null != _categoryid)
            strWhere += " and [categoryid]=" + _categoryid;
        if (!string.IsNullOrEmpty(_pname))
            strWhere += " and [pname] like '%" + _pname + "%'";
        object objcount = sqlM.GetFistColumn(CommandType.Text, strSql);
        int count = 0;
        if (DBNull.Value != objcount && null != objcount)
            count = int.Parse(objcount.ToString());
        #endregion

        #region 绑定分页控件
        pinfo.Recordcount = count;
        pinfo.PageSize = 20;
        pinfo.TotalPage = (count % pinfo.PageSize == 0) ? count / pinfo.PageSize : count / pinfo.PageSize + 1;
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            pinfo.CurrentPageIndex = int.Parse(Request.QueryString["page"]);
        else
            pinfo.CurrentPageIndex = 1;
        Pager1.PInfo = pinfo;
        #endregion

        #region 绑定产品信息（带分页）
        if (pinfo.CurrentPageIndex > 1)
            strSql = "select top " + pinfo.PageSize + " * from [Product] where [productid] not in(select top " + (pinfo.CurrentPageIndex - 1) * pinfo.PageSize + " [productid] from [Product] where 1=1";
        else
            strSql = "select top " + pinfo.PageSize + " * from [Product] where 1=1";
        if (pinfo.CurrentPageIndex > 1)
            strSql += strWhere + " order by [updatetime] desc) " + strWhere + " order by [updatetime] desc";
        else
            strSql += strWhere + " order by [updatetime] desc";
        rptPro.DataSource = sqlM.GetDataSet(CommandType.Text, strSql);
        rptPro.DataBind();
        #endregion
    }

    //搜索职位
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(selCategory.Value))
            BindData(txtPname.Value.Trim(), int.Parse(selCategory.Value));
        else
            BindData(txtPname.Value.Trim(), null);
    }

    //删除选中项
    protected void BtnDelete_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(Request.Form["arts"]))
            return;
        else
        {
            strSql = "delete from [Product] where [productid] in (" + Request.Form["arts"] + ")";
            sqlM.ExecuteSql(CommandType.Text, strSql);
        }

        //绑定数据
        if (!string.IsNullOrEmpty(selCategory.Value))
            BindData(txtPname.Value.Trim(), int.Parse(selCategory.Value));
        else
            BindData(txtPname.Value.Trim(), null);
    }

}


        ////显示图片
        //protected string GetPictureSrc(string _pnum) {
        //    PictureBll pbll = new PictureBll();
        //    var p = pbll.GetPicture(null, _pnum);
        //    if (null != p)
        //        return p.src + p.filename;
        //    else
        //        return string.Empty;
        //}
