using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_SysUserEdit : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    string[] moduleids = null;
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
        if (Request.QueryString["type"] == "3")
        {
            #region 绑定一级模块
            strSql = "select * from [SysModule] order by [sort]";
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            rptM.DataSource = ds.Tables[0];
            ds.Tables[0].DefaultView.RowFilter = "parentid=0";
            rptM.DataBind();
            #endregion

            #region 获取指定用户的模块权限集
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                strSql = "select [moduleids] from [SysUser] where userid=" + Request.QueryString["uid"];
                object obj = sqlM.GetFistColumn(CommandType.Text, strSql);
                if (null != obj && DBNull.Value != obj)
                {
                    moduleids = obj.ToString().Split(',');
                }
            }
            #endregion

            if (null != moduleids)
            {
                #region 绑定二级模块CheckboxList，并选中
                foreach (RepeaterItem i in rptM.Items)
                {
                    Label labId = i.FindControl("labId") as Label;
                    CheckBoxList cblM = i.FindControl("cblM") as CheckBoxList;
                    cblM.DataTextField = "mname";
                    cblM.DataValueField = "moduleid";
                    cblM.DataSource = ds.Tables[0];
                    ds.Tables[0].DefaultView.RowFilter = "parentid=" + labId.Text;
                    cblM.DataBind();

                    foreach (ListItem m in cblM.Items)
                    {
                        foreach (string s in moduleids)
                        {
                            if (m.Value == s)
                            {
                                m.Selected = true; break;
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }

    //提交
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] == "1")
        {//新增操作
            strSql = "insert into SysUser([username],[password],[addusername],[addtime])values(@username,@password,@addusername,@addtime)";

            OleDbParameter[] oleParams ={
                                        new OleDbParameter("@username",txtname.Value.Trim()),
                                        new OleDbParameter("@password",CFunc.Encrypt(txtpwd.Value.Trim())),
                                        new OleDbParameter("@addusername",User.Identity.Name),
                                        new OleDbParameter("@addtime",DateTime.Now.ToString())
                                       };
           if(sqlM.ExecuteSql(CommandType.Text, strSql, oleParams)>0)
               Response.Write("<script>alert('添加成功');location.href='SysUsers.aspx';</script>");
        }

        else if (Request.QueryString["type"] == "2")
        {//修改密码
            int uid = int.Parse(Request.QueryString["uid"]);
            strSql = "update [SysUser] set [password]=@password where [userid]=@userid";

            OleDbParameter[] oleParams ={
                                        new OleDbParameter("@password",CFunc.Encrypt(txtNewpwd.Value.Trim())),
                                        new OleDbParameter("@userid",uid)
                                       };
            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='SysUsers.aspx';</script>");
        }

        else if (Request.QueryString["type"] == "3")
        { //更新权限操作
            int uid = int.Parse(Request.QueryString["uid"]);
            string mids = null;

            foreach (RepeaterItem i in rptM.Items)
            {
                bool bl = false;
                Label labId = i.FindControl("labId") as Label;
                CheckBoxList cblM = i.FindControl("cblM") as CheckBoxList;

                foreach (ListItem m in cblM.Items)
                {
                    if (m.Selected)
                    {
                        if (!bl)
                        {
                            mids += labId.Text + ",";
                            bl = true;
                        }
                        mids += m.Value + ",";
                    }
                }
            }

            if (!string.IsNullOrEmpty(mids))
                mids = mids.Substring(0, mids.Length - 1);

            strSql = "update [SysUser] set [moduleids]='" + mids + "' where [userid]=" + uid;
            if (sqlM.ExecuteSql(CommandType.Text, strSql) > 0)
                Response.Write("<script>alert('更新成功');location.href=location.href;</script>");
        }
    }  
}