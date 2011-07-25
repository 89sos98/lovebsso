using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class fnadmin_JobEdit : System.Web.UI.Page
{
    protected string contentVal = null;
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    int jobid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["jobid"]))
            jobid = int.Parse(Request.QueryString["jobid"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/JobList.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }
    }

    private void BindData()
    {
        if (jobid != 0) {
            strSql = "select * from [Job] where [jobid]=" + jobid;
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            if (null != ds && null != ds.Tables[0])
            {
                txtJobname.Value = ds.Tables[0].Rows[0]["jobname"].ToString();
                contentVal = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["jobdesc"].ToString());
            }
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (jobid != 0)
        { //更新操作
            strSql = "update Job set [jobname]=@jobname,[jobdesc]=@jobdesc,[publishtime]=@publishtime where [jobid]=@jobid";

            OleDbParameter[] oleParams ={
                                        new OleDbParameter("@jobname",txtJobname.Value.Trim()),
                                        new OleDbParameter("@jobdesc",HttpUtility.HtmlEncode(Request["content"])),
                                        new OleDbParameter("@publishtime",DateTime.Now.ToString()),
                                        new OleDbParameter("@jobid",jobid)
                                       };

            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='JobList.aspx';</script>");
        }
        else
        { //添加操作
            strSql = "insert into Job([jobname],[jobdesc],[publishtime])values(@jobname,@jobdesc,@publishtime)";

            OleDbParameter[] oleParams ={
                                        new OleDbParameter("@jobname",txtJobname.Value.Trim()),
                                        new OleDbParameter("@jobdesc",HttpUtility.HtmlEncode(Request["content"])),
                                        new OleDbParameter("@publishtime",DateTime.Now.ToString())
                                       };

            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('发布成功');location.href='JobList.aspx';</script>");
        }
    }
}

