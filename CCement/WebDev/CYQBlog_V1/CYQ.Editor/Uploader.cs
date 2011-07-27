using System;
using System.Web;
using System.IO;
using Web.Core;



namespace CYQ.Editor
{
    public class Uploader
    {

        /// <summary>
        /// ִ���ļ��ϴ����������ϴ����ȫ���ļ�·��
        /// </summary>
        /// <param name="oFile">HttpPostedFile����</param>
        /// <param name="UpFileType">�ļ��ϴ���ʽ��0���ط�,1�ط�ͼƬ��2�ط�flash</param>
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
                //��ʾ���� 
                HttpContext.Current.Response.Write("<script language='javascript'>alert('�ļ��ϴ�ʧ��[�ļ���Сֻ����С��4MB!]');</script>");
            }

            return null;
        }

        /// <summary>
        /// ���ϻط����ϴ��ļ�ȫ��·��
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="fileName"></param>
        private static void SendResults(string fileUrl,int fileType)
        {
            string fileHtml = "";

            if (fileType == 1)//ͼƬ
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
