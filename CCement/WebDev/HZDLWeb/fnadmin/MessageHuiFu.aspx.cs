using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class fnadmin_MessageHuiFu : System.Web.UI.Page
{
    private DataSet ds = new DataSet();
    private SqlManage sqlm = SqlManage.GetInstance();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if ((!string.IsNullOrEmpty(Request.QueryString["msgid"])) && (!string.IsNullOrEmpty(Request.QueryString["huifu"])))
            {
                if (Convert.ToInt32(Request.QueryString["huifu"]) == CFunc.YHuiFu)
                {
                    BindData(Convert.ToInt32(Request.QueryString["msgid"].ToString()));
                }
            }
        }

    }


    private void BindData(int msgid)
    {
        string sql = "select top 1 * from [Message] where parentid=" + msgid + " and msgtype=2 order by msgid desc";
        ds = sqlm.GetDataSet(CommandType.Text, sql);
        if (ds != null && ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.txtName.Value = ds.Tables[0].Rows[0]["username"].ToString();
                this.txtContent.Value = ds.Tables[0].Rows[0]["content"].ToString();
            }
        }
    }

    /// <summary>
    /// 更新和添加回复操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if ((!string.IsNullOrEmpty(txtContent.Value.Trim())) && (!string.IsNullOrEmpty(txtName.Value.Trim())))
        {


            if (Convert.ToInt32(Request.QueryString["huifu"]) == CFunc.WHuiFu)
            {
                //添加回复
                string sql = "insert into [Message]([msgtype],[username],[content],[uptime],[parentid],[huifu]) values(@msgtype,@username,@content,@uptime,@parentid,@huifu)";
                OleDbParameter[] para = { 
                                            new OleDbParameter("@msgtype", 2),
                                            new OleDbParameter("@username",txtName.Value.Trim().ToString()),
                                            new OleDbParameter("@content", txtContent.Value.Trim().ToString()), 
                                            new OleDbParameter("@uptime", DateTime.Now.ToString()), 
                                            new OleDbParameter("@parentid", Convert.ToInt32(Request.QueryString["msgid"])),new OleDbParameter("@huifu",1) };
                //将留言的回复状态改变为1
                string sql2 = "update [Message] set huifu=1 where msgid=" + Convert.ToInt32(Request.QueryString["msgid"]);
                if (sqlm.ExecuteSql(CommandType.Text, sql, para) > 0 && sqlm.ExecuteSql(CommandType.Text, sql2) > 0)
                {
                    Response.Write("<script>alert('添加回复成功！');location.href='MessageList.aspx?mt=1';</script>");
                }
                else
                {
                    Response.Write("<script>alert('添加回复失败！');</script>");
                }
            }
            else
            {
                //更新回复
                string sql = "update [Message] set [msgtype]=2,[username]=@username,[content]=@content,[uptime]=@uptime where [parentid]=@parentid";
                string a = txtName.Value.Trim();
                OleDbParameter[] para = { new OleDbParameter("@username", txtName.Value.Trim()), new OleDbParameter("@content", txtContent.Value.Trim()), new OleDbParameter("@uptime", DateTime.Now.ToString()), new OleDbParameter("@parentid", Convert.ToInt32(Request.QueryString["msgid"])) };
                if (sqlm.ExecuteSql(CommandType.Text, sql, para) > 0)
                {
                    Response.Write("<script>alert('更新回复成功！');location.href='MessageList.aspx?mt=1';</script>");
                }
                else
                {
                    Response.Write("<script>alert('更新回复失败！');</script>");
                }
            }
        }
        else
        {
            Response.Write("<script>alert('请填写回复人和回复内容！');</script>");
        }
    }
}
