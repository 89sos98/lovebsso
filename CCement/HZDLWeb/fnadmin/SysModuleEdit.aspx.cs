using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class fnadmin_SysModuleEdit : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    string moduleid = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        moduleid = Request.QueryString["moduleid"];
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/SysModules.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }
    }

    private void BindData()
    {
        #region 绑定父级
        if (selParent.Items.Count <= 0)
        {
            selParent.Items.Add(new ListItem("顶级模块", "0"));

            strSql = "select * from [SysModule] where parentid='0'";
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            if (null != ds && null != ds.Tables[0]) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    selParent.Items.Add(new ListItem(dr["mname"].ToString(), dr["moduleid"].ToString()));
                }
            }
        }
        #endregion

        if (!string.IsNullOrEmpty(moduleid))
        {
            strSql = "select * from [SysModule] where moduleid='" + moduleid + "'";
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            if (null != ds && null != ds.Tables[0]) {
                txtname.Value = ds.Tables[0].Rows[0]["mname"].ToString();
                txttitle.Value = ds.Tables[0].Rows[0]["mtitle"].ToString();
                txthref.Value = ds.Tables[0].Rows[0]["href"].ToString();
                selParent.Value = ds.Tables[0].Rows[0]["parentid"].ToString();
            }
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(moduleid))
        {//更新
            strSql = "update SysModule set [parentid]=@parentid,[mname]=@mname,[mtitle]=@mtitle,[href]=@href where [moduleid]=@moduleid";

            OleDbParameter[] oleParams ={
                                       new OleDbParameter("@parentid",selParent.Value),
                                       new OleDbParameter("@mname",txtname.Value.Trim()),
                                       new OleDbParameter("@mtitle",txttitle.Value.Trim()),
                                       new OleDbParameter("@href",txthref.Value.Trim()),
                                       new OleDbParameter("@moduleid",moduleid)
                                       };
            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='SysModules.aspx';</script>");
        }
        else
        {//新增

            strSql = "select Max([moduleid]) from SysModule where [parentid]='" + selParent.Value + "'";
            object obj = sqlM.GetFistColumn(CommandType.Text, strSql);

            string maxId = null;
            string mid = null;
            if (null != obj && DBNull.Value != obj) {
                maxId = obj.ToString();
            }

            if (!string.IsNullOrEmpty(maxId))
            {
                int s = int.Parse(maxId) + 1;
                mid = "0" + s;
            }
            else
            {
                if (selParent.Value == "0")
                    mid = "01";
                else
                    mid = selParent.Value + "01";
            }

            float sort = 0;
            strSql = "select Max([sort]) from [SysModule] where [parentid]='" + selParent.Value + "'";
            object obj2 = sqlM.GetFistColumn(CommandType.Text, strSql);
            if (null != obj2 && DBNull.Value != obj2)
                sort= float.Parse(obj2.ToString());

            strSql = "insert into [SysModule]([moduleid],[parentid],[mname],[mtitle],[href],[sort])values(@moduleid,@parentid,@mname,@mtitle,@href,@sort)";

            OleDbParameter[] oleParams ={
                                       new OleDbParameter("@moduleid",mid),
                                       new OleDbParameter("@parentid",selParent.Value),
                                       new OleDbParameter("@mname",txtname.Value.Trim()),
                                       new OleDbParameter("@mtitle",txttitle.Value.Trim()),
                                       new OleDbParameter("@href",txthref.Value.Trim()),
                                       new OleDbParameter("@sort",sort+1)
                                       };
            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('添加成功');location.href='SysModules.aspx';</script>");
        }
    }
}
