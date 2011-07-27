


using System.Web;
namespace Web.Editor
{
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                string fullPath = CYQ.Editor.Uploader.UploadFile(context.Request.Files[0], 0);
                if (fullPath != null)
                {
                    string function =fullPath.IndexOf(".swf") > -1?"SendUploadFlash":"SendUploadImg";
                    context.Response.Write("<script language=\"javascript\">parent.deditorClass."+function+"('" + fullPath + "','" + context.Request["editid"].Replace("_iframe", string.Empty) + "');</script>");
                    return;
                }
            }
            context.Response.Write("<script language=\"javascript\">parent.alert('上传失败');</script>");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
