using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Hr : System.Web.UI.Page
{
    private SqlManage sqlm = SqlManage.GetInstance();
    protected DataSet ds = new DataSet();
    private PagerInfo pinfo = new PagerInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
       
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="categoryId"></param>
    private void BindData()
    {

        int count = 0;
        count = Convert.ToInt32(sqlm.GetFistColumn(CommandType.Text, "select count(*) from [Job]"));
        pinfo.Recordcount = count;
        pinfo.PageSize = 2;
        pinfo.TotalPage = (count % pinfo.PageSize == 0) ? count / pinfo.PageSize : count / pinfo.PageSize + 1;
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            pinfo.CurrentPageIndex = int.Parse(Request.QueryString["page"]);
        else
            pinfo.CurrentPageIndex = 1;
        string sql = string.Empty;
        if (pinfo.CurrentPageIndex == 1)
        {
            sql = "select top " + pinfo.PageSize + " * from [Job] order by [publishtime] desc";
        }
        else
        {
            sql = "select top " + pinfo.PageSize + " * from [Job] where [jobid] not in (select top " + pinfo.PageSize * (pinfo.CurrentPageIndex - 1) + " [jobid] from [Job] where 1=1 order by [publishtime] desc) order by [publishtime] desc";
        }
        Pager1.PInfo = pinfo;
        if (count > 0)
        {
            ds = sqlm.GetDataSet(CommandType.Text, sql);
        }
    }
}
