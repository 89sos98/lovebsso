using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_MessageEdit : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    sbyte? mtype;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["mt"]))
            mtype = sbyte.Parse(Request.QueryString["mt"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/MessageList.aspx?mt=" + mtype))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");
            BindData();
        }
    }

    private void BindData()
    {
        if (string.IsNullOrEmpty(Request.QueryString["msgid"]))
            return;
        strSql = "select * from [Message] where [msgid]=" + Request.QueryString["msgid"];

        DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
        if (null != ds && null != ds.Tables[0]) {
            labComp.Text = ds.Tables[0].Rows[0]["compname"].ToString();
            labUser.Text = ds.Tables[0].Rows[0]["username"].ToString();
            labTel.Text = ds.Tables[0].Rows[0]["tel"].ToString();
            labTime.Text = ds.Tables[0].Rows[0]["uptime"].ToString();
            labIp.Text = ds.Tables[0].Rows[0]["upIp"].ToString();
            labAddress.Text = ds.Tables[0].Rows[0]["address"].ToString(); 
            litDesc.Text = ds.Tables[0].Rows[0]["content"].ToString();


            if (mtype == (sbyte)MessageType.Message)
            {
                labEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
            }
            else if (mtype == (sbyte)MessageType.Product) {
                strSql = "select [pname] from [Product] where [productid]=" + ds.Tables[0].Rows[0]["productid"].ToString();
                object obj = sqlM.GetFistColumn(CommandType.Text, strSql);
                labPro.Text = obj.ToString();  
            }         
        }

    }    
}
