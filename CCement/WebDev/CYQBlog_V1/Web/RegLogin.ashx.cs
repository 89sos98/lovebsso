using System;
using CYQ.Data.Xml;
using Module;
using Web.Core;
using CYQ.Entity;


namespace Web
{
    public class RegLogin : HttpCustom
    {
        string userName, password, email;
        protected override void Page_Load()
        {
            switch (GetPara(2))
            {
                case "logout":
                    base.UserAction.Logout();
                    GoTo(Convert.ToString(Request.UrlReferrer));
                    break;
            }
        }
        protected override void OnPost()
        {
            userName = Get("txtUserName");
            password = Get("txtPassword");
            email = Get("txtEmail");
            switch (ThisAction)
            {

                case "Reg":
                    if (Click("btnCheck"))//点检查按钮时
                    {
                        CheckCanBeRegistry(true);
                    }
                    else if (Click("btnReg")) //点提交按钮时
                    {
                        if (CheckCanBeRegistry(false))
                        {
                            if (UserAction.Reg(userName, password, email))
                            {
                                GoTo("/" + userName+Config.UrlAspx);
                            }
                        }
                        Document.Set("msg", Language.Get(IDKey.postMessage));
                    }
                    break;
                case "Login":
                    bool result = UserAction.Login(userName, password, 60 * 24);
                    if (result)
                    {
                        string back = Tool.Common.GetPara(UrlPara, 3);
                        if (back.Length > 2)
                        {
                            GoTo(Encode.Url(back, false));
                        }
                        else
                        {
                            GoTo(string.Format("/{0}/admin", userName));
                        }
                    }
                    else
                    {
                        Document.Set("msg", Language.Get("loginerror"));
                    }
                    break;
            }
        }

        #region Post业务代码处理
        public bool CheckCanBeRegistry(bool outMsg)
        {

            bool result = userName.Length > 4 && DomainFiter.IsOk(userName) && !UserAction.IsExits(userName);
            if (!result || outMsg)
            {
                Document.Set("msg", Language.Get((result ? "usernamcanreg" : "usernameexist")));
                Document.SetFor("txtUserName", SetType.Value, userName);
            }
            return result;
        }
        #endregion
    }
}
