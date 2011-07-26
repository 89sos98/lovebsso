using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class fnadmin_JobList : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    PagerInfo pinfo = new PagerInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (!CFunc.HasPageRight("/fnadmin/JobList.aspx"))
            //    Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");
            BindData(string.Empty, string.Empty, true);
        }
    }

    // 绑定页面数据
    private void BindData(string _jobname, string _workingplace, bool _showpast)
    {
        #region 获取职位记录数
        strSql = "select count([jobid]) from [Job] where 1=1";
        string strWhere = null;
        if (!string.IsNullOrEmpty(_jobname))
            strWhere += " and [jobname] like '%" + _jobname + "%'";
        if (!string.IsNullOrEmpty(_workingplace))
            strWhere += " and [workingplace] like '%" + _workingplace + "%'";
        if (!_showpast)
            strWhere += " and [validuntil]>='" + DateTime.Now.ToShortTimeString() + "'";
        strSql += strWhere;
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

        #region 绑定职位信息（带分页）
        if (pinfo.CurrentPageIndex > 1)
            strSql = "select top " + pinfo.PageSize + " * from [Job] where [jobid] not in(select top " + (pinfo.CurrentPageIndex - 1) * pinfo.PageSize + " [jobid] from Job where 1=1";
        else
            strSql = "select top " + pinfo.PageSize + " * from [Job] where 1=1";

        if (pinfo.CurrentPageIndex > 1)
            strSql += strWhere + " order by [publishtime] desc) " + strWhere + " order by [publishtime] desc";
        else
            strSql += strWhere + " order by [publishtime] desc";
        rptJob.DataSource = sqlM.GetDataSet(CommandType.Text, strSql);
        rptJob.DataBind();
        #endregion
    }

    //搜索职位
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        BindData(txtJob.Value.Trim(), null, true);
    }

    //删除选中项
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.Form["arts"]))
            return;
        else
        {
            strSql = "delete from [Job] where [jobid] in (" + Request.Form["arts"] + ")";
            sqlM.ExecuteSql(CommandType.Text, strSql);
        }

        //绑定数据
        BindData(txtJob.Value.Trim(), null, true);
    }
}