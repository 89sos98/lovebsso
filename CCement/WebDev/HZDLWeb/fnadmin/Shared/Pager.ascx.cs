using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

public partial class fnadmin_Shared_Pager : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (null == PInfo)
            PInfo = new PagerInfo();
    }

    private PagerInfo _pinfo;
    public PagerInfo PInfo
    {
        get { return _pinfo; }
        set { _pinfo = value; }
    }

    /// <summary>
    /// 组成URL
    /// </summary>
    /// <param name="_page">要请求的页码</param>
    /// <returns></returns>
    public static string GetUrl(int _page)
    {
        string[] urlInfo = HttpUtility.UrlDecode(HttpContext.Current.Request.Url.ToString()).Split('?');
        Dictionary<string, string> dic = new Dictionary<string, string>();

        //if (urlInfo.Length > 1) {
        //    var urlParams = urlInfo[1].Split('&');

        //    foreach (var i in urlParams) {
        //        var p = i.Split('=');
        //        dic.Add(p[0], p[1]);
        //    }
        //}

        NameValueCollection urlParams = HttpContext.Current.Request.QueryString;
        foreach (string i in urlParams.AllKeys)
        {
            dic.Add(i, urlParams[i]);
        }

        if (null != dic && dic.Count > 0)
        {
            if (dic.ContainsKey("page"))
            { dic["page"] = _page.ToString(); }
            else
            { dic.Add("page", _page.ToString()); }
        }
        else
        {
            dic.Add("page", _page.ToString());
        }

        string strUrl = urlInfo[0] + "?";
        int j = 1;//计数器
        foreach (KeyValuePair<string, string> i in dic)
        {
            if (j == 1)
            {
                strUrl += i.Key + "=" + i.Value;
                j++;
            }
            else
            {
                strUrl += "&" + i.Key + "=" + i.Value;
                j++;
            }
        }

        return strUrl;
    }

    protected void BtnGo_Click(object sender, EventArgs e)
    {
        string strUrl = GetUrl(int.Parse(txtpage.Value));
        Response.Redirect(strUrl);
    }

}