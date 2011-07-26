using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_CategoryEdit : System.Web.UI.Page
{
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    int categoryid;
    sbyte area;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["categoryid"]))
            categoryid = int.Parse(Request.QueryString["categoryid"]);
        if (!string.IsNullOrEmpty(Request.QueryString["area"]))
            area = sbyte.Parse(Request.QueryString["area"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/Category.aspx?area=" + area))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }
    }

    //初始化数据
    private void BindData()
    {
        ListItem item = new ListItem("顶级分类", "0#0");
        CFunc.BindCategory(selParent, item, false, (CategoryArea)area);
        if (categoryid != 0)
        {
            strSql = "select * from [Products] where [ID]=" + categoryid;
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            if (null != ds && null != ds.Tables[0])
            {
                txtCname.Value = ds.Tables[0].Rows[0]["PName"].ToString();
                string selValue = ds.Tables[0].Rows[0]["CategoryId"].ToString() + "#" + (Convert.ToInt32(ds.Tables[0].Rows[0]["DengJi"].ToString()) - 1).ToString();
                selParent.Value = selValue;
            }
        }
    }

    //提交
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (categoryid != 0)
        { //更新操作
            string[] selVal = selParent.Value.Split('#');
            int ctoId = Convert.ToInt32(selVal[0]);
            int dengJi = Convert.ToInt32(selVal[1]) + 1;
            strSql = "update [Products] set [CategoryID]=@CategoryID,[DengJi]=@DengJi,[PName]=@PName,[UpTime]=@UpTime where [ID]=@ID";

            OleDbParameter[] oleParams ={
                                        //new OleDbParameter("@IsCorP",0),
                                        new OleDbParameter("@CategoryID",ctoId), 
                                        new OleDbParameter("@DengJi",dengJi),
                                        new OleDbParameter("@PName",txtCname.Value.Trim()),
                                        new OleDbParameter("@UpTime",DateTime.Now.ToString()),
                                        new OleDbParameter("@ID",categoryid)
                                        };

            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='Category.aspx?area=" + area + "';</script>");
        }
        else
        { //添加操作

            float sort = 0;

            #region 获取排序值
            //strSql = "select Max([sort]) from [Category] where [area]=" + area + " and [parentid]=" + selParent.Value;
            //object obj = sqlM.GetFistColumn(CommandType.Text, strSql);
            //if (null != obj && DBNull.Value != obj)
            //    sort = float.Parse(obj.ToString());

            #endregion


            string[] selVal = selParent.Value.Split('#');
            int ctoId = Convert.ToInt32(selVal[0]);
            int dengJi = Convert.ToInt32(selVal[1]) + 1;

            strSql = "insert into [Products]([CategoryID],[DengJi],[PName],[UpTime],[PublishTime])values(@CategoryID,@DengJi,@PName,@UpTime,@PublishTime)";
            OleDbParameter[] oleParams ={
                                        // new OleDbParameter("@IsCorP",),
                                        new OleDbParameter("@CategoryID",ctoId), 
                                        new OleDbParameter("@DengJi",dengJi),
                                        new OleDbParameter("@PName",txtCname.Value.Trim()),
                                        new OleDbParameter("@UpTime",DateTime.Now.ToString()),
                                        new OleDbParameter("@PublishTime",DateTime.Now.ToString())
                                       };

            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('添加成功');location.href='Category.aspx?area=" + area + "';</script>");
        }
    }
}
