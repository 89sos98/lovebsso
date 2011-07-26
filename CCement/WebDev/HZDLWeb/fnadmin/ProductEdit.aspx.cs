using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

public partial class fnadmin_ProductEdit : System.Web.UI.Page
{
    protected string descVal = null;
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    int productid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["productid"]))
            productid = int.Parse(Request.QueryString["productid"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/ProductList.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }
    }

    private void BindData()
    {
        ListItem item = new ListItem("请选择产品系列", string.Empty);

        CFunc.BindCategory(selCategory, item, false, CategoryArea.Product);

        if (productid != 0)
        {
            strSql = "select * from [Product] where [productid]=" + productid;
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
            if (null != ds && null != ds.Tables[0])
            {
                selCategory.Value = ds.Tables[0].Rows[0]["categoryid"].ToString();
                txtPname.Value = ds.Tables[0].Rows[0]["pname"].ToString();
              
                descVal = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["desc"].ToString());

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["img"].ToString()))
                {
                    imgProduct.Src = ds.Tables[0].Rows[0]["img"].ToString();
                    hidImg.Value = ds.Tables[0].Rows[0]["img"].ToString();
                }
            }
        }
    }

    /// <summary>
    /// 上传控件上传图片
    /// </summary>
    /// <param name="fileUploadImg"></param>
    private string UploadPic(FileUpload fileUploadImg)
    {
        try
        {
            if (!fileUploadImg.HasFile) return string.Empty;

            string virtualFilePath = ConfigurationManager.AppSettings["PRODUCTDIC"];

            string savePath = HttpContext.Current.Server.MapPath(virtualFilePath); //保存文件物理路径

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //调用CFileUpload类
            CFileUpload upload = new CFileUpload(fileUploadImg, savePath, true);

            //上传图片文件
            bool ret = upload.UploadImage();
            if (ret)
            {
                //return ConfigurationManager.AppSettings["SiteURL"] + virtualFilePath + upload.LastUploadedFile;
                return virtualFilePath + upload.LastUploadedFile;
            }
            else
            {
                Console.Write("<script>alert('不支持格式或上传失败!');</script>");
                HttpContext.Current.Response.End();
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            HttpContext.Current.Response.End();
        }
        return string.Empty;
    }

    //提交
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        string imgsrc = UploadPic(fileUploadImg);
        if (productid!=0)
        {//更新操作 
            if (string.IsNullOrEmpty(imgsrc))
                imgsrc = hidImg.Value;
            strSql = "update [Product] set [categoryid]=@categoryid,[pname]=@pname,[img]=@img,[desc]=@desc,[publishtime]=@publishtime,[updatetime]=@updatetime where [productid]=@productid";

            OleDbParameter[] oleParams ={
                                    new OleDbParameter("@categoryid",int.Parse(selCategory.Value)),
                                    new OleDbParameter("@pname",txtPname.Value.Trim()),
                                    new OleDbParameter("@img",imgsrc),
                                    new OleDbParameter("@desc",HttpUtility.HtmlEncode(Request["desc"])),
                                    new OleDbParameter("@publishtime",DateTime.Now.ToString()),
                                    new OleDbParameter("@updatetime",DateTime.Now.ToString()),
                                    new OleDbParameter("@productid",productid)
                                    };

            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='ProductList.aspx';</script>");
        }
        else
        { //添加操作

            strSql = "insert into [Product]([categoryid],[pname],[img],[desc],[publishtime],[updatetime])values(@categoryid,@pname,@img,@desc,@publishtime,@updatetime)";

            OleDbParameter[] oleParams ={
                                    new OleDbParameter("@categoryid",int.Parse(selCategory.Value)),
                                    new OleDbParameter("@pname",txtPname.Value.Trim()),
                                    new OleDbParameter("@img",imgsrc),
                                    new OleDbParameter("@desc",HttpUtility.HtmlEncode(Request["desc"])),
                                    new OleDbParameter("@publishtime",DateTime.Now.ToString()),
                                    new OleDbParameter("@updatetime",DateTime.Now.ToString()),
                                    new OleDbParameter("@productid",productid)
                                    };
            if (sqlM.ExecuteSql(CommandType.Text, strSql, oleParams) > 0)
                Response.Write("<script>alert('添加成功');location.href='ProductList.aspx';</script>");
        }
    }
}
