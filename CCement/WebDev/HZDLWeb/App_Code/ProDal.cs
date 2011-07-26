using System;
using System.Collections.Generic;
using System.Web;
using System.Data.OleDb;
using System.Data;

/// <summary>
///ProDal 的摘要说明
/// </summary>
public class ProDal
{
	public ProDal()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public static List<Product> GetLsit(int categoryId,int dengJi)
    {
        SqlManage sqlm=SqlManage.GetInstance();
        string sql = "select * from [Products] where [CategoryId]="+categoryId+" and [DengJi]="+dengJi+" order by ID asc";
        List<Product> list=new List<Product>();
        OleDbDataReader dataReader = sqlm.GetSqlDataReader(CommandType.Text, sql);
        while (dataReader.Read())
        {
            Product product = new Product();
            product.Id = Convert.ToInt32(dataReader["ID"]);
            product.IsCorP = Convert.ToInt32(dataReader["IsCorP"]);
            product.CategoryId = Convert.ToInt32(dataReader["CategoryId"]);
            product.DengJi = Convert.ToInt32(dataReader["DengJi"]);
            product.PName = dataReader["PName"].ToString();
            product.PContent = dataReader["PContent"].ToString();
            product.UpTime = Convert.ToDateTime(dataReader["UpTime"].ToString());
            product.PublishTime = Convert.ToDateTime(dataReader["PublishTime"].ToString());
            list.Add(product);
        }
        dataReader.Close();
        return list;
    }
}
