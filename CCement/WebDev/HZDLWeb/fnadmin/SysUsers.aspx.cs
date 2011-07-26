using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_SysUsers : System.Web.UI.Page
{
    string strsql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/SysUsers.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");
            BindData();
        }
    }

    private void BindData()
    {
        strsql = "select * from [SysUser]";
        DataSet ds = sqlM.GetDataSet(CommandType.Text, strsql);

        rptUsers.DataSource = ds;
        rptUsers.DataBind();
    }

    //删除选中项
    protected void BtnDelete_Click(object sender, EventArgs e) {
        if (string.IsNullOrEmpty(Request.Form["arts"]))
            return;
        else
        {
            strsql = "delete from [SysUser] where [userid] in(" + Request.Form["arts"] + ")";
            sqlM.ExecuteSql(CommandType.Text, strsql);
        }

         Response.Redirect("UserList.aspx");
    }
}
