using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.OleDb;

public partial class fnadmin_ArticleEdit : System.Web.UI.Page
{
    protected string contentVal = null;

    string strsql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    int productId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["articleid"]))
            productId = int.Parse(Request.QueryString["articleid"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/ArticleList.aspx"))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData();
        }

    }

    //绑定数据
    private void BindData()
    {
        ListItem item = new ListItem("顶级产品", "0#0");

        CFunc.BindCategory(selCategory, item, false, CategoryArea.Article);

        if (productId != 0)
        {
            strsql = "select * from [Products] where [ID]=" + productId;
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strsql);
            if (null != ds && null != ds.Tables[0])
            {
                string seValue = selCategory.Value;
                string selValue = ds.Tables[0].Rows[0]["CategoryId"].ToString() + "#" + (Convert.ToInt32( ds.Tables[0].Rows[0]["DengJi"].ToString())-1).ToString();
                selCategory.Value = selValue;
                txtTitle.Value = ds.Tables[0].Rows[0]["PName"].ToString();
                //txtSource.Value = ds.Tables[0].Rows[0]["source"].ToString();
                //txtKey.Value = ds.Tables[0].Rows[0]["key"].ToString();
                //txtAuthor.Value = ds.Tables[0].Rows[0]["author"].ToString();

                contentVal = HttpUtility.HtmlDecode(ds.Tables[0].Rows[0]["PContent"].ToString());

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["img"].ToString()))
                //{
                //    imgArticle.Src = ds.Tables[0].Rows[0]["img"].ToString();
                //    hidImg.Value = ds.Tables[0].Rows[0]["img"].ToString();
                //}

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

            string virtualFilePath = ConfigurationManager.AppSettings["ARTICLEDIC"];

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

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        //等到上传图片的名称
        //string imgsrc = UploadPic(fileUploadImg);
        if (productId != 0)
        { //更新操作
            //if (string.IsNullOrEmpty(imgsrc))
            //    imgsrc = hidImg.Value;
            string[] selValues = selCategory.Value.ToString().Split('#');
            int id = Convert.ToInt32(selValues[0]);
            int dengJi = Convert.ToInt32(selValues[1]) + 1;
            strsql = "update [Products] set [CategoryId]=@categoryid,[PName]=@title,[PContent]=@content,[UpTime]=@updatetime,[DengJi]=@Dengji where [ID]=@ID";

            OleDbParameter[] oleParams ={
                                       new OleDbParameter("@categoryid",id),
                                       new OleDbParameter("@title",txtTitle.Value.Trim()),
                                     
                                       new OleDbParameter("@content",HttpUtility.HtmlEncode(Request["content"])),
                                       new OleDbParameter("@updatetime",DateTime.Now.ToString()),
                                       new OleDbParameter("@Dengji",dengJi), 
                                       new OleDbParameter("@ID",productId)
                                       };

            if (sqlM.ExecuteSql(CommandType.Text, strsql, oleParams) > 0)
                Response.Write("<script>alert('更新成功');location.href='ArticleList.aspx';</script>");
        }
        else
        { //添加操作
            string[] selValues = selCategory.Value.ToString().Split('#');
            int id = Convert.ToInt32(selValues[0]);
            int dengJi = Convert.ToInt32(selValues[1]) + 1;

            int click = 0;

            strsql = "insert into [Products]([IsCorP],[CategoryId],[DengJi],[PName],[PContent],[UpTime],[PublishTime]) values(@IsCorP,@CategoryId,@DengJi,@PName,@PContent,@UpTime,@PublishTime)";

            OleDbParameter[] oleParams ={
                                        new OleDbParameter("@IsCorP",1),
                                        new OleDbParameter("@CategoryId",id),
                                         new OleDbParameter("@DengJi",dengJi),
                                       new OleDbParameter("@PName",txtTitle.Value.Trim().ToString()),
                                       new OleDbParameter("@PContent",HttpUtility.HtmlEncode(Request["content"])),
                                       new OleDbParameter("@UpTime",DateTime.Now.ToString()),
                                        new OleDbParameter("@PublishTime",DateTime.Now.ToString())
                                       };
            if (sqlM.ExecuteSql(CommandType.Text, strsql, oleParams) > 0)
                Response.Write("<script>alert('发布成功');location.href='ArticleList.aspx';</script>");
        }
    }
}
