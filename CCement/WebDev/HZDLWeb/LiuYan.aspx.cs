using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;


public partial class LiuYan : System.Web.UI.Page
{
    private SqlManage sqlm = SqlManage.GetInstance();
    private PagerInfo pinfo = new PagerInfo();
    protected DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData(1);
    }

    /// <summary>
    /// 添加留言
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if ((!string.IsNullOrEmpty(txtName.Value.Trim()))&&(!string.IsNullOrEmpty(txtContent.Value.Trim())))
        {
            string sql = "insert into [Message]([msgtype],[username],[content],[uptime],[parentid]) values(" + 1 + ",'" + txtName.Value.Trim().ToString() + "','" + txtContent.Value.Trim().ToString() + "','" + DateTime.Now.ToString() + "',0)";
            //OleDbParameter[] para = { 
            //                                new OleDbParameter("@msgtype", 1),
            //                                new OleDbParameter("@username",txtName.Value.Trim().ToString()),
            //                                new OleDbParameter("@content", txtContent.Value.Trim()), 
            //                                new OleDbParameter("@uptime", DateTime.Now.ToString()), 
            //                                new OleDbParameter("@parentid",0) };
            if (sqlm.ExecuteSql(CommandType.Text,sql)>0)
            {
                Response.Write("<script>alert('留言成功！');location.href='LiuYan.aspx';</script>");
            }
        }
    }


    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="categoryId"></param>
    private void BindData(int msgtype)
    {

        int count = 0;
        count = Convert.ToInt32(sqlm.GetFistColumn(CommandType.Text, "select count(*) from [Message] where msgtype=" + msgtype));
        pinfo.Recordcount = count;
        pinfo.PageSize = 5;
        pinfo.TotalPage = (count % pinfo.PageSize == 0) ? count / pinfo.PageSize : count / pinfo.PageSize + 1;
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            pinfo.CurrentPageIndex = int.Parse(Request.QueryString["page"]);
        else
            pinfo.CurrentPageIndex = 1;
        string sql = string.Empty;
        if (pinfo.CurrentPageIndex == 1)
        {
            sql = "select top " + pinfo.PageSize + " * from [Message] where [msgtype]=" + msgtype + " order by [msgid] desc";
        }
        else
        {
            sql = "select top " + pinfo.PageSize + " * from [Message] where [msgtype]=" + msgtype + " and [msgid] not in (select top " + pinfo.PageSize * (pinfo.CurrentPageIndex - 1) + " [msgid] from [Message] where msgtype=" + msgtype + " order by [msgid] desc) order by [msgid] desc";
        }
        Pager1.PInfo = pinfo;
        if (count > 0)
        {
            ds = sqlm.GetDataSet(CommandType.Text, sql);
        }
    }



    protected DataSet GetHuiFu(int msgid)
    {
        string sql = "select top 1 * from [Message] where [msgtype]=2 and parentid="+msgid+" order by [msgid] desc";
        return sqlm.GetDataSet(CommandType.Text, sql);
    }
}
