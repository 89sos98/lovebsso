using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_SysModules : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/SysModules.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }
    }

    private void BindData()
    {
        strSql = "select * from [SysModule] order by [sort]";
        DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
        rptModule.DataSource = ds.Tables[0];
        ds.Tables[0].DefaultView.RowFilter = "parentid=0";
        rptModule.DataBind();

        for (int i = 0; i < rptModule.Items.Count; i++)
        {
            Label labId = rptModule.Items[i].FindControl("labModuleId") as Label;
            Repeater rptS = rptModule.Items[i].FindControl("rptSecond") as Repeater;
            rptS.DataSource = ds.Tables[0];
            ds.Tables[0].DefaultView.RowFilter = "parentid=" + labId.Text;
            rptS.DataBind();
        }
    }

    protected void rptModule_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string moduleid = e.CommandArgument.ToString();
        if (!string.IsNullOrEmpty(moduleid))
        {
            switch (e.CommandName)
            {
                case "delete": //删除
                    strSql = "delete from [SysModule] where [parentid]='" + moduleid + "'";
                    sqlM.ExecuteSql(CommandType.Text, strSql);
                    strSql = "delete from [SysModule] where [moduleid]='" + moduleid + "'";
                    sqlM.ExecuteSql(CommandType.Text, strSql);
                    BindData();
                    break;
                case "empty": //清空 
                    strSql = "delete from [SysModule] where [parentid]='" + moduleid + "'";
                    sqlM.ExecuteSql(CommandType.Text, strSql);
                    BindData();
                    break;
                case "move": //下移 
                    //module = bll.GetModule(moduleid);
                    //module.sort += 0.1F;
                    break;
                default: break;
            }
        }
    }

    protected void rptSecond_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string moduleid = e.CommandArgument.ToString();
        if (!string.IsNullOrEmpty(moduleid))
        {
            switch (e.CommandName)
            {
                case "delete": //删除
                    strSql = "delete from [SysModule] where [parentid]='" + moduleid + "'";
                    sqlM.ExecuteSql(CommandType.Text, strSql);
                    strSql = "delete from [SysModule] where [moduleid]='" + moduleid + "'";
                    sqlM.ExecuteSql(CommandType.Text, strSql);
                    BindData();
                    break;
                case "move": //下移 
                    break;
                default: break;
            }
        }
    }
}