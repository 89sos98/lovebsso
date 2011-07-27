using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using CYQ.Data;
using CYQ.Data.Table;
using System.Configuration;
using CYQ.Entity.MySpace;

namespace Web.Core
{
    public class UserLogin : IDisposable
    {
        MAction action;
        private string _UserNameColumnName;
        private string _PasswordColumnName;
        private string _EmailColumnName;
        private string _NickNameColumnName;
        public UserLogin()
        {
            action = new MAction(TableNames.Blog_User);
            action.EndTransation();
            _UserNameColumnName = Users.UserName.ToString();
            _PasswordColumnName = Users.Password.ToString();
            _EmailColumnName = Users.Email.ToString();
            _NickNameColumnName = Users.NickName.ToString();
        }

        /// <summary>
        /// 网站主域名
        /// </summary>
        public string Domain
        {
            get
            {
                return Config.Domain;
            }
        }
        MDataRow _UserInfo;
        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        public MDataRow UserInfo
        {
            get
            {
                if (_UserInfo == null)
                {
                    IsOnline(false);
                }
                return _UserInfo;
            }
        }
        #region 登陆
        public bool Login(string userName, string password, int minutesCount)
        {
            //userName = Filter(userName);
            if (action.Fill(string.Format("{0}='{1}' and {2}='{3}'", _UserNameColumnName, userName, _PasswordColumnName, Encode.Password(password, true)), "Login"))
            {
                if (action.Get<string>(_PasswordColumnName) == Encode.Password(password, true))
                {
                    //写入登陆信息
                    SetCookie(userName, password, minutesCount);
                }
                return true;
            }
            return false;
        }
        public void SetCookie(string userName, string password, int minutesCount)
        {
            HttpCookie myCookie = new HttpCookie(Domain, Encode.Cookie(userName, true) + "@" + Encode.Password(password,true).Substring(password.Length));
            myCookie.Domain = Domain;
            myCookie.Expires = System.DateTime.Now.AddMinutes(minutesCount);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        #endregion

        #region 注册
        public bool Reg(string userName, string password, string email)
        {
            action.Set(_UserNameColumnName, userName);
            action.Set(_PasswordColumnName, Encode.Password(password, true));
            action.Set(_EmailColumnName, email);
            action.Set(_NickNameColumnName, userName);
            bool result = action.Insert();
            if (result)
            {
                Login(userName, password, 60 * 24);
            }
            return result;
        }
        #endregion
        public bool Logout()
        {
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[Domain];
            if (myCookie != null)
            {
                myCookie.Domain = Domain;
                myCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            return true;
        }

        /// <summary>
        /// 用户是否在线
        /// </summary>
        /// <param name="userBaseBean">返回用户实体数据</param>
        /// <param name="AutoReturnToLogin">用户未登陆时，是否自动转向登陆页</param>
        /// <returns></returns>
        public bool IsOnline(bool autoToLogin)
        {
            bool isOnline = false;
            string userName = null;
            string passowrdKey = string.Empty;
            HttpCookie myCookie = HttpContext.Current.Request.Cookies[Domain];
            if (null != myCookie)
            {
                string cookieValue = myCookie.Value;
                if (cookieValue.IndexOf("@") > -1)
                {
                    string[] items = cookieValue.Split('@');
                    if (items.Length == 2)
                    {
                        userName = Encode.Cookie(items[0], false);
                        passowrdKey = items[1];
                        isOnline = IsExits(userName);
                    }
                }
            }
            if (isOnline)
            {
                if (action.Data[0].Value != null && action.Get<string>(_UserNameColumnName) == userName)
                {
                    _UserInfo = action.Data;
                }
                else
                {
                    _UserInfo = Get(userName);
                }
                if (_UserInfo.Get<string>(_PasswordColumnName).IndexOf(passowrdKey) == -1)
                {
                    isOnline = false;
                    _UserInfo = null;
                }
            }
            else if (autoToLogin)
            {

                HttpContext.Current.Response.Redirect(Config.HttpHost + Config.LoginUrl + "/" + Encode.Url(Convert.ToString(HttpContext.Current.Request.UrlReferrer), true) + Config.UrlAspx);
                //}
                //else
                //{
                //    HttpContext.Current.Response.Redirect(Config.HttpHost + Config.LoginUrl +Config.UrlAspx+ "?back=" + Encode.Url(Convert.ToString(HttpContext.Current.Request.UrlReferrer), true));
                //}
            }
            return isOnline;
        }

        public bool IsExits(string userName)
        {
            userName = Filter(userName);
            if (!string.IsNullOrEmpty(userName))
            {
                return action.GetCount(string.Format("{0}='{1}'", _UserNameColumnName, userName)) > 0;
            }
            return false;
        }
        public MDataRow Get(string userName)
        {
            userName = Filter(userName);
            if (!string.IsNullOrEmpty(userName))
            {
                if (action.Fill(string.Format("{0}='{1}'", _UserNameColumnName, userName)))
                {
                    return action.Data;
                }
            }
            return null;
        }
        private string Filter(string value)
        {
            if (DomainFiter.IsOkKey(value))
            {
                return value;
            }
            return "";
        }
        #region IDisposable 成员

        public void Dispose()
        {
            action.Dispose();
        }

        #endregion
    }
}
