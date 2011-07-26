using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class fnadmin_Shared_Lefter : System.Web.UI.UserControl
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindData();

    }

    private void BindData() {
        DataSet ds = null;
        if (HttpContext.Current.User.Identity.Name != ConfigurationManager.AppSettings["superuser"])
        {
            strSql = "select [moduleids] from [SysUser] where username='" + HttpContext.Current.User.Identity.Name + "'";
            object obj = sqlM.GetFistColumn(CommandType.Text, strSql);
            if (DBNull.Value != obj && null != obj)
            {
                string[] mids = obj.ToString().Split(',');
                string strids = null;
                for (int i = 0; i < mids.Length; i++)
                {
                    if (i == mids.Length - 1)
                        strids += "'" + mids[i] + "'";
                    else
                        strids += "'" + mids[i] + "',";
                }

                strSql = "select * from SysModule where 1=1";
                if (!string.IsNullOrEmpty(strids))
                    strSql += " and moduleid in(" + strids + ")";
                strSql += " order by [sort]";
                ds = sqlM.GetDataSet(CommandType.Text, strSql);
            }
        }
        else
        {
            strSql = "select * from [SysModule] order by [sort]";
            ds = sqlM.GetDataSet(CommandType.Text, strSql);
        }
        if (null != ds)
        {
            rptFirst.DataSource = ds.Tables[0];
            ds.Tables[0].DefaultView.RowFilter = "parentid=0";
            rptFirst.DataBind();

            foreach (RepeaterItem item in rptFirst.Items)
            {
                Label labId = item.FindControl("labId") as Label;
                Repeater rptS = item.FindControl("rptSecond") as Repeater;

                rptS.DataSource = ds.Tables[0];
                ds.Tables[0].DefaultView.RowFilter = "parentid=" + labId.Text;
                rptS.DataBind();
            }
        }

    }

}
