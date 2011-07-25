<%@ WebHandler Language="C#" Class="ValidCode" %>

using System;
using System.Web;
using System.Drawing;
using System.IO;

public class ValidCode : IHttpHandler {
    private const int VALID_CODE_LENGTH = 4;
    private const string COOKIE_PREFIX = "VCode";
    private const string VALID_CODE_CHARS = "123456789abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";

    //private static Image _backgroundImage = null;

    static ValidCode()
    {
        //string imageFilePath = System.Configuration.ConfigurationManager.AppSettings["validCodeBg"];
        //imageFilePath = HttpContext.Current.Server.MapPath(imageFilePath);
        //_backgroundImage = Image.FromFile(imageFilePath);
    }
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "image/gif";
        context.Response.Buffer = true;

        int width = VALID_CODE_LENGTH * 15;
        string vcode = GetRandomString(VALID_CODE_LENGTH);

        Bitmap img = new Bitmap(width, 20);
        Graphics g = Graphics.FromImage(img);
        g.Clear(Color.White);

        //颜色数组
        Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Chocolate, Color.Brown, Color.DarkCyan, Color.Purple };

        //字体数组
        string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial" };

        Random random = new Random();

        //绘制文字
        for (int i = 0; i < vcode.Length; i++)
        {
            Color nowColor = c[random.Next(c.Length - 1)];
            Font nowFont = new Font(font[random.Next(font.Length - 1)], 12, FontStyle.Bold);
            Brush nowBrush = new SolidBrush(nowColor);
            g.DrawString(vcode.Substring(i, 1), nowFont, nowBrush, 1 + (i * 12), 1);
        }

        //绘制边框
        g.DrawRectangle(new Pen(c[random.Next(c.Length - 1)]), 0, 0, img.Width - 1, img.Height - 1);

        //输出到内存,并显示
        System.IO.MemoryStream imgMemory = new System.IO.MemoryStream();
        img.Save(imgMemory, System.Drawing.Imaging.ImageFormat.Gif);
        context.Response.ClearContent();
        context.Response.BinaryWrite(imgMemory.ToArray());
        g.Dispose();
        imgMemory.Dispose();
        imgMemory.Close();
        img.Dispose();

        vcode = CFunc.Encrypt(vcode.ToLower());
        context.Response.Cookies.Add(new HttpCookie(COOKIE_PREFIX, vcode));

        context.Response.Flush();
        context.Response.End();
    }

    

    /// <summary>
    /// 生成验证字符串
    /// </summary>
    private string GetRandomString(int codeLength)
    {
        string vcode = string.Empty;
        Random random = new Random(Guid.NewGuid().GetHashCode());

        for (int i = 0; i < codeLength; i++)
        {
            vcode += VALID_CODE_CHARS[random.Next(0, VALID_CODE_CHARS.Length - 1)];
        }

        return vcode;
    }
    
    public bool IsReusable {
        get {
            return true;
        }
    }
    
    

}