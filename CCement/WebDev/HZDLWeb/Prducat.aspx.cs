using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class Prducat : System.Web.UI.Page
{
    protected Product product = new Product();
    SqlManage sqlm = SqlManage.GetInstance();
    private int productId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["productId"]))
            {
                productId = Convert.ToInt32(Request.QueryString["productId"]);
                string sql = "select * from [Products] where [ID]=" + productId;
                OleDbDataReader dataReader = sqlm.GetSqlDataReader(CommandType.Text, sql);
                if (dataReader.Read())
                {
                    product.Id = Convert.ToInt32(dataReader["ID"]);
                    product.IsCorP = Convert.ToInt32(dataReader["IsCorP"]);
                    product.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    product.DengJi = Convert.ToInt32(dataReader["DengJi"]);
                    product.PName = dataReader["PName"].ToString();
                    product.PContent = dataReader["PContent"].ToString();
                    product.UpTime = Convert.ToDateTime(dataReader["UpTime"].ToString());
                    product.PublishTime = Convert.ToDateTime(dataReader["PublishTime"].ToString());
                }
                dataReader.Close();
            }
            else
            {
                String sql = "select top 1 * from [Products] where 1=1 order by [UpTime] desc";
                OleDbDataReader dataReader = sqlm.GetSqlDataReader(CommandType.Text, sql);
                if (dataReader.Read())
                {
                    product.Id = Convert.ToInt32(dataReader["ID"]);
                    product.IsCorP = Convert.ToInt32(dataReader["IsCorP"]);
                    product.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
                    product.DengJi = Convert.ToInt32(dataReader["DengJi"]);
                    product.PName = dataReader["PName"].ToString();
                    product.PContent = dataReader["PContent"].ToString();
                    product.UpTime = Convert.ToDateTime(dataReader["UpTime"].ToString());
                    product.PublishTime = Convert.ToDateTime(dataReader["PublishTime"].ToString());
                }
                dataReader.Close();
            }
        }
    }
}
