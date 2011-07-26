using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_Category : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    sbyte? area = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["area"]))
            area = sbyte.Parse(Request.QueryString["area"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/Category.aspx?area=" + area))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData(area);
        }
    }

    private void BindData(sbyte? _area)
    {
        strSql = "select * from [Products] where [IsCorP]=0";
        //if (null != _area)
        //    strSql += " and area= " + _area;
        DataSet ds= sqlM.GetDataSet(CommandType.Text, strSql);
        rptCategory.DataSource = ds.Tables[0];
        ds.Tables[0].DefaultView.RowFilter = "[CategoryId]=0";
        rptCategory.DataBind();

        foreach (RepeaterItem i in rptCategory.Items)
        {
            Label labId = i.FindControl("labCateId") as Label;
            Repeater rptS = i.FindControl("rptSecond") as Repeater;
            rptS.DataSource = ds.Tables[0];
            ds.Tables[0].DefaultView.RowFilter = "CategoryId=" + labId.Text;
            rptS.DataBind();
        }
    }

    protected void rptCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int cateid = int.Parse(e.CommandArgument.ToString());
        switch (e.CommandName)
        {
            case "delete": //删除
                strSql = "delete from [Products] where [ID]=" + cateid;
                sqlM.ExecuteSql(CommandType.Text, strSql);
                strSql = "delete from [Products] where [ID]=" + cateid;
                sqlM.ExecuteSql(CommandType.Text, strSql);
                BindData(area);
                break;
            case "empty": //清空 
                strSql = "delete from [Products] where [ID]=" + cateid;
                sqlM.ExecuteSql(CommandType.Text, strSql);
                BindData(area);
                break;
            case "move": //下移 
                //module = bll.GetModule(moduleid);
                //module.sort += 0.1F;
                break;
            default: break;
        }
    }

    protected void rptSecond_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int cateid = int.Parse(e.CommandArgument.ToString());
        switch (e.CommandName)
        {
              case "delete": //删除
                strSql = "delete from [Products] where [ID]=" + cateid;
                sqlM.ExecuteSql(CommandType.Text, strSql);
                strSql = "delete from [Products] where [ID]=" + cateid;
                sqlM.ExecuteSql(CommandType.Text, strSql);
                BindData(area);
                break;
            case "move": //下移 
                //module = bll.GetModule(moduleid);
                //module.sort += 0.1F;
                break;
            default: break;

        }
    }
}
