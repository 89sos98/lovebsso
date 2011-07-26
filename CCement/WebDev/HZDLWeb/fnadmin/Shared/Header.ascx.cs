using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class fnadmin_Shared_Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnOut_Click(object sender, EventArgs e)
    {
        //清除Session
        Session.RemoveAll();

        //注销登陆
        FormsAuthentication.SignOut();

        //跳转至登陆页面
        //FormsAuthentication.RedirectToLoginPage();
        Response.Redirect("~/fnadmin/Login.aspx");
    }
}
