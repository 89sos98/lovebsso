using System;
using System.Web;
using System.Data;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public static class CFunc
{

    public static int YHuiFu = 1;
    public static int WHuiFu = 0;


    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string Encrypt(string password)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
    }

    /// <summary>
    /// 获取IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetIP()
    {
        string ip = string.Empty;
        if (null != HttpContext.Current.Request)
        {
            ip = HttpContext.Current.Request.UserHostAddress;
            if (HttpContext.Current.Request.ServerVariables != null)
            {
                foreach (string k in HttpContext.Current.Request.ServerVariables.AllKeys)
                {
                    if (k != "REMOTE_ADDR")
                        continue;
                    else
                    {
                        ip = HttpContext.Current.Request.ServerVariables[k];
                        break;
                    }
                }
            }
            if (ip == null) ip = string.Empty;
        }
        return ip;
    }

    /// <summary>
    /// 获取字符串中指定位置开始的指定长度的字符串，支持汉字英文混合 汉字为2字节计数
    /// </summary>
    /// <param name="strSub">输入中英混合字符串</param>
    /// <param name="start">开始截取的起始位置</param>
    /// <param name="length">要截取的字符串长度（字节数）</param>
    /// <returns></returns>
    public static string GetSubString(string _str, int _start, int _length)
    {
        if (string.IsNullOrEmpty(_str))
            return null;

        string temp = _str;
        int j = 0, k = 0, p = 0;

        CharEnumerator ce = temp.GetEnumerator();
        while (ce.MoveNext())
        {
            j += (ce.Current > 0 && ce.Current < 255) ? 1 : 2;

            if (j <= _start)
            {
                p++;
            }
            else
            {
                if (j == GetLength(temp))
                {
                    temp = temp.Substring(p, k + 1);
                    break;
                }
                if (j <= _length + _start)
                {
                    k++;
                }
                else
                {
                    temp = temp.Substring(p, k);
                    break;
                }
            }
        }

        return temp;
    }

    /// <summary>
    /// 获取指定字符串长度，汉字以2字节计算
    /// </summary>
    /// <param name="aOrgStr">要统计的字符串</param>
    /// <returns></returns>
    public static int GetLength(String aOrgStr)
    {
        int intLen = aOrgStr.Length;
        int i;
        char[] chars = aOrgStr.ToCharArray();
        for (i = 0; i < chars.Length; i++)
        {
            if (System.Convert.ToInt32(chars[i]) > 255)
            {
                intLen++;
            }
        }
        return intLen;
    }

    /// <summary>
    /// 返回链接地址
    /// </summary>
    /// <param name="_href"></param>
    /// <param name="_link"></param>
    /// <returns></returns>
    public static string GetHref(string _href, string _link)
    {
        return string.IsNullOrEmpty(_href) ? _link : _href;
    }

    /// <summary>
    /// 根据类型id获取类型名称
    /// </summary>
    /// <param name="_categoryid"></param>
    /// <returns></returns>
    public static string GetCategoryName(string _categoryid) {
        SqlManage sqlM = SqlManage.GetInstance();
        if (Convert.ToInt32(_categoryid)==0)
        {
            return "顶级产品";
        }
        string strsql = "select PName from [Products] where [ID]=" + _categoryid+" and [IsCorP]=0";
        object obj = sqlM.GetFistColumn(CommandType.Text, strsql);
        if (DBNull.Value != obj&&obj!=null)
            return obj.ToString();
        return null;
    }

    /// <summary>
    /// 判定当前用户是否有访问某个页面的权限
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static bool HasPageRight(string _url)
    {
        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            FormsAuthentication.RedirectToLoginPage();
            HttpContext.Current.Response.End();
        }
        if (HttpContext.Current.User.Identity.Name == ConfigurationSettings.AppSettings["superuser"]) return true;

        //给定url的模块信息
        string strSql = "select * from [SysModule] where [href]='" + _url + "'";
        SqlManage sqlM = SqlManage.GetInstance();
        DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
       
        //当前用户信息
        strSql = "select [moduleids] from [SysUser] where [username]='" + HttpContext.Current.User.Identity.Name + "'";
        object obj = sqlM.GetFistColumn(CommandType.Text, strSql);

        if (null != ds && null != ds.Tables[0]&&ds.Tables[0].Rows.Count>0)
        {
            if (null != obj && DBNull.Value != obj)
            {
                string[] mids = obj.ToString().Split(',');
                foreach (string i in mids)
                {
                    if (i == ds.Tables[0].Rows[0]["moduleid"].ToString()) { return true; }
                }
            }
        }

        return false;
    }


    /// <summary>
    /// 绑定分类信息下拉列表添加一个值
    /// </summary>
    /// <param name="selCategory">htmlselect控件</param>
    /// <param name="listitem">第一项</param>
    /// <param name="def">是否显示默认分类</param>
    public static void BindCategorys(HtmlSelect _selCategory, ListItem _item, bool _def, CategoryArea? _area)
    {
        if (_selCategory.Items.Count <= 0)
        {
            _selCategory.Items.Add(_item);

            //查询所有分类信息
            string strsql = "select * from [Products] where IsCorP=0";
            //if (null != _area)
            //    strsql += " and [area]=" + (sbyte)_area;
            //if (!_def)
            //    strsql += " and [default]=" + false;
            //strsql += " order by [sort]";
            SqlManage sqlM = SqlManage.GetInstance();
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strsql);
            if (null != ds && null != ds.Tables[0])
            {
                //查询一级分类
                DataRow[] firstClass = ds.Tables[0].Select("CategoryId=0 and DengJi=1", "UpTime desc");

                foreach (DataRow r in firstClass)
                {
                    //绑定一级分类到dropdowlist
                    _selCategory.Items.Add(new ListItem(r["PName"].ToString(), r["ID"].ToString()));
                    //查询当前一级分类下的二级分类
                    DataRow[] secondClass = ds.Tables[0].Select("CategoryId=" + int.Parse(r["ID"].ToString()) + " and DengJi=2", "UpTime desc");
                    foreach (DataRow sr in secondClass)
                    {
                        //绑定二级分类到dropdownlilst
                        _selCategory.Items.Add(new ListItem("  ——" + sr["PName"].ToString(), sr["ID"].ToString()));
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绑定分类信息下拉列表中添加两个值
    /// </summary>
    /// <param name="selCategory">htmlselect控件</param>
    /// <param name="listitem">第一项</param>
    /// <param name="def">是否显示默认分类</param>
    public static void BindCategory(HtmlSelect _selCategory,ListItem _item,bool _def,CategoryArea? _area)
    {
        if (_selCategory.Items.Count <= 0)
        {
            _selCategory.Items.Add(_item);

            //查询所有分类信息
            string strsql = "select * from [Products] where IsCorP=0";
            //if (null != _area)
            //    strsql += " and [area]=" + (sbyte)_area;
            //if (!_def)
            //    strsql += " and [default]=" + false;
            //strsql += " order by [sort]";
            SqlManage sqlM = SqlManage.GetInstance();
            DataSet ds = sqlM.GetDataSet(CommandType.Text, strsql);
            if (null != ds && null != ds.Tables[0])
            {
                //查询一级分类
                DataRow[] firstClass = ds.Tables[0].Select("CategoryId=0 and DengJi=1","UpTime desc");

                foreach (DataRow r in firstClass)
                {
                    //绑定一级分类到dropdowlist
                    _selCategory.Items.Add(new ListItem(r["PName"].ToString(), r["ID"].ToString()+"#"+r["DengJi"].ToString()));
                    //查询当前一级分类下的二级分类
                    DataRow[] secondClass = ds.Tables[0].Select("CategoryId=" + int.Parse(r["ID"].ToString())+" and DengJi=2", "UpTime desc");
                    foreach (DataRow sr in secondClass)
                    {
                        //绑定二级分类到dropdownlilst
                        _selCategory.Items.Add(new ListItem("  ——" + sr["PName"].ToString(), sr["ID"].ToString() + "#" + sr["DengJi"].ToString()));
                    }
                }
            }
        }
    }

}

