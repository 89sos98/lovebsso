using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Data;

public partial class fnadmin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string uname = txtuname.Value.Trim();
        string pwd = CFunc.Encrypt(txtpwd.Value.Trim());
        string vcode = CFunc.Encrypt(txtcode.Value.Trim().ToLower());

        if (vcode != Request.Cookies["VCode"].Value)
        {
            Response.Write("<script>alert('验证码输入有误!');location.href=location.href;</script>");
            return;
        }

        if (uname == ConfigurationManager.AppSettings["superuser"])
        {
            //超级用户登陆验证
            if (pwd != ConfigurationManager.AppSettings["superuserpwd"])
            {
                Response.Write("<script>alert('密码错误!');location.href=location.href;</script>");
            }
            else
            {
                //超级用户登陆成功
                //1.保存验证票据
                FormsAuthentication.SetAuthCookie(uname, false);
                //2.页面跳转
                FormsAuthentication.RedirectFromLoginPage(uname, false);
            }
        }
        else
        {
            //用户登录验证
            SqlManage sqlm = SqlManage.GetInstance();
            string strSql = "select [username] from [SysUser] where [username]='" + uname + "' and [password]='" + pwd + "'";
            object obj = sqlm.GetFistColumn(CommandType.Text, strSql);
            if (DBNull.Value != obj && null!=obj)
            {
                //保存验证票据
                FormsAuthentication.SetAuthCookie(uname, false);
                //页面跳转
                FormsAuthentication.RedirectFromLoginPage(uname, false);
            }
            else
                Response.Write("<script>alert('用户名或密码错误!');location.href=location.href;</script>");
        }
    }
}
