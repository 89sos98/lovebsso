using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Pager : System.Web.UI.UserControl
{
    public PagerInfo PInfo = new PagerInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBind();
        }
        
    }

    private void DataBind()
    {
        
        if (slePage.Items.Count <= 0)
        {
            for (int i = 1; i <= (PInfo.TotalPage < 8 ? PInfo.TotalPage : 8); i++)
            {
                slePage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
        {
            slePage.SelectedValue = Request.QueryString["page"];
        }
    }



    //public PagerInfo Pinfo
    //{
    //    get { return _pinfo; }
    //    set { _pinfo = value; }
    //}

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

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Write("sdfas");
        string strUrl = GetUrl(int.Parse(slePage.SelectedValue));
        Response.Redirect(strUrl);
    }
    protected void BtnGo_Click(object sender, EventArgs e)
    {
        
    }

}