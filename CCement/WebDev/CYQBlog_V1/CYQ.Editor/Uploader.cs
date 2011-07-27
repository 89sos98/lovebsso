using System;
using System.Web;
using System.IO;
using Web.Core;



namespace CYQ.Editor
{
    public class Uploader
    {

        /// <summary>
        /// 执行文件上传，并返回上传后的全部文件路径
        /// </summary>
        /// <param name="oFile">HttpPostedFile对象</param>
        /// <param name="UpFileType">文件上传方式：0不回发,1回发图片，2回发flash</param>
        public static string UploadFile(HttpPostedFile postFile,int fileType)
        {
            FileUpload file = new FileUpload(postFile);
            if (file.Upload())
            {
                switch (fileType)
                {
                    case 1:
                    case 2:
                        SendResults(file.fileNameWithHttp, fileType);
                        break;
                }
                return file.fileNameWithHttp;
            }
            else
            {
                //提示错误 
                HttpContext.Current.Response.Write("<script language='javascript'>alert('文件上传失败[文件大小只允许小于4MB!]');</script>");
            }

            return null;
        }

        /// <summary>
        /// 向上回发已上传文件全部路径
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="fileName"></param>
        private static void SendResults(string fileUrl,int fileType)
        {
            string fileHtml = "";

            if (fileType == 1)//图片
            {
                fileHtml = "<img src=\"" + fileUrl + "\" />";
            }
            else if (fileType == 2)//flash
            {
                fileHtml += "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"200\" height=\"150\">";
                fileHtml += "  <param name=\"movie\" value=\"" + fileUrl + "\" />";
                fileHtml += "  <param name=\"quality\" value=\"high\" />";
                fileHtml += "  <embed src=\"" + fileUrl + "\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"200\" height=\"150\"></embed>";
                fileHtml += "</object>";
            }

            HttpContext.Current.Response.Write("<script type=\"text/javascript\">window.parent.document.getElementById('fileUp_Load').innerHTML='';window.parent.document.getElementById('fileUp_Load').style.display='none';window.parent.document.getElementById('filePreview_div').style.display='block';window.parent.document.getElementById('filePreview').innerHTML='" + fileHtml + "';window.parent.document.getElementById('txtFileUrl').value='" + fileUrl + "';</script>");
        }

    }
}
